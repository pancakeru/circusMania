using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TroupeCardController : MonoBehaviour
{
    public animalProperty myAnimalProperty;
    public List<Sprite> bgSprites = new List<Sprite>();

    public void Init(animalProperty givenAnimalProperty)
    {
        myAnimalProperty = givenAnimalProperty;
        transform.GetChild(0).GetComponent<Image>().sprite = bgSprites[0];
        transform.GetChild(1).GetComponent<Image>().sprite = myAnimalProperty.animalCoreImg;
    }

    public void OnClick()
    {
        TroupeController.instance.DisplayCardDetail(gameObject);
        TroupeController.instance.SetCardsBackground();
    }

}
