using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreenScript : MonoBehaviour
{
    public TMP_Text scoreText; 
    public TMP_Text scoreBreakdown;
    public TMP_Text totalScore;
    public TMP_Text moneyEarned;


    void Start()
    {
        


    }

    void Update()
    {
        
    }


    void AddingEffect(float value) {
        float starting = 0;

        while (starting < value) {
            starting++;
        }
    }

}
