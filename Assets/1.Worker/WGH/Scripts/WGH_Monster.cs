using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WGH_Monster : MonoBehaviour
{
    private Animator _anim;
    private bool _isBoss;

    public void TakeDamage()
    {
        // TODO : 데미지 받을 때 애니메이션, 이펙트, 사운드 등 발동
    }

    public void SetColor(Color color)
    {
        SpriteRenderer render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(render != null)
            render.color = color;
    }

    public void OnDIe()
    {
        Destroy(gameObject);
    }
}
