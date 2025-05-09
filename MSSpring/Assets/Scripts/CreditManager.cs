using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreditManager : MonoBehaviour
{
    [SerializeField] List<CreditEntry> cutScenes = new List<CreditEntry>();
    int pageNum = 0;

    Image cutSceneImage;
    TypewriterEffect typewriter;

    float timer = 0;
    float timerPerSlide = 4;

    void Start()
    {
        cutSceneImage = GetComponent<Image>();
        typewriter = transform.GetChild(0).GetComponent<TypewriterEffect>();

        UpdateCredit();
    }

    void Update()
    {
        if (timer >= timerPerSlide)
        {
            timer = 0;
            UpdateCredit();
        }
        else timer += Time.deltaTime;
    }

    public void UpdateCredit()
    {
        pageNum++;
        if (pageNum >= cutScenes.Count)
        {
            EndCredit();
            return;
        }

        cutSceneImage.sprite = cutScenes[pageNum].image;
        typewriter.fullText = cutScenes[pageNum].text;
        typewriter.StartTyping();
    }

    public void EndCredit()
    {
        StartCoroutine(FadeOutCredit());
    }

    IEnumerator FadeOutCredit()
    {
        float time = 0f;
        float duration = 1f;

        cutSceneImage.raycastTarget = false;
        Color originalColor = cutSceneImage.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(1f, 0f, t);
            cutSceneImage.color = newColor;

            yield return null;
        }

        Destroy(gameObject);
    }
}

[System.Serializable]
public class CreditEntry
{
    public Sprite image;
    [TextArea] public string text;
}
