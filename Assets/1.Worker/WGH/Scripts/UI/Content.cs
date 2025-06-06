using System;
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

    public void InitPartner(Sprite image, string text, Func<bool> onClickAction, E_PartnerCat catType, long cost)
    {
        Image.sprite = image;
        DescriptionTmp.text = text;
        Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{cost}";
        Button.onClick.RemoveAllListeners();

        // TODO : 돈이 없을 경우 onClickAction이 진행되지 않도록 설정해야 함!!!!
        UnityAction wrapper = null;
        wrapper = () => 
        { 
            bool isActive = onClickAction.Invoke();
            if (isActive == false) return;

            Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{PartnerManager.Instance.GetCurCost(catType)}";
            Button.onClick.RemoveListener(wrapper); 
            Button.onClick.AddListener(() =>
            {
                PartnerManager.Instance.UpgradeDamage(catType, Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
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

    public void SetPartnerText(E_PartnerCat catType)
    {

    }
}
