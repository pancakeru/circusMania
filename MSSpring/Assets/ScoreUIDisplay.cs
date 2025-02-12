using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreUIDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI yellowText;
    [SerializeField]
    private TextMeshProUGUI lastText;
    [SerializeField]
    private TextMeshProUGUI redText;
    [SerializeField]
    private TextMeshProUGUI blueText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField]
    private float changeDuration;

    [Header("For Test")]
    [SerializeField]
    TestAction nextTestAction;
    [SerializeField]
    bool ifTest;
    [SerializeField]
    int changeTo;

    private Coroutine yellowCoroutine;
    private Coroutine lastCoroutine;
    private Coroutine redCoroutine;
    private Coroutine blueCoroutine;
    private Coroutine totalScoreCoroutine;

    private void Update()
    {
        if (ifTest)
        {
            ifTest = false;
            switch (nextTestAction)
            {
                case TestAction.ChangeYellow:
                    UpdateYellowScore(changeTo);
                    break;

                case TestAction.ChangeLast:
                    UpdateLastScore(changeTo);
                    break;

                case TestAction.ChangeRed:
                    UpdateRedScore(changeTo);
                    break;

                case TestAction.ChangeBlue:
                    UpdateBlueScore(changeTo);
                    break;

                case TestAction.ChangeTotalScore: // 新增总分测试
                    UpdateTotalScore(changeTo);
                    break;
            }
        }
    }

    public void UpdateYellowScore(int newValue)
    {
        if (yellowCoroutine != null) StopCoroutine(yellowCoroutine);
        yellowCoroutine = StartCoroutine(AnimateText(yellowText, newValue));
    }

    public void UpdateLastScore(int newValue)
    {
        if (lastCoroutine != null) StopCoroutine(lastCoroutine);
        lastCoroutine = StartCoroutine(AnimateText(lastText, newValue));
    }

    public void UpdateRedScore(int newValue)
    {
        if (redCoroutine != null) StopCoroutine(redCoroutine);
        redCoroutine = StartCoroutine(AnimateText(redText, newValue));
    }

    public void UpdateBlueScore(int newValue)
    {
        if (blueCoroutine != null) StopCoroutine(blueCoroutine);
        blueCoroutine = StartCoroutine(AnimateText(blueText, newValue));
    }

    private IEnumerator AnimateText(TextMeshProUGUI text, int targetValue)
    {
        int startValue = int.Parse(text.text);
        float elapsedTime = 0f;

        while (elapsedTime < changeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / changeDuration;
            int newValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, t));
            text.text = newValue.ToString();
            yield return null;
        }

        text.text = targetValue.ToString(); // 确保最终值正确
    }

    public void UpdateTotalScore(int newValue) // 新增总分更新
    {
        if (totalScoreCoroutine != null) StopCoroutine(totalScoreCoroutine);
        totalScoreCoroutine = StartCoroutine(AnimateText(totalScoreText, newValue));
    }

    public enum TestAction
    {
        ChangeYellow,
        ChangeLast,
        ChangeRed,
        ChangeBlue,
        ChangeTotalScore
    }
}
