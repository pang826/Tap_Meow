using UnityEngine;

public class Custom_HorLayout : MonoBehaviour
{
    [SerializeField, Header("컨텐츠간 너비")] private float _space = 10f;
    [SerializeField, Header("컨텐츠 영역 패딩 L,R,T,B 순서")] private Vector4 _padding;
    [SerializeField] private TextAnchor _alignment;



    /// <summary>
    /// 내부 요소 재정렬
    /// </summary>
    public void Refresh()
    {
        // 꺼진 오브젝트는 탐지하지 않음
        RectTransform[] childRects = GetComponentsInChildren<RectTransform>();

        Vector2 pivot = GetPivotFromAlignment();

        for (int i = 0; i < childRects.Length; i++)
        {
            childRects[i].anchorMin = new Vector2(0, 1);
            childRects[i].anchorMax = new Vector2(0, 1);
            childRects[i].pivot = pivot;
        }

        float mainHeight = ((RectTransform)transform).rect.height;
        float y = 0;

        switch (_alignment)
        {
            case TextAnchor.UpperLeft:
            case TextAnchor.UpperCenter:
            case TextAnchor.UpperRight:
                y = -1 * _padding.z;
                break;
            case TextAnchor.MiddleLeft:
            case TextAnchor.MiddleCenter:
            case TextAnchor.MiddleRight:
                y = -1 * (_padding.z - _padding.w + mainHeight) / 2f;
                break;
            case TextAnchor.LowerLeft:
            case TextAnchor.LowerCenter:
            case TextAnchor.LowerRight:
                y = _padding.w;
                break;
        }

        float x = _padding.x;

        for (int i = 0; i < childRects.Length; i++)
        {
            childRects[i].anchoredPosition = new Vector2(x, y);
            x += childRects[i].sizeDelta.x;

            if (i == 0 || i == childRects.Length - 1)
                continue;

            x += _space;
        }
    }

    private Vector2 GetPivotFromAlignment()
    {
        switch (_alignment)
        {
            case TextAnchor.UpperLeft:
                return new Vector2(0, 1);
            case TextAnchor.UpperCenter:
                return new Vector2(0.5f, 1);
            case TextAnchor.UpperRight:
                return new Vector2(1, 1);
            case TextAnchor.MiddleLeft:
                return new Vector2(0, 0.5f);
            case TextAnchor.MiddleCenter:
                return new Vector2(0.5f, 0.5f);
            case TextAnchor.MiddleRight:
                return new Vector2(1, 0.5f);
            case TextAnchor.LowerLeft:
                return new Vector2(0, 0);
            case TextAnchor.LowerCenter:
                return new Vector2(0.5f, 0);
            case TextAnchor.LowerRight:
                return new Vector2(1, 0);
        }
        throw new System.Exception("피벗 반환 오류");
    }

    #region 인스펙터 업데이트

#if UNITY_EDITOR
    private static UnityEditor.EditorApplication.CallbackFunction _refreshCallback;
#endif

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (_refreshCallback == null)
            {
                _refreshCallback = () =>
                {
                    if (this != null)
                        Refresh();
                };
            }

            UnityEditor.EditorApplication.delayCall -= _refreshCallback;
            UnityEditor.EditorApplication.delayCall += _refreshCallback;
        }
#endif
    }

    #endregion
}
