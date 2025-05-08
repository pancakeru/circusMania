using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExplainingCardController : MonoBehaviour
{
    [SerializeField] Image profile;
    [SerializeField] Image star;
    [SerializeField] TextMeshProUGUI direction;
    [SerializeField] TextMeshProUGUI rest;
    [SerializeField] TextMeshProUGUI animalName;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] Image background;
    [SerializeField] List<Sprite> bgSprites = new List<Sprite>();

    float width = 150;
    float up = 220;

    float[] starFills = new float[] { 0, 0.2f, 0.4f, 0.6f, 0.81f, 1f };

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetValue(animalProperty theAnimalProperty)
    {
        profile.sprite = theAnimalProperty.animalCoreImg;
        animalName.text = theAnimalProperty.animalName;
        string directionDirection = theAnimalProperty.baseBallChange < 0 ? "LEFT" : theAnimalProperty.baseBallChange > 0 ? "RIGHT" : "SPECIAL";
        direction.text = $"{directionDirection} {Mathf.Abs(theAnimalProperty.baseBallChange)}";
        rest.text = theAnimalProperty.restTurn.ToString();
        description.text = theAnimalProperty.ReturnSimpleExplanation();
        star.fillAmount = starFills[GlobalManager.instance.animalLevels[theAnimalProperty.animalName]];
        background.sprite = theAnimalProperty.baseRedChange > 0 ? bgSprites[0]
                          : theAnimalProperty.baseYellowChange > 0 ? bgSprites[1]
                          : theAnimalProperty.baseBlueChange > 0 ? bgSprites[2]
                          : theAnimalProperty.animalCoreImg;
    }

    public void StartExplain(RectTransform target, bool IfDown, animalProperty property)
    {
        gameObject.SetActive(true);

        Vector3 targetPosition = target.position;
        float screenMidX = Screen.width / 2f;

        if (IfDown)
        {
            transform.position = new Vector3(targetPosition.x, targetPosition.y + up, targetPosition.z);
        }
        else
        {
            if (targetPosition.x < screenMidX)
            {
                transform.position = new Vector3(targetPosition.x + width, targetPosition.y, targetPosition.z);
            }
            else
            {
                transform.position = new Vector3(targetPosition.x - width, targetPosition.y, targetPosition.z);
            }
        }

        SetValue(property);
    }

    public void StartExplain(Vector3 worldPosition, bool IfDown, animalProperty property)
    {
        gameObject.SetActive(true);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        float screenMidX = Screen.width / 2f;

        if (IfDown)
        {
            transform.position = new Vector3(screenPosition.x, screenPosition.y + up, screenPosition.z);
        }
        else
        {
            if (screenPosition.x < screenMidX)
            {
                transform.position = new Vector3(screenPosition.x + width, screenPosition.y, screenPosition.z);
            }
            else
            {
                transform.position = new Vector3(screenPosition.x - width, screenPosition.y, screenPosition.z);
            }
        }

        SetValue(property);
    }

    public void DoneExplain()
    {
        gameObject.SetActive(false);
    }
}
