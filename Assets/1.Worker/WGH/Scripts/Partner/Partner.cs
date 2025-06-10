using System.Collections;
using UnityEngine;

public abstract class Partner : MonoBehaviour
{
    protected float _dmg;
    protected float _attackSpped;
    protected int _level = 1;
    protected long _cost;
    protected int _baseCost;

    protected float _curTime;
    protected float _attackCoolTime;
    protected Animator _anim;
    public void Init(float dmg, float attackSpeed, long cost)
    {
        _dmg = dmg;
        _attackSpped = attackSpeed;
        _attackCoolTime = 1 / attackSpeed;
        _cost = cost;
        _baseCost = (int)cost;
    }
    public void LoadInit(float dmg, float attackSpeed, long cost)
    {
        _dmg = dmg;
        _attackSpped = attackSpeed;
        _attackCoolTime = 1 / attackSpeed;
        _cost = cost;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
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
                Attack();
                _curTime = 0;
            }
            yield return null;
        }
    }

    public virtual float GetDamage()
    { return _dmg; }

    public virtual void UpgradeDamage()
    { 
        _dmg += _dmg;
        _level++;
        IncreaseCost(_level);
    }

    public virtual float GetAttackSpeed()
    {
        return _attackSpped;
    }
    public virtual long GetCurCost() { return _cost; }
    public virtual int GetCurLevel() { return _level; }
    public virtual void IncreaseCost(int level)
    {
        _cost = Mathf.FloorToInt(_baseCost * Mathf.Pow(1.3f, level));
    }
    public virtual void UpgradeAttackSpped(float plusAttackSpped)
    { _attackSpped += plusAttackSpped; }
}
