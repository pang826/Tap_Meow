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

    public long gold;
    public long goldGainPer;
    public int fish;
    public int relicPart;

    public int damageUpgradePrice;
    public int criticalChanceUpgradePrice;
    public int criticalDmgUpgradePrice;
    public int goldUpgradePrice;

    public int damageLevel;
    public int criticalChanceLevel;
    public int criticalDmgLevel;
    public int goldLevel;

    public int feverGaze;
    public int curFeverGaze;

    public List<PartnerSaveData> SpawnPartnerList;

    public long LastQuitTimeTicks;
}
