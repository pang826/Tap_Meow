using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class WGH_MonsterManager : MonoBehaviour
{
    public static WGH_MonsterManager Instance;

    [SerializeField] private WGH_ParserMonster _parser;
    [SerializeField] List<Sprite> _monsterSprites = new List<Sprite>();

    public UnityAction OnSpawnMonster;              // 몬스터 소환시
    public UnityAction OnDieMonster;                // 몬스터 사망시
    public UnityAction OnBossDie;                   // 보스 몬스터 사망시

    [SerializeField] private Transform _spawnPos;   // 몬스터 소환 위치
    private int _stage;                             // 현재 스테이지

    [Header("현재 몬스터 정보")]
    [SerializeField] private float _curHp;
    [SerializeField] private WGH_Monster _curMonster;
    [SerializeField] private GameObject _monsterPrefab;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        OnDieMonster += ClearStage;
        OnDieMonster += () => SpawnMonster(_stage);
        StartCoroutine(SpawnRoutine(_stage));
    }
    /// <summary>
    /// 몬스터 스폰
    /// </summary>
    private void SpawnMonster(int stage)
    {
        WGH_MonsterData monsterData = _parser.monsterDataList.Find(m => m.Stage == stage + 1);
        if (monsterData == null) { Debug.Log("스테이지에 해당하는 몬스터 데이터가 없습니다"); return; }
        
        //if(_monDic.TryGetValue(monsterData.MonType, out GameObject prefab) == false) { Debug.Log("프리팹을 찾을 수 없습니다"); return; }
        
        if (_curMonster == null)
        {
            GameObject newMon = Instantiate(_monsterPrefab, _spawnPos.position, Quaternion.identity);
            _curMonster = newMon.GetComponent<WGH_Monster>();
        }
        _curMonster.Init(_monsterSprites[stage], monsterData.MonColor);
        SetMonster(_curMonster, monsterData.Hp);
        
        // 스폰할 때 이벤트 발동
        OnSpawnMonster?.Invoke();
    }
    /// <summary>
    /// 최초 소환에 사용
    /// </summary>
    IEnumerator SpawnRoutine(int stage)
    {
        yield return new WaitForSeconds(1);
        SpawnMonster(stage);
    }
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
                    _curMonster?.Deactive();
                    OnDieMonster?.Invoke();
                    return;
                }
                _curMonster?.TakeDamage();
                break;

            case E_AttackType.Critical:
                _curHp -= WGH_StatManager.Instance.GetCriticalDamage();
                if (_curHp <= 0f)
                {
                    _curMonster?.Deactive();
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
