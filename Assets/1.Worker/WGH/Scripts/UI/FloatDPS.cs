using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatDPS : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalDpsTmp;
    [SerializeField] private TextMeshProUGUI _partnerDpsTmp;
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
