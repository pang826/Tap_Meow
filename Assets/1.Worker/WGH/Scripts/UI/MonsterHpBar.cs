using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    private TextMeshProUGUI _tmp;
    private Slider _slider;
    private void Start()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        _slider = GetComponentInParent<Slider>();
        MonsterManager.Instance.OnSpawnMonster += InitFloatHp;
        MonsterManager.Instance.OnHit += FloatHp;
    }

    private void InitFloatHp()
    {
        _slider.maxValue = MonsterManager.Instance.GetCurMonHp();
        _slider.value = MonsterManager.Instance.GetCurMonHp();
        _tmp.text = $"{MonsterManager.Instance.GetCurMonHp()}";
    }
    private void FloatHp()
    {
        
        _slider.value = MonsterManager.Instance.GetCurMonHp();
        _tmp.text = $"{MonsterManager.Instance.GetCurMonHp()}";
    }

    private void OnDisable()
    {
        MonsterManager.Instance.OnSpawnMonster -= InitFloatHp;
        MonsterManager.Instance.OnHit -= FloatHp;
    }
}
