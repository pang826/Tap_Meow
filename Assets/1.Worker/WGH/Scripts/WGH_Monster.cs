using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WGH_Monster : MonoBehaviour
{
    private Touch _touch;
    [SerializeField] private float _hp;
    public float Hp { get { return _hp; } }

    public bool IsBoss;


    private void Update()
    {
        // 화면 터치 + UI 클릭이 아닐경우
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                TakeDamage();
            }
        }

        // 마우스 클릭 + UI 클릭이 아닐경우
        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
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
