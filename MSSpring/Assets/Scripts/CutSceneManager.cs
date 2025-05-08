using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    [SerializeField] List<CutSceneEntry> cutScenes = new List<CutSceneEntry>();
    int pageNum = -1;

    Image cutSceneImage;
    TypewriterEffect typewriter;

    bool isTyping = false;

    void Start()
    {
        cutSceneImage = GetComponent<Image>();
        typewriter = transform.GetChild(0).GetComponent<TypewriterEffect>();

        UpdateCutScene();
    }

    public void UpdateCutScene()
    {
        if (typewriter.isDoneTyping)
        {
            isTyping = false;
        }

        if (isTyping)
        {
            typewriter.StopAllCoroutines();
            typewriter.isDoneTyping = true;
            isTyping = false;
            typewriter.GetComponent<TextMeshProUGUI>().text = typewriter.fullText;
            return;
        }
        else
        {
            isTyping = true;

            pageNum++;
            if (pageNum >= cutScenes.Count)
            {
                StartCoroutine(FadeOutCutscene());
                return;
            }

            cutSceneImage.sprite = cutScenes[pageNum].image;
            typewriter.fullText = cutScenes[pageNum].text;
            typewriter.StartTyping();
        }
    }

    IEnumerator FadeOutCutscene()
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
public class CutSceneEntry
{
    public Sprite image;
    [TextArea] public string text;
}
