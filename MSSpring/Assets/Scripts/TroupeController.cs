using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TroupeController : MonoBehaviour
{
    MenuController menuController;

    public GameObject troupeCard;
    List<animalProperty> tempTroupe = new List<animalProperty>();
    List<GameObject> troupeCards = new List<GameObject>();

    GameObject cardsGroup;
    GameObject slide;

    Vector2 cardStartPos = new Vector2 (-700f, 50f);
    Vector2 slideStartPos = new Vector2();
    Vector2 slideEndPos = new Vector2();
    int cardsPerRow = 4;

    void Start()
    {
        GetComponent<Canvas>().enabled = false;

        menuController = FindAnyObjectByType<MenuController>();

        cardsGroup = transform.GetChild(0).gameObject;
        slide = transform.GetChild(1).gameObject;
        slide.GetComponent<Slider>().onValueChanged.AddListener(SlideCards);

        for (int i = 0; i < 9; i++)
        {
            tempTroupe.Add(CreateTempAnimalInstance());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Canvas>().enabled)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                slide.GetComponent<Slider>().value = Mathf.Clamp01(slide.GetComponent<Slider>().value - scroll * 0.2f);
            }
        }
    }

    void DisplayCards()
    {
        for (int i = 0; i < troupeCards.Count; i++)
        {
            Destroy(troupeCards[i]);
        }
        troupeCards.Clear();

        for (int i = 0; i < tempTroupe.Count; i++)
        {
            GameObject newCard = Instantiate(troupeCard, cardsGroup.transform);
            if (tempTroupe[i].animalCoreImg != null) newCard.transform.GetChild(1).GetComponent<Image>().sprite = tempTroupe[i].animalCoreImg;
            newCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = tempTroupe[i].animalName;
            newCard.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = tempTroupe[i].returnScoreAction();
            newCard.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = tempTroupe[i].returnBallAction();
            troupeCards.Add(newCard);
        }

        float width = troupeCards[0].GetComponent<RectTransform>().rect.width * 4.5f;
        float height = troupeCards[0].GetComponent<RectTransform>().rect.height * 5.5f;

        for (int i = 0; i < troupeCards.Count; i++)
        {
            troupeCards[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(cardStartPos.x + (i % cardsPerRow) * width, cardStartPos.y - (i / cardsPerRow) * height);
        }

        slideStartPos = new Vector2(0, cardStartPos.y);
        slideEndPos = new Vector2(0, cardStartPos.y + height * troupeCards.Count / cardsPerRow);
    }

    public void Enable()
    {
        GetComponent<Canvas>().enabled = true;
        DisplayCards();
    }

    public void Disable()
    {
        GetComponent<Canvas>().enabled = false;
        menuController.Enable();

    }

    public void SlideCards(float value)
    {
        cardsGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Lerp(slideStartPos.y, slideEndPos.y, value));
    }

    animalProperty CreateTempAnimalInstance()
    {
        animalProperty newAnimal = ScriptableObject.CreateInstance<animalProperty>();

        newAnimal.animalName = "none";
        newAnimal.animalCoreImg = null;
        newAnimal.animalPrice = 0;
        newAnimal.baseYellowChange = 0f;
        newAnimal.baseRedChange = 0f;
        newAnimal.baseBlueChange = 0f;
        newAnimal.restTurn = 0;
        newAnimal.scoreActionTemplate = "Gain 10 Score";
        newAnimal.amount1 = 0;
        newAnimal.amount2 = 0;
        newAnimal.amount3 = 0;
        newAnimal.ballActionTemplate = "Throw R1, rest 3T";
        newAnimal.amount4 = 0;
        newAnimal.amount5 = 0;
        newAnimal.amount6 = 0;

        return newAnimal;
    }
}
