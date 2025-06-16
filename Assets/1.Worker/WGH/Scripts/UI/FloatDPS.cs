using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatDPS : MonoBehaviour
{
    private TextMeshProUGUI _totalDpsTmp;
    private TextMeshProUGUI _partnerDpsTmp;
    private void Awake()
    {
        _totalDpsTmp = transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _partnerDpsTmp = transform.GetChild(1).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        StartCoroutine(FloatRoutine());
    }

    IEnumerator FloatRoutine()
    {
        float maxTime = 1;
        float curTime = 0;
        while (true) 
        {
            curTime += Time.deltaTime;
            if(curTime >= maxTime)
            {
                curTime = 0;
                _totalDpsTmp.text = PlayerDataManager.Instance.GetDPS(true).ToString();
                _partnerDpsTmp.text = PlayerDataManager.Instance.GetDPS().ToString();
            }
            yield return null;
        }
    }
}
