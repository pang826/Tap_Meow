using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }

    [SerializeField, Header("데미지")] private long _playerDmg;                         // 플레이어 데미지
    
    [SerializeField, Header("크리티컬 확률")] private float _criticalChance;             // 크리티컬 확률
    [SerializeField, Header("크리티컬 데미지")] private float _playerCriticalPer;        // 플레이어 크리티컬 데미지
    [Header("피버 MAX 값 / 현재 피버 게이지")]
    [SerializeField] private int _feverGaze;                                            // 피버가 발동되는 게이지
    [SerializeField] private int _curFeverGaze;                                         // 현재 피버 게이지
    
    [SerializeField,Header("골드 획득량")] private int _goldGainPer;                     // 골드 획득량 비율
    [SerializeField] private int _upgradeGoldPer;                                       // 강화시 상승하는 골드
    [SerializeField] private int _curGold;                                              // 현재 골드(임시로 여기에 만들어놓고 후에 데이터매니저 or 게임매니저로 이동)

    public UnityAction OnChangeFeverGaze;                                               // 현재 피버 게이지 변경 이벤트
    public UnityAction OnMaxFeverGaze;                                                  // 피버게이지를 모두 채웠을 때 호출
    public UnityAction OnEndFeverGaze;                                                  // 피버게이지를 모두 소진했을 때 호출
    public UnityAction OnChangeGold;                                                    // 골드를 획득하거나 소모했을 때 호출
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        _upgradeGoldPer = 1;
    }
    private void Start()
    {
        OnMaxFeverGaze += DecreaseFeverGaze;
    }
    private int _damageUpgradePrice = 5;
    private int _criticalChanceUpgradePrice = 5;
    private int _criticalDamageUpgradePrice = 5;
    private int _goldUpgradePrice = 5;
    // 메서드
    public int GetPrice(E_Stat stat)
    {
        switch(stat)
        {
            case E_Stat.Damage:
                return _damageUpgradePrice;
            case E_Stat.CriticalChance:
                return _criticalChanceUpgradePrice;
            case E_Stat.CriticalDamage:
                return _criticalDamageUpgradePrice;
            case E_Stat.GoldGainPer:
                return _goldUpgradePrice;
            default:
                Debug.Log("아직 GetPrice에 추가하지 않은 목록");
                return 0;
        }
    }
    public void UpgradeStat(E_Stat stat)
    {
        if (_curGold < GetPrice(stat)) return;

        ConsumeGold(GetPrice(stat));
        switch (stat)
        {
            case E_Stat.Damage:
                _playerDmg += _playerDmg;
                _damageUpgradePrice++;
                break;
            case E_Stat.CriticalChance:
                _criticalChance += 0.1f;
                _criticalChanceUpgradePrice++;
                break;
            case E_Stat.CriticalDamage:
                _playerCriticalPer++;
                _criticalDamageUpgradePrice++;
                break;
            case E_Stat.GoldGainPer:
                _goldGainPer += _goldGainPer;
                _goldUpgradePrice++;
                break;
        }
    }
    public long GetPlayerDmg() { return _playerDmg; }                                  // 플레이어 데미지 값을 가져오는 메서드
    public void ReinforcePlayerDmg(int plusDmg, float time)                           // 플레이어 데미지를 일시적으로 증가시키는 메서드(비영구적, 버프에 사용)
    { StartCoroutine(UpgredePlayerDmgRoutine(plusDmg, time)); }
    public float GetCriticalChance() { return _criticalChance / 100f; }                 // 현재 크리티컬 확률 반환 메서드
    public float GetCriticalDamage()                                                    // 현재 크리티컬 데미지 증가율에 따른 데미지 반환 메서드
    {
        float bonusDmg = _playerCriticalPer / 100f;
        return _playerDmg + bonusDmg;
    }
    public int GetFeverGaze() { return  _feverGaze; }                                   // Max 피버 게이지를 반환
    public int GetCurFeverGaze() { return _curFeverGaze; }                              // 현재 피버 게이지를 반환
    public void IncreaseCurFeverGaze()                                                  // 피버 게이지를 증가(일단은 1씩)
    { 
        _curFeverGaze++;
        OnChangeFeverGaze?.Invoke();

        // 피버가 가득차면 이벤트 호출
        if(_curFeverGaze >= _feverGaze)
            OnMaxFeverGaze?.Invoke();
    }
    private void DecreaseFeverGaze() { StartCoroutine(DecreaseFeverGazeRoutine()); }    // 피버게이지 감소
    public float GetGoldPer() { return _goldGainPer; }                                  // 골드 획득량 반환
    public void GainGold()                                                              // 골드 획득
    {
        _curGold += _goldGainPer;
        OnChangeGold?.Invoke();
    }
    public int GetCurGold() { return _curGold; }                                        // 현재 골드 반환
    public void ConsumeGold(int gold) { _curGold -= gold; OnChangeGold?.Invoke(); }
    // 코루틴
    IEnumerator UpgredePlayerDmgRoutine(int plusDmg, float time)                      // ReinforcePlayerDmg 메서드 용 코루틴
    {
        _playerDmg += plusDmg;
        yield return new WaitForSeconds(time);
        _playerDmg -= plusDmg;
        yield break;
    }

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

    private void OnDisable()
    {
        OnMaxFeverGaze -= DecreaseFeverGaze;
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
