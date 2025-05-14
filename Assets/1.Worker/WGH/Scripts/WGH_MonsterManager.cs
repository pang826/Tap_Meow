using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WGH_MonsterManager : MonoBehaviour
{
    public static WGH_MonsterManager Instance;

    [SerializeField] private WGH_ParserMonster _parser;
    [SerializeField] List<GameObject> _monsterPrefabs = new List<GameObject>();
    private Dictionary<string, GameObject> _monDic = new Dictionary<string, GameObject>();

    public UnityAction OnSpawnMonster;              // 몬스터 소환시
    public UnityAction OnDieMonster;                // 몬스터 사망시
    public UnityAction OnBossDie;                   // 보스 몬스터 사망시

    [SerializeField] private Transform _spawnPos;   // 몬스터 소환 위치
    private int _stage;                             // 현재 스테이지

    [Header("현재 몬스터 정보")]
    [SerializeField] private float _curHp;
    [SerializeField] private WGH_Monster _curMonster;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // 리스트의 몬스터 프리팹 딕셔너리에 할당
        foreach(GameObject mon in _monsterPrefabs) { _monDic[mon.name] = mon; }

        OnDieMonster += ClearStage;
        OnDieMonster += () => SpawnMonster(_stage);
        SpawnMonster(_stage);
    }
    /// <summary>
    /// 몬스터 스폰
    /// </summary>
    private void SpawnMonster(int stage)
    {
        WGH_MonsterData monsterData = _parser.monsterDataList.Find(m => m.Stage == stage);
        if (monsterData == null) { Debug.Log("스테이지에 해당하는 몬스터 데이터가 없습니다"); return; }

        if(_monDic.TryGetValue(monsterData.MonType, out GameObject prefab) == false) { Debug.Log("프리팹을 찾을 수 없습니다"); return; }

        GameObject monster = Instantiate(prefab, _spawnPos.position, Quaternion.identity);
        WGH_Monster newMon = monster.GetComponent<WGH_Monster>();


        if(newMon != null) 
        {
            SetMonster(newMon, monsterData.Hp);
            newMon.SetColor(monsterData.MonColor);
        }
            
        // 스폰할 때 이벤트 발동
        OnSpawnMonster?.Invoke();
    }
    /// <summary>
    /// 현재 소환된 몬스터 정보 초기화
    /// </summary>

    private void SetMonster(WGH_Monster monster, float hp)
    {
        _curMonster = monster;
        _curHp = hp;
    }

    public void ReceiveHit(E_AttackType hitType)
    {
        switch (hitType)
        {
            case E_AttackType.Attack:
                _curHp -= WGH_StatManager.Instance.GetPlayerDmg();
                if (_curHp <= 0f)
                {
                    _curMonster?.OnDIe();
                    OnDieMonster?.Invoke();
                    return;
                }
                _curMonster?.TakeDamage();
                break;

            case E_AttackType.Critical:
                _curHp -= WGH_StatManager.Instance.GetCriticalDamage();
                if (_curHp <= 0f)
                {
                    _curMonster?.OnDIe();
                    OnDieMonster?.Invoke();
                    return;
                }
                _curMonster?.TakeDamage();
                break;

            case E_AttackType.DefenseReduction:
                break;
        }
    }
    /// <summary>
    /// 스테이지 클리어 시 동작하는 메서드
    /// </summary>
    private void ClearStage()
    {
        _stage++;
    }

    private void OnDestroy()
    {
        OnDieMonster -= ClearStage;
        OnDieMonster -= () => SpawnMonster(_stage);
    }
}
