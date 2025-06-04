using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackSetting : MonoBehaviour
{
    public List<Sprite> Sprites;
    private Image _image;
    private void Awake()
    {
        _image = GetComponent<Image>();
    }
    void Start()
    {
        Invoke(nameof(ChangeSprite), 0.05f);
        ProgressManager.Instance.OnChangeTheme += ChangeSprite;
    }

    private void ChangeSprite()
    {
        Debug.Log("맵변경");
        _image.sprite = Sprites[(ProgressManager.Instance.GetStage() - 1) / 5];
    }
    private void OnDisable()
    {
        ProgressManager.Instance.OnChangeTheme -= ChangeSprite;
    }

}
