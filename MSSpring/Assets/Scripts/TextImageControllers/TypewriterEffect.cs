using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.05f;  // 控制每个字符出现的间隔时间
    public string fullText;            // 你想显示的完整文本
    private TextMeshProUGUI textComponent;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(TypeText());
    }

    public IEnumerator TypeText()
    {
        textComponent.text = ""; // 清空初始文本
        foreach (char letter in fullText)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
