using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_SwordPartner : Partner
{
    protected override void Attack()
    {
        _anim.SetTrigger("isAttack");
        MonsterManager.Instance.ReceiveHit(E_AttackType.PartnerAttack, E_PartnerCat.SwordCat);
    }
}
