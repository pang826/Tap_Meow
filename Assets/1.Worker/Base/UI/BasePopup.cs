using DG.Tweening;
using UnityEngine;

public abstract class BasePopup : MonoBehaviour
{
    protected AnimatedPopup Anim;

    protected BlockPanel Block => transform.parent.GetComponentInChildren<BlockPanel>();

    protected virtual void Awake()
    {
        FindAnim();
    }

    protected virtual void Start()
    {
        Show();
    }

    private void FindAnim()
    {
        if(TryGetComponent<AnimatedPopup>(out AnimatedPopup m_anim))
        {
            Anim = m_anim;
        }
    }

    /// <summary>
    /// 팝업을 띄웁니다.
    /// </summary>
    public virtual async void Show()
    {
        if (Anim != null)
            await Anim.PlayAnim(E_PopupAnimType.Show)?.AsyncWaitForCompletion();
    }

    /// <summary>
    /// 팝업을 제거합니다.
    /// </summary>
    public virtual async void Dead()
    {
        if (Anim != null)
            await Anim.PlayAnim(E_PopupAnimType.Dead)?.AsyncWaitForCompletion();

        GameObject.Destroy(GetComponentInParent<BasePopupCanvas>().gameObject);
    }
}
