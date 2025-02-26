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
            case DisplaySequence.ShowScore:

                //1000 是分数字
                DisplayText(scoreText, 1000, DisplaySequence.Breakdown);

            break;

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

            case DisplaySequence.TotalCalc:
                
                DisplayText(totalScore, 10000, DisplaySequence.MoneyCalc);

            break;

            case DisplaySequence.MoneyCalc:

                DisplayText(moneyEarned, 20000, DisplaySequence.EndButton);

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
        scoreBreakdown.text += $"Act #{currentIndex + 1, -35} +{test[currentIndex]}\n";
        currentIndex += 1;
        lineDelay = 0.5f;
    }

}
