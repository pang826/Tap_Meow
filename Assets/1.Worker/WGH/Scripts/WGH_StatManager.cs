using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public class WGH_StatManager : MonoBehaviour
{
    public static WGH_StatManager Instance { get; private set; }

    [Header("데미지/데미지 강화 값")]
    [SerializeField] private float _playerDmg;                                          // 플레이어 데미지
    [SerializeField] private float _UpgradeDmg;                                         // 강화시 상승하는 데미지
    [Header("크확 / 확률 강화 값 / 크뎀 증가율 / 크뎀 증가율 강화 값")]
    [SerializeField] private float _criticalChance;                                     // 크리티컬 확률
    [SerializeField] private float _upgradeCriticalChance;                              // 강화시 상승하는 크리티컬 확률
    [SerializeField] private float _playerCriticalPer;                                  // 플레이어 크리티컬 데미지 증가율
    [SerializeField] private float _upgradeCriticalDmg;                                 // 강화시 상승하는 크리티컬 데미지 증가율
    [Header("피버 MAX 값 / 현재 피버 게이지")]
    [SerializeField] private int _feverGaze;                                            // 피버가 발동되는 게이지
    [SerializeField] private int _curFeverGaze;                                         // 현재 피버 게이지
    [Header("골드 획득량 / 현재 골드 / 동료 공격 속도")]
    [SerializeField] private int _goldGainPer;                                          // 골드 획득량
    [SerializeField] private int _curGold;                                              // 현재 골드(임시로 여기에 만들어놓고 후에 데이터매니저 or 게임매니저로 이동)
    [SerializeField] private float _partnerAttackSpeed;                                 // 동료 공격 속도

    public UnityAction OnChangeFeverGaze;                                               // 현재 피버 게이지 변경 이벤트
    public UnityAction OnMaxFeverGaze;                                                  // 피버게이지를 모두 채웠을 때 호출
    public UnityAction OnEndFeverGaze;                                                  // 피버게이지를 모두 소진했을 때 호출
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        OnMaxFeverGaze += DecreaseFeverGaze;
    }
    // 메서드
    #region // 데미지
    public float GetPlayerDmg() { return _playerDmg; }                                  // 플레이어 데미지 값을 가져오는 메서드
    public void UpgradePlayerDmg() { _playerDmg += _UpgradeDmg; }                       // 플레이어 데미지 강화 메서드(영구적)
    public void ReinforcePlayerDmg(float plusDmg, float time)                           // 플레이어 데미지를 일시적으로 증가시키는 메서드(비영구적, 버프에 사용)
    { StartCoroutine(UpgredePlayerDmgRoutine(plusDmg, time)); }
    #endregion
    #region // 크리티컬
    public float GetCriticalChance() { return _criticalChance / 100f; }                 // 현재 크리티컬 확률 반환 메서드
    public void UpgradeCriticalChance() { _criticalChance += _upgradeCriticalChance; }  // 크리티컬 확률 강화 메서드(영구적)
    public float GetCriticalDamage()                                                    // 현재 크리티컬 데미지 증가율에 따른 데미지 반환 메서드
    {
        float bonusDmg = _playerCriticalPer / 100f;
        return _playerDmg + bonusDmg;
    }
    public void UpgradePlayerCriticalDmg()                                              // 크리티컬 데미지 증가율 강화 메서드(영구적)
    { _playerCriticalPer += _upgradeCriticalDmg; }
    #endregion
    #region // 피버 게이지
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
    #endregion
    public int GetGoldPer() { return _goldGainPer; }                                    // 골드 획득량 반환
    public void GainGold()                                                              // 골드 획득
    {
        _curGold += _goldGainPer;
    }
    public float GetPartnerSpeed() { return _partnerAttackSpeed; }                      // 파트너 공속 반환
    // 코루틴
    IEnumerator UpgredePlayerDmgRoutine(float plusDmg, float time)                      // ReinforcePlayerDmg 메서드 용 코루틴
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
}
