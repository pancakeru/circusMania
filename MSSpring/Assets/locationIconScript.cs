using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class locationIconScript : MonoBehaviour
{
    private Image myImage;

    [Header("State Sprites")]
    public Sprite passedSprite;
    public Sprite noneSprite;
    public Sprite failedSprite;

    public enum Status
    {
        passed,
        none,
        failed
    }

    public Status currentState;
    private Status previousState;

    void Start()
    {
        myImage = GetComponentInChildren<Image>();
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
                break;

            case Status.none:
                myImage.sprite = noneSprite;
                break;

            case Status.failed:
                myImage.sprite = failedSprite;
                break;
        }
    }
}
