using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BladeOfStorm : Relic
{
    private void Awake()
    {
        Init(_sptrite, "Blade Of Storm", 1, 1, 5, "TapDMG + 30%");
    }
    protected void Start()
    {
        base.Start();
        PlayerDataManager.Instance.OnUpgradeDmg += Effect;
    }

    private void OnDisable()
    {
        PlayerDataManager.Instance.OnUpgradeDmg -= Effect;
    }
    public override void Effect()
    {
        switch (_level)
        {
            case 0:
                break;
            case 1:
                PlayerDataManager.Instance.ReinforceDmg(Mathf.RoundToInt((PlayerDataManager.Instance.GetPlayerDmgLevel() * 2) * 0.3f));
                break;
            case 2:
                PlayerDataManager.Instance.ReinforceDmg(Mathf.RoundToInt((PlayerDataManager.Instance.GetPlayerDmgLevel() * 2) * 0.4f));
                break;
            case 3:
                PlayerDataManager.Instance.ReinforceDmg(Mathf.RoundToInt((PlayerDataManager.Instance.GetPlayerDmgLevel() * 2) * 0.5f));
                break;
            case 4:
                PlayerDataManager.Instance.ReinforceDmg(Mathf.RoundToInt((PlayerDataManager.Instance.GetPlayerDmgLevel() * 2) * 0.6f));
                break;
            case 5:
                PlayerDataManager.Instance.ReinforceDmg(Mathf.RoundToInt((PlayerDataManager.Instance.GetPlayerDmgLevel() * 2) * 0.7f));
                break;
        }
    }
}
