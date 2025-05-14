using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_StatManager : MonoBehaviour
{
    public static WGH_StatManager Instance { get; private set; }

    [Header("수치")]
    [SerializeField] private float _playerDmg;                      // 플레이어 데미지
    [SerializeField] private float _playerUpgradeDmg;               // 강화시 상승하는 데미지
    [SerializeField] private float _goldGainPer;                    // 골드 획득량
    [SerializeField] private float _partnerAttackSpeed;             // 동료 공격 속도

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // 메서드
    public float GetPlayerDmg() { return _playerDmg; }                              // 플레이어 데미지 값을 가져오는 메서드
    public void UpgradePlayerDmg() { _playerDmg += _playerUpgradeDmg; }             // 플레이어 데미지를 증가시키는 메서드(영구적)
    public void ReinforcePlayerDmg(float plusDmg, float time)                       // 플레이어 데미지를 일시적으로 증가시키는 메서드(비영구적, 버프에 사용)
    { StartCoroutine(UpgredePlayerDmgRoutine(plusDmg, time)); }        

    // 코루틴
    IEnumerator UpgredePlayerDmgRoutine(float plusDmg, float time)                  // ReinforcePlayerDmg 메서드 용 코루틴
    {
        _playerDmg += plusDmg;
        yield return new WaitForSeconds(time);
        _playerDmg -= plusDmg;
        yield break;
    }
}
