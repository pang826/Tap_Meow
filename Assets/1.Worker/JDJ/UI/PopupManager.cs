using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PopupManager
{
    private const string POPUP_PATH = "Popups/";
    private static Stack<BasePopupCanvas> _popups = new Stack<BasePopupCanvas>();

    private static BasePopup LoadPopup(string m_popupName)
    {
        BasePopupCanvas pcObj = GameObject.Instantiate(Resources.Load<BasePopupCanvas>($"{POPUP_PATH}{m_popupName}"));

        if (pcObj == null)
            throw new System.Exception("등록되지 않은 팝업");

        _popups.Push(pcObj);
        return pcObj.Popup;
    }

    public static BasePopup ShowPopup(string m_popupName)
    {
        return LoadPopup(m_popupName);
    }

    public static T ShowPopup<T>(string m_popupName)
    {
        return ShowPopup(m_popupName).GetComponent<T>();
    }

    public static void DeadPopup()
    {
        if(_popups.Count > 0)
        {
            _popups.Pop().Popup.Dead();
        }
    }
}
