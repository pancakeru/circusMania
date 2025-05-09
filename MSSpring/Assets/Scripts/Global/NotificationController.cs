using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class NotificationController : MonoBehaviour
{
    public Image ballImage;
    RectTransform rectTransform;
    Vector2 targetAnchoredPos = new Vector2 (510, 350);

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(960, 350);

        StartCoroutine(UIAnimation());
    }

    IEnumerator UIAnimation()
    {
        Time.timeScale = 1;
        float speed = 1000f;

        while (rectTransform.anchoredPosition.x > targetAnchoredPos.x)
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, targetAnchoredPos, speed * Time.deltaTime);
            yield return null;
        }

        rectTransform.anchoredPosition = targetAnchoredPos;
        Destroy(gameObject, 3f);
    }
}
