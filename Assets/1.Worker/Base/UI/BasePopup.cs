using DG.Tweening;
using UnityEngine;

public abstract class BasePopup : MonoBehaviour
{
    protected AnimatedPopup Anim;
    protected BlockPanel Block => transform.parent.GetComponentInChildren<BlockPanel>();

    protected virtual void Awake()
    {
        Anim = GetComponent<AnimatedPopup>();
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

        Destroy(GetComponentInParent<BasePopupCanvas>().gameObject);
    }
}
