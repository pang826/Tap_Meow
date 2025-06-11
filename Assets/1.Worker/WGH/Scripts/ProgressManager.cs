using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;
    private string _savePath;

    private int _stage = 1;

    public UnityAction OnChangeTheme;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _savePath = Application.persistentDataPath + "/save.json";
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
        data.curMonsterIndex = MonsterManager.Instance.GetCurMonsterIndex();

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_savePath, json);
    }

    public void LoadGame()
    {
        if (File.Exists(_savePath))
        {
            string json = File.ReadAllText(_savePath);
            GameProgress data = JsonUtility.FromJson<GameProgress>(json);

            PlayerDataManager.Instance.LoadProgress(data);
            PartnerManager.Instance.LoadProgress(data);
            _stage = data.curStage;
            MonsterManager.Instance.SetCurMonsterIndex(data.curMonsterIndex);
        }

        SoundManager.Instance.PlayBGM(E_BGM.BGM2, 0.2f);
    }
    private void ClearStage() { _stage++; if (_stage % 5 == 1) OnChangeTheme?.Invoke(); }

    public int GetStage() { return _stage; }
    private void OnDisable()
    {
        MonsterManager.Instance.OnBossDie -= ClearStage;
    }
}
