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

	[SerializeField] private TextMeshProUGUI redPopularityChangeText;
	[SerializeField] private Image redPopularityChangeImage;
	[SerializeField] private TextMeshProUGUI yellowPopularityChangeText;
	[SerializeField] private Image yellowPopularityChangeImage;
	[SerializeField] private TextMeshProUGUI bluePopularityChangeText;
	[SerializeField] private Image bluePopularityChangeImage;
	public bool isPopularityVisualChanged { get; private set; } = false;

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

	ShowManager showManager;
	public List<Sprite> speedUpButtonSprites = new List<Sprite>();

    private void Start()
    {
        showManager = transform.parent.transform.parent.GetComponent<ShowManager>();
    }

    private void Update()
	{
		if (ifTest)
		{
			ifTest = false;
			switch (nextTestAction)
			{
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

	public void ScoreSound()
	{
		AudioManagerScript.Instance.PlayBattleSound(AudioManagerScript.Instance.UI[5]);
	}

	private IEnumerator AnimateText(TextMeshProUGUI text, float targetValue, int decimalPlaces)
	{
		float startValue = float.Parse(text.text);
		float elapsedTime = 0f;

		while (elapsedTime < changeDuration)
		{
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
		/*if (totalScoreCoroutine != null) StopCoroutine(totalScoreCoroutine);
		totalScoreCoroutine = StartCoroutine(AnimateText(totalScoreText, newValue, 0));*/
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
		if (currentScore != 0f)
		{
			return currentScore / targetScore;
		}
		else
		{
			return 0f;
		}
	}

	private void ChangeFillAmount(string scoreType, float fillAmount)
	{
		switch (scoreType)
		{
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

	public void ResetPopularityChangeVisual()
	{
		redPopularityChangeText.text = "0";
		yellowPopularityChangeText.text = "0";
		bluePopularityChangeText.text = "0";

		redPopularityChangeText.color = new Color(0.4470588f, 0.4117647f, 0.4196078f, 0f);
		yellowPopularityChangeText.color = new Color(0.4470588f, 0.4117647f, 0.4196078f, 0f);
		bluePopularityChangeText.color = new Color(0.4470588f, 0.4117647f, 0.4196078f, 0f);

		redPopularityChangeImage.color = new Color(1f, 1f, 1f, 0f);
		yellowPopularityChangeImage.color = new Color(1f, 1f, 1f, 0f);
		bluePopularityChangeImage.color = new Color(1f, 1f, 1f, 0f);

		isPopularityVisualChanged = false;

		redPopularityChangeImage.gameObject.SetActive(true);
		yellowPopularityChangeImage.gameObject.SetActive(true);
		bluePopularityChangeImage.gameObject.SetActive(true);
	}

	public void SetPopularityChangeVisualActiveToFalse()
	{
		redPopularityChangeImage.gameObject.SetActive(false);
		yellowPopularityChangeImage.gameObject.SetActive(false);
		bluePopularityChangeImage.gameObject.SetActive(false);
	}

	public void UpdatePopularityChangeVisual(float[] respectiveEndTurnReputationChange, float visualChangeSpeed)
	{
		if (respectiveEndTurnReputationChange[0] > 0f)
		{
			redPopularityChangeText.text = "+" + respectiveEndTurnReputationChange[0].ToString();
		}
		else
		{
			redPopularityChangeText.text = respectiveEndTurnReputationChange[0].ToString();
		}
		if (respectiveEndTurnReputationChange[1] > 0f)
		{
			yellowPopularityChangeText.text = "+" + respectiveEndTurnReputationChange[1].ToString();
		}
		else
		{
			yellowPopularityChangeText.text = respectiveEndTurnReputationChange[1].ToString();
		}
		if (respectiveEndTurnReputationChange[2] > 0f)
		{
			bluePopularityChangeText.text = "+" + respectiveEndTurnReputationChange[2].ToString();
		}
		else
		{
			bluePopularityChangeText.text = respectiveEndTurnReputationChange[2].ToString();
		}

		//Text Color Change
		redPopularityChangeText.color = new Color(0.4470588f, 0.4117647f, 0.4196078f, redPopularityChangeText.color.a + visualChangeSpeed);
		yellowPopularityChangeText.color = new Color(0.4470588f, 0.4117647f, 0.4196078f, yellowPopularityChangeText.color.a + visualChangeSpeed);
		bluePopularityChangeText.color = new Color(0.4470588f, 0.4117647f, 0.4196078f, bluePopularityChangeText.color.a + visualChangeSpeed);

		//Image Color Change
		redPopularityChangeImage.color = new Color(1f, 1f, 1f, redPopularityChangeImage.color.a + visualChangeSpeed);
		yellowPopularityChangeImage.color = new Color(1f, 1f, 1f, yellowPopularityChangeImage.color.a + visualChangeSpeed);
		bluePopularityChangeImage.color = new Color(1f, 1f, 1f, bluePopularityChangeImage.color.a + visualChangeSpeed);

		if (redPopularityChangeText.color.a >= 1f && yellowPopularityChangeText.color.a >= 1f && bluePopularityChangeText.color.a >= 1f)
		{
			if (redPopularityChangeImage.color.a >= 1f && yellowPopularityChangeImage.color.a >= 1f && bluePopularityChangeImage.color.a >= 1f)
			{
				isPopularityVisualChanged = true;
				UpdateLastScore(respectiveEndTurnReputationChange[3], "");
			}
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

	public void OnSpeedUpButtonClick()
	{
		showManager.SpeedUp();

    }
}
