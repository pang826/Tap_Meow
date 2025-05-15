using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WGH_Monster : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _render;
    private Animator _anim;
    private bool _isBoss;

    private void Awake()
    {
        _render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }
    public void TakeDamage()
    {
        // TODO : 데미지 받을 때 애니메이션, 이펙트, 사운드 등 발동
    }

    public void Init(Sprite newSprite, Color color)
    {
        _render.sprite = newSprite;
        SpriteRenderer render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (render != null)
            render.color = color;
        gameObject.SetActive(true);

    }
    public void SetColor(Color color)
    {
        SpriteRenderer render = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if(render != null)
            render.color = color;
    }
    public void Deactive()
    {
        gameObject.SetActive(false);
    }
    public void OnDIe()
    {
        Destroy(gameObject);
    }
}
