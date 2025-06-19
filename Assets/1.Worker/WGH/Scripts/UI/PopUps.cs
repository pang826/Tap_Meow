using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUps : MonoBehaviour
{
    private GameObject _contentPrefab;
    [SerializeField] private List<Sprite> _partnerSptrites;
    [SerializeField] private List<Sprite> _statSptrites;
    [SerializeField] private List<Sprite> _relicSptrites;

    private Button _statPopUpButton;
    private Button _partnerPopUpButton;
    private Button _relicPopUpButton;
    private RectTransform _statRect;
    private RectTransform _partnerRect;
    private RectTransform _relicRect;
    
    private float _topYPos;
    Vector2 _startPos;

    private Transform _partnerContent;
    private Transform _statContent;
    private Transform _relicContent;

    private bool _isStatPopUp;
    private bool _isPartnerPopUp;
    private bool _isRelicPopUp;
    private void Awake()
    {
        _contentPrefab = Resources.Load<GameObject>("ContentPrefab");

        _statPopUpButton = transform.GetChild(0).GetComponent<Button>();
        _partnerPopUpButton = transform.GetChild(1).GetComponent<Button>();
        _relicPopUpButton = transform.GetChild(2).GetComponent<Button>();
        _statRect = _statPopUpButton.transform.GetChild(1).GetComponent<RectTransform>();
        _partnerRect = _partnerPopUpButton.transform.GetChild(1).GetComponent<RectTransform>();
        _relicRect = _relicPopUpButton.transform.GetChild(1).GetComponent<RectTransform>();
        _startPos = _statRect.anchoredPosition;

        _partnerContent = _partnerRect.GetChild(0).GetChild(0);
        _statContent = _statRect.GetChild(0).GetChild(0);
        _relicContent = _relicRect.GetChild(0).GetChild(0);
    }

    private void Start()
    {
        _topYPos = 2500f;
        _statPopUpButton.onClick.AddListener(() => MoveScrollViewUp(_statPopUpButton));
        _partnerPopUpButton.onClick.AddListener(() => MoveScrollViewUp(_partnerPopUpButton));
        _relicPopUpButton.onClick.AddListener(() => MoveScrollViewUp(_relicPopUpButton));

        Invoke(nameof(SetStatPopUpContent), 3);
        Invoke(nameof(SetPartnerPopUpContent), 3);
        Invoke(nameof(LoadRelicContent), 3);
        RelicManager.Instance.OnGetRelic += (relic) => SetReclicPopUpContent(relic);
    }

    private void OnDisable()
    {
        _statPopUpButton.onClick.RemoveAllListeners();
        _partnerPopUpButton.onClick.RemoveAllListeners();
        _relicPopUpButton.onClick.RemoveAllListeners();

        RelicManager.Instance.OnGetRelic -= SetReclicPopUpContent;
    }
    public void MoveScrollViewUp(Button button)
    {
        RectTransform statScrollView = button.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 statPos = _statRect.anchoredPosition;
        Vector2 partnerPos = _partnerRect.anchoredPosition;
        Vector2 relicPos = _relicRect.anchoredPosition;
        Sequence seq = DOTween.Sequence();
        if(button == _statPopUpButton)      // 스탯 팝업 버튼을 클릭했을 때
        {
            if (_isPartnerPopUp)            // 파트너 팝업이 올라와 있으면 내려가도록
            {
                _isPartnerPopUp = false;
                seq.Append(_partnerRect.DOAnchorPos(partnerPos - new Vector2(0, _topYPos), 0.2f).SetEase(Ease.InCubic));
            }
            else if(_isRelicPopUp)          // 유물 팝업이 올라와 있으면 내려가도록
            {
                _isRelicPopUp = false;
                seq.Append(_relicRect.DOAnchorPos(relicPos - new Vector2(0, _topYPos), 0.2f).SetEase(Ease.InCubic));
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
            else if (_isRelicPopUp)             // 유물 팝업이 올라와 있으면 내려가도록
            {
                _isRelicPopUp = false;
                seq.Append(_relicRect.DOAnchorPos(relicPos - new Vector2(0, _topYPos), 0.2f).SetEase(Ease.InCubic));
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
        else if(button == _relicPopUpButton) 
        {
            if (_isStatPopUp)                   // 스탯 팝업이 올라와 있으면 내려가도록
            {
                _isStatPopUp = false;
                seq.Append(_statRect.DOAnchorPos(statPos - new Vector2(0, _topYPos), 0.2f).SetEase(Ease.InCubic));
            }
            else if (_isPartnerPopUp)            // 파트너 팝업이 올라와 있으면 내려가도록
            {
                _isPartnerPopUp = false;
                seq.Append(_partnerRect.DOAnchorPos(partnerPos - new Vector2(0, _topYPos), 0.2f).SetEase(Ease.InCubic));
            }
            if (!_isRelicPopUp)
            {
                _isRelicPopUp = true;
                seq.Append(_relicRect.DOAnchorPos(relicPos + new Vector2(0, _topYPos), 0.5f).SetEase(Ease.OutCubic));
            }
            else
            {
                _isRelicPopUp = false;
                seq.Append(_relicRect.DOAnchorPos(relicPos - new Vector2(0, _topYPos), 0.2f).SetEase(Ease.InCubic));
            }
        }
    }

    private void SetPartnerPopUpContent()        // 파트너 팝업 콘텐츠 등록
    {
        for (int i = 1; i < (int)E_PartnerCat.MaxCount; i++)
        {
            int index = i;
            GameObject contentPrefab = Instantiate(_contentPrefab, _partnerContent);
            Content content = contentPrefab.GetComponent<Content>();
            
            content.InitPartner(_partnerSptrites[index - 1], PartnerManager.Instance.GetPartnerName(index), 
                () => PartnerManager.Instance.SpawnPartner(index), (E_PartnerCat)index, PartnerManager.Instance.GetPartnerCost(index));
        }
    }

    private void SetStatPopUpContent()          // 스탯 팝업 콘텐츠 등록
    {
        for(int i = 1; i < (int)E_Stat.MaxCount; i++)
        {
            int index = i;
            GameObject contentPrefab = Instantiate(_contentPrefab, _statContent);
            Content content = contentPrefab.GetComponent<Content>();
            content.InitPlayerStat(_statSptrites[index - 1], $"{(E_Stat)index}", (E_Stat)index, PlayerDataManager.Instance.GetPrice((E_Stat)index));
        }
    }

    private void SetReclicPopUpContent(E_Relic type)        // 유물 팝업 콘텐츠 등록
    {
        GameObject contentPrefab = Instantiate(_contentPrefab, _relicContent);
        Content content = contentPrefab.GetComponent<Content>();
        content.AssignRelic(type);
    }

    private void LoadRelicContent()
    {
        foreach(var relic in RelicManager.Instance.SpawnRelicDic)
        {
            SetReclicPopUpContent(relic.Key);
        }
    }
}
