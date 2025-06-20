using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunestoneOfWisdom : Relic
{
    private void Awake()
    {
        Init(_sptrite, "Runestone Of Wisdom", 1, 5, "FeverGaze - 10");
        _descriptions = new string[] { "FeverGaze - 50","FeverGaze - 100","FeverGaze - 150","FeverGaze - 200","FeverGaze - 250" };
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
        int mount = 0;
        switch (_level)
        {
            case 0:
                break;
            case 1:
                PlayerDataManager.Instance.DecreaseMaxFeverFromRelic(10);
                mount = 50;
                break;
            case 2:
                PlayerDataManager.Instance.DecreaseMaxFeverFromRelic(20);
                mount = 100;
                break;
            case 3:
                PlayerDataManager.Instance.DecreaseMaxFeverFromRelic(30);
                mount = 150;
                break;
            case 4:
                PlayerDataManager.Instance.DecreaseMaxFeverFromRelic(40);
                mount = 200;
                break;
            case 5:
                PlayerDataManager.Instance.DecreaseMaxFeverFromRelic(50);
                mount = 250;
                break;
        }
        RelicManager.Instance.OnRelicEffectFever?.Invoke(mount);
    }
}
