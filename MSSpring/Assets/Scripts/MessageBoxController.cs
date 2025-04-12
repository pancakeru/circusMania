using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageBoxController : MonoBehaviour
{
    public MessageType messageType;
    public TextMeshProUGUI uiText;
    public Image uiBg;
    public GameObject yesButton;
    public GameObject noButton;

    void Start()
    {
        if (messageType == MessageType.Warning)
        {
            Destroy(gameObject, 2f);

            yesButton.SetActive(false);
            noButton.SetActive(false);
        }
        else
        {
            yesButton.SetActive(true);
            noButton.SetActive(true);
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

    }

    public void No()
    {

    }
}
