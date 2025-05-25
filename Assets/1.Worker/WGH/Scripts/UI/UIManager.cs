using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject ContentPrefab;
    [SerializeField] private List<Sprite> _sptrites;

    [Header("팝업버튼")]
    [SerializeField] private Button _statPopUpButton;
    [SerializeField] private Button _partnerPopUpButton;
    private RectTransform _statRect;
    private RectTransform _partnerRect;
    [Header("위치")]
    private float _topYPos;
    Vector2 _startPos;

    private Transform _partnerContent;

    private bool _isStatPopUp;
    private bool _isPartnerPopUp;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        _statRect = _statPopUpButton.transform.GetChild(0).GetComponent<RectTransform>();
        _partnerRect = _partnerPopUpButton.transform.GetChild(0).GetComponent<RectTransform>();
        _startPos = _statRect.anchoredPosition;

        _partnerContent = _partnerRect.GetChild(0).GetChild(0);
    }

    private void Start()
    {
        _topYPos = 1600f;
        _statPopUpButton.onClick.AddListener(() => MoveScrollViewUp(_statPopUpButton));
        _partnerPopUpButton.onClick.AddListener(() => MoveScrollViewUp(_partnerPopUpButton));

        for(int i = 1; i < (int)E_PartnerCat.MaxCount; i++) 
        {
            int index = i;
            GameObject contentPrefab = Instantiate(ContentPrefab, _partnerContent);
            Content content = contentPrefab.GetComponent<Content>();
            content.Init(() => WGH_PartnerManager.Instance.SpawnPartner(index));
        }
    }

    private void OnDisable()
    {
        _statPopUpButton.onClick.RemoveAllListeners();
        _partnerPopUpButton.onClick.RemoveAllListeners();
    }
    public void MoveScrollViewUp(Button button)
    {
        RectTransform statScrollView = button.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 statPos = _statRect.anchoredPosition;
        Vector2 partnerPos = _partnerRect.anchoredPosition;
        Sequence seq = DOTween.Sequence();
        if(button == _statPopUpButton)      // 스탯 팝업 버튼을 클릭했을 때
        {
            if (_isPartnerPopUp)            // 파트너 팝업이 올라와 있으면 내려가도록
            {
                _isPartnerPopUp = false;
                seq.Append(_partnerRect.DOAnchorPos(partnerPos - new Vector2(0, _topYPos), 0.2f).SetEase(Ease.InCubic));
            }
            if (!_isStatPopUp) {
                _isStatPopUp = true;
                seq.Append(_statRect.DOAnchorPos(statPos + new Vector2(0, _topYPos), 0.5f).SetEase(Ease.OutCubic));
            }
            else {
                _isStatPopUp = false;
                seq.Append(_statRect.DOAnchorPos(statPos - new Vector2(0, _topYPos), 0.2f).SetEase(Ease.InCubic));
            }
        }
        else if(button == _partnerPopUpButton)  // 파트너 팝업 버튼을 클릭했을 때
        {
            if (_isStatPopUp)                   // 스탯 팝업이 올라와 있으면 내려가도록
            {
                _isStatPopUp = false;
                seq.Append(_statRect.DOAnchorPos(statPos - new Vector2(0, _topYPos), 0.2f).SetEase(Ease.InCubic));
            }
            if (!_isPartnerPopUp) {
                _isPartnerPopUp = true;
                seq.Append(_partnerRect.DOAnchorPos(partnerPos + new Vector2(0, _topYPos), 0.5f).SetEase(Ease.OutCubic));
            }
            else {
                _isPartnerPopUp = false;
                seq.Append(_partnerRect.DOAnchorPos(partnerPos - new Vector2(0, _topYPos), 0.2f).SetEase(Ease.InCubic));
            }
        }
    }

    public void SetPartnerPopUpContent()
    {

    }
}
