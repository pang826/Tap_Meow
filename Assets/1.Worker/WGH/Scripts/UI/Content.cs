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
        Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{cost}";
        Button.onClick.RemoveAllListeners();

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

    public void InitPlayerStat(Sprite image, string text, E_Stat statType, long cost)
    {
        TextMeshProUGUI buttonText = Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (image != null)
        Image.sprite = image;
        if(text != null)
        DescriptionTmp.text = text;
        buttonText.text = $"{cost}";
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(() => { 
            PlayerDataManager.Instance.UpgradeStat(statType);
            buttonText.text = $"{PlayerDataManager.Instance.GetPrice(statType)}"; 
        });
    }

    public void AssignRelic(E_Relic type)
    {
        Relic relic = RelicManager.Instance.GetRelic(type);
        TextMeshProUGUI buttonText = Button.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Image.sprite = relic.GetSprite();
        DescriptionTmp.text = $"{relic.GetName()}\n{relic.GetDescription()}";
        buttonText.text = $"{relic.GetMount()}\nUpgrade";
        LevelTmp.text = $"LV.{relic.GetLevel()}";
        relic.OnChangeMount += () =>
        {
            buttonText.text = $"{relic.GetMount()}\nUpgrade";
        };
        relic.OnUpgrade += () =>
        {
            LevelTmp.text = $"LV.{relic.GetLevel()}";
        };
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(() => {
            relic.Upgrade();
            buttonText.text = $"{relic.GetMount()}\nUpgrade";
        });
    }
}
