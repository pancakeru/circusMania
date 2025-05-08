using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TroupeController : MonoBehaviour, ISaveData
{
    public static TroupeController instance;

    MenuController menuController;

    public GameObject troupeCardSimple;
    public TroupeDetailController troupeCardDetailed;
    [HideInInspector] public GameObject troupeCardSelected;
    List<GameObject> troupeCards = new List<GameObject>();

    public GameObject cardsGroup;
    public GameObject slide;

    Vector2 cardStartPos = new Vector2(215f, -400f);
    Vector2 cardOffset = new Vector2(330f, -275f);
    Vector2 slideStartPos = new Vector2();
    Vector2 slideEndPos = new Vector2();
    int cardsPerRow = 3;
    int visibleRows = 3;

    public TextMeshProUGUI textCoin;
    private int upgradePrice;
    int coin = -1;

    public bool isFirstGameCompleted = false;

    List<animalProperty> newAnimalOrder;
    float[] starFills = new float[] { 0, 0.175f, 0.383f, 0.622f, 0.8f, 1f };

    [SerializeField] private priceMultiplier multis;

    [HideInInspector] public Dictionary<string, List<int>> animalPriceChanges = new Dictionary<string, List<int>>();
    [HideInInspector] public int previousLevelIndex = -1;
    [HideInInspector] public int maxChartLength;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        cardOffset = new Vector2(cardOffset.x * troupeCardSimple.transform.localScale.x, cardOffset.y * troupeCardSimple.transform.localScale.y);
        upgradePrice = 10;
        previousLevelIndex = 0;
        maxChartLength = 5;
    }

    void DisplayCards()
    {
        for (int i = 0; i < troupeCards.Count; i++)
        {
            Destroy(troupeCards[i]);
        }
        troupeCards.Clear();

        foreach (animalProperty animal in newAnimalOrder)
        {
            if (GlobalManager.instance.isAnimalUnlocked[animal.animalName])
            {
                GameObject newCard = Instantiate(troupeCardSimple, cardsGroup.transform);
                newCard.GetComponent<TroupeCardController>().Init(animal);

                troupeCards.Add(newCard);
            }
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
        troupeCardDetailed.ballActionName.text = theAnimalProperty.baseBallChange < 0 ? "LEFT" : theAnimalProperty.baseBallChange > 0 ? "RIGHT" : "SPECIAL";
        troupeCardDetailed.ballActionNum.text = Mathf.Abs(theAnimalProperty.baseBallChange).ToString();
        troupeCardDetailed.restTurn.text = theAnimalProperty.restTurn.ToString();
        troupeCardDetailed.scoreAction.text = theAnimalProperty.ReturnAllExplanation();

        troupeCardDetailed.ballActionImage.sprite = theAnimalProperty.baseBallChange < 0 ? troupeCardDetailed.spriteLeft
                                                  : theAnimalProperty.baseBallChange > 0 ? troupeCardDetailed.spriteRight : troupeCardDetailed.spriteSpecial;

        troupeCardDetailed.SetLineChart();
    }

    public void SetCardsBackground()
    {
        foreach (var card in troupeCards)
        {
            card.GetComponent<TroupeCardController>().bg.sprite = card.GetComponent<TroupeCardController>().bgSprites[0];
        }
        troupeCardSelected.GetComponent<TroupeCardController>().bg.sprite = troupeCardSelected.GetComponent<TroupeCardController>().bgSprites[1];
    }

    public void Enable()
    {
        GetComponent<Canvas>().enabled = true;

        if (GlobalManager.instance.currentLevelIndex != previousLevelIndex)
        {
            foreach (animalProperty animal in GlobalManager.instance.allAnimals.properies)
            {
                animalPriceChanges[animal.animalName].Add(GlobalManager.instance.animalPrices[animal.animalName]);
                if (animalPriceChanges[animal.animalName].Count > maxChartLength) animalPriceChanges[animal.animalName].RemoveAt(0);
            }
            GlobalManager.instance.SetAnimalPriceChanges(animalPriceChanges);
            previousLevelIndex = GlobalManager.instance.currentLevelIndex;
        }

        DisplayCards();
        coin = GlobalManager.instance.getCurCoinAmount();
        UpdateText();
    }

    public void Disable()
    {
        GlobalManager.instance.setCoinAmount(coin);
        AudioManagerScript.Instance.PlayUISound(AudioManagerScript.Instance.UI[0]);
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
            cardController.star.fillAmount = starFills[GlobalManager.instance.animalLevels[cardController.myAnimalProperty.animalName]];

            int animalCount = NumberInTroupe(cardController.myAnimalProperty.animalName);
            cardController.textNum.text = animalCount.ToString();

            /* Black Animals
            if (animalCount == 0) cardController.profile.color = Color.black; 
            else cardController.profile.color = Color.white;
            */
        }

        textCoin.text = $"{coin}";
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
        animalProperty animal = troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty;
        int price = GlobalManager.instance.animalPrices[animal.animalName];
        if (coin >= price)
        {
            coin -= price;
            GlobalManager.instance.addAnAnimal(troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty);
            UpdateText();
        }
        else CanvasMain.instance.DisplayWarning("Not enough coins!");
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
        else CanvasMain.instance.DisplayWarning("Cannot sell more animal of this kind!");
    }

    public void Upgrade()
    {
        animalProperty animal = troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty;
        if (GlobalManager.instance.animalLevels[animal.animalName] < GlobalManager.instance.maxLevel)
        {
            if (coin >= GetUpgradePrice(animal))
            {
                coin -= GetUpgradePrice(animal);
                GlobalManager.instance.UpdateLevel(animal.animalName, 1);
                UpdateText();
            }
            else CanvasMain.instance.DisplayWarning("Not enough coins!");
        }
        else CanvasMain.instance.DisplayWarning("Already reached max level!");

    }

    public int GetUpgradePrice(animalProperty property)
    {
        return (int)Mathf.Floor(upgradePrice * multis.multipliers[GlobalManager.instance.animalLevels[property.animalName] - 1]);
    }

    public void LoadGlobalSaveData(GlobalSaveData globalSaveData)
    {
        menuController = FindAnyObjectByType<MenuController>();

        slide.GetComponent<Slider>().onValueChanged.AddListener(SlideCards);

        newAnimalOrder = new List<animalProperty>();
        HashSet<string> animalNames = new HashSet<string>();
        foreach (var unlockData in DataManager.instance.unlockLoader.unlockData)
        {
            foreach (string animalName in unlockData.animalToUnlock)
            {
                if (!animalNames.Contains(animalName))
                {
                    animalProperty foundAnimal = System.Array.Find(GlobalManager.instance.allAnimals.properies, ap => ap.animalName == animalName);

                    if (foundAnimal != null)
                    {
                        newAnimalOrder.Add(foundAnimal);
                        animalNames.Add(animalName);
                    }
                    else
                    {
                        Debug.LogWarning($"Animal '{animalName}' not found in allAnimals.properies.");
                    }
                }
            }
        }

        Dictionary<string, List<int>> globalSaveDataAnimalPriceChanges = GlobalManager.instance.GetAnimalPriceChanges();
        if (globalSaveDataAnimalPriceChanges == null)
        {
            foreach (animalProperty animal in GlobalManager.instance.allAnimals.properies)
            {
                animalPriceChanges[animal.animalName] = new List<int>
                {
                    GlobalManager.instance.animalPrices[animal.animalName]
                };
            }
            GlobalManager.instance.SetAnimalPriceChanges(animalPriceChanges);
        }
        else
        {
            animalPriceChanges = globalSaveDataAnimalPriceChanges;
            previousLevelIndex = GlobalManager.instance.currentLevelIndex;
        }
    }
}
