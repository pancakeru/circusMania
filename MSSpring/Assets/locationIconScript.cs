using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class locationIconScript : MonoBehaviour
{
    private Image myImage;

    [Header("State Sprites")]
    public Sprite passedSprite;
    public Sprite noneSprite;
    public Sprite failedSprite;
    public String location;
    private TMP_Text myText;

    public enum Status
    {
        passed,
        none,
        failed,
        current
    }

    public Status currentState;
    private Status previousState;

    void Start()
    {
        myImage = GetComponentInChildren<Image>();
        myText = GetComponentInChildren<TMP_Text>();
       // myText.text = location;
        previousState = currentState; // To force a refresh on start
        UpdateSprite();               // Initial sprite setup
    }

    void Update()
    {
        // Only update when the state changes
        if (currentState != previousState)
        {
            UpdateSprite();
            previousState = currentState;
        }
    }

    void UpdateSprite()
    {
        switch (currentState)
        {
            case Status.passed:
                myImage.sprite = passedSprite;
                myText.text = location;
                myText.fontSize = 20;
                myText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50);
                break;

            case Status.none:
                myImage.sprite = noneSprite;
                myText.text = " ";
                break;

            case Status.failed:
                myImage.sprite = failedSprite;
                 myText.text = " ";
                break;

            case Status.current:
                myImage.sprite = passedSprite;
                myText.text = location;
                myText.fontSize = 23;
                myText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 90);
            break;

        }
    }
}
