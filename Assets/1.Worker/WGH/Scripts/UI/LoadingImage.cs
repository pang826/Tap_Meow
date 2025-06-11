using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImage : MonoBehaviour
{
    private Image _image;
    private Slider _slider;
    [SerializeField] private float _speed;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _slider = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        StartCoroutine(SliderRoutine());
    }

    IEnumerator SliderRoutine()
    {
        while(_slider.value <= 1)
        {
            _slider.value += Time.deltaTime * _speed;
            if(_slider.value >= 1 )
                gameObject.SetActive(false);
            yield return null;
        }
        
    }
}
