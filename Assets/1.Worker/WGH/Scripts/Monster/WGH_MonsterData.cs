using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_MonsterData
{
    public int Stage;
    public string MonType;
    public bool IsBoss;
    public string Theme;
    
    public WGH_MonsterData(int stage, string monsterType, bool isBoss, string theme)
    {
        Stage = stage;
        MonType = monsterType;
        IsBoss = isBoss;
        Theme = theme;
    }
}
