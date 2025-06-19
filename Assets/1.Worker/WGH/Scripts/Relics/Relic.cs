using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Relic : MonoBehaviour
{
    protected Image _image;
    [SerializeField] protected Sprite _sptrite;
    protected string _name;
    protected int[] _price = { 1, 2, 4, 8 };

    [SerializeField] protected int _mount;
    [SerializeField] protected int _level;
    protected int _maxLevel;
    protected string _description;

    public UnityAction OnChangeMount;
    public UnityAction OnUpgrade;
    protected void Start()
    {
        Effect();
    }
    public virtual void Init(Sprite image, string name, int level, int maxLevel, string description)
    {
        _sptrite = image; _name = name; _level = level; _maxLevel = maxLevel; _description = description;
        _image = GetComponent<Image>();
        _image.sprite = _sptrite;
    }
    public virtual void LoadInit(int level, int mount)
    {
        _level = level;
        _mount = mount;
    }
    public abstract void Effect();
    public virtual int GetPrice(int index) => _price[index];
    public virtual int GetLevel() => _level;
    public virtual int GetMount() => _mount;
    public virtual void IncreaseMount() { _mount++; OnChangeMount?.Invoke(); }
    public virtual Sprite GetSprite() { return _sptrite; }
    public virtual string GetName() { return _name; }
    public virtual string GetDescription() { return _description; }
    public virtual void Upgrade()
    {
        // 최대 레벨인지 확인
        if (_level >= _price.Length + 1)
            return;

        int requiredMount = _price[_level - 1];

        // 업그레이드 가능 여부 확인
        if (_mount >= requiredMount)
        {
            _mount -= requiredMount;
            OnChangeMount?.Invoke();
            _level++;
            OnUpgrade?.Invoke();
        }
    }
}
