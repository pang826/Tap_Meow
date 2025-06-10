using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageText : MonoBehaviour
{
    private TextMeshProUGUI _tmp;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        _tmp.text = $"STAGE\n{ProgressManager.Instance.GetStage()}";
        MonsterManager.Instance.OnBossDie += () => Invoke(nameof(SetStageText), 0.2f);
    }

    private void SetStageText()
    {
        _tmp.text = $"STAGE\n{ProgressManager.Instance.GetStage()}";
    }

    private void OnDisable()
    {
        MonsterManager.Instance.OnBossDie -= SetStageText;
    }
}
