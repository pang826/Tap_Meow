using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WGH_Monster : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _render;
    private Coroutine hitCoroutine;

    private void Awake()
    {
        _render = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    public void TakeDamage()
    {
        if (hitCoroutine != null)
            StopCoroutine(hitCoroutine);
        hitCoroutine = StartCoroutine(HitEffect());
    }

    public void Init(Sprite newSprite)
    {
        _render.sprite = newSprite;
        gameObject.SetActive(true);
    }
    public void Deactive()
    {
        PlayerDataManager.Instance.GainGold();
        StartCoroutine(DieEffect());
    }
    IEnumerator HitEffect()
    {
        Color origin = _render.color;
        _render.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _render.color = origin;
    }

    IEnumerator DieEffect()
    {
        float duration = 0.3f;
        float time = 0f;
        Color origin = _render.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, time / duration);
            _render.color = new Color(origin.r, origin.g, origin.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
        MonsterManager.Instance.OnDieMonster?.Invoke();
        _render.color = new Color(origin.r, origin.g, origin.b, 1f);
    }
}
