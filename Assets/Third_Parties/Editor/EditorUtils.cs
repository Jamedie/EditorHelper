using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;


/// <summary>
/// Script who Creat windows for change resolution, set alpha for overlay and take screen shot
/// (for screen shot if you want spam you must be in runtime)
/// </summary>

public class EditorUtils : EditorWindow
{
    #region PublicAttributes
    //float for change alpha of overlay
    public float m_fOverlayAlpha = 1;
    public GameObject m_CanvasEditior;
    #endregion
    #region PrivateAttributes
    //private attribute for store overlay gameobject
    GameObject m_Overlay;
    //private attribute for store default path
    string m_szCurrentScreenshotFolderPath;
    string[] m_szAvailableFormat = new string[] { "png", "jpg", "tga", "exr" };
    string m_szCurrentScreenShotFormat = "png";
    Rect m_ButtonRectDropDown;
    #endregion

    #region Menu Item Declaration
    [MenuItem("Tools/Editor Utils")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(EditorUtils));
    }
    #endregion

    #region PublicFunction

    #endregion

    #region PrivateFunction
    #region Overlay
    private void ChangeOverlayAlpha(float _fOverlayAlpha)
    {
        Image DeadZonne = GetOverlay().GetComponent<Image>();
        if (DeadZonne != null)
        {
            Color _NewColor = new Color(DeadZonne.color.r, DeadZonne.color.g, DeadZonne.color.b, m_fOverlayAlpha);
            DeadZonne.color = _NewColor;
        }
    }

    private float GetcurrentOverlayAlpha()
    {
        Image DeadZonne = GameObject.Find("Overlay").GetComponent<Image>();
        if (DeadZonne != null)
        {
            return DeadZonne.color.a;
        }
        else
        {
            return 0;
        }
    }

    public GameObject GetOverlay()
    {
        if (m_Overlay != null)
        {
            return m_Overlay;
        }
        else if (GameObject.Find("Overlay") != null)
        {
            m_Overlay = GameObject.Find("Overlay");
            return m_Overlay;
        }
        else
        {
            if (m_CanvasEditior != null)
            {
                GameObject.Instantiate(m_CanvasEditior);
            }
            m_Overlay = GameObject.Find("Overlay");
            return m_Overlay;
        }
    }

    private void ChangeStateOverlay(bool _NewState)
    {
        if (GetOverlay() != null)
        {
            m_Overlay.SetActive(_NewState);
        }
    }
    #endregion
    //Return screenshot path location
    private string GetScreenShotpath()
    {
        m_szCurrentScreenshotFolderPath = Application.persistentDataPath + "/Screenshots/";
        return m_szCurrentScreenshotFolderPath;
    }
    //Take screenshot in gameview
    private void TakeScreenShot()
    {
        //Disable Overlay
        m_Overlay = GetOverlay();
        ChangeStateOverlay(false);

        int _iWidth, _Height;
        GameViewSizeManager.GetGameRenderSize(out _iWidth, out _Height);

        string _Path = GetScreenShotpath() + _iWidth + "x" + _Height;

        if (!Directory.Exists(_Path))
        {
            Directory.CreateDirectory(_Path);
        }

        DirectoryInfo _Dir = new DirectoryInfo(_Path);

        string fileName = "Screenshot_" + _iWidth + "x" + _Height + "_" + DirCount(_Dir) + "." + m_szCurrentScreenShotFormat;
        ScreenCapture.CaptureScreenshot(_Path + "/" + fileName);
        _Dir.Refresh();



    }
   
    //count number of element in specific directory for name of screenshot
    private static long DirCount(DirectoryInfo d)
    {
        long i = 0;
        // Add file sizes.
        FileInfo[] fis = d.GetFiles();
        foreach (FileInfo fi in fis)
        {            
            if (fi.Name.Contains("Screen"))
            {
                i++;
            }
        }
        return i;
    } 
    private void ChangeCurrentScreenShotFormat(string _szNewFormat)
    {
        m_szCurrentScreenShotFormat = _szNewFormat;
    }

    private IEnumerator TakeScreenshotCoroutine()
    {


        //Wait for the end of frame
        yield return new WaitForEndOfFrame();  
        //Disable Overlay
        m_Overlay = GetOverlay();
        ChangeStateOverlay(false);

        int _iWidth, _Height;
        GameViewSizeManager.GetGameRenderSize(out _iWidth, out _Height);

        string _Path = GetScreenShotpath() + _iWidth + "x" + _Height;

        if (!Directory.Exists(_Path))
        {
            Directory.CreateDirectory(_Path);
        }

        DirectoryInfo _Dir = new DirectoryInfo(_Path);

        string fileName = "Screenshot_" + _iWidth + "x" + _Height + "_" + DirCount(_Dir) + "." + m_szCurrentScreenShotFormat;

        //Create a screenshot texture with screen size
        Texture2D _ScreenshotTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        //Read pixels from screen
        _ScreenshotTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        //Apply pixels to the texture
        _ScreenshotTexture.Apply();
        byte[] bytes = _ScreenshotTexture.EncodeToPNG();

        //Save our test image (could also upload to WWW)
        File.WriteAllBytes(_Path + "/" + fileName, bytes);
    }

    public RenderTexture rt;
    // Use this for initialization
    public void SaveTexture()
    {
        byte[] bytes = ScreenCapture.CaptureScreenshotAsTexture().EncodeToPNG();
        System.IO.File.WriteAllBytes("C:/Users/Wonder Partner/AppData/LocalLow/DefaultCompany/EtditorHelper/Screenshots/SavedScreen.png", bytes);
    }
    #endregion

    //Create all buttons / slider of custom windows
    #region OnGui   
    void OnGUI()
    {
        GUILayout.Label("Device Preview");
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Label("Smartphone");
        if (GUILayout.Button("Iphone X"))
        {
            string _RezName = "Iphone X";
            if (GameViewSizeManager.FindRecorderSizeObj(_RezName) == null)
            {
                GameViewSizeManager.AddSize(1125, 2436, _RezName);
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj(_RezName));
        }
        if (GUILayout.Button("Iphone XR"))
        {
            string _RezName = "Iphone XR";
            if (GameViewSizeManager.FindRecorderSizeObj(_RezName) == null)
            {
                GameViewSizeManager.AddSize(828, 1792, _RezName);
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj(_RezName));
        }
        if (GUILayout.Button("Huawei P30"))
        {
            string _RezName = "Huawei P30";
            if (GameViewSizeManager.FindRecorderSizeObj(_RezName) == null)
            {
                GameViewSizeManager.AddSize(1080, 2340, _RezName);
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj(_RezName));
        }
        if (GUILayout.Button("Full HD"))
        {
            if (GameViewSizeManager.FindRecorderSizeObj("Redmi Note 3 Pro") == null)
            {
                //GameViewSizeManager.AddCustomSize(GameViewSizeManager.GameViewSizeType.FixedResolution, GameViewSizeGroupType.Standalone, 1080, 1920, "Redmi Note 3 pro");
                GameViewSizeManager.AddSize(1080, 1920, "Redmi Note 3 pro");
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj("Redmi Note 3 Pro"));
        }

        GUILayout.EndVertical();
        GUILayout.BeginVertical(); //Instead of hardcoding it, you can use a for()    loop to get multiple columns.
        GUILayout.Label("Tablet");
        if (GUILayout.Button("Ipad 12.9\""))
        {
            string _RezName = "Ipad 12.9";
            if (GameViewSizeManager.FindRecorderSizeObj(_RezName) == null)
            {
                GameViewSizeManager.AddSize(2048, 2732, _RezName);
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj(_RezName));
        }
        if (GUILayout.Button("Galaxy Tab S4"))
        {
            string _RezName = "Galaxy Tab S4";
            if (GameViewSizeManager.FindRecorderSizeObj(_RezName) == null)
            {
                GameViewSizeManager.AddSize(1600, 2560, _RezName);
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj(_RezName));
        }
        if (GUILayout.Button("Huawei Media Pad M5 Lite"))
        {

        }
        if (GUILayout.Button("Lenovo TAB 4"))
        {

        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal(); //Remember to end each one!

        GUILayout.Label("Stores Preview");
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        GUILayout.Label("Android");
        if (GUILayout.Button("Smartphone Android"))
        {
            string _RezName = "Smartphone Android";
            if (GameViewSizeManager.FindRecorderSizeObj(_RezName) == null)
            {
                GameViewSizeManager.AddSize(1080, 1920, _RezName);
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj(_RezName));
        }
        if (GUILayout.Button("Tablet 7\""))
        {
            string _RezName = "Tablet 7";
            if (GameViewSizeManager.FindRecorderSizeObj(_RezName) == null)
            {
                GameViewSizeManager.AddSize(800, 1280, _RezName);
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj(_RezName));
        }
        if (GUILayout.Button("Tablet 10\""))
        {
            string _RezName = "Tablet 10";
            if (GameViewSizeManager.FindRecorderSizeObj(_RezName) == null)
            {
                GameViewSizeManager.AddSize(1538, 2048, _RezName);
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj(_RezName));
        }

        GUILayout.EndVertical();
        GUILayout.BeginVertical(); //Instead of hardcoding it, you can use a for()    loop to get multiple columns.
        GUILayout.Label("Ios");
        if (GUILayout.Button("Iphone 6.5\""))
        {
            string _RezName = "Iphone 6.5";
            if (GameViewSizeManager.FindRecorderSizeObj(_RezName) == null)
            {
                GameViewSizeManager.AddSize(1242, 2688, _RezName);
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj(_RezName));
        }
        if (GUILayout.Button("Iphone 5.5\""))
        {
            string _RezName = "Iphone 5.5";
            if (GameViewSizeManager.FindRecorderSizeObj(_RezName) == null)
            {
                GameViewSizeManager.AddSize(1242, 2208, _RezName);
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj(_RezName));
        }
        if (GUILayout.Button("Ipad 12.9\""))
        {
            string _RezName = "Ipad 12.9";
            if (GameViewSizeManager.FindRecorderSizeObj(_RezName) == null)
            {
                GameViewSizeManager.AddSize(2048, 2732, _RezName);
            }
            GameViewSizeManager.SelectSize(GameViewSizeManager.FindRecorderSizeObj(_RezName));
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal(); //Remember to end each one!

        //Overlay
        GUILayout.Label("Overlay Setting");
        if (GUILayout.Button("Change Overlay State"))
        {
            if (GetOverlay() != null && m_Overlay != null)
            {
                ChangeStateOverlay(!m_Overlay.activeSelf);
            }
        }
        GUILayout.BeginHorizontal();
        GUILayout.Label("Alpha");
        m_fOverlayAlpha = GUILayout.HorizontalSlider(m_fOverlayAlpha, 0, 1, GUILayout.ExpandWidth(true));
        //Chage alpha
        if (GetOverlay() != null || m_Overlay != null)
        {
            ChangeOverlayAlpha(m_fOverlayAlpha);
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("ScreenShot");
        if (GUILayout.Button("Selected screenshot format : " + m_szCurrentScreenShotFormat, GUILayout.Width(Screen.width)))
        {
            PopupWindow.Show(m_ButtonRectDropDown, new PopupExample(m_szAvailableFormat, "Select desired screenshot format", ChangeCurrentScreenShotFormat));
        }
        if (Event.current.type == EventType.Repaint) m_ButtonRectDropDown = GUILayoutUtility.GetLastRect();

        if (GUILayout.Button("TakeScreenShot"))
        {
            TakeScreenShot();
            //EditorCoroutineUtility.StartCoroutine(TakeScreenshotCoroutine(), this);
           // SaveTexture();
        }
        if (GUILayout.Button("Open ScreenshotFolder"))
        {
            DirectoryInfo _Dir = new DirectoryInfo(GetScreenShotpath());
            _Dir.Refresh();

            Application.OpenURL(GetScreenShotpath());
        }
    }
    #endregion
}
