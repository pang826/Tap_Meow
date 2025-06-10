using UnityEngine;
using UnityEngine.UI;

public class BasePopupCanvas : MonoBehaviour
{
    protected Canvas _canvas;
    protected CanvasScaler _canvasScale;
    public BasePopup Popup => GetComponentInChildren<BasePopup>(true);

    protected virtual void Awake()
    {
        CanvasInit();
    }

    protected virtual void CanvasInit()
    {
        _canvas = GetComponent<Canvas>();
        _canvasScale = GetComponent<CanvasScaler>();

        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = Camera.main;
        _canvasScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    }

    public void DestoryPopup() 
    {
        Destroy(this.gameObject);
    }


    //private void InitScreenRatio()
    //{
    //    float standardRatio = _canvasScale.referenceResolution.x / _canvasScale.referenceResolution.y;
    //    float currentRatio = (float)Screen.width / (float)Screen.height;

    //    if (currentRatio > standardRatio) _canvasScale.matchWidthOrHeight = 1;
    //    else if (currentRatio < standardRatio) _canvasScale.matchWidthOrHeight = 0;
    //}

    //private void Update()
    //{
    //    InitScreenRatio();
    //}
}
