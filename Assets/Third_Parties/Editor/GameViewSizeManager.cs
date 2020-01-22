using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;


public class GameViewSizeManager
{
    #region MainGameView
    public static EditorWindow GetMainGameView()
    {
        Type T = Type.GetType("UnityEditor.GameView,UnityEditor");
        MethodInfo GetMainGameView = T.GetMethod("GetMainGameView", BindingFlags.NonPublic | BindingFlags.Static);
        System.Object Res = GetMainGameView.Invoke(null, null);
        return (EditorWindow)Res;
    }
    #endregion

    #region PublicFunction
    public static object AddSize(int width, int height, string _NewSizeName)
    {
        var sizeObj = NewSizeObj(width, height, _NewSizeName);

        var group = Group();
        var obj = @group.GetType().GetMethod("AddCustomSize", BindingFlags.Public | BindingFlags.Instance);
        obj.Invoke(@group, new object[] { sizeObj });

        return sizeObj;
    }
    public static object FindRecorderSizeObj(string _SizeName)
    {
        var group = Group();

        var customs = @group.GetType().GetField("m_Custom", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(@group);

        var itr = (IEnumerator)customs.GetType().GetMethod("GetEnumerator").Invoke(customs, new object[] { });
        while (itr.MoveNext())
        {
            var txt = (string)itr.Current.GetType().GetField("m_BaseText", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(itr.Current);
            if (txt.Equals(_SizeName, StringComparison.OrdinalIgnoreCase))
            {
                return itr.Current;
            }
        }
        return null;
    }
    public static void SelectSize(object size)
    {
        var index = IndexOf(size);

        var gameView = GetMainGameView();
        var obj = gameView.GetType().GetMethod("SizeSelectionCallback", BindingFlags.Public | BindingFlags.Instance);
        obj.Invoke(gameView, new object[] { index, size });
    }
    public static int IndexOf(object sizeObj)
    {
        var group = Group();
        var method = @group.GetType().GetMethod("IndexOf", BindingFlags.Public | BindingFlags.Instance);
        int index = (int)method.Invoke(@group, new object[] { sizeObj });

        var builtinList = @group.GetType().GetField("m_Builtin", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(@group);

        method = builtinList.GetType().GetMethod("Contains");
        if ((bool)method.Invoke(builtinList, new object[] { sizeObj }))
            return index;

        method = @group.GetType().GetMethod("GetBuiltinCount");
        index += (int)method.Invoke(@group, new object[] { });

        return index;
    }
    public static void GetGameRenderSize(out int width, out int height)
    {
        var gameView = GetMainGameView();
        var prop = gameView.GetType().GetProperty("targetSize", BindingFlags.NonPublic | BindingFlags.Instance);
        var size = (Vector2)prop.GetValue(gameView, new object[0] { });
        width = (int)size.x;
        height = (int)size.y;
    }
    public static string GetGameRenderName()
    {
        var gameView = GetMainGameView();
        var prop = gameView.GetType().GetProperty("get_title", BindingFlags.NonPublic | BindingFlags.Instance);
        var name = (string)prop.GetValue(gameView, new object[0] { });
        return name;

    }
    public static void PrintTypeDatas(object obj)
    {
        //Recupération de l'objet System.Type représentant le type sous-jacent de l'objet
        Type objectType = obj.GetType();

        //Récupération des informations concernant les propriétés de l'objet
        PropertyInfo[] properties = objectType.GetProperties();

        //Récupération des informations concernant les méthodes de l'objet
        MethodInfo[] methods = objectType.GetMethods();

        //Révèle le type sous-jacent
        Console.WriteLine(obj.GetType().Name);

        foreach (var property in properties)
        {
            string propertyData = String.Format("Property : {0} - Type : {1}", property.Name, property.PropertyType.Name);
            Debug.Log(propertyData);
        }

        foreach (var method in methods)
        {
            string methodData = String.Format("Method : {0} - Return type : {1}", method.Name, method.ReturnType);
            Debug.Log(methodData);
        }
    }
    #endregion

    #region PrivateFunction
    static object NewSizeObj(int width, int height, string _NewSizeName)
    {
        var T = Type.GetType("UnityEditor.GameViewSize,UnityEditor");
        var TT = Type.GetType("UnityEditor.GameViewSizeType,UnityEditor");

        var c = T.GetConstructor(new Type[] { TT, typeof(int), typeof(int), typeof(string) });
        var sizeObj = c.Invoke(new object[] { 1, width, height, _NewSizeName });
        return sizeObj;
    }
    static object Group()
    {
        var T = Type.GetType("UnityEditor.GameViewSizes,UnityEditor");
        var sizes = T.BaseType.GetProperty("instance", BindingFlags.Public | BindingFlags.Static);
        var instance = sizes.GetValue(null, new object[0] { });

        var currentGroup = instance.GetType().GetProperty("currentGroup", BindingFlags.Public | BindingFlags.Instance);
        var group = currentGroup.GetValue(instance, new object[0] { });
        return group;
    }
    #endregion
}
