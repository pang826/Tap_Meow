using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI DescriptionTmp;
    public Button Button;
    public TextMeshProUGUI ButtonTmp;

    private void Awake()
    {
        Image = GetComponentInChildren<Image>();
        DescriptionTmp = GetComponentInChildren<TextMeshProUGUI>();
        Button = GetComponentInChildren<Button>();
        ButtonTmp = Button.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void InitPartner(Sprite image, string text, UnityAction onClickAction, E_PartnerCat catType)
    {
        Image.sprite = image;
        DescriptionTmp.text = text;
        Button.onClick.RemoveAllListeners();
        UnityAction wrapper = null;
        wrapper = () => 
        { 
            onClickAction.Invoke(); 
            Button.onClick.RemoveListener(wrapper); 
            Button.onClick.AddListener(() =>
            {
                WGH_PartnerManager.Instance.UpgradeDamage(catType);
            }); 
            
        };
        Button.onClick.AddListener(wrapper);
    }

    public void InitPlayerStat(Sprite image, string text, E_Stat statType)
    {
        if(image != null)
        Image.sprite = image;
        if(text != null)
        DescriptionTmp.text = text;
        Button.onClick.RemoveAllListeners();
        switch(statType)
        {
            case E_Stat.Damage:
                Button.onClick.AddListener(PlayerDataManager.Instance.UpgradePlayerDmg);
                break;
            case E_Stat.CriticalChance:
                Button.onClick.AddListener(PlayerDataManager.Instance.UpgradeCriticalChance);
                break;
            case E_Stat.CriticalDamage:
                Button.onClick.AddListener(PlayerDataManager.Instance.UpgradePlayerCriticalDmg);
                break;
            case E_Stat.GoldGainPer:
                Button.onClick.AddListener(PlayerDataManager.Instance.UpgradeGoldPer);
                break;
        }
        
    }
}
