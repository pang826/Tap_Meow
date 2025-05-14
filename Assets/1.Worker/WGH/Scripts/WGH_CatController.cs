using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_CatController : MonoBehaviour
{
    private Animator anim;

    private Touch _touch;

    [SerializeField] private int _feverGaze;        // 피버가 발동되는 게이지
    [SerializeField] private int _curFeverGaze;     // 현재 피버 게이지

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        // 화면 터치
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            if(_touch.phase == TouchPhase.Began ) 
            {
                Attack();
            }
        }

        // 마우스 클릭
        if (Input.GetMouseButtonDown(0))
            Attack();
    }
    /// <summary>
    /// 공격 메서드(애니메이션, 이펙트, 사운드 등을 호출하고 피버게이지 상승)
    /// </summary>
    public void Attack()
    {
        _curFeverGaze++;
        anim.SetTrigger("isAttack");
    }
}
