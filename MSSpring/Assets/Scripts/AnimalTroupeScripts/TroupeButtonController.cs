using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TroupeButtonController : MonoBehaviour
{
    [SerializeField] string buttonName;
    [SerializeField] Image buttonBg;
    [SerializeField] TextMeshProUGUI buttonText;
    bool isClicked = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (isClicked)
        {
            isClicked = false;

            switch (buttonName)
            {
                case "buy":

                    TroupeController.instance.Buy();
                    buttonText.text = "Buy";
                    buttonBg.color = Color.white;

                    break;

                case "sell":

                    TroupeController.instance.Sell();
                    buttonText.text = "Sell";
                    buttonBg.color = Color.white;

                    break;

                case "upgrade":

                    TroupeController.instance.Upgrade();
                    buttonText.text = "Upgrade";
                    buttonBg.color = Color.white;

                    break;

                default:

                    Debug.Log("[Error] Assign Button Type");

                    break;
            }
        }
        else
        {
            isClicked = true;

            switch (buttonName)
            {
                case "buy":

                    buttonText.text = $"${TroupeController.instance.price}";
                    buttonBg.color = Color.yellow;

                    break;

                case "sell":

                    buttonText.text = $"${GlobalManager.instance.animalPrices[TroupeController.instance.troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty.animalName]}";
                    buttonBg.color = Color.yellow;

                    break;

                case "upgrade":

                    buttonText.text = $"${TroupeController.instance.price}";
                    buttonBg.color = Color.yellow;

                    break;

                default:

                    Debug.Log("[Error] Assign Button Type");

                    break;
            }
        }
    }
}
