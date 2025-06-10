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
public class GameProgress
{
    public int curStage;
    public int curMonsterIndex;

    public long playerDmg;
    public float criticalChance;
    public float criticalDmgPercent;

    public int gold;
    public int goldGainPer;

    public int damageUpgradePrice;
    public int criticalChanceUpgradePrice;
    public int criticalDmgUpgradePrice;
    public int goldUpgradePrice;

    public int feverGaze;
    public int curFeverGaze;

    public List<PartnerSaveData> SpawnPartnerList;
}
