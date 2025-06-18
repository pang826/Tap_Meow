using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartnerSaveData
{
    public int Type;
    public float Damage;
    public float AttackSpeed;
    public int Level;
    public long Cost;
}
[Serializable]
public class RelicSaveData
{
    public int Type;
    public int Level;
    public int Mount;
}
[Serializable]
public class GameProgress
{
    public int CurStage;
    public int CurMonsterIndex;

    public long PlayerDmg;
    public float CriticalChance;
    public float CriticalDmgPercent;

    public long Gold;
    public long GoldGainPer;
    public int Fish;
    public int RelicPart;

    public int DamageUpgradePrice;
    public int CriticalChanceUpgradePrice;
    public int CriticalDmgUpgradePrice;
    public int GoldUpgradePrice;

    public int DamageLevel;
    public int CriticalChanceLevel;
    public int CriticalDmgLevel;
    public int GoldLevel;

    public int CurFeverGaze;

    public List<PartnerSaveData> SpawnPartnerList;
    public List<RelicSaveData> SpawnRelicList;

    public long LastQuitTimeTicks;
}
