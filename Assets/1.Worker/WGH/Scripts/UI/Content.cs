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

    public void Init(Sprite image, string text, UnityAction onClickAction, E_PartnerCat catType)
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
}
