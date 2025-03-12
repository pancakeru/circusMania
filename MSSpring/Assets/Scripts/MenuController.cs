using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    TroupeController troupeController;
    ShopManager shopManager;
    ShowManager showManager;
    public GameObject lvlsDisplay;
    [SerializeField] private TextMeshProUGUI coinDisplay;

    void Start()
    {
        //Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        troupeController = FindFirstObjectByType<TroupeController>();
        shopManager = FindFirstObjectByType<ShopManager>();
        showManager = FindFirstObjectByType<ShowManager>();

        troupeController.GetComponent<Canvas>().enabled = false;
        shopManager.transform.parent.GetComponent<Canvas>().enabled = false;
        showManager.gameObject.SetActive(false);
        coinDisplay.text = "Coin: " + GlobalManager.instance.getCurCoinAmount();
        lvlsDisplay.SetActive(false);
        

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

    public void ButtonShow()
    {
        showManager.gameObject.SetActive(true);
        Disable();
        showManager.EnterOneShow();
    }

    public void Enable()
    {
        //设置开头的状态
        if (lvlsDisplay.activeSelf)
        {
            ShowLevels();
        }
        coinDisplay.text = "Coin: " + GlobalManager.instance.getCurCoinAmount();
        GetComponent<Canvas>().enabled = true;
    }

    public void Disable()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void ShowLevels() {
       if (lvlsDisplay.activeSelf) {
        lvlsDisplay.SetActive(false);
       } else {
        lvlsDisplay.SetActive(true);
       }

    }
}
