using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTourScheduleScrollViewContentRectTransformController : MonoBehaviour
{
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (rectTransform.offsetMin.x > 270f)
        {
            rectTransform.SetLeft(270f);
            rectTransform.SetRight(-270f);
        }
        if (rectTransform.offsetMin.x < -290f)
        {
            rectTransform.SetLeft(-290f);
            rectTransform.SetRight(290f);
        }
    }
}

public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}
