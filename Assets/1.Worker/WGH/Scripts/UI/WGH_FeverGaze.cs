using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WGH_FeverGaze : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = 0;
    }

    private void Start()
    {
        WGH_PlayerDataManager.Instance.OnChangeFeverGaze += IncreaseGaze;
    }

    private void IncreaseGaze()
    {
        slider.value = WGH_PlayerDataManager.Instance.GetCurFeverGaze();
    }

    private void OnDisable()
    {
        WGH_PlayerDataManager.Instance.OnChangeFeverGaze -= IncreaseGaze;
    }
}
