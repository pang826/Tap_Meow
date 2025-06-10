using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldFloating : MonoBehaviour
{
    private Sprite _sprite;
    private TextMeshProUGUI _text;
    private void Awake()
    {
        _text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        PlayerDataManager.Instance.OnChangeGold += DisplayCurGold;
        DisplayCurGold();
    }

    private void DisplayCurGold()
    {
        _text.text = $"{PlayerDataManager.Instance.GetCurGold()} G";
    }

    private void OnDisable()
    {
        PlayerDataManager.Instance.OnChangeGold -= DisplayCurGold;
    }
}
