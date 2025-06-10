using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockPanel : MonoBehaviour
{
    private Image _img;
    private PointEvtHandler _evtHandler;

    /// <summary>
    /// Block 패널 입력시 동작할 행동을 설정합니다.
    /// </summary>
    public void SetCustomBlockAction(Action<PointerEventData> m_customAction)
    {
        if(_evtHandler == null)
            _evtHandler = transform.AddComponent<PointEvtHandler>();
        
        _evtHandler.OnClickDownHandler = m_customAction;
    }
}
