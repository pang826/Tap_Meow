using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    private Queue<float> _tapTimestamps = new Queue<float>();                           // 플레이어 터치카운트 저장 큐
    private float _averageTimeTap = 1;                                                  // 몇초동안 평균 탭 속도를 측정할 지
    private float _tapTimer = 0f;
    private int _tapCount = 0;
    private double _realTimeTPS = 0;
    [Header("플레이어 능력치")]
    [SerializeField] private long _playerDmg;                                           // 플레이어 데미지
    [SerializeField, Header("크리티컬 확률")] private float _criticalChance;             // 크리티컬 확률
    [SerializeField, Header("크리티컬 데미지")] private float _playerCriticalPer;        // 플레이어 크리티컬 데미지
    
    [Header("피버")]
    private int _maxFeverGaze = 1000;                                                   // 피버가 발동되는 게이지
    private int _curFeverGaze;                                                          // 현재 피버 게이지

    [Header("재화")]
    [SerializeField] private long _goldGainPer;                                         // 골드 획득량
    [SerializeField] private long _curGold;                                             // 현재 골드(임시로 여기에 만들어놓고 후에 데이터매니저 or 게임매니저로 이동)
    private int _fish;                                                                  // 추가 서비스 재화(ex 탭타이탄 보석)
    private int _relicPart;                                                             // 유물 구매에 필요한 유물조각
    
    [Header("강화비용, 레벨")]
    private int _damageUpgradePrice = 5, _criticalChanceUpgradePrice = 5, _criticalDamageUpgradePrice = 5, _goldUpgradePrice = 5;
    private int _dmgLv = 0, _criticalChanceLv = 1, _criticalDmgLv = 1, _goldGainLv = 1;

    public UnityAction OnChangeFeverGaze;                                               // 현재 피버 게이지 변경 이벤트
    public UnityAction OnMaxFeverGaze;                                                  // 피버게이지를 모두 채웠을 때 호출
    public UnityAction OnEndFeverGaze;                                                  // 피버게이지를 모두 소진했을 때 호출
    public UnityAction OnChangeGold;                                                    // 골드를 획득하거나 소모했을 때 호출
    public UnityAction OnChangeFish;                                                    // 생선을 획득하거나 소모했을 때 호출
    public UnityAction OnChangeRelicPart;                                               // 유물파편을 획득하거나 소모했을 때 호출
    public UnityAction OnUpgradeDmg;
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
    private void Update()
    {
        _tapTimer += Time.deltaTime;

        if (_tapTimer >= 1f)
        {
            _realTimeTPS = _tapCount;
            _tapCount = 0;
            _tapTimer = 0;
        }
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
                _dmgLv++;
                _playerDmg = _dmgLv * 2;
                _damageUpgradePrice++;
                OnUpgradeDmg?.Invoke();
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
    public void ReinforceDmgFromRelic(long plusDmg) { _playerDmg += plusDmg; }
    public void DecreaseMaxFeverFromRelic(int mount) { _maxFeverGaze -= mount; }
    // =================== Getter ===================
    public long GetPlayerDmg() => _playerDmg;                                           // 플레이어 데미지 값을 가져오는 메서드
    public int GetPlayerDmgLevel() => _dmgLv;
    public float GetCriticalChance() => _criticalChance / 100f;                         // 현재 크리티컬 확률 반환 메서드
    public float GetCriticalDamage() => _playerDmg + (_playerCriticalPer / 100f);       // 현재 크리티컬 데미지 증가율에 따른 데미지 반환 메서드
    public int GetFeverGaze() => _maxFeverGaze;                                         // Max 피버 게이지를 반환
    public int GetCurFeverGaze() => _curFeverGaze;                                      // 현재 피버 게이지를 반환
    public long GetCurGold() => _curGold;                                               // 현재 골드 반환
    public long GetGoldGainPer() => _goldGainPer;                                       // 현재 골드 획득량 반환
    public int GetCurFish() => _fish;                                                   // 현재 물고기 갯수 반환
    public int GetCurRelicPart() => _relicPart;                                         // 현재 유물파편 갯수 반환
    // =================== 재화 처리 ===================
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
    public void GainFish(int mount)
    {
        _fish += mount;
        OnChangeFish?.Invoke();
    }
    public void GainRelicPart(int mount)
    {
        _relicPart += mount;
        OnChangeRelicPart?.Invoke();
    }
    // =================== 피버 처리 ===================
    public void IncreaseCurFeverGaze()                                                  // 피버 게이지를 증가(일단은 1씩)
    { 
        _curFeverGaze++;
        OnChangeFeverGaze?.Invoke();

        // 피버가 가득차면 이벤트 호출
        if(_curFeverGaze >= _maxFeverGaze)
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
        double tapDPS = includeTap ? _playerDmg * _realTimeTPS : 0;
        double companionDPS = PartnerManager.Instance?.GetTotalPartnerDPS() ?? 0;
        return tapDPS + companionDPS;
    }
    // 탭 입력 시 이 메서드 호출
    public void RegisterTap()
    {
        float currentTime = Time.time;
        _tapTimestamps.Enqueue(currentTime);
        // 오래된 시간 제거
        while (_tapTimestamps.Count > 0 && currentTime - _tapTimestamps.Peek() > _averageTimeTap)
        {
            _tapTimestamps.Dequeue();
        }
        _tapCount++;
    }

    public GameProgress ExportProgress()
    {
        return new GameProgress
        {
            PlayerDmg = _playerDmg, CriticalChance = _criticalChance, CriticalDmgPercent = _playerCriticalPer,
            Gold = _curGold, GoldGainPer = _goldGainPer,Fish = _fish, RelicPart = _relicPart,
            DamageUpgradePrice = _damageUpgradePrice, CriticalChanceUpgradePrice = _criticalChanceUpgradePrice,
            CriticalDmgUpgradePrice = _criticalDamageUpgradePrice,GoldUpgradePrice = _goldUpgradePrice,
            DamageLevel = _dmgLv, CriticalChanceLevel = _criticalChanceLv, CriticalDmgLevel = _criticalDmgLv, GoldLevel = _goldGainLv,
             CurFeverGaze = _curFeverGaze,
        };
    }

    public void LoadProgress(GameProgress data)
    {
        _playerDmg = data.DamageLevel * 2; _criticalChance = data.CriticalChance; _playerCriticalPer = data.CriticalDmgPercent;
        _curGold = data.Gold; _goldGainPer = data.GoldGainPer; _fish = data.Fish; _relicPart = data.RelicPart;
        _damageUpgradePrice = data.DamageUpgradePrice;_criticalChanceUpgradePrice = data.CriticalChanceUpgradePrice;
        _criticalDamageUpgradePrice = data.CriticalDmgUpgradePrice; _goldUpgradePrice = data.GoldUpgradePrice;
        _dmgLv = data.DamageLevel; _criticalChanceLv = data.CriticalChanceLevel; _criticalDmgLv = data.CriticalDmgLevel; _goldGainLv = data.GoldLevel;
        _curFeverGaze = data.CurFeverGaze;
    }
}
