using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_StatManager : MonoBehaviour
{
    public static WGH_StatManager Instance { get; private set; }

    [Header("수치")]
    [SerializeField] private float _playerDmg;                      // 플레이어 데미지
    [SerializeField] private float _goldGainPer;                    // 골드 획득량
    [SerializeField] private float _partnerAttackSpeed;             // 동료 공격 속도

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    public float GetPlayerDmg()
    {
        return _playerDmg; 
    }
}
