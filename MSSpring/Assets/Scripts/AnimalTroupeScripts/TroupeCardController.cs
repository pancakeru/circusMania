using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TroupeCardController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public animalProperty myAnimalProperty;
    public List<Sprite> bgSprites = new List<Sprite>();
    List<Sprite> profileSprites = new List<Sprite>();

    public Image profile;
    public Image bg;
    public TextMeshProUGUI textNum;
    public TextMeshProUGUI textLv;

    public void Init(animalProperty givenAnimalProperty)
    {
        myAnimalProperty = givenAnimalProperty;
        bg.sprite = bgSprites[0];
        
        profileSprites = TroupeController.instance.GetComponent<SpriteExtractor>().animalSprites[myAnimalProperty.animalName];
        profile.sprite = profile.sprite = profileSprites[0];
    }

    public void OnClick()
    {
        TroupeController.instance.DisplayCardDetail(gameObject);
        TroupeController.instance.SetCardsBackground();
        AudioManagerScript.Instance.PlayUISound(AudioManagerScript.Instance.UI[0]);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        profile.sprite = profileSprites[1];
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        profile.sprite = profileSprites[0];
    }
}
