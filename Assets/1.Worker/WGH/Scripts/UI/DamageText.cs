using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TextMeshProUGUI _damageText;
    private CanvasGroup _canvasGroup;
    private void Awake()
    {
        _damageText = GetComponent<TextMeshProUGUI>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(long damage)
    {
        _damageText.text = damage.ToString();
        _canvasGroup.alpha = 1f;
        GetComponent<RectTransform>().localPosition = Vector3.zero;
        StartCoroutine(FadeAndMove());
    }

    private IEnumerator FadeAndMove()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, 1f, 0);
        float duration = 1f;
        float elapsed = 0;

        _canvasGroup.alpha = 1;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            _canvasGroup.alpha = 1 - t;
            yield return null;
        }

        DamageTextManager.Instance.ReturnToPool(gameObject);
    }
}
