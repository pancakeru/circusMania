using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class ImageFadeInGroup : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;

    private List<Image> childImages = new List<Image>();
    private List<Text> childTexts = new List<Text>();
    private List<TMP_Text> childTMPs = new List<TMP_Text>(); 

    private void Start()
    {
        // Collect all components
        childImages.AddRange(GetComponentsInChildren<Image>());
        childTexts.AddRange(GetComponentsInChildren<Text>());
        childTMPs.AddRange(GetComponentsInChildren<TMP_Text>()); 

        // Set initial alpha to 0
        foreach (Image img in childImages)
        {
            Color c = img.color;
            c.a = 0f;
            img.color = c;
        }

        foreach (Text txt in childTexts)
        {
            Color c = txt.color;
            c.a = 0f;
            txt.color = c;
        }

        foreach (TMP_Text tmp in childTMPs)
        {
            Color c = tmp.color;
            c.a = 0f;
            tmp.color = c;
        }

        StartCoroutine(FadeInAll());
    }

    private IEnumerator FadeInAll()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);

            foreach (Image img in childImages)
            {
                Color c = img.color;
                c.a = alpha;
                img.color = c;
            }

            foreach (Text txt in childTexts)
            {
                Color c = txt.color;
                c.a = alpha;
                txt.color = c;
            }

            foreach (TMP_Text tmp in childTMPs)
            {
                Color c = tmp.color;
                c.a = alpha;
                tmp.color = c;
            }

            yield return null;
        }
    }
}
