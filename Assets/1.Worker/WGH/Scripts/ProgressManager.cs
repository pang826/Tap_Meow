using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProgressManager : MonoBehaviour
{
    private string savePath;

    public int Stage;
    private void Awake()
    {
        savePath = Application.persistentDataPath + "/save.json";
    }
    private void Start()
    {
        LoadGame();
        
        MonsterManager.Instance.OnDieMonster += ClearStage;
    }
    public void SaveGame()
    {
        GameProgress data = PlayerDataManager.Instance.ExportProgress();
        data.SpawnPartnerList = WGH_PartnerManager.Instance.ExportProgress();
        data.curStage = Stage;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameProgress data = JsonUtility.FromJson<GameProgress>(json);

            PlayerDataManager.Instance.LoadProgress(data);
            WGH_PartnerManager.Instance.LoadProgress(data);
            MonsterManager.Instance.Init(data.curStage);
            Stage = data.curStage;
        }
    }

    private void ClearStage() { Stage++; }
}
