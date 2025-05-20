using UnityEngine;
using UnityEngine.UI;

public class SlidePopup : BasePopup
{
    public static Vector4 ContentsAreaSize;

    private float MidPosY => _rt.sizeDelta.y * _midBreakRatio;
    private float TopPosY => _rt.sizeDelta.y * _topBreakRatio;
    private bool IsTop
    {
        get { return _isTop; }
        set
        {
            _isTop = value;

            if (_isTop == true)
                { _nextPos = new Vector2(0, TopPosY); }
            else
                { _nextPos = new Vector2(0, MidPosY); }
        }
    }


    [SerializeField, Header("슬라이더 정지지점 표기 여부")] private bool _isShowBreakPoint;
    [SerializeField, Header("중간 정지 지점"), Range(0.1f,1f)] private float _midBreakRatio; 
    [SerializeField, Header("상단 정지 지점"), Range(0.1f,1f)] private float _topBreakRatio;

    [Space(40f)]
    [SerializeField, Header("뷰 영역 조절 버튼")] private Button _viewControlBtn;
    [SerializeField, Header("뷰 영역 닫기 버튼")] private Button _closeBtn;

    [Space(40f)]
    [SerializeField, Header("영역 내 컨텐츠 범위")] private RectTransform _contentsArea;
    [SerializeField, Header("영역 내 레이아웃 프리팹")] private GameObject _layoutPrefab;

    private RectTransform _rt;
    private Vector2 _nextPos;
    private bool _isTop;

    private RProp<Vector2> _contenetsPos;
    private GameObject _layoutObject;

    protected override void Awake()
    {
        base.Awake();
        
        _rt = (RectTransform)transform;

        if(ContentsAreaSize == default)
        {
            ContentsAreaSize = new Vector4
               (_contentsArea.offsetMin.x,
                _contentsArea.offsetMin.y,
                -1 * _contentsArea.offsetMax.x,
                -1 * _contentsArea.offsetMax.y);
        }
    }

    protected override void Start()
    {
        _contenetsPos = new RProp<Vector2>(E_ReactiveType.SlidePopupPos);

        _nextPos = new Vector2(0, MidPosY);
        Anim.SetPos(E_PopupAnimType.Show, _nextPos);
        Anim.PlayAnim(E_PopupAnimType.Show);
        RegistEvents();

        Show();
        _layoutObject = Instantiate(_layoutPrefab);
    }

    private void OnDisable()
    {
        _viewControlBtn.onClick.RemoveAllListeners();
        _closeBtn.onClick.RemoveAllListeners();
        Destroy(_layoutObject);
    }

    private void RegistEvents()
    {
        _viewControlBtn.onClick.AddListener(SetNextShowOption);
        _viewControlBtn.onClick.AddListener(Show);
        
        _closeBtn.onClick.AddListener(PopupManager.DeadPopup);
    }

    private void SetNextShowOption()
    {
        IsTop = !IsTop;
        Anim.SetPos(E_PopupAnimType.Show, _nextPos);
    }

    private void Update()
    {
        if(Anim.Isplaying == true)
        {
            _contenetsPos.Value = _contentsArea.position;
        }
    }

    #region 디버깅용

    private Vector3[] _corners = new Vector3[4];

    private void OnDrawGizmos()
    {
        if (_isShowBreakPoint == false)
            return;

        RectTransform rt = transform.parent.GetComponent<RectTransform>();
        rt.GetWorldCorners(_corners);

        //corners [bl] [tl] [tr] [br]

        Vector3 left = _corners[1];
        Vector3 right = _corners[2];

        float height = Vector3.Distance(_corners[1], _corners[0]);

        float midY = _corners[0].y + height * _midBreakRatio;
        float topY = _corners[0].y + height * _topBreakRatio;

        // 중간 지점 라인
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(left.x, midY, 0), new Vector3(right.x, midY, 0));

        // 상단 지점 라인
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(left.x, topY, 0), new Vector3(right.x, topY, 0));
    }

    #endregion
}
