using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextManager : MonoBehaviour
{
    public static DamageTextManager Instance;
    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public GameObject damageTextPrefab;
    private Transform _canvasTransform;

    private Queue<GameObject> pool = new Queue<GameObject>();
    private void Start()
    {
        _canvasTransform = GameObject.FindGameObjectWithTag("MainCanvas").transform;
    }
    public void ShowDamage(long damage, Vector3 worldPos)
    {
        GameObject dt = GetFromPool();
        dt.transform.SetParent(_canvasTransform, false);

        // 월드 위치를 스크린 좌표로 변환
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        // 스크린 좌표를 캔버스 로컬 좌표로 변환
        RectTransform canvasRect = _canvasTransform as RectTransform;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, Camera.main, out localPoint);

        dt.GetComponent<RectTransform>().localPosition = localPoint;
        dt.SetActive(true);
        dt.GetComponent<DamageText>().Show(damage);
    }

    private GameObject GetFromPool()
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : Instantiate(damageTextPrefab);

        obj.SetActive(true);

        // 안전하게 alpha 리셋
        var group = obj.GetComponent<CanvasGroup>();
        if (group != null)
            group.alpha = 1f;

        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.transform.SetParent(this.transform);

        var group = obj.GetComponent<CanvasGroup>();
        if (group != null)
            group.alpha = 1f; // 💡 다시 사용할 때 보이도록 설정

        var rect = obj.GetComponent<RectTransform>();
        rect.anchoredPosition = Vector2.zero;
        rect.localScale = Vector3.one;

        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
