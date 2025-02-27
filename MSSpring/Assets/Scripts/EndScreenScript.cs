using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.UIElements;

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

    private enum DisplaySequence {
        ShowScore,
        Breakdown,
        TotalCalc,
        MoneyCalc,
        EndButton
    }
    private DisplaySequence currentState;

    void Start()
    {
        currentState = DisplaySequence.ShowScore;

    }

    void Update()
    {
        switch (currentState) {

            //第一部分-宏观信息
            case DisplaySequence.ShowScore:
                lvlName.text = "Level 1";
                scoreText.text = "Required Score: 1000";

                currentState = DisplaySequence.Breakdown;

            break;

            //第二部分-分数结算
            case DisplaySequence.Breakdown:
                lineDelay-= 1 * Time.deltaTime;

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
                DisplayText(totalScore, 10000, DisplaySequence.MoneyCalc);

            break;

            case DisplaySequence.MoneyCalc:
                //金币
                DisplayText(moneyEarned, 20000, DisplaySequence.EndButton);

            break;

            case DisplaySequence.EndButton:

                //Play Again 按钮

            break;
        }
    }

    
    void DisplayText(TMP_Text text, float value, DisplaySequence nextState) {
        if (starting < value) {
            starting+= addSpeed;
            addSpeed += 1;
            text.text = starting.ToString();
        } else {
            starting = 0;
            text.text = value.ToString();
            currentState = nextState;
        }
    }

    void DisplayList() {
        scoreBreakdown.text += $"Act #{currentIndex + 1, -30} {test[currentIndex]}\n";
        currentIndex += 1;
        lineDelay = 0.5f;
    }

}
