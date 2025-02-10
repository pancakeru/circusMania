using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class iconDetect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("hovered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("no hover");
    }

    void OnMouseEnter() {
        //animation (idle state)
        Debug.Log("hovered");
    }

    void OnMouseExit() {
        //animation (idle state)
        Debug.Log("not hovered");
    }

    

}
