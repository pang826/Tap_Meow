using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
using System;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance;
    private string _savePath;

    private int _stage = 1;

    public UnityAction OnChangeTheme;
    public UnityAction OnClearStage;
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
        data.SpawnRelicList = RelicManager.Instance.Save();
        data.CurStage = _stage;
        data.CurMonsterIndex = MonsterManager.Instance.GetCurMonsterIndex();
        
        data.LastQuitTimeTicks = DateTime.Now.Ticks;                        // 앱 종료 시간 기록

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
            PlayerDataManager.Instance.OnChangeGold?.Invoke();              // 현재 골드량 플로팅 역할
            OnChangeTheme?.Invoke();                                        // 현재 스테이지 플로팅 역할
            PartnerManager.Instance.LoadProgress(data);
            RelicManager.Instance.Load(data);

            _stage = data.CurStage;
            MonsterManager.Instance.SetCurMonsterIndex(data.CurMonsterIndex);
            
            if(data.SpawnPartnerList != null)
            GrantIdleReward(data.LastQuitTimeTicks);
        }
    }
    // 원할 때 방치보상 받기(임시로 게임 시작하자마자 적용)
    private void GrantIdleReward(long lastQuitTicks)
    {
        if (lastQuitTicks == 0) return;

        DateTime lastQuitTime = new DateTime(lastQuitTicks);
        TimeSpan idleTime = DateTime.Now - lastQuitTime;

        double maxIdleSeconds = 4 * 3600;
        double actualIdleSeconds = Math.Min(idleTime.TotalSeconds, maxIdleSeconds);

        double dps = PlayerDataManager.Instance.GetDPS();
        double monHp = MonsterManager.Instance.GetAvgMonHp();

        if (monHp <= 0)
        {
            Debug.LogWarning("몬스터 HP가 0 이하입니다. 방치 보상 계산을 생략합니다.");
            return;
        }

        double killsPerSec = dps / monHp;
        double goldPerKill = PlayerDataManager.Instance.GetGoldGainPer();

        double totalKills = killsPerSec * actualIdleSeconds;
        long totalGold = (long)Math.Floor(totalKills * goldPerKill);

        if (totalGold < 0)
        {
            Debug.LogWarning($"[방치보상] 잘못된 totalGold 계산: {totalGold}, 보상 지급 생략");
            return;
        }

        PlayerDataManager.Instance.AddGold(totalGold);
        Debug.Log($"[방치보상] {idleTime.TotalMinutes:F1}분 방치 → 골드 {totalGold} 획득");
    }

    private void ClearStage() 
    { 
        _stage++; 
        if (_stage % 5 == 1) OnChangeTheme?.Invoke();
        OnClearStage?.Invoke();
    }

    public int GetStage() { return _stage; }
    private void OnDisable()
    {
        MonsterManager.Instance.OnBossDie -= ClearStage;
    }
}
