using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WGH_MonsterManager : MonoBehaviour
{
    public static WGH_MonsterManager Instance;

    [SerializeField] List<GameObject> _monsterList = new List<GameObject>();
    private Dictionary<int, GameObject> _monDic = new Dictionary<int, GameObject>();

    public UnityAction OnSpawnMonster;              // 몬스터 소환시
    public UnityAction OnDieMonster;                // 몬스터 사망시
    public UnityAction OnBossDie;                   // 보스 몬스터 사망시

    [SerializeField] private Transform _spawnPos;   // 몬스터 소환 위치
    private int _stage;                             // 현재 스테이지
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 리스트의 몬스터 프리팹 딕셔너리에 할당
        for(int i = 0; i < _monsterList.Count; i++) 
        {
            _monDic.Add(i, _monsterList[i]);
        }

        OnDieMonster += ClearStage;
        OnDieMonster += () => SpawnMonster(_stage);
        SpawnMonster(_stage);
    }
    /// <summary>
    /// 몬스터 스폰
    /// </summary>
    private void SpawnMonster(int stage)
    {
        if (_monsterList.Count <= stage) return;    // 보스몬스터 클리어 시 더 이상 소환 X (TODO : 프로토타입 용. 이후 개발시에는 삭제)
        GameObject monster = Instantiate(_monDic[stage], _spawnPos.position, Quaternion.identity);
        if(_stage % 5 == 0 && _stage != 0)
            monster.GetComponent<WGH_Monster>().IsBoss = true;
        OnSpawnMonster?.Invoke();
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
