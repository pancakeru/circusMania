using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TroupeController : MonoBehaviour
{
    public static TroupeController instance;

    MenuController menuController;

    public GameObject troupeCardSimple;
    public TroupeDetailController troupeCardDetailed;
    [HideInInspector] public GameObject troupeCardSelected;
    List<GameObject> troupeCards = new List<GameObject>();

    GameObject cardsGroup;
    GameObject slide;

    Vector2 cardStartPos = new Vector2 (215f, -420f);
    Vector2 cardOffset = new Vector2(275f, -275f);
    Vector2 slideStartPos = new Vector2();
    Vector2 slideEndPos = new Vector2();
    int cardsPerRow = 3;
    int visibleRows = 3;

    public TextMeshProUGUI textCoin;
    int coin = -1;
    public int price = 3;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        cardOffset = new Vector2(cardOffset.x * troupeCardSimple.transform.localScale.x, cardOffset.y * troupeCardSimple.transform.localScale.y);
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
        /* Slide is not used. 
        if (GetComponent<Canvas>().enabled)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0 && troupeCards.Count > cardsPerRow * visibleRows)
            {
                slide.GetComponent<Slider>().value = Mathf.Clamp01(slide.GetComponent<Slider>().value - scroll * 0.6f);
            }
        }
        */
    }

    void DisplayCards()
    {
        for (int i = 0; i < troupeCards.Count; i++)
        {
            Destroy(troupeCards[i]);
        }
        troupeCards.Clear();

        List<animalProperty> myTroupe = GlobalManager.instance.getAllAnimals();

        for (int i = 0; i < GlobalManager.instance.allAnimals.properies.Length; i++)
        {
            GameObject newCard = Instantiate(troupeCardSimple, cardsGroup.transform);
            newCard.GetComponent<TroupeCardController>().Init(GlobalManager.instance.allAnimals.properies[i]);

            troupeCards.Add(newCard);
        }

        for (int i = 0; i < troupeCards.Count; i++)
        {
            troupeCards[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(cardStartPos.x + (i % cardsPerRow) * cardOffset.x, cardStartPos.y + (i / cardsPerRow) * cardOffset.y);
        }

        slideStartPos = new Vector2(0, 0);
        slideEndPos = new Vector2(0, -cardOffset.y * (Mathf.CeilToInt((float)troupeCards.Count / cardsPerRow) - visibleRows));

        DisplayCardDetail(troupeCards[0]);
        SetCardsBackground();
    }

    public void DisplayCardDetail(GameObject selectedTroupeCard)
    {
        troupeCardSelected = selectedTroupeCard;
        animalProperty theAnimalProperty = troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty;
        troupeCardDetailed.coreImage.sprite = theAnimalProperty.animalCoreImg;
        troupeCardDetailed.animalName.text = theAnimalProperty.animalName;
        troupeCardDetailed.ballAction.text = theAnimalProperty.returnBallAction1Only();
        troupeCardDetailed.restTurn.text = theAnimalProperty.restTurn.ToString();
        troupeCardDetailed.scoreAction.text = theAnimalProperty.returnScoreActionNoRest();

        
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
        coin = GlobalManager.instance.getCurCoinAmount();
        UpdateText();
    }

    public void Disable()
    {
        GlobalManager.instance.setCoinAmount(coin);

        GetComponent<Canvas>().enabled = false;
        menuController.Enable();
    }

    public void SlideCards(float value)
    {
        cardsGroup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, Mathf.Lerp(slideStartPos.y, slideEndPos.y, value));
    }

    public void UpdateText()
    {
        foreach (GameObject card in troupeCards)
        {
            TroupeCardController cardController = card.GetComponent<TroupeCardController>();
            cardController.textLv.text = GlobalManager.instance.animalLevels[cardController.myAnimalProperty.animalName].ToString();

            int animalCount = NumberInTroupe(cardController.myAnimalProperty.animalName);
            cardController.textNum.text = animalCount.ToString(); 
            if (animalCount == 0) cardController.profile.color = Color.black; 
        }

        textCoin.text = $"Coin: {coin}";
    }

    public int NumberInTroupe(string animalName)
    {
        int animalCount = 0;
        foreach (animalProperty animal in GlobalManager.instance.getAllAnimals())
        {
            if (animal.animalName == animalName) animalCount++;
        }

        return animalCount;
    }

    public void Buy()
    {
        if (coin >= price)
        {
            coin -= price;
            GlobalManager.instance.addAnAnimal(troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty);
            UpdateText();
        }
    }

    public void Sell()
    {
        animalProperty animal = troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty;
        if (NumberInTroupe(animal.animalName) > 1)
        {
            GlobalManager.instance.removeAnAnimal(animal);
            coin += GlobalManager.instance.animalPrices[animal.animalName];
            UpdateText();
        }
    }

    public void Upgrade()
    {
        animalProperty animal = troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty;
        if (coin >= price)
        {
            coin -= price;
            GlobalManager.instance.UpdateLevel(animal.animalName, 1);
            UpdateText();
        }
    }
}
