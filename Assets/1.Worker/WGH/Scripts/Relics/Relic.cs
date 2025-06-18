using UnityEngine;
using UnityEngine.UI;

public abstract class Relic : MonoBehaviour
{
    protected Image _image;
    [SerializeField] protected Sprite _sptrite;
    protected string _name;
    protected int _price;

    [SerializeField] protected int _mount;
    [SerializeField] protected int _level;
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
    public virtual void LoadInit(int level, int mount)
    {
        _level = level;
        _mount = mount;
    }
    public abstract void Effect();
    public virtual int GetLevel() => _level;
    public virtual int GetMount() => _mount;
    public virtual void IncreaseMount() { _mount++; }
    public virtual void Upgrade()
    {
        int[] upgradeCosts = { 1, 2, 4, 8 };

        // 최대 레벨인지 확인
        if (_level >= upgradeCosts.Length + 1)
            return;

        int requiredMount = upgradeCosts[_level - 1];

        // 업그레이드 가능 여부 확인
        if (_mount >= requiredMount)
        {
            _mount -= requiredMount;
            _level++;
        }
    }
}
