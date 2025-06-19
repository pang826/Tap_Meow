using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WGH_FeverGaze : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI _tmp;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        _tmp = GetComponentInChildren<TextMeshProUGUI>();
        slider.value = 0;
    }

    private void Start()
    {
        PlayerDataManager.Instance.OnChangeFeverGaze += IncreaseGaze;
        RelicManager.Instance.OnRelicEffectFever += (mount) => SetMaxFeverGaze(mount);

        _tmp.text = $"{PlayerDataManager.Instance.GetCurFeverGaze()}";
    }

    private void IncreaseGaze()
    {
        slider.value = PlayerDataManager.Instance.GetCurFeverGaze();
        _tmp.text = $"{PlayerDataManager.Instance.GetCurFeverGaze()}";
    }

    private void SetMaxFeverGaze(int mount) { slider.maxValue = 1000 - mount; }

    private void OnDisable()
    {
        PlayerDataManager.Instance.OnChangeFeverGaze -= IncreaseGaze;
        RelicManager.Instance.OnRelicEffectFever -= SetMaxFeverGaze;
    }
}
