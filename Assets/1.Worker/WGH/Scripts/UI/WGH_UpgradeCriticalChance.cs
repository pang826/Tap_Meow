using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WGH_UpgradeCriticalChance : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(WGH_StatManager.Instance.UpgradeCriticalChance);
    }

    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
