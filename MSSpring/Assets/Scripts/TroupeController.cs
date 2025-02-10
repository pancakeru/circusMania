using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TroupeController : MonoBehaviour
{
    public GameObject troupeCard;
    List<animalProperty> tempTroupe = new List<animalProperty>();
    List<GameObject> troupeCards = new List<GameObject>();

    Vector2 startPos = new Vector2 (-650f, 50f);
    int cardsPerRow = 4;

    void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            tempTroupe.Add(CreateTempAnimalInstance());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GetComponent<Canvas>().enabled = true;
            DisplayCards();
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
            GameObject newCard = Instantiate(troupeCard, transform.GetChild(0).transform);
            troupeCards.Add(newCard);
        }

        float width = troupeCards[0].GetComponent<RectTransform>().rect.width * 4.5f;
        float height = troupeCards[0].GetComponent<RectTransform>().rect.height * 5.5f;

        for (int i = 0; i < troupeCards.Count; i++)
        {
            troupeCards[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(startPos.x + (i % cardsPerRow) * width, startPos.y - (i / cardsPerRow) * height);
        }
    }

    public void DisableSelf()
    {
        GetComponent<Canvas>().enabled = false;
    }

    private animalProperty CreateTempAnimalInstance()
    {
        animalProperty newAnimal = ScriptableObject.CreateInstance<animalProperty>();

        newAnimal.animalName = "none";
        newAnimal.animalCoreImg = null;
        newAnimal.animalPrice = 0;
        newAnimal.baseYellowChange = 0f;
        newAnimal.baseRedChange = 0f;
        newAnimal.baseBlueChange = 0f;
        newAnimal.restTurn = 0;
        newAnimal.scoreActionTemplate = "none";
        newAnimal.amount1 = 0;
        newAnimal.amount2 = 0;
        newAnimal.amount3 = 0;
        newAnimal.ballActionTemplate = "none";
        newAnimal.amount4 = 0;
        newAnimal.amount5 = 0;
        newAnimal.amount6 = 0;

        return newAnimal;
    }
}
