using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_MonsterData
{
    public int Stage;
    public string MonType;
    public float Hp;
    public Color MonColor;
    public bool IsBoss;
    
    public WGH_MonsterData(int stage, string monsterType, float monsterHp, Color monsterColor, bool isBoss)
    {
        Stage = stage;
        MonType = monsterType;
        Hp = monsterHp;
        MonColor = monsterColor;
        IsBoss = isBoss;
    }
}
