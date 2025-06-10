using System;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class PointEvtHandler : MonoBehaviour
    , IPointerUpHandler, IPointerDownHandler, IDragHandler, IPointerEnterHandler,
    IPointerExitHandler, IBeginDragHandler, IEndDragHandler
{
    public Image Img { get; private set; }
    public Action<PointerEventData> OnClickDownHandler;
    public Action<PointerEventData> OnClickUpHandler;
    public Action<PointerEventData> OnDragHandler;

    public Action<PointerEventData> OnPointerEnterHandler;
    public Action<PointerEventData> OnPointerExitHandler;

    public Action<PointerEventData> OnDragBeginHandler;
    public Action<PointerEventData> OnDragEndHandler;

    private void Awake()
    {
        Img = GetComponent<Image>();
    }

    private void OnDisable()
    {
        OnClickDownHandler = null;
        OnClickUpHandler = null;
        OnDragHandler = null;

        OnPointerEnterHandler = null;
        OnPointerExitHandler = null;

        OnDragBeginHandler = null;
        OnDragEndHandler = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragHandler?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClickDownHandler?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnClickUpHandler?.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterHandler?.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitHandler?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragEndHandler?.Invoke(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDragBeginHandler?.Invoke(eventData);
    }
}
