using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    TroupeController troupeController;
    ShopManager shopManager;

    void Start()
    {
        troupeController = FindFirstObjectByType<TroupeController>();
        shopManager = FindFirstObjectByType<ShopManager>();

        troupeController.GetComponent<Canvas>().enabled = false;
        shopManager.transform.parent.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonAnimalTroupe()
    {
        troupeController.Enable();
        Disable();
    }

    public void ButtonShop()
    {
        shopManager.Enable();
        Disable();
    }

    public void Enable()
    {
        GetComponent<Canvas>().enabled = true;
    }

    public void Disable()
    {
        GetComponent<Canvas>().enabled = false;
    }
}
