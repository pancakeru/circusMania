using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class areaReport : MonoBehaviour
{
    public int spotNum;
    public Vector3 myPosition;

    void Start()
    {
        myPosition = this.GetComponentInChildren<RectTransform>().position;
        this.GetComponent<Transform>().position = myPosition;
    }

    void Update()
    {
        
    }

    public void Report() {

    }

}
