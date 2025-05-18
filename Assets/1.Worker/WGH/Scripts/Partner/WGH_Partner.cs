using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WGH_Partner : MonoBehaviour
{
    protected float _dmg;
    protected float _attackSpped;

    protected float _curTime;
    protected float _attackCoolTime;
    protected Animator _anim;
    public void Init(float dmg, float attackSpeed)
    {
        _dmg = dmg;
        _attackSpped = attackSpeed;
        _attackCoolTime = 1 / attackSpeed;
    }

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }
    protected abstract void Attack();

    IEnumerator AttackRoutine()
    {
        while (true)
        {
            _curTime += Time.deltaTime;
            if (_curTime >= _attackCoolTime)
            {
                Debug.Log("파트너 공격");
                Attack();
                _curTime = 0;
            }
            yield return null;
        }
    }
}
