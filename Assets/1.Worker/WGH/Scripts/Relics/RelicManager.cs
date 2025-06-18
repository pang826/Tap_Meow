using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance;

    public List<GameObject> RelicList;
    public Dictionary<E_Relic, GameObject> RelicDic = new Dictionary<E_Relic, GameObject>();
    public Dictionary<E_Relic, Relic> SpawnRelicDic = new Dictionary<E_Relic, Relic>();

    public UnityAction<E_Relic> OnGetRelic;
    public UnityAction<int> OnRelicEffectFever;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < RelicList.Count; i++)
        {
            RelicDic[(E_Relic)i] = RelicList[i];
        }
    }

    public void PurchaseRelic()
    {
        int randNum = Random.Range(0, RelicList.Count);
        if (SpawnRelicDic.TryGetValue((E_Relic)randNum, out Relic rel) == true)
        {
            rel.IncreaseMount();
            return;
        }
        GameObject obj = Instantiate(RelicList[randNum], transform);
        SpawnRelicDic[(E_Relic)randNum] = obj.GetComponent<Relic>();
        OnGetRelic?.Invoke((E_Relic)randNum);
    }

    public Relic GetRelic(E_Relic relic)
    {
        if (SpawnRelicDic.TryGetValue(relic, out Relic rel) == true)
        {
            return SpawnRelicDic[relic];
        }
        else
        {
            Debug.Log("소유하지 않은 유물");
            return null;
        }
    }
    public List<RelicSaveData> Save()
    {
        List<RelicSaveData> saveList = new List<RelicSaveData>();

        foreach(var relic in SpawnRelicDic) 
        {
            RelicSaveData data = new RelicSaveData
            {
                Type = (int)relic.Key,
                Level = relic.Value.GetLevel(),
                Mount = relic.Value.GetMount()
            };
            saveList.Add(data);
        }
        return saveList;
    }
    public void Load(GameProgress data)
    {
        foreach(var relic in data.SpawnRelicList)
        {
            if(RelicDic.TryGetValue((E_Relic)relic.Type, out GameObject obj))
            {
                GameObject relicObj = Instantiate(obj, transform);
                Relic loadRelic = relicObj.GetComponent<Relic>();
                loadRelic.LoadInit(relic.Level, relic.Mount);
                SpawnRelicDic[(E_Relic)relic.Type] = loadRelic;
            }
        }
    }
}
