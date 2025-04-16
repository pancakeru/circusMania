using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.03f;
    [TextArea]
    public string fullText;
    private TextMeshProUGUI textComponent;
    public bool isDoneTyping = false; 

    private void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    public void StartTyping()
    {
        StopAllCoroutines(); 
        StartCoroutine(TypeText());
    }

    public IEnumerator TypeText()
    {
        textComponent.text = "";
        string currentText = "";

        bool insideTag = false;

        foreach (char letter in fullText)
        {
            currentText += letter;

            if (letter == '<') insideTag = true;
            if (letter == '>') insideTag = false;

            if (!insideTag)
            {
                textComponent.text = currentText;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        // Ensure final text is fully displayed
        textComponent.text = fullText;
        isDoneTyping = true;
    }
}
