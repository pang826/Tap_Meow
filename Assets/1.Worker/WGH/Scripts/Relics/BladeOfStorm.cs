using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BladeOfStorm : Relic
{
    private void Awake()
    {
        Init(_sptrite, "Blade Of Storm", 1, 5, "TapDMG + 30%");
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
                PlayerDataManager.Instance.ReinforceDmgFromRelic(Mathf.RoundToInt((PlayerDataManager.Instance.GetPlayerDmgLevel() * 2) * 0.3f));
                _description = "TapDMG + 30%";
                break;
            case 2:
                PlayerDataManager.Instance.ReinforceDmgFromRelic(Mathf.RoundToInt((PlayerDataManager.Instance.GetPlayerDmgLevel() * 2) * 0.4f));
                _description = "TapDMG + 40%";
                break;
            case 3:
                PlayerDataManager.Instance.ReinforceDmgFromRelic(Mathf.RoundToInt((PlayerDataManager.Instance.GetPlayerDmgLevel() * 2) * 0.5f));
                _description = "TapDMG + 50%";
                break;
            case 4:
                PlayerDataManager.Instance.ReinforceDmgFromRelic(Mathf.RoundToInt((PlayerDataManager.Instance.GetPlayerDmgLevel() * 2) * 0.6f));
                _description = "TapDMG + 60%";
                break;
            case 5:
                PlayerDataManager.Instance.ReinforceDmgFromRelic(Mathf.RoundToInt((PlayerDataManager.Instance.GetPlayerDmgLevel() * 2) * 0.7f));
                _description = "TapDMG + 70%";
                break;
        }
    }
}
