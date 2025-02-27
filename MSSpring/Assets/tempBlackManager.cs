using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class tempBlackManager : MonoBehaviour
{
    public Image targetImage;  // 需要渐变的 UI Image
    public float fadeDuration = 1.5f;  // 渐变持续时间
    [Range(0, 255)]
    public float targetAlpha = 204;  // 目标透明度 (0-255)

    private void Start()
    {
        //Fade();
    }

    private void Update()
    {
        //Debug.Log(targetImage.color.a);
        
    }
    public void Inital()
    {
        targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, 0);
    }
    public void Fade()
    {
        if (targetImage != null)
        {
            StartCoroutine(FadeToAlpha(targetImage, targetAlpha/255, fadeDuration)); // **转换到 0-1 之间**
        }
        else
        {
            Debug.LogError("tempBlackManager: targetImage 未赋值！");
        }
    }

    private IEnumerator FadeToAlpha(Image img, float targetAlphaNormalized, float duration)
    {
        Debug.Log(targetAlphaNormalized);
        float startAlpha = img.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 计算新的透明度
            float newAlpha = Mathf.Lerp(startAlpha, targetAlphaNormalized, t);
            img.color = new Color(img.color.r, img.color.g, img.color.b, newAlpha);
            Debug.Log(newAlpha);
            yield return null;
        }

        // **确保最终值准确**
        img.color = new Color(img.color.r, img.color.g, img.color.b, targetAlphaNormalized);
    }
}
