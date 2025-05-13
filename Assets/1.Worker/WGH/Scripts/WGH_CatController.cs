using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_CatController : MonoBehaviour
{
    private Touch _touch;
    [SerializeField] private float _damage;
    public WGH_Monster Monster;

    [SerializeField] private int _feverGaze;        // 피버가 발동되는 게이지
    [SerializeField] private int _curFeverGaze;     // 현재 피버 게이지

    private void Start()
    {
        
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
    public void Attack()
    {
        Monster.TakeDamage(_damage);
        _curFeverGaze++;
        Debug.Log(Monster.Hp);
    }
}
