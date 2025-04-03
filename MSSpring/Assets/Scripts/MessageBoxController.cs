using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageBoxController : MonoBehaviour
{
    public TextMeshProUGUI uiText;
    public Image uiBg;

    void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        Destroy(gameObject);
    }
}
