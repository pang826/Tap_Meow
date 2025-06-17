using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Relic : MonoBehaviour
{
    protected Image _image;
    [SerializeField] protected Sprite _sptrite;
    protected string _name;
    protected int _price;
    protected int _level;
    protected int _maxLevel;
    protected string _description;
    protected void Start()
    {
        Effect();
    }
    public virtual void Init(Sprite image, string name, int price, int level, int maxLevel, string description)
    {
        _sptrite = image; _name = name; _price = price; _level = level; _maxLevel = maxLevel; _description = description;
        _image = GetComponent<Image>();
        _image.sprite = _sptrite;
    }
    public virtual void LoadInit(int level)
    {
        _level = level;
    }
    public abstract void Effect();
    public virtual int GetLevel() => _level;
}
