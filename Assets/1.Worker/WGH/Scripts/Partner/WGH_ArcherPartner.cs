using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_ArcherPartner : Partner
{
    protected override void Attack()
    {
        _anim.SetTrigger("isAttack");
        MonsterManager.Instance.ReceiveHit(E_AttackType.PartnerAttack, E_PartnerCat.ArcherCat);
    }
}
