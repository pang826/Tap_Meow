using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;
    private string savePath;

    private int _stage = 1;

    public UnityAction OnChangeTheme;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        savePath = Application.persistentDataPath + "/save.json";
    }
    private void Start()
    {
        LoadGame();
        MonsterManager.Instance.OnBossDie += ClearStage;
    }
    public void SaveGame()
    {
        GameProgress data = PlayerDataManager.Instance.ExportProgress();
        data.SpawnPartnerList = PartnerManager.Instance.ExportProgress();
        data.curStage = _stage;

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
            PartnerManager.Instance.LoadProgress(data);
            _stage = data.curStage;
        }
    }
    private void ClearStage() { _stage++; if (_stage % 5 == 1) OnChangeTheme?.Invoke(); }

    public int GetStage() { return _stage; }
    private void OnDisable()
    {
        MonsterManager.Instance.OnBossDie -= ClearStage;
    }
}
