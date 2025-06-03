using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    [SerializeField] private WGH_ParserMonster _parser;

    public UnityAction OnHit;                       // 피격시
    public UnityAction OnSpawnMonster;              // 몬스터 소환시
    public UnityAction OnDieMonster;                // 몬스터 사망시
    public UnityAction OnBossDie;                   // 보스 몬스터 사망시

    private Transform _spawnPos;                    // 몬스터 소환 위치
    private int _stage = 1;                         // 현재 스테이지
    private int _monsterIndex = 1;
    public List<MonsterTheme> _monsterThemes;

    [Header("현재 몬스터 정보")]
    private int _baseHp = 10;
    [SerializeField] private long _curHp;
    [SerializeField] private bool _isBoss;
    //[SerializeField] private float _dropGold;
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
        _spawnPos = GameObject.FindGameObjectWithTag("MonsterSpawnPos").transform;
        OnBossDie += ClearStage;
        OnDieMonster += HandleMonsterDefeat;
        StartCoroutine(SpawnRoutine());
    }
    /// <summary>
    /// 몬스터 스폰
    /// </summary>
    private void SpawnMonster(int stage)
    {
        bool isBoss = (_monsterIndex == 10);
        _isBoss = isBoss;

        WGH_MonsterData monsterData = _parser.monsterDataList.Find(m => m.Stage == stage && m.IsBoss == isBoss);
        if (monsterData == null) { Debug.Log("스테이지에 해당하는 몬스터 데이터가 없습니다"); _curMonster = null; return; }

        // 테마 인덱스 계산
        int themeIndex = Mathf.Clamp((stage - 1) / 5, 0, _monsterThemes.Count - 1);
        MonsterTheme theme = _monsterThemes[themeIndex];
        Sprite monSprite = isBoss ? GetRandom(theme.BossMonsters) : GetRandom(theme.NormalMonsters);

        if (_curMonster == null)
        {
            GameObject newMon = Instantiate(_monsterPrefab, _spawnPos.position, Quaternion.identity);
            _curMonster = newMon.GetComponent<WGH_Monster>();
        }
        _curMonster.Init(monSprite);
        if (!_isBoss)
            SetMonster(_curMonster, _baseHp * (int)(_stage * 1.2f));
        else
            SetMonster(_curMonster, _baseHp * (int)(_stage * 5f));
        
        // 스폰할 때 이벤트 발동
        OnSpawnMonster?.Invoke();
    }
    /// <summary>
    /// 최초 소환에 사용
    /// </summary>
    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3);
        SpawnMonster(_stage);
    }
    private void SetMonster(WGH_Monster monster, long hp)
    {
        _curMonster = monster;
        _curHp = hp;
        if (_isBoss) monster.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        else monster.transform.localScale = Vector3.one;
    }

    public void ReceiveHit(E_AttackType hitType, E_PartnerCat catType = E_PartnerCat.None)
    {
        if (_curMonster == null) return;
        switch (hitType)
        {
            case E_AttackType.Attack:
                _curHp -= PlayerDataManager.Instance.GetPlayerDmg();
                break;

            case E_AttackType.Critical:
                _curHp -= (int)PlayerDataManager.Instance.GetCriticalDamage();
                break;

            case E_AttackType.DefenseReduction:
                break;

            case E_AttackType.PartnerAttack:
                _curHp -= (int)WGH_PartnerManager.Instance.GetPartnerDamage(catType);
                break;
        }
        if (_curHp <= 0f)
        {
            _curMonster?.Deactive();
            OnDieMonster?.Invoke();
            return;
        }
        OnHit?.Invoke();
        _curMonster?.TakeDamage();
    }

    private void HandleMonsterDefeat()
    {
        if (_isBoss)
        {
            OnBossDie?.Invoke(); // 스테이지 올라감
            _stage++;
            _monsterIndex = 1;
        }
        else
        {
            _monsterIndex++;
        }

        SpawnMonster(_stage);
    }
    private Sprite GetRandom(List<Sprite> sprites)
    {
        if (sprites == null || sprites.Count == 0) return null;
        return sprites[Random.Range(0, sprites.Count)];
    }

    public long GetCurMonHp() { return _curHp; }
    /// <summary>
    /// 스테이지 클리어 시 동작하는 메서드
    /// </summary>
    private void ClearStage() { _stage++; }

    private void OnDestroy()
    {
        OnBossDie -= ClearStage;
        OnDieMonster -= () => SpawnMonster(_stage);
    }

    public void Init(int stage)
    {
        _stage = stage;
    }
}
