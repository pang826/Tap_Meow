using UnityEngine;
using UnityEngine.UI;

public class FixBotPopup : MonoBehaviour
{
    public enum E_DetailPopup
    {
        None = -1,
        StatShop,
        PetShop,
        AccShop,
        DDDD
    }

    public static E_DetailPopup CurrentPopupType = E_DetailPopup.None;
    private E_DetailPopup _currentPopupType 
    { 
        get { return CurrentPopupType; }
        set { CurrentPopupType = value; } 
    }

    [SerializeField] private Button _statShopBtn;
    [SerializeField] private Button _petShopBtn;
    [SerializeField] private Button _accShopBtn;

    private BasePopup _slidePopup;

    private void Start()
    {
        RegistEvents();
    }

    private void OnDisable()
    {
        _statShopBtn.onClick.RemoveAllListeners();
        _petShopBtn.onClick.RemoveAllListeners();
        _accShopBtn.onClick.RemoveAllListeners();
    }

    private void RegistEvents()
    {
        _statShopBtn.onClick.AddListener(()=> ActiveSlider(E_DetailPopup.StatShop));
        _petShopBtn.onClick.AddListener(()=> ActiveSlider(E_DetailPopup.PetShop));
        _accShopBtn.onClick.AddListener(()=> ActiveSlider(E_DetailPopup.AccShop));
    }

    private void ActiveSlider(E_DetailPopup m_type)
    {
        if(_currentPopupType == m_type && _currentPopupType != E_DetailPopup.None)
        {
            PopupManager.DeadPopup();
            _slidePopup = null;
            _currentPopupType = E_DetailPopup.None;
        }
        else
        {
            // 기존에 슬라이더가 없었다면
            if(_currentPopupType == E_DetailPopup.None)
            {
                _slidePopup = PopupManager.ShowPopup<BasePopup>("MainSliderPopup");
            }
            else
            {
                SwitchContents(m_type);
            }

            _currentPopupType = m_type;
        }
    }

    /// <summary>
    /// 대상 타입으로 내용 구성을 변경합니다.
    /// </summary>
    private void SwitchContents(E_DetailPopup m_targetType)
    {

    }
}
