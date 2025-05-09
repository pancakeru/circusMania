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
	public object[] test;
	private int curRepu;

	public GameObject endButton;

	public Image progressSLider;

	[SerializeField] private TextMeshProUGUI percentText;

	[SerializeField] private Color startBarColor;
	[SerializeField] private Color endBarColor;


	[Header("新的元件")]
	public List<singleLevelSummary> summaries;
	public TMP_Text totalPopularity;
	public Transform winTrans;
	public Transform loseTrans;

	private List<ScoreContainer> scores;

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
	private bool ifWinLose;

	void Start()
	{
		//currentState = DisplaySequence.ShowScore;
		lvlName.text = "Level " + (GlobalManager.instance.GetCurrentGlobalLevel().levelIndex+1);
		audioObj = GameObject.FindWithTag("audio manager");
		audioObj.GetComponent<AudioManagerScript>().PlayUISound(audioObj.GetComponent<AudioManagerScript>().UI[2]);
	}

	public void InitialScore(List<ScoreContainer>scores ,int curRepu, int moneyEarned, bool ifWinLose)
	{
		//curReql = required;
		
		//test = eachTurn.ToArray();
		//curTotal = total;
		this.curRepu = curRepu;
		curMoneyEarned = moneyEarned;

		this.scores = scores;
		this.ifWinLose = ifWinLose;
	}

	public void StartDisplay()
	{
		currentState = DisplaySequence.ShowScore;
		starting = 0;
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
					if (currentIndex < Mathf.Min(scores.Count,3)) {
						DisplayList();
					} else {
						currentState = DisplaySequence.TotalCalc;
					}
				}
				break;

			//第三部分收获
			case DisplaySequence.TotalCalc:
				//数量
				//DisplayTextForTotalScore(totalScore, curRepu, DisplaySequence.MoneyCalc);
				DisplayPopularity(totalPopularity, curRepu, DisplaySequence.MoneyCalc);
				break;

			case DisplaySequence.MoneyCalc:
				//金币
				if (ifWinLose)
				{
					if (curRepu >= 0)
					{
						DisplayText(moneyEarned, curMoneyEarned, DisplaySequence.EndButton);
					}
					else
					{
						currentState = DisplaySequence.EndButton;
					}
				}
				else
				{
					currentState = DisplaySequence.EndButton;
				}
				
				

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
		//修改，把是否改变动物价格交给showManager判断
        //GlobalManager.instance.CalculateAnimalPrice();
        ShowManager.instance.LeaveShow(curMoneyEarned);
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
			progressSLider.fillAmount = (starting / curRepu) - ((int)(starting / curRepu));
			percentText.text = (int)(starting / curRepu * 100) + "/100%";
            text.text = starting.ToString();
        }
        else
        {
            starting = 0;
            text.text = value.ToString();
            currentState = nextState;
        }
    }

    void DisplayPopularity(TMP_Text text, float targetValue, DisplaySequence nextState)
    {
        bool isIncreasing = targetValue >= 0;

        if ((isIncreasing && starting < targetValue) || (!isIncreasing && starting > targetValue))
        {
            // 增减方向
            float delta = addSpeed * Time.deltaTime*30f;
            addSpeed += Time.deltaTime * 30f;

            starting += isIncreasing ? delta : -delta;

            // 限制不超出目标
            if ((isIncreasing && starting > targetValue) || (!isIncreasing && starting < targetValue))
            {
                starting = targetValue;
            }

            text.text = Mathf.FloorToInt(starting).ToString();
        }
        else
        {
            starting = 0f;
            addSpeed = 1f;
            text.text = Mathf.FloorToInt(targetValue).ToString();
            currentState = nextState;
			if (ifWinLose)
			{
                if (curRepu >= 0)
                {
                    winTrans.gameObject.SetActive(true);
                }
                else
                {
					ShowManager.win = false;
                    loseTrans.gameObject.SetActive(true);
                }
            }
			
        }
    }

    void DisplayList()
	{
		//ebug.Log(currentIndex);
		//scoreBreakdown.text += $"Act #{currentIndex + 1,-30} {(int)(test[currentIndex])}\n";
		summaries[currentIndex].gameObject.SetActive(true);
		ScoreContainer ct;
        if (currentIndex < scores.Count)
        {
            ct = scores[currentIndex];
        }
        else
        {
            // 取最后三个中的一个
            int offset = currentIndex - scores.Count;
            ct = scores[scores.Count - 3 + offset];
        }

        summaries[currentIndex].SetSliderValues(Mathf.Clamp(ct.redPlayer/ct.redRequ,0,1),Mathf.Clamp( ct.yellowPlayer/ct.yellowRequ,0,1), Mathf.Clamp(ct.bluePlayer/ct.blueRequ,0,1));
		currentIndex += 1;
		lineDelay = 0.5f;
	}

	public void PlayIdle() {
		audioObj.GetComponent<AudioManagerScript>().PlayUISound(audioObj.GetComponent<AudioManagerScript>().UI[0]);
		audioObj.GetComponent<AudioManagerScript>().PlayEnvironmentSound(audioObj.GetComponent<AudioManagerScript>().Environment[1]);
	}

}

public class ScoreContainer
{
	public float redRequ;
	public float yellowRequ;
	public float blueRequ;

	public float redPlayer;
	public float yellowPlayer;
	public float bluePlayer;

    public ScoreContainer(
        float redRequ, float yellowRequ, float blueRequ,
        float redPlayer, float yellowPlayer, float bluePlayer)
    {
        this.redRequ = redRequ;
        this.yellowRequ = yellowRequ;
        this.blueRequ = blueRequ;

        this.redPlayer = redPlayer;
        this.yellowPlayer = yellowPlayer;
        this.bluePlayer = bluePlayer;
    }
}
