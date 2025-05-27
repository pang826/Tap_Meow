using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI Tmp;
    public Button Button;

    private void Awake()
    {
        Image = GetComponentInChildren<Image>();
        Tmp = GetComponentInChildren<TextMeshProUGUI>();
        Button = GetComponentInChildren<Button>();
    }

    public void InitPartner(Sprite image, string text, UnityAction onClickAction, E_PartnerCat catType)
    {
        Image.sprite = image;
        Tmp.text = text;
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
        Tmp.text = text;
        Button.onClick.RemoveAllListeners();
        switch(statType)
        {
            case E_Stat.Damage:
                Button.onClick.AddListener(WGH_PlayerDataManager.Instance.UpgradePlayerDmg);
                break;
            case E_Stat.CriticalChance:
                Button.onClick.AddListener(WGH_PlayerDataManager.Instance.UpgradeCriticalChance);
                break;
            case E_Stat.CriticalDamage:
                Button.onClick.AddListener(WGH_PlayerDataManager.Instance.UpgradePlayerCriticalDmg);
                break;
            case E_Stat.GoldGainPer:
                Button.onClick.AddListener(WGH_PlayerDataManager.Instance.UpgradeGoldPer);
                break;
        }
        
    }
}
