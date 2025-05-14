using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_MonsterData
{
    public int Stage;
    public string MonsterType;
    public float MonsterHp;
    public Color MonsterColor;
    
    public WGH_MonsterData(int stage, string monsterType, float monsterHp, Color monsterColor)
    {
        Stage = stage;
        MonsterType = monsterType;
        MonsterHp = monsterHp;
        MonsterColor = monsterColor;
    }
}
