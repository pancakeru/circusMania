using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class TroupeController : MonoBehaviour
{
    public static TroupeController instance;

    MenuController menuController;

    public GameObject troupeCardSimple;
    public GameObject troupeCardDetailed;
    [HideInInspector] public GameObject troupeCardSelected;
    List<animalProperty> tempTroupe = new List<animalProperty>();
    List<GameObject> troupeCards = new List<GameObject>();

    GameObject cardsGroup;
    GameObject slide;

    Vector2 cardStartPos = new Vector2 (215f, -220f);
    Vector2 cardOffset = new Vector2(310f, -250f);
    Vector2 slideStartPos = new Vector2();
    Vector2 slideEndPos = new Vector2();
    int cardsPerRow = 3;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        menuController = FindAnyObjectByType<MenuController>();

        cardsGroup = transform.GetChild(1).gameObject;
        slide = transform.GetChild(2).gameObject;
        slide.GetComponent<Slider>().onValueChanged.AddListener(SlideCards);
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Canvas>().enabled)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
            {
                slide.GetComponent<Slider>().value = Mathf.Clamp01(slide.GetComponent<Slider>().value - scroll * 0.6f);
            }
        }
    }

    void DisplayCards()
    {
        tempTroupe = GlobalManager.instance.getAllAnimals();
        for (int i = 0; i < troupeCards.Count; i++)
        {
            Destroy(troupeCards[i]);
        }
        troupeCards.Clear();

        for (int i = 0; i < tempTroupe.Count; i++)
        {
            GameObject newCard = Instantiate(troupeCardSimple, cardsGroup.transform);
            newCard.GetComponent<TroupeCardController>().Init(tempTroupe[i]);
            
            troupeCards.Add(newCard);
        }

        for (int i = 0; i < troupeCards.Count; i++)
        {
            troupeCards[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(cardStartPos.x + (i % cardsPerRow) * cardOffset.x, cardStartPos.y + (i / cardsPerRow) * cardOffset.y);
        }

        slideStartPos = new Vector2(0, 0);
        int visibleRows = 3; 
        slideEndPos = new Vector2(0, -cardOffset.y * (Mathf.CeilToInt((float)troupeCards.Count / cardsPerRow) - visibleRows));

        Debug.Log(tempTroupe[5].animalName + tempTroupe[5].returnBallAction());
        DisplayCardDetail(troupeCards[0]);
    }

    public void DisplayCardDetail(GameObject selectedTroupeCard)
    {
        troupeCardSelected = selectedTroupeCard;
        animalProperty theAnimalProperty = troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty;
        troupeCardDetailed.transform.GetChild(1).GetComponent<Image>().sprite = theAnimalProperty.animalCoreImg;
        troupeCardDetailed.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = theAnimalProperty.animalName;
        troupeCardDetailed.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = theAnimalProperty.returnBallAction1Only();
        troupeCardDetailed.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = theAnimalProperty.restTurn.ToString();
        troupeCardDetailed.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = theAnimalProperty.returnScoreActionNoRest();
    }

    public void SetCardsBackground()
    {
        foreach (var card in troupeCards)
        {
            card.GetComponent<TroupeCardController>().transform.GetChild(0).GetComponent<Image>().sprite = card.GetComponent<TroupeCardController>().bgSprites[0];
        }
        troupeCardSelected.GetComponent<TroupeCardController>().transform.GetChild(0).GetComponent<Image>().sprite = troupeCardSelected.GetComponent<TroupeCardController>().bgSprites[1];
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
}
