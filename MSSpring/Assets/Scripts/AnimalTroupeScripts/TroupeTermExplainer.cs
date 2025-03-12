using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TroupeTermExplainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    void Start()
    {
        transform.parent.transform.GetChild(1).gameObject.SetActive(false);
        transform.parent.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
        {
            MechanicNumberType myMechanicNumberType = TroupeController.instance.troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty.mechanicNumberType;
            string explainText = myMechanicNumberType == MechanicNumberType.Power ? "<b>Power(n)</b>\nActions will upgrade based on Power.\r\n\r\n" :
                                 myMechanicNumberType == MechanicNumberType.Excited ? "<b>Excited(n)</b>\nAfter passing a ball, the animal is excited for next n turns and each rest turn triggers an effect.\r\n" :
                                 myMechanicNumberType == MechanicNumberType.WarmUp ? "<b>Warm Up(n)</b>\nAfter passing n ball, the animal does an effect that triggers only once per show.\r\n" :
                                 "none";
            if (explainText == "none") return;

            transform.parent.transform.GetChild(1).gameObject.SetActive(true);
            transform.parent.transform.GetChild(2).gameObject.SetActive(true);
            transform.parent.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = explainText;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
        {
            transform.parent.transform.GetChild(1).gameObject.SetActive(false);
            transform.parent.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
