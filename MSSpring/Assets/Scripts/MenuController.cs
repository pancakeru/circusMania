using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    TroupeController troupeController;
    ShopManager shopManager;
    //ShowManager showManager;
    public GameObject lvlsDisplay;
    [SerializeField] private TextMeshProUGUI coinDisplay;

    public GameObject managerPrefab;

    void Start()
    {
        //Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
        troupeController = FindFirstObjectByType<TroupeController>();
        shopManager = FindFirstObjectByType<ShopManager>();
        //showManager = FindFirstObjectByType<ShowManager>();

        troupeController.GetComponent<Canvas>().enabled = false;
        //shopManager.transform.parent.GetComponent<Canvas>().enabled = false;
        //showManager.gameObject.SetActive(false);
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

    /*
    public void ButtonShop()
    {
        shopManager.Enable();
        Disable();
    }
    */

    public void ButtonShow()
    {
        CallSound();
        BGMSwap("battle");
        Debug.Log("重新创建一个界面");
        GameObject instance = Instantiate(
            managerPrefab
            );
        instance.SetActive(true);
        instance.GetComponent<ShowManager>().EnterOneShow();
        Disable();
        
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

    public void CallSound() {
        GameObject audioObj = GameObject.FindWithTag("audio manager");
		audioObj.GetComponent<AudioManagerScript>().PlayUISound(audioObj.GetComponent<AudioManagerScript>().UI[0]);
    }

    public void BGMSwap(string name) {
        GameObject audioObj = GameObject.FindWithTag("audio manager");
        AudioManagerScript audioScript = audioObj.GetComponent<AudioManagerScript>();
		
        //audioScript.FadeOutEnvironment(2);

        switch (name) {
            case "battle":
                audioScript.PlayEnvironmentSound(audioScript.Environment[0]);
            break;

            case "idle":
                audioScript.PlayEnvironmentSound(audioScript.Environment[1]);
            break;

        }
    }
}
