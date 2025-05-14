using UnityEngine;
using UnityEngine.UI;

public class BasePopupCanvas : MonoBehaviour
{
    protected Canvas _canvas;
    protected CanvasScaler _canvasScale;

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

}
