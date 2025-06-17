using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunestoneOfWisdom : Relic
{
    private void Awake()
    {
        Init(_sptrite, "Runestone Of Wisdom", 1, 1, 5, "FeverGaze - 10");
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
                mount = 10;
                break;
            case 2:
                PlayerDataManager.Instance.DecreaseMaxFeverFromRelic(20);
                mount = 20;
                break;
            case 3:
                PlayerDataManager.Instance.DecreaseMaxFeverFromRelic(30);
                mount = 30;
                break;
            case 4:
                PlayerDataManager.Instance.DecreaseMaxFeverFromRelic(40);
                mount = 40;
                break;
            case 5:
                PlayerDataManager.Instance.DecreaseMaxFeverFromRelic(50);
                mount = 50;
                break;
        }
        RelicManager.Instance.OnRelicEffectFever?.Invoke(mount);
    }
}
