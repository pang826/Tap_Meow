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
        ProgressManager.Instance.OnChangeTheme += () => Invoke(nameof(SetStageText), 0.2f);
    }

    private void SetStageText()
    {
        _tmp.text = $"STAGE\n{ProgressManager.Instance.GetStage()}";
    }

    private void OnDisable()
    {
        ProgressManager.Instance.OnChangeTheme -= SetStageText;
    }
}
