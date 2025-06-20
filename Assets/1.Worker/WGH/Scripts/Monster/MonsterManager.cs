using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;

    private WGH_ParserMonster _parser;

    public UnityAction OnHit;                       // 피격시
    public UnityAction OnSpawnMonster;              // 몬스터 소환시
    public UnityAction OnDieMonster;                // 몬스터 사망시
    public UnityAction OnBossDie;                   // 보스 몬스터 사망시

    private Transform _spawnPos;                    // 몬스터 소환 위치
    private int _monsterIndex = 1;
    private int _maxMonsterIndex = 10;
    public List<MonsterTheme> _monsterThemes;

    [Header("현재 몬스터 정보")]
    private int _baseHp;
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

        _parser = GameObject.FindGameObjectWithTag("MonsterParser").GetComponent<WGH_ParserMonster>();
    }
    // 최초 소환에 사용
    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3);
        SpawnMonster(ProgressManager.Instance.GetStage());
    }
    private void Start()
    {
        _spawnPos = GameObject.FindGameObjectWithTag("MonsterSpawnPos").transform;
        OnDieMonster += HandleMonsterDefeat;
        _baseHp = ProgressManager.Instance.GetStage() * 16;
        StartCoroutine(SpawnRoutine());

        ProgressManager.Instance.OnChangeTheme += PowBaseHp;
    }
    // 몬스터 스폰 메서드
    private void SpawnMonster(int stage)
    {
        bool isBoss = (_monsterIndex == _maxMonsterIndex);
        _isBoss = isBoss;

        int maxDefinedStage = 25;
        int stageKey = ((stage - 1) % maxDefinedStage) + 1;
        WGH_MonsterData monsterData = _parser.monsterDataList.Find(m => m.Stage == stageKey && m.IsBoss == isBoss);
        if (monsterData == null) { Debug.Log("스테이지에 해당하는 몬스터 데이터가 없습니다"); _curMonster = null; return; }

        // 테마 인덱스 계산(한 싸이클을 모두 돌면 다시 처음 테마부터 반복)
        int themeIndex = ((stage - 1) / 5) % _monsterThemes.Count;
        MonsterTheme theme = _monsterThemes[themeIndex];
        // bool 값으로 판단하여 일반몬스터 / 보스몬스터의 스프라이트 저장
        Sprite monSprite = isBoss ? GetRandom(theme.BossMonsters) : GetRandom(theme.NormalMonsters);
        // 최초의 몬스터 오브젝트 소환
        if (_curMonster == null)
        {
            GameObject newMon = Instantiate(_monsterPrefab, _spawnPos.position, Quaternion.identity);
            _curMonster = newMon.GetComponent<WGH_Monster>();
        }
        // 비활성화 된 몬스터 오브젝트 활성화
        _curMonster.Init(monSprite);
        if (!_isBoss)
            SetMonster(_curMonster, _baseHp * (int)(ProgressManager.Instance.GetStage() * 1.2f));
        else if(stage % 5 == 0 && _isBoss)
            SetMonster(_curMonster, _baseHp * (int)(ProgressManager.Instance.GetStage() * 10f));
        else
            SetMonster(_curMonster, _baseHp * (int)(ProgressManager.Instance.GetStage() * 5f));
        
        // 스폰할 때 이벤트 발동
        OnSpawnMonster?.Invoke();
    }
    
    // 몬스터 소환 시 객체, 체력, 크기 적용 메서드
    private void SetMonster(WGH_Monster monster, long hp)
    {
        _curMonster = monster;
        _curHp = hp;
        if (_isBoss) monster.transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
        else monster.transform.localScale = Vector3.one;
    }
    // 피격 스타일 별 다른 반응(공격하는 모든 객체에서 호출하여 데미지 적용)
    public void ReceiveHit(E_AttackType hitType, E_PartnerCat catType = E_PartnerCat.None)
    {
        if (_curMonster == null || _curHp <= 0) return;
        switch (hitType)
        {
            case E_AttackType.Attack:
                _curHp -= PlayerDataManager.Instance.GetPlayerDmg();
                PlayerDataManager.Instance.RegisterTap();
                DamageTextManager.Instance.ShowDamage(PlayerDataManager.Instance.GetPlayerDmg(), Vector3.zero);
                break;

            case E_AttackType.Critical:
                _curHp -= (int)PlayerDataManager.Instance.GetCriticalDamage();
                PlayerDataManager.Instance.RegisterTap();
                break;

            case E_AttackType.DefenseReduction:
                break;

            case E_AttackType.PartnerAttack:
                _curHp -= (int)PartnerManager.Instance.GetPartnerDamage(catType);
                break;
        }
        OnHit?.Invoke();

        if (_curHp <= 0f)
        {
            _curMonster?.Deactive();
            return;
        }
        
        _curMonster?.TakeDamage();
    }
    // 몬스터 처치 시 보스 여부에 따라 다른 기능 작동 메서드
    private void HandleMonsterDefeat()
    {
        if (_isBoss)
        {
            OnBossDie?.Invoke();    // 스테이지 올라감
            _monsterIndex = 1;      // 몬스터 인덱스 초기화
        }
        else
        {
            _monsterIndex++;
        }
        // 다음 몬스터 소환
        SpawnMonster(ProgressManager.Instance.GetStage());
    }
    private Sprite GetRandom(List<Sprite> sprites)
    {
        if (sprites == null || sprites.Count == 0) return null;
        return sprites[Random.Range(0, sprites.Count)];
    }
    private void PowBaseHp() { _baseHp *= 10; }

    public long GetCurMonHp() { return _curHp; }
    public long GetAvgMonHp() { return _baseHp * (int)(ProgressManager.Instance.GetStage() * 1.2f); }

    private void OnDisable()
    { 
        OnDieMonster -= HandleMonsterDefeat;
        ProgressManager.Instance.OnChangeTheme -= PowBaseHp;
    }

    // 세이브 & 로드 용 메서드
    public int GetCurMonsterIndex() { return _monsterIndex; }
    public int GetMaxMonsterIndex() { return _maxMonsterIndex; }
    public void SetCurMonsterIndex(int index) {  _monsterIndex = index; }
    public void SetMaxMonsterIndex(int index) { _maxMonsterIndex = index; }
}
