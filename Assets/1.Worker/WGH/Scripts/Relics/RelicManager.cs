using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance;

    public List<GameObject> RelicList;
    public Dictionary<E_Relic, GameObject> RelicDic = new Dictionary<E_Relic, GameObject> ();
    public Dictionary<E_Relic, Relic> SpawnRelicDic = new Dictionary<E_Relic, Relic> ();
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < RelicList.Count; i++)
        {
            RelicDic[(E_Relic)i] = RelicList[i];
        }
    }

    public void GetRelic(int relic)
    {
        if (SpawnRelicDic.TryGetValue((E_Relic)relic, out Relic rel) == true) return;
        GameObject obj = Instantiate(RelicList[relic], transform);
        SpawnRelicDic[(E_Relic)relic] = obj.GetComponent<Relic>();
    }

    public List<RelicSaveData> Save()
    {
        List<RelicSaveData> saveList = new List<RelicSaveData>();

        foreach(var relic in SpawnRelicDic) 
        {
            RelicSaveData data = new RelicSaveData
            {
                Type = (int)relic.Key,
                Level = relic.Value.GetLevel()
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
                loadRelic.LoadInit(relic.Level);
                SpawnRelicDic[(E_Relic)relic.Type] = loadRelic;
            }
        }
    }
}
