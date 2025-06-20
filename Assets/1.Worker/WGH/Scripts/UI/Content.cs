using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Content : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI DescriptionTmp;
    public Button Button;
    public TextMeshProUGUI ButtonTmp;
    public TextMeshProUGUI LevelTmp;

    private void Awake()
    {
        //Image = GetComponentInChildren<Image>();
        //DescriptionTmp = GetComponentInChildren<TextMeshProUGUI>();
        //Button = GetComponentInChildren<Button>();
        //ButtonTmp = Button.GetComponentInChildren<TextMeshProUGUI>();
        //LevelTmp = Image.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void InitPartner(Sprite image, string text, Func<bool> onClickAction, E_PartnerCat catType, long cost)
    {
        Image.sprite = image;
        DescriptionTmp.text = text;
        ButtonTmp.text = $"{cost}";
        Button.onClick.RemoveAllListeners();
        LevelTmp.text = $"LV.{PartnerManager.Instance.PartnerDic[catType].GetComponent<Partner>().GetCurLevel()}";
        UnityAction wrapper = null;
        wrapper = () => 
        { 
            bool isActive = onClickAction.Invoke();
            if (isActive == false) return;

            ButtonTmp.text = $"{PartnerManager.Instance.GetCurCost(catType)}";
            Button.onClick.RemoveListener(wrapper); 
            Button.onClick.AddListener(() =>
            {
                PartnerManager.Instance.UpgradeDamage(catType, ButtonTmp);
                LevelTmp.text = $"LV.{PartnerManager.Instance.PartnerDic[catType].GetComponent<Partner>().GetCurLevel()}";
            }); 
            
        };
        Button.onClick.AddListener(wrapper);
    }

    public void InitPlayerStat(Sprite image, string text, E_Stat statType, long cost)
    {
        if (image != null)
        Image.sprite = image;
        LevelTmp.text = $"LV.{PlayerDataManager.Instance.GetStatLevel(statType)}";
        if (text != null)
        DescriptionTmp.text = text;
        ButtonTmp.text = $"{cost}";
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(() => { 
            PlayerDataManager.Instance.UpgradeStat(statType);
            LevelTmp.text = $"LV. {PlayerDataManager.Instance.GetStatLevel(statType)}";
            ButtonTmp.text = $"{PlayerDataManager.Instance.GetPrice(statType)}"; 
        });
    }

    public void AssignRelic(E_Relic type)
    {
        Relic relic = RelicManager.Instance.GetRelic(type);
        TextMeshProUGUI buttonText = Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Image.sprite = relic.GetSprite();
        DescriptionTmp.text = $"{relic.GetName()}\n{relic.GetDescription(relic.GetLevel())}";
        buttonText.text = $"{relic.GetMount()}\nUpgrade";
        LevelTmp.text = $"LV.{relic.GetLevel()}";
        relic.OnChangeMount += () =>
        {
            buttonText.text = $"{relic.GetMount()}\nUpgrade";
        };
        relic.OnUpgrade += () =>
        {
            LevelTmp.text = $"LV.{relic.GetLevel()}";
            DescriptionTmp.text = $"{relic.GetName()}\n{relic.GetDescription(relic.GetLevel())}";
        };
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(() => {
            relic.Upgrade();
            buttonText.text = $"{relic.GetMount()}\nUpgrade";
        });
    }
}
