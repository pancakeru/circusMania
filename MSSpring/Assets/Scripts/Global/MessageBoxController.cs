using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class MessageBoxController : MonoBehaviour
{
    [HideInInspector] public MessageType messageType;
    [HideInInspector] public Action action;

    public TextMeshProUGUI uiText;
    public Image uiBg;
    public GameObject yesButton;
    public GameObject noButton;

    [SerializeField] GameObject darkBackground;

    void Start()
    {
        if (messageType == MessageType.Warning)
        {
            Destroy(gameObject, 2f);

            yesButton.SetActive(false);
            noButton.SetActive(false);
            darkBackground.SetActive(false);
        }
        else
        {
            yesButton.SetActive(true);
            noButton.SetActive(true);
            darkBackground.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (messageType == MessageType.Warning) Destroy(gameObject);
    }

    public void Yes()
    {
        action?.Invoke();
        Destroy(gameObject);
    }

    public void No()
    {
        Destroy(gameObject);
    }
}
