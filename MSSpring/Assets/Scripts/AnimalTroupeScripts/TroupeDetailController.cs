using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq.Expressions;

public class TroupeDetailController : MonoBehaviour
{
    TroupeController troupeController;
    public TroupeLineChart troupeLineChart;
    public Image coreImage;
    public TextMeshProUGUI animalName;
    public TextMeshProUGUI ballAction;
    public TextMeshProUGUI restTurn;
    public TextMeshProUGUI scoreAction;
    public TextMeshProUGUI numInTroupe;
    public GameObject lineChartIcon;

    Dictionary<string, List<int>> animalPriceChanges = new Dictionary<string, List<int>>();
    List<GameObject> lineChartIcons = new List<GameObject>();
    int maxLengthOfChart = 5;

    Vector2 lineChartIconBasePos = Vector2.one * 9000;

    void Start()
    {
        foreach (animalProperty animal in GlobalManager.instance.allAnimals.properies)
        {
            animalPriceChanges[animal.animalName] = new List<int>
            {
                GlobalManager.instance.animalPrices[animal.animalName]
            };
        }

        for (int i = 0; i < maxLengthOfChart; i++)
        {
            GameObject newIcon = Instantiate(lineChartIcon, troupeLineChart.transform);
            newIcon.GetComponent<RectTransform>().anchoredPosition = lineChartIconBasePos;
            lineChartIcons.Add(newIcon);
        }

        troupeController = transform.parent.GetComponent<TroupeController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetLineChart()
    {
        AnimalBallPassTimes allAnimalBallPassTimes = ShowManager.instance.GetComponent<ShowAnimalBallPassTimesCounter>().GenerateAnimalBallPassTimes();
        string theAnimalName = troupeController.troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty.animalName;
        float newBallPassTimes = 0;

        switch (theAnimalName.ToLower())
        {
            case "monkey": newBallPassTimes = allAnimalBallPassTimes.monkey; break;
            case "elephant": newBallPassTimes = allAnimalBallPassTimes.elephant; break;
            case "bear": newBallPassTimes = allAnimalBallPassTimes.bear; break;
            case "lion": newBallPassTimes = allAnimalBallPassTimes.lion; break;
            case "giraffe": newBallPassTimes = allAnimalBallPassTimes.giraffe; break;
            case "snake": newBallPassTimes = allAnimalBallPassTimes.snake; break;
            case "fox": newBallPassTimes = allAnimalBallPassTimes.fox; break;
            case "seal": newBallPassTimes = allAnimalBallPassTimes.seal; break;
            case "ostrich": newBallPassTimes = allAnimalBallPassTimes.ostrich; break;
            case "kangaroo": newBallPassTimes = allAnimalBallPassTimes.kangaroo; break;
            case "buffalo": newBallPassTimes = allAnimalBallPassTimes.buffalo; break;
            case "goat": newBallPassTimes = allAnimalBallPassTimes.goat; break;
            case "lizard": newBallPassTimes = allAnimalBallPassTimes.lizard; break;
            default:
                Debug.LogWarning($"Bug Found"); break;
        }

        int newPrice = (int)newBallPassTimes / maxLengthOfChart + GlobalManager.instance.initPrice;

        if (!(newPrice == animalPriceChanges[theAnimalName][animalPriceChanges[theAnimalName].Count - 1])) //if current price != previous price
        {
            animalPriceChanges[theAnimalName].Add((int)(newBallPassTimes / maxLengthOfChart));
            if (animalPriceChanges[theAnimalName].Count > maxLengthOfChart) animalPriceChanges[theAnimalName].RemoveAt(0);
        }

        List<Vector2> ballPassTimesToVertex = new List<Vector2>();
        for (int i = 0; i < animalPriceChanges[theAnimalName].Count; i++)
        {
            ballPassTimesToVertex.Add(new Vector2(i, animalPriceChanges[theAnimalName][i]));

        }
        troupeLineChart.points = ballPassTimesToVertex;


        Rect chartRect = troupeLineChart.rectTransform.rect;
        for (int i = 0; i < lineChartIcons.Count; i++)
        {
            RectTransform iconRect = lineChartIcons[i].GetComponent<RectTransform>();

            if (i < ballPassTimesToVertex.Count)
            {
                Vector2 point = ballPassTimesToVertex[i];
                Vector2 anchored = new Vector2(point.x * chartRect.width, point.y * chartRect.height);
                iconRect.anchoredPosition = anchored;
            }
            else
            {
                iconRect.anchoredPosition = lineChartIconBasePos;
            }
        }

        troupeLineChart.SetAllDirty();
    }

}
