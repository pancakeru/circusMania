using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (transform.parent.GetComponent<MenuController>() == null)
        {
            if (transform.parent.GetComponent<TroupeController>() != null) transform.parent.GetComponent<TroupeController>().Disable();
            if (transform.parent.GetComponent<ShopManager>() != null) transform.parent.GetComponent<ShopManager>().Disable();
        }
        else
        {
            transform.parent.GetComponent<MenuController>().startScreen.Enable();
        } 
    }
}
