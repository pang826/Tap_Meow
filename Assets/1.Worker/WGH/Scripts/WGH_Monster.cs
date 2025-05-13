using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_Monster : MonoBehaviour
{
    [SerializeField] private float _hp;
    public float Hp { get { return _hp; } }
    private WGH_CatController _cat;

    public bool IsBoss;

    private void OnEnable()
    {
        _cat = GameObject.FindGameObjectWithTag("Player").GetComponent<WGH_CatController>();
        AllowcateMonster();
    }
    public void TakeDamage(float dmg)
    {
        _hp -= dmg;
        if(_hp <= 0)
        {
            WGH_MonsterManager.Instance.OnDieMonster?.Invoke();
            Destroy(gameObject);
        }
    }

    private void AllowcateMonster()
    {
        _cat.Monster = this;
    }
}
