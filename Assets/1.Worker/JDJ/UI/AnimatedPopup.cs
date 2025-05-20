using DG.Tweening;
using System;
using UnityEngine;

public class AnimatedPopup : MonoBehaviour
{
    [SerializeField, Header("초기 생성 위치")] private Vector2 _initPos;
    [SerializeField, Header("생성 후 이동 될 위치")] private Vector2 _showedPos;
    [SerializeField, Header("종료 시 이동 될 위치")] private Vector2 _deadPos;

    [SerializeField, Header("생성 재생 시간")] private float _showTime = 1f;
    [SerializeField, Header("종료 재생 시간")] private float _deadTime = 1f;

    public bool Isplaying { get; private set; }

    protected RectTransform RT;
    private Sequence _currentSq;
    private Func<Sequence> _showAnimFunc;
    private Func<Sequence> _deadAnimFunc;

    /// <summary>
    /// 오브젝트 생성 시 각 애니메이션을 초기화합니다.
    /// </summary>
    private void Awake()
    {
        RT = (RectTransform)transform;
        _showAnimFunc = SetStandardShowAnim;
        _deadAnimFunc = SetStandardDeadAnim;
    }

    private void OnEnable()
    {
        RT.anchoredPosition = _initPos;
    }

    private Sequence SetStandardShowAnim()
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(RT.DOAnchorPos(_showedPos, _showTime));

        return sq;
    }

    private Sequence SetStandardDeadAnim()
    {
        Sequence sq = DOTween.Sequence();
        sq.Append(RT.DOAnchorPos(_deadPos, _deadTime));

        return sq;
    }

    /// <summary>
    /// 애니메이션 동작을 변경합니다.
    /// </summary>
    public virtual void ChangeAnim(E_PopupAnimType m_type, Func<Sequence> m_customAction)
    {
        switch (m_type)
        {
            case E_PopupAnimType.Show:
                _showAnimFunc = m_customAction;
                break;
            case E_PopupAnimType.Dead:
                _deadAnimFunc = m_customAction;
                break;
        }
    }

    /// <summary>
    /// 애니메이션을 재생합니다.
    /// </summary>
    public virtual Sequence PlayAnim(E_PopupAnimType m_type)
    {
        Isplaying = true;

        _currentSq?.Kill();

        switch (m_type)
        {
            case E_PopupAnimType.Show:
                
                return _currentSq = _showAnimFunc?.Invoke().Play().OnComplete(() => Isplaying = false);

            case E_PopupAnimType.Dead:

                return _currentSq = _deadAnimFunc?.Invoke().Play().OnComplete(() => Isplaying = false);
        }

        throw new Exception("재생 가능한 애니메이션을 잘못설정하였습니다.");
    }

    /// <summary>
    /// 애니메이션의 시간을 설정합니다.
    /// </summary>
    public virtual void SetTime(E_PopupAnimType m_type, float m_time)
    {
        switch (m_type)
        {
            case E_PopupAnimType.Show:
                _showTime = m_time;
                break;

            case E_PopupAnimType.Dead:
                _deadTime = m_time;
                break;
        }
    }

    /// <summary>
    /// 생성 후 이동위치 지점, 삭제 이동위치 지점을 설정합니다.
    /// </summary>
    public virtual void SetPos(E_PopupAnimType m_type, Vector2 m_pos)
    {
        switch (m_type)
        {
            case E_PopupAnimType.Show:
                _showedPos = m_pos;
                break;

            case E_PopupAnimType.Dead:
                _deadPos = m_pos;
                break;
        }
    }
}
