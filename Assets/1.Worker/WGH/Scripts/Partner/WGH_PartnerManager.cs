using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_PartnerManager : MonoBehaviour
{
    public static WGH_PartnerManager Instance;

    [SerializeField] private WGH_ParsePartner _parser;

    [SerializeField] private List<GameObject> _partnerPrefabs = new List<GameObject>();
    private Dictionary<E_PartnerCat, GameObject> _partnerDic = new Dictionary<E_PartnerCat, GameObject>();
    private Dictionary<E_PartnerCat, WGH_Partner> _spawnParnterDic = new Dictionary<E_PartnerCat, WGH_Partner>();
    [SerializeField] private List<Transform> _spawnPos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _partnerDic[E_PartnerCat.SwordCat] = _partnerPrefabs[0];
        _partnerDic[E_PartnerCat.ArcherCat] = _partnerPrefabs[1];
        _partnerDic[E_PartnerCat.MageCat] = _partnerPrefabs[2];
    }
    public void SpawnPartner(int type)
    {
        WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type);
        if (data == null) { Debug.Log("파트너 데이터가 없습니다"); return; }

        if(_partnerDic.TryGetValue((E_PartnerCat)type, out GameObject obj)) 
        {
            obj = Instantiate(_partnerDic[(E_PartnerCat)type]);
            obj.transform.position = _spawnPos[type - 1].position;
            WGH_Partner partner = obj.GetComponent<WGH_Partner>();
            partner.Init(data.Damage, data.AttackSpeed);
            
            _spawnParnterDic[(E_PartnerCat)type] = partner;
        }
    }
    // 초반 CSV 설정 데미지를 받는 메서드
    public float GetInitPartnerDamage(int type) { WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type); return data.Damage; }
    // 몬스터 이름을 받는 메서드
    public string GetPartnerName(int type) { WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type); return data.Name; }
    public void UpgradeDamage(E_PartnerCat catType)
    {
        if (_spawnParnterDic.TryGetValue(catType, out WGH_Partner partner))
        {
            Debug.Log($"업그레이드{partner.GetDamage()}");
            //partner.UpgradeDamage();
        }
    }
}
