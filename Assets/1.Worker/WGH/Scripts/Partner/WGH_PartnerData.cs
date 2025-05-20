using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_PartnerData
{
    public int Number;
    public string Name;
    public float Damage;
    public float AttackSpeed;
    public WGH_PartnerData(int num, string name, float dmg, float attackSpeed)
    {
        Number = num;
        Name = name;
        Damage = dmg;
        AttackSpeed = attackSpeed;
    }
}
