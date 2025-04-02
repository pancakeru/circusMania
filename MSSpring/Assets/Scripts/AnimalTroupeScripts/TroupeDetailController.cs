using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TroupeDetailController : MonoBehaviour
{
    public TroupeLineChart troupeLineChart;
    public Image coreImage;
    public TextMeshProUGUI animalName;
    public TextMeshProUGUI ballAction;
    public TextMeshProUGUI restTurn;
    public TextMeshProUGUI scoreAction;
    public TextMeshProUGUI numInTroupe;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLineChart()
    {
        troupeLineChart.points = new List<Vector2>()
        {
            new Vector2(0f, 2f),
            new Vector2(1f, 1f),
            new Vector2(2f, 3f),
            new Vector2(3f, 1f),
            new Vector2(4f, 2f),
        };

        troupeLineChart.SetAllDirty();
    }
}
