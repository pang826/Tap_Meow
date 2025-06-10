using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedTopPopup : MonoBehaviour
{
    [SerializeField] private Button _settingBtn;

    private void Start()
    {
        RegistEvt();
    }

    private void RegistEvt()
    {
        _settingBtn.onClick.AddListener(ShowSettingPopup);
    }

    private void ShowSettingPopup()
    {
        PopupManager.ShowPopup("SettingPopup");
    }
}
