using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_SwordPartner : WGH_Partner
{
    protected override void Attack()
    {
        _anim.SetTrigger("isAttack");
        WGH_MonsterManager.Instance.ReceiveHit(E_AttackType.PartnerAttack, E_PartnerCat.SwordCat);
    }
}
