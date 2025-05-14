using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WGH_CatController : MonoBehaviour
{
    private Animator _anim;

    private Touch _touch;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    private void Update()
    {
        // UI가 아닌 화면 터치
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false
            && WGH_StatManager.Instance.GetFeverGaze() > WGH_StatManager.Instance.GetCurFeverGaze())
        {
            _touch = Input.GetTouch(0);
            if(_touch.phase == TouchPhase.Began ) 
            {
                Attack();
            }
        }

        // UI가 아닌 마우스 클릭
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false 
            && WGH_StatManager.Instance.GetFeverGaze() > WGH_StatManager.Instance.GetCurFeverGaze())
            Attack();
    }
    /// <summary>
    /// 공격 메서드(애니메이션, 이펙트, 사운드 등을 호출하고 피버게이지 상승)
    /// </summary>
    public void Attack()
    {
        WGH_MonsterManager.Instance.ReceiveHit(E_AttackType.Attack);
        WGH_StatManager.Instance.IncreaseCurFeverGaze();
        _anim.SetTrigger("isAttack");
    }
}
