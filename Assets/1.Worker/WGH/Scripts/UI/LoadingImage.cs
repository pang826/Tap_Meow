using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImage : MonoBehaviour
{
    private Image _image;
    private Slider _slider;
    [SerializeField] private float _speed;

    [SerializeField] private Animator _anim;
    private void Awake()
    {
        _image = GetComponent<Image>();
        _slider = GetComponentInChildren<Slider>();
        _anim = GetComponentInChildren<Animator>();
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
            if (_slider.value >= 1)
            {
                SoundManager.Instance.PlayBGM(E_BGM.BGM1, 0.8f);
                gameObject.SetActive(false);
            }
            yield return null;
        }
    }
}
