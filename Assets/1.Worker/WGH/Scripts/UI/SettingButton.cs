using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    private Button _button;
    [SerializeField] private GameObject _image;

    bool _isOpen;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(FloatImage);
    }

    private void FloatImage()
    {
        if(_isOpen == false)
        {
            _isOpen = true;
            _image.gameObject.SetActive(true);
        }
        else
        {
            _image.gameObject.SetActive(false);
            _isOpen = false;
        }
    }
}
