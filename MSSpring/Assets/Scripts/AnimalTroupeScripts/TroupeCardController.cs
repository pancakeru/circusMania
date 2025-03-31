using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TroupeCardController : MonoBehaviour
{
    public animalProperty myAnimalProperty;
    public List<Sprite> bgSprites = new List<Sprite>();

    public Image profile;
    public Image bg;
    public TextMeshProUGUI textNum;
    public TextMeshProUGUI textLv;

    public void Init(animalProperty givenAnimalProperty)
    {
        myAnimalProperty = givenAnimalProperty;
        bg.sprite = bgSprites[0];
        profile.sprite = myAnimalProperty.animalCoreImg;
    }

    public void OnClick()
    {
        TroupeController.instance.DisplayCardDetail(gameObject);
        TroupeController.instance.SetCardsBackground();
    }

}
