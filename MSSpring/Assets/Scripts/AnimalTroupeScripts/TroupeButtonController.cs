using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TroupeButtonController : MonoBehaviour, IPointerExitHandler
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
            SetToDefault();

            switch (buttonName)
            {
                case "buy":

                    TroupeController.instance.Buy();

                    break;

                case "sell":

                    TroupeController.instance.Sell();

                    break;

                case "upgrade":

                    TroupeController.instance.Upgrade();

                    break;

                default:

                    //Debug.Log("[Error] Assign Button Type");

                    break;
            }
        }
        else
        {
            SetToConfirming();
        }
    }

    void SetToDefault()
    {
        isClicked = false;

        switch (buttonName)
        {
            case "buy":

                buttonText.text = "Buy";
                buttonBg.color = Color.white;

                break;

            case "sell":

                buttonText.text = "Sell";
                buttonBg.color = Color.white;

                break;

            case "upgrade":

                buttonText.text = "Upgrade";
                buttonBg.color = Color.white;

                break;

            default:

                //Debug.Log("[Error] Assign Button Type");

                break;
        }
    }

    void SetToConfirming()
    {
        isClicked = true;

        switch (buttonName)
        {
            case "buy":

                buttonText.text = $"${GlobalManager.instance.animalPrices[TroupeController.instance.troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty.animalName]}";
                buttonBg.color = Color.yellow;

                break;

            case "sell":

                buttonText.text = $"${GlobalManager.instance.animalPrices[TroupeController.instance.troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty.animalName]}";
                buttonBg.color = Color.yellow;

                break;

            case "upgrade":

                buttonText.text = $"${TroupeController.instance.GetUpgradePrice(TroupeController.instance.troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty)}";
                buttonBg.color = Color.yellow;

                break;

            default:

                //Debug.Log("[Error] Assign Button Type");

                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetToDefault();
    }
}
