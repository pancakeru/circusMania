using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

	[SerializeField] private TextMeshProUGUI targetRed;
	[SerializeField] private TextMeshProUGUI targetYellow;
	[SerializeField] private TextMeshProUGUI targetBlue;
	private float targetRedScore;
	private float targetYellowScore;
	private float targetBlueScore;
	[SerializeField] private Image redFill;
	[SerializeField] private Image yellowFill;
	[SerializeField] private Image blueFill;

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
		if (ifTest) {
			ifTest = false;
			switch (nextTestAction) {
				case TestAction.ChangeYellow:
					UpdateYellowScore(changeTo, "测试");
					break;

				case TestAction.ChangeLast:
					UpdateLastScore(changeTo, "测试");
					break;

				case TestAction.ChangeRed:
					UpdateRedScore(changeTo, "测试");
					break;

				case TestAction.ChangeBlue:
					UpdateBlueScore(changeTo, "测试");
					break;

				case TestAction.ChangeTotalScore: // 新增总分测试
					UpdateTotalScore(changeTo, "测试");
					break;
			}
		}
	}

	public void UpdateYellowScore(float newValue, string changeName)
	{
		//Debug.Log("改动黄"+newValue.ToString());
		if (yellowCoroutine != null) StopCoroutine(yellowCoroutine);
		yellowCoroutine = StartCoroutine(AnimateText(yellowText, newValue, 0));
		ScoreSound();
		ChangeFillAmount("Yellow", GetScoreNormalized(newValue, targetYellowScore));
	}

	public void UpdateLastScore(float newValue, string changeName)
	{
		//Debug.Log("改动上一次" + newValue.ToString());
		if (lastCoroutine != null) StopCoroutine(lastCoroutine);
		lastCoroutine = StartCoroutine(AnimateText(lastText, newValue, 0));
	}

	public void UpdateRedScore(float newValue, string changeName)
	{
		//Debug.Log("改动红" + newValue.ToString());
		if (redCoroutine != null) StopCoroutine(redCoroutine);
		redCoroutine = StartCoroutine(AnimateText(redText, newValue, 0));
		ScoreSound();
		ChangeFillAmount("Red", GetScoreNormalized(newValue, targetRedScore));
	}

	public void UpdateBlueScore(float newValue, string changeName)
	{
		//Debug.Log("改动蓝" + newValue.ToString());
		if (blueCoroutine != null) StopCoroutine(blueCoroutine);
		blueCoroutine = StartCoroutine(AnimateText(blueText, newValue, 0));
		ScoreSound();
		ChangeFillAmount("Blue", GetScoreNormalized(newValue, targetBlueScore));
	}

	public void ScoreSound() {
		AudioManagerScript.Instance.PlayBattleSound(AudioManagerScript.Instance.UI[5]);
	}

	private IEnumerator AnimateText(TextMeshProUGUI text, float targetValue, int decimalPlaces)
	{
		float startValue = float.Parse(text.text);
		float elapsedTime = 0f;

		while (elapsedTime < changeDuration) {
			elapsedTime += Time.deltaTime;
			float t = elapsedTime / changeDuration;
			float newValue = Mathf.Lerp(startValue, targetValue, t);

			// **格式化数值，保留指定的小数位**
			text.text = newValue.ToString("F" + decimalPlaces);
			yield return null;
		}

		// **确保最终值正确**
		text.text = targetValue.ToString("F" + decimalPlaces);
	}

	public void UpdateTotalScore(float newValue, string changeName) // 新增总分更新
	{
		return;
		//Debug.Log("改动总" + newValue.ToString());
		if (totalScoreCoroutine != null) StopCoroutine(totalScoreCoroutine);
		totalScoreCoroutine = StartCoroutine(AnimateText(totalScoreText, newValue, 0));
	}

	public void UpdateTargetScores(float targetRed, float targetYellow, float targetBlue)
	{
		this.targetRed.text = targetRed.ToString();
		targetRedScore = targetRed;
		this.targetYellow.text = targetYellow.ToString();
		targetYellowScore = targetYellow;
		this.targetBlue.text = targetBlue.ToString();
		targetBlueScore = targetBlue;
	}

	private float GetScoreNormalized(float currentScore, float targetScore)
	{
		if (currentScore != 0f) {
			return currentScore / targetScore;
		} else {
			return 0f;
		}
	}

	private void ChangeFillAmount(string scoreType, float fillAmount)
	{
		switch (scoreType) {
			case "Red":
				redFill.fillAmount = fillAmount;
				break;
			case "Yellow":
				yellowFill.fillAmount = fillAmount;
				break;
			case "Blue":
				blueFill.fillAmount = fillAmount;
				break;
		}
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
