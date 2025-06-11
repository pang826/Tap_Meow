using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartnerManager : MonoBehaviour
{
    public static PartnerManager Instance;

    [SerializeField] private WGH_ParsePartner _parser;

    [SerializeField] private List<GameObject> _partnerPrefabs = new List<GameObject>();
    private Dictionary<E_PartnerCat, GameObject> _partnerDic = new Dictionary<E_PartnerCat, GameObject>();
    private Dictionary<E_PartnerCat, Partner> _spawnParnterDic = new Dictionary<E_PartnerCat, Partner>();
    [SerializeField] private List<Transform> _spawnPos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        Init();
    }

    private void Init()
    {
        for (int i = 1; i <= _partnerPrefabs.Count; i++) {
            _partnerDic[(E_PartnerCat)i] = _partnerPrefabs[i - 1];
        }
    }
    public bool SpawnPartner(int type)
    {
        WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type);
        if (data == null) { Debug.Log("파트너 데이터가 없습니다"); return false; }

        if(_partnerDic.TryGetValue((E_PartnerCat)type, out GameObject obj) && PlayerDataManager.Instance.GetCurGold() >= GetPartnerCost(type)) 
        {
            obj = Instantiate(_partnerDic[(E_PartnerCat)type]);
            obj.transform.position = _spawnPos[type - 1].position;
            Partner partner = obj.GetComponent<Partner>();
            partner.Init(data.Damage, data.AttackSpeed, data.Cost);
            
            _spawnParnterDic[(E_PartnerCat)type] = partner;
            PlayerDataManager.Instance.ConsumeGold((int)partner.GetCurCost());
            partner.IncreaseCost(partner.GetCurLevel());
            return true;
        }
        return false;
    }
    // 초반 CSV 설정 데미지를 받는 메서드
    //public float GetInitPartnerDamage(int type) { WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type); return data.Damage; }
    // 몬스터 이름을 받는 메서드
    public string GetPartnerName(int type) { WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type); return data.Name; }
    public long GetPartnerCost(int type) { WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type); return data.Cost; }
    public float GetPartnerDamage(E_PartnerCat catType) { return _spawnParnterDic[catType].GetDamage(); }
    public void UpgradeDamage(E_PartnerCat catType, TextMeshProUGUI tmp) 
    { 
        if(PlayerDataManager.Instance.GetCurGold() < GetCurCost(catType)) return;
        _spawnParnterDic[catType].UpgradeDamage();
        PlayerDataManager.Instance.ConsumeGold((int)_spawnParnterDic[catType].GetCurCost());
        tmp.text = $"{GetCurCost(catType)}";
    }
    public long GetCurCost(E_PartnerCat catType) { return _spawnParnterDic[catType].GetCurCost(); }
    public List<PartnerSaveData> ExportProgress()
    {
        List<PartnerSaveData> saveList = new List<PartnerSaveData>();

        foreach(var partner in _spawnParnterDic)
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

                _spawnParnterDic[(E_PartnerCat)partnerData.Type] = partner;
            }
        }
    }
}
