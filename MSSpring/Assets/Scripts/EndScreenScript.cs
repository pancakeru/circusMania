using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EndScreenScript : MonoBehaviour
{
	public TMP_Text scoreText;
	public TMP_Text scoreBreakdown;
	public TMP_Text totalScore;
	public TMP_Text moneyEarned;
	public TMP_Text lvlName;

	private float starting = 0;
	private float addSpeed = 1;
	private float lineDelay = 0.3f;
	private int currentIndex = 0;
	public int[] test;

	public GameObject endButton;

	public Image progressSLider;

	[SerializeField] private TextMeshProUGUI percentText;

	[SerializeField] private Color startBarColor;
	[SerializeField] private Color endBarColor;

	private enum DisplaySequence
	{
		Wait,
		ShowScore,
		Breakdown,
		TotalCalc,
		MoneyCalc,
		EndButton
	}
	private DisplaySequence currentState;

	private int curReql;
	private int curTotal;
	private int curMoneyEarned;
	private GameObject audioObj;

	void Start()
	{
		//currentState = DisplaySequence.ShowScore;
		lvlName.text = "Level " + GlobalManager.instance.GetCurrentGlobalLevel().levelIndex;
		audioObj = GameObject.FindWithTag("audio manager");
		audioObj.GetComponent<AudioManagerScript>().PlayUISound(audioObj.GetComponent<AudioManagerScript>().UI[2]);
	}

	public void InitialScore(int required, int[] eachTurn, int total, int moneyEarned)
	{
		curReql = required;
		test = eachTurn;
		curTotal = total;
		curMoneyEarned = moneyEarned;
	}

	public void StartDisplay()
	{
		currentState = DisplaySequence.ShowScore;
	}

	void Update()
	{
		switch (currentState) {

			//第一部分-宏观信息
			case DisplaySequence.ShowScore:
				//scoreText.text = "Required Score: " + curReql.ToString();
				audioObj.GetComponent<AudioManagerScript>().PlayBattleSound(audioObj.GetComponent<AudioManagerScript>().Battle[4]);
				audioObj.GetComponent<AudioManagerScript>().battleSource.loop = true;

				currentState = DisplaySequence.Breakdown;

				break;

			//第二部分-分数结算
			case DisplaySequence.Breakdown:
				lineDelay -= 1 * Time.deltaTime;

				if (lineDelay <= 0) {
					if (currentIndex < test.Length) {
						DisplayList();
					} else {
						currentState = DisplaySequence.TotalCalc;
					}
				}
				break;

			//第三部分收获
			case DisplaySequence.TotalCalc:
				//数量
				DisplayTextForTotalScore(totalScore, curTotal, DisplaySequence.MoneyCalc);

				break;

			case DisplaySequence.MoneyCalc:
				//金币
				DisplayText(moneyEarned, curMoneyEarned, DisplaySequence.EndButton);

				break;

			case DisplaySequence.EndButton:

				audioObj.GetComponent<AudioManagerScript>().battleSource.loop = false;
				//audioObj.GetComponent<AudioManagerScript>().battleSource.Stop();
				//Play Again 按钮
				endButton.SetActive(true);
				//endButton.GetComponent<Button>().onClick.AddListener(() => { });
				break;
		}
	}

	public void Leave()
	{
		GlobalManager.instance.ToNextGlobalLevel();
		GlobalManager.instance.changeCoinAmount(15);
		FindAnyObjectByType<ShowManager>().LeaveShow();
	}

	void DisplayText(TMP_Text text, float value, DisplaySequence nextState)
	{
		if (starting < value) {
			starting += addSpeed;
			addSpeed += 1;
			text.text = starting.ToString();
		} else {
			starting = 0;
			text.text = value.ToString();
			currentState = nextState;
		}
	}

    void DisplayTextForTotalScore(TMP_Text text, float value, DisplaySequence nextState)
    {
        if (starting < value)
        {
            starting += addSpeed;
            addSpeed += 1;
			progressSLider.fillAmount = (starting / curReql) - ((int)(starting / curReql));
			percentText.text = (int)(starting / curReql * 100) + "/100%";
            text.text = starting.ToString();
        }
        else
        {
            starting = 0;
            text.text = value.ToString();
            currentState = nextState;
        }
    }

    void DisplayList()
	{
		//ebug.Log(currentIndex);
		scoreBreakdown.text += $"Act #{currentIndex + 1,-30} {test[currentIndex]}\n";
		currentIndex += 1;
		lineDelay = 0.5f;
	}

	public void PlayIdle() {
		audioObj.GetComponent<AudioManagerScript>().PlayUISound(audioObj.GetComponent<AudioManagerScript>().UI[0]);
		audioObj.GetComponent<AudioManagerScript>().PlayEnvironmentSound(audioObj.GetComponent<AudioManagerScript>().Environment[1]);
	}

}
