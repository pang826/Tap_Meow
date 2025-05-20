using UnityEngine;
using UnityEngine.UI;

public class SettingPopup : BasePopup
{
    [SerializeField] private Button _closeBtn;

    protected override void Start()
    {
        base.Start();
        _closeBtn.onClick.AddListener(PopupManager.DeadPopup);
    }

    private void OnDisable()
    {
        _closeBtn.onClick.RemoveAllListeners();
    }
}
