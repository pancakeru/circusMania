using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    public bool willFadeOut = true;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float visibleDuration = 1f;
    public float staggerDelay = 0.5f;

    private List<Image> images = new List<Image>();

    private void Awake()
    {
        images.AddRange(GetComponentsInChildren<Image>());
    }

    private void OnEnable()
    {
        StartCoroutine(FadeImagesSequence());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator FadeImagesSequence()
    {
        for (int i = 0; i < images.Count; i++)
        {
            StartCoroutine(FadeIn(images[i]));
            yield return new WaitForSeconds(staggerDelay);
        }
    }

    IEnumerator FadeIn(Image img)
    {
        SetAlpha(img, 0f);

        // Fade In
        float t = 0f;
        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeInDuration);
            SetAlpha(img, alpha);
            yield return null;
        }

        // Stay visible
        yield return new WaitForSeconds(visibleDuration);

        // Fade Out
        if (willFadeOut)
        {
            StartCoroutine(FadeOut(img));
        }
    }

    private IEnumerator FadeOut(Image img)
    {
        float t = 0f;
        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(t / fadeOutDuration);
            SetAlpha(img, alpha);
            yield return null;
        }
    }

    void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}
