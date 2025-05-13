using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BasePopup : MonoBehaviour
{
    [Header("ǥ�� ��ġ ����")]
    [SerializeField] private Vector2 _showedPos;
    [SerializeField] private Vector2 _hidedPos;

    [Space(20f), Header("�˾� �ĸ� ����")]
    [SerializeField] private Image _blockImg;
    [SerializeField] private E_BlockOptions _blockOptions;
    [SerializeField] private Color _blockColor;

    protected Canvas _canvas;
    protected CanvasScaler _canvasScale;
    private PointEvtHandler _evtHandler;

    protected virtual void Awake()
    {
        CanvasInit();
        BlockInit();
    }

    protected virtual void CanvasInit()
    {
        _canvas = GetComponent<Canvas>();
        _canvasScale = GetComponent<CanvasScaler>();

        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
        _canvas.worldCamera = Camera.main;
        _canvasScale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
    }

    private void BlockInit()
    {
        if (_blockOptions == E_BlockOptions.None)
        {
            _blockImg.gameObject.SetActive(false);
            return;
        }

        if (_blockOptions.HasFlag(E_BlockOptions.ShowingColor))
        {
            _blockImg.color = _blockColor;
        }
        else
        {
            _blockImg.color = new Color(0, 0, 0, 0);
        }

        if (_blockOptions.HasFlag(E_BlockOptions.BlockPanel) == false)
        {
            _blockImg.raycastTarget = false;
        }

        if (_blockOptions.HasFlag(E_BlockOptions.UseBlockAction))
        {
            _evtHandler = _blockImg.gameObject.AddComponent<PointEvtHandler>();
            _evtHandler.OnClickDownHandler += BlockAction;
        }
    }

    protected virtual void BlockAction(PointerEventData m_evtData) { }

    protected virtual void OnDisable()
    {
        if (_evtHandler != null)
            _evtHandler.OnClickDownHandler -= BlockAction;
    }

    /// <summary>
    /// �˾� ���� ȿ���� �����մϴ�.
    /// </summary>
    public abstract void Show();
    /// <summary>
    /// �˾� ����� ȿ���� �����մϴ�.
    /// </summary>
    public abstract void Hide();
}
