using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TroupeDetailController : MonoBehaviour, ISaveData
{
    TroupeController troupeController;
    public TroupeLineChart troupeLineChart;
    public Image coreImage;
    public TextMeshProUGUI animalName;
    public TextMeshProUGUI ballActionName;
    public TextMeshProUGUI ballActionNum;
    public TextMeshProUGUI restTurn;
    public TextMeshProUGUI scoreAction;
    public GameObject lineChartIcon;

    [Header("Icon Sprites")]
    public Image ballActionImage;
    public Sprite spriteLeft;
    public Sprite spriteRight;
    public Sprite spriteSpecial;
    public Sprite spriteRest;

    Dictionary<string, List<int>> animalPriceChanges = new Dictionary<string, List<int>>();
    List<GameObject> lineChartIcons = new List<GameObject>();
    int maxChartLength = 5;
    float lengthFix = 0.5f;
    int maxChartHeight = 2;

    Vector2 lineChartIconBasePos = Vector2.one * 9000;

    public void SetLineChart()
    {
        string theAnimalName = troupeController.troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty.animalName;

        if (animalPriceChanges[theAnimalName][animalPriceChanges[theAnimalName].Count - 1] != GlobalManager.instance.animalPrices[theAnimalName])
        {
            animalPriceChanges[theAnimalName].Add(GlobalManager.instance.animalPrices[theAnimalName]);
            if (animalPriceChanges[theAnimalName].Count > maxChartLength) animalPriceChanges[theAnimalName].RemoveAt(0);
        }

        List<Vector2> ballPassTimesToVertex = new List<Vector2>();
        for (int i = 0; i < animalPriceChanges[theAnimalName].Count; i++)
        {
            int clamped = Mathf.Min(animalPriceChanges[theAnimalName][i], GlobalManager.instance.maxPrice);
            ballPassTimesToVertex.Add(new Vector2(i * lengthFix, (float)clamped * maxChartHeight / GlobalManager.instance.maxPrice));

        }
        troupeLineChart.points = ballPassTimesToVertex;
        troupeLineChart.SetAllDirty(); //Draw Line

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


    }

    public void LoadGlobalSaveData(GlobalSaveData globalSaveData)
    {
        foreach (animalProperty animal in GlobalManager.instance.allAnimals.properies)
        {
            animalPriceChanges[animal.animalName] = new List<int>
            {
                GlobalManager.instance.animalPrices[animal.animalName]
            };
        }

        for (int i = 0; i < maxChartLength; i++)
        {
            GameObject newIcon = Instantiate(lineChartIcon, troupeLineChart.transform);
            newIcon.GetComponent<RectTransform>().anchoredPosition = lineChartIconBasePos;
            lineChartIcons.Add(newIcon);
        }

        troupeController = transform.parent.GetComponent<TroupeController>();
    }
}
