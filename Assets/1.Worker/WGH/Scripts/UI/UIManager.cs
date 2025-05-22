using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("팝업버튼")]
    [SerializeField] private Button _statPopUpButton;
    [SerializeField] private Button _partnerPopUpButton;
    [Header("위치")]
    [SerializeField] private float _topYPos;
    [SerializeField] private Vector2 _botYPos;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _statPopUpButton.onClick.AddListener(MoveScrollViewUp);
    }

    private void OnDisable()
    {
        _statPopUpButton.onClick.RemoveAllListeners();
    }
    public void MoveScrollViewUp()
    {
        RectTransform statScrollView = _statPopUpButton.transform.GetChild(0).GetComponent<RectTransform>();
        Vector2 originalPos = statScrollView.anchoredPosition;

        Sequence seq = DOTween.Sequence();
        seq.Append(statScrollView.DOAnchorPos(originalPos + new Vector2(0, 200f), 0.5f).SetEase(Ease.OutCubic));
    }
}
