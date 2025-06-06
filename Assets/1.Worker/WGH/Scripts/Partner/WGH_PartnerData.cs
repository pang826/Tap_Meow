using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_PartnerData
{
    public int Number;
    public string Name;
    public float Damage;
    public float AttackSpeed;
    public long Cost;
    public WGH_PartnerData(int num, string name, float dmg, float attackSpeed, long cost)
    {
        Number = num;
        Name = name;
        Damage = dmg;
        AttackSpeed = attackSpeed;
        Cost = cost;
    }
}
