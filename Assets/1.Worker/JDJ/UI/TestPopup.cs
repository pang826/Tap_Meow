using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestPopup : BasePopup
{
    public override void Hide()
    {
        throw new System.NotImplementedException();
    }

    public override void Show()
    {
        throw new System.NotImplementedException();
    }

    protected override void BlockAction(PointerEventData m_evtData)
    {
        Debug.Log("입력 감지");
    }
}
