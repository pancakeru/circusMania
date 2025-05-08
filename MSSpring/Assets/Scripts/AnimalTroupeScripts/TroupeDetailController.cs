using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

public class TroupeDetailController : MonoBehaviour
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

    List<GameObject> lineChartIcons = new List<GameObject>();
    float lengthFix = 0.5f;
    int maxChartHeight = 2;

    Vector2 lineChartIconBasePos = Vector2.one * 9000;

    void Start()
    {
        troupeController = transform.parent.GetComponent<TroupeController>();

        for (int i = 0; i < troupeController.maxChartLength; i++)
        {
            GameObject newIcon = Instantiate(lineChartIcon, troupeLineChart.transform);
            newIcon.GetComponent<RectTransform>().anchoredPosition = lineChartIconBasePos;
            lineChartIcons.Add(newIcon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowManager.instance.GetComponent<ShowAnimalBallPassTimesCounter>().monkey = 10;
            SetLineChart();
        }*/
    }

    public void SetLineChart()
    {
        string theAnimalName = troupeController.troupeCardSelected.GetComponent<TroupeCardController>().myAnimalProperty.animalName;

        List<Vector2> ballPassTimesToVertex = new List<Vector2>();
        for (int i = 0; i < troupeController.animalPriceChanges[theAnimalName].Count; i++)
        {
            int clamped = Mathf.Min(troupeController.animalPriceChanges[theAnimalName][i], GlobalManager.instance.maxPrice);
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

}
