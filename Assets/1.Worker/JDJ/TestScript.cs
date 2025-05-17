using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour, IRreceiver
{
    RectTransform rt;

    private void Start()
    {
        rt = (RectTransform)transform;
        ReactiveContainer.RegistReceiver(E_ReactiveType.SlidePopupPos, this);
        ApplyRect();
    }

    private void ApplyRect()
    {
        Vector4 size = SlidePopup.ContentsAreaSize;
        rt.offsetMin = new Vector2(size.x, size.w);
        rt.offsetMax = new Vector2(-1*size.z, -1*size.y);
    }

    public void SendValue(object m_value)
    {
        if (rt == null)
            return;
        rt.position = (Vector2)m_value;
    }
}
