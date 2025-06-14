using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartnerManager : MonoBehaviour
{
    public static PartnerManager Instance;

    [SerializeField] private WGH_ParsePartner _parser;                                                          // 파트너 파서

    [SerializeField] private List<GameObject> _partnerPrefabs = new List<GameObject>();                         // 파트너 프리팹
    private Dictionary<E_PartnerCat, GameObject> _partnerDic = new Dictionary<E_PartnerCat, GameObject>();      // 파트너 기본 값 저장
    private Dictionary<E_PartnerCat, Partner> _spawnPartnerDic = new Dictionary<E_PartnerCat, Partner>();       // 소환된 파트너 저장
    [SerializeField] private List<Transform> _spawnPos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Init();
    }
    // 최초 파트너 값 저장
    private void Init()
    {
        for (int i = 1; i <= _partnerPrefabs.Count; i++) {
            _partnerDic[(E_PartnerCat)i] = _partnerPrefabs[i - 1];
        }
    }
    // 파트너 소환
    public bool SpawnPartner(int type)
    {
        // 파서의 파트너 데이터의 넘버와 비교하여 있을 경우 해당 데이터 저장
        WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type);
        if (data == null) { Debug.Log("파트너 데이터가 없습니다"); return false; }
        // 딕셔너리에 해당 타입의 파트너가 있고 현재 코인이 파트너 소환 값보다 클 경우 소환
        if(_partnerDic.TryGetValue((E_PartnerCat)type, out GameObject obj) && PlayerDataManager.Instance.GetCurGold() >= GetPartnerCost(type)) 
        {
            obj = Instantiate(_partnerDic[(E_PartnerCat)type]);
            obj.transform.position = _spawnPos[type - 1].position;
            Partner partner = obj.GetComponent<Partner>();
            partner.Init(data.Damage, data.AttackSpeed, data.Cost);
            
            _spawnPartnerDic[(E_PartnerCat)type] = partner;
            PlayerDataManager.Instance.ConsumeGold((int)partner.GetCurCost());
            partner.IncreaseCost(partner.GetCurLevel());
            return true;
        }
        return false;
    }
    // 초반 CSV 설정 데미지를 받는 메서드
    //public float GetInitPartnerDamage(int type) { WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type); return data.Damage; }
    // 파트너 이름을 받는 메서드
    public string GetPartnerName(int type) { WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type); return data.Name; }
    // 파트너 소환 가격을 받는 메서드
    public long GetPartnerCost(int type) { WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type); return data.Cost; }
    // 소환된 파트너의 현재 데미지 반환 메서드
    public float GetPartnerDamage(E_PartnerCat catType) { return _spawnPartnerDic[catType].GetDamage(); }
    // 소환된 파트너의 데미지 강화 메서드 + 버튼 문구 초기화
    public void UpgradeDamage(E_PartnerCat catType, TextMeshProUGUI tmp) 
    { 
        if(PlayerDataManager.Instance.GetCurGold() < GetCurCost(catType)) return;
        _spawnPartnerDic[catType].UpgradeDamage();
        PlayerDataManager.Instance.ConsumeGold((int)_spawnPartnerDic[catType].GetCurCost());
        tmp.text = $"{GetCurCost(catType)}";
    }
    // 소환된 파트너의 강화에 필요한 값 반환 메서드
    public long GetCurCost(E_PartnerCat catType) { return _spawnPartnerDic[catType].GetCurCost(); }
    // 저장에 사용하는 메서드
    public List<PartnerSaveData> ExportProgress()
    {
        List<PartnerSaveData> saveList = new List<PartnerSaveData>();

        foreach(var partner in _spawnPartnerDic)
        {
            PartnerSaveData data = new PartnerSaveData
            {
                Type = (int)partner.Key,
                Damage = partner.Value.GetDamage(),
                AttackSpeed = partner.Value.GetAttackSpeed(),
                Cost = partner.Value.GetCurCost()
            };
            saveList.Add(data);
        }
        return saveList;
    }
    // 로드에 사용하는 메서드
    public void LoadProgress(GameProgress data)
    {
        foreach(var partnerData in data.SpawnPartnerList)
        {
            if(_partnerDic.TryGetValue((E_PartnerCat)partnerData.Type, out GameObject prefab))
            {
                GameObject obj = Instantiate(prefab);
                obj.transform.position = _spawnPos[partnerData.Type - 1].position;
                Partner partner = obj.GetComponent<Partner>();
                partner.LoadInit(partnerData.Damage, partnerData.AttackSpeed, partnerData.Cost);

                _spawnPartnerDic[(E_PartnerCat)partnerData.Type] = partner;
            }
        }
    }
    // 모든 소환된 파트너의 총 DPS를 반환하는 메서드
    public float GetTotalPartnerDPS()
    {
        float totalDPS = 0f;

        foreach (var partner in _spawnPartnerDic.Values)
        {
            float damage = partner.GetDamage();
            float attackSpeed = partner.GetAttackSpeed();

            if (attackSpeed > 0)
                totalDPS += damage / attackSpeed;
        }

        return totalDPS;
    }
}
