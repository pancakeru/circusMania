using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaReport : MonoBehaviour
{
    public int spotNum;
    public Vector3 myPosition;
    private Canvas canvas;

    void Start()
{
    canvas = this.GetComponentInParent<Canvas>();
    RectTransform rectTransform = this.GetComponentInChildren<RectTransform>();

    if (canvas != null && rectTransform != null)
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, rectTransform.position);
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, screenPoint, Camera.main, out worldPoint);
        
        myPosition = worldPoint;
    }
}


    void Update()
    {
        
    }

    public void Report() {

    }

}
