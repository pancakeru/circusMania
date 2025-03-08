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
    GameObject troupeCardSelected;
    List<animalProperty> tempTroupe = new List<animalProperty>();
    List<GameObject> troupeCards = new List<GameObject>();

    GameObject cardsGroup;
    GameObject slide;

    Vector2 cardStartPos = new Vector2 (75f, -375f);
    Vector2 slideStartPos = new Vector2();
    Vector2 slideEndPos = new Vector2();
    int cardsPerRow = 4;

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

        /*
         * 这里Neil修改了，因为在列表为空时会报错
         */
        float width = troupeCardSimple.GetComponent<RectTransform>().localScale.x * 450f;
        float height = troupeCardSimple.GetComponent<RectTransform>().localScale.y * 550f;

        for (int i = 0; i < troupeCards.Count; i++)
        {
            troupeCards[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(cardStartPos.x + (i % cardsPerRow) * width, cardStartPos.y - (i / cardsPerRow) * height);
        }

        slideStartPos = new Vector2(0, 0);
        slideEndPos = new Vector2(0, -100 + height * (troupeCards.Count / cardsPerRow - 2));

        DisplayCardDetail(troupeCards[0]);
    }

    public void DisplayCardDetail(GameObject selectedTroupeCard)
    {
        troupeCardSelected = selectedTroupeCard;
        animalProperty theAnimalProperty = troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty;
        troupeCardDetailed.transform.GetChild(1).GetComponent<Image>().sprite = theAnimalProperty.animalCoreImg;
        troupeCardDetailed.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = theAnimalProperty.animalName;
        troupeCardDetailed.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = theAnimalProperty.returnBallAction();
        troupeCardDetailed.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = theAnimalProperty.returnScoreAction();
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
