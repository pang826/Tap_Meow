using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WGH_PartnerManager : MonoBehaviour
{
    public static WGH_PartnerManager Instance;

    [SerializeField] private WGH_ParsePartner _parser;

    [SerializeField] private List<GameObject> partnerPrefabs = new List<GameObject>();
    private Dictionary<E_PartnerCat, GameObject> partnerDic = new Dictionary<E_PartnerCat, GameObject>();
    [SerializeField] private List<Transform> spawnPos;

    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        partnerDic[E_PartnerCat.SwordCat] = partnerPrefabs[0];
        partnerDic[E_PartnerCat.ArcherCat] = partnerPrefabs[1];
        partnerDic[E_PartnerCat.MageCat] = partnerPrefabs[2];
    }
    public void SpawnPartner(int type)
    {
        WGH_PartnerData data = _parser.partnerDataList.Find(p => p.Number == type);
        if (data == null) { Debug.Log("파트너 데이터가 없습니다"); return; }

        if(partnerDic.TryGetValue((E_PartnerCat)type, out GameObject obj)) 
        {
            obj = Instantiate(partnerDic[(E_PartnerCat)type]);
            obj.transform.position = spawnPos[type - 1].position;
            WGH_Partner partner = obj.GetComponent<WGH_Partner>();
            partner.Init(data.Damage, data.AttackSpeed);
        }
    }
}
