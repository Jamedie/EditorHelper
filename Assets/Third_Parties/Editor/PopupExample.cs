using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class PopupExample : PopupWindowContent
{
    string m_szPopUpTitle;
    string[] m_szDropDownValuesArray;
    UnityAction<string> m_UnityAction;

    public PopupExample(string[] _DropDownValueArray, string _PopUpTitle, UnityAction<string> _UnityAction)
    {
        m_szPopUpTitle = _PopUpTitle;
        m_szDropDownValuesArray = _DropDownValueArray;
        m_UnityAction = _UnityAction;
    }

    public override Vector2 GetWindowSize()
    {
        return new Vector2(200, 150);
    }

    public override void OnGUI(Rect rect)
    {
        GUILayout.Label(m_szPopUpTitle);
        if (m_szDropDownValuesArray.Length > 0)
        {
           for (int i = 0; i < m_szDropDownValuesArray.Length; i++)
            {
                if (GUILayout.Button(m_szDropDownValuesArray[i]))
                {
                    OnSelectedValue(m_szDropDownValuesArray[i]);
                }
            }
        }
    }

    void OnSelectedValue(string _szSelectedValue)
    {
        m_UnityAction.Invoke(_szSelectedValue);
        this.editorWindow.Close();
    }
}