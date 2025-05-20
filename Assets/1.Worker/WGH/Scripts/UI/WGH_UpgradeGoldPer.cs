using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WGH_UpgradeGoldPer : MonoBehaviour
{
    Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(WGH_PlayerDataManager.Instance.UpgradeGoldPer);
    }
    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
