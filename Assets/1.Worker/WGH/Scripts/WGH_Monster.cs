using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_Monster : MonoBehaviour
{
    private Touch _touch;
    [SerializeField] private float _hp;
    public float Hp { get { return _hp; } }

    public bool IsBoss;


    private void Update()
    {
        // 화면 터치
        if (Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                TakeDamage();
            }
        }

        // 마우스 클릭
        if (Input.GetMouseButtonDown(0))
            TakeDamage();
    }
    public void TakeDamage()
    {
        _hp -= WGH_StatManager.Instance.GetPlayerDmg();
        if(_hp <= 0)
        {
            WGH_MonsterManager.Instance.OnDieMonster?.Invoke();
            Destroy(gameObject);
        }
    }
}
