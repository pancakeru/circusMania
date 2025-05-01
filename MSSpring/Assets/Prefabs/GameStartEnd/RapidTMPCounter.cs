using System.Collections;
using UnityEngine;
using TMPro;

public class RapidTMPCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;
    public int finalValue = 1000;
    [SerializeField] private float duration = 1f;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (tmpText == null)
            tmpText = GetComponent<TMP_Text>();
    }

    public void SetFinalValueAndStart(int value)
    {
        finalValue = value;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(CountUp());
    }

    private IEnumerator CountUp()
    {
        float timer = 0f;
        int current = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);
            current = Mathf.FloorToInt(Mathf.Lerp(0, finalValue, progress));
            tmpText.text = current.ToString();
            yield return null;
        }

        tmpText.text = finalValue.ToString();
    }
}
