using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ProgressManager : MonoBehaviour
{
    private string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/save.json";

        // 시작할 때 불러오기
        //LoadGame();
    }

    public void SaveGame()
    {
        GameProgress data = PlayerDataManager.Instance.ExportProgress();
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
        }
    }
}
