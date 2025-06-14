using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    [Header("플레이어 능력치")]
    [SerializeField] private long _playerDmg;                                           // 플레이어 데미지
    public double AverageTapsPerSecond = 3;                                             // 플레이어 평균 탭 속도
    [SerializeField, Header("크리티컬 확률")] private float _criticalChance;             // 크리티컬 확률
    [SerializeField, Header("크리티컬 데미지")] private float _playerCriticalPer;        // 플레이어 크리티컬 데미지
    
    [Header("피버")]
    [SerializeField] private int _feverGaze;                                            // 피버가 발동되는 게이지
    [SerializeField] private int _curFeverGaze;                                         // 현재 피버 게이지

    [Header("골드")]
    [SerializeField,Header("골드 획득량")] private long _goldGainPer;                     // 골드 획득량 비율
    [SerializeField] private long _curGold;                                              // 현재 골드(임시로 여기에 만들어놓고 후에 데이터매니저 or 게임매니저로 이동)
    
    [Header("강화비용, 레벨")]
    private int _damageUpgradePrice = 5, _criticalChanceUpgradePrice = 5, _criticalDamageUpgradePrice = 5, _goldUpgradePrice = 5;
    private int _dmgLv = 1, _criticalChanceLv = 1, _criticalDmgLv = 1, _goldGainLv = 1;

    public UnityAction OnChangeFeverGaze;                                               // 현재 피버 게이지 변경 이벤트
    public UnityAction OnMaxFeverGaze;                                                  // 피버게이지를 모두 채웠을 때 호출
    public UnityAction OnEndFeverGaze;                                                  // 피버게이지를 모두 소진했을 때 호출
    public UnityAction OnChangeGold;                                                    // 골드를 획득하거나 소모했을 때 호출
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        OnMaxFeverGaze += DecreaseFeverGaze;
    }
    private void OnDisable()
    {
        OnMaxFeverGaze -= DecreaseFeverGaze;
    }

    // =================== 업그레이드 관련 ===================
    public int GetPrice(E_Stat stat)
    {
        return stat switch
        {
            E_Stat.Damage => _damageUpgradePrice,
            E_Stat.CriticalChance => _criticalChanceUpgradePrice,
            E_Stat.CriticalDamage => _criticalDamageUpgradePrice,
            E_Stat.GoldGainPer => _goldUpgradePrice,
            _ => 0,
        };
    }
    public void UpgradeStat(E_Stat stat)
    {
        if (_curGold < GetPrice(stat)) return;
        ConsumeGold(GetPrice(stat));

        switch (stat)
        {
            case E_Stat.Damage:
                _playerDmg += _playerDmg;
                _dmgLv++;
                _damageUpgradePrice++;
                break;
            case E_Stat.CriticalChance:
                _criticalChance += 0.1f;
                _criticalChanceLv++;
                _criticalChanceUpgradePrice++;
                break;
            case E_Stat.CriticalDamage:
                _playerCriticalPer++;
                _criticalDmgLv++;
                _criticalDamageUpgradePrice++;
                break;
            case E_Stat.GoldGainPer:
                _goldGainPer += _goldGainPer;
                _goldGainLv++;
                _goldUpgradePrice++;
                break;
        }
    }
    // =================== Getter ===================
    public long GetPlayerDmg() => _playerDmg;                                           // 플레이어 데미지 값을 가져오는 메서드
    public float GetCriticalChance() => _criticalChance / 100f;                         // 현재 크리티컬 확률 반환 메서드
    public float GetCriticalDamage() => _playerDmg + (_playerCriticalPer / 100f);       // 현재 크리티컬 데미지 증가율에 따른 데미지 반환 메서드
    public int GetFeverGaze() => _feverGaze;                                            // Max 피버 게이지를 반환
    public int GetCurFeverGaze() => _curFeverGaze;                                      // 현재 피버 게이지를 반환
    public long GetCurGold() => _curGold;                                                // 현재 골드 반환
    public long GetGoldGainPer() => _goldGainPer;                                        // 현재 골드 획득량 반환
    // =================== 골드 처리 ===================
    public void GainGold()                                                              // 골드 획득
    {
        _curGold += _goldGainPer;
        OnChangeGold?.Invoke();
    }
    public void AddGold(long amount)                                                     // 로드 시 사용하는 방치 보상 골드 추가 메서드
    {
        _curGold += amount;
        OnChangeGold?.Invoke();
    }
    public void ConsumeGold(int gold) 
    { 
        _curGold -= gold; 
        OnChangeGold?.Invoke(); 
    }
    // =================== 피버 처리 ===================
    public void IncreaseCurFeverGaze()                                                  // 피버 게이지를 증가(일단은 1씩)
    { 
        _curFeverGaze++;
        OnChangeFeverGaze?.Invoke();

        // 피버가 가득차면 이벤트 호출
        if(_curFeverGaze >= _feverGaze)
            OnMaxFeverGaze?.Invoke();
    }
    private void DecreaseFeverGaze() { StartCoroutine(DecreaseFeverGazeRoutine()); }    // 피버게이지 감소

    IEnumerator DecreaseFeverGazeRoutine()
    {
        WaitForSeconds time = new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(0.5f);
        while(true)
        {
            if (_curFeverGaze <= 0)
            {
                OnEndFeverGaze?.Invoke();
                yield break;
            }
            _curFeverGaze--;
            OnChangeFeverGaze?.Invoke();
            yield return time;
            yield return null;
        }
    }
    // =================== DPS 계산 ===================
    public double GetDPS(bool includeTap = false)
    {
        double tapDPS = includeTap ? _playerDmg * AverageTapsPerSecond : 0;
        double companionDPS = PartnerManager.Instance?.GetTotalPartnerDPS() ?? 0;

        return tapDPS + companionDPS;
    }

    public GameProgress ExportProgress()
    {
        return new GameProgress
        {
            playerDmg = _playerDmg,
            criticalChance = _criticalChance,
            criticalDmgPercent = _playerCriticalPer,
            gold = _curGold,
            goldGainPer = _goldGainPer,
            damageUpgradePrice = _damageUpgradePrice,
            criticalChanceUpgradePrice = _criticalChanceUpgradePrice,
            criticalDmgUpgradePrice = _criticalDamageUpgradePrice,
            goldUpgradePrice = _goldUpgradePrice,
            feverGaze = _feverGaze,
            curFeverGaze = _curFeverGaze,
        };
    }

    public void LoadProgress(GameProgress data)
    {
        _playerDmg = data.playerDmg;
        _criticalChance = data.criticalChance;
        _playerCriticalPer = data.criticalDmgPercent;
        _curGold = data.gold;
        _goldGainPer = data.goldGainPer;
        _damageUpgradePrice = data.damageUpgradePrice;
        _criticalChanceUpgradePrice = data.criticalChanceUpgradePrice;
        _criticalDamageUpgradePrice = data.criticalDmgUpgradePrice;
        _goldUpgradePrice = data.goldUpgradePrice;
        _feverGaze = data.feverGaze;
        _curFeverGaze = data.curFeverGaze;
    }
}
