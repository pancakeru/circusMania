using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TroupeCardController : MonoBehaviour
{
    public animalProperty myAnimalProperty;

    public void Init(animalProperty givenAnimalProperty)
    {
        myAnimalProperty = givenAnimalProperty;
        if (givenAnimalProperty == null)
        {
            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!");
            return;
        }
        transform.GetChild(1).GetComponent<Image>().sprite = myAnimalProperty.animalCoreImg;
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = myAnimalProperty.animalName;
        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = myAnimalProperty.returnBallAction();
        transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = myAnimalProperty.returnScoreAction();

        //Debug.Log(myAnimalProperty.returnBallAction());
    }

    public void OnClick()
    {
        TroupeController.instance.DisplayCardDetail(gameObject);
    }
}
