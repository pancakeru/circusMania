using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuController : MonoBehaviour, ISaveData
{
    TroupeController troupeController;
    MrShopManager shopManager;
    //ShowManager showManager;
    public GameObject lvlsDisplay;
    [SerializeField] private TextMeshProUGUI coinDisplay;
    [SerializeField] private TextMeshProUGUI levelDisplay;

    public GameObject managerPrefab;

    public GameObject shopButton;

    public StartScreenManager startScreen;

    void Start()
    {
        troupeController = FindFirstObjectByType<TroupeController>();
        shopManager = FindFirstObjectByType<MrShopManager>();

        troupeController.GetComponent<Canvas>().enabled = false;
        shopManager.transform.GetComponent<Canvas>().enabled = false;

        lvlsDisplay.SetActive(false);

        Enable();
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

    public void ButtonShow(bool isTutorial)
    {
        CallSound();
        BGMSwap("battle");
        //Debug.Log("重新创建一个界面");
        GameObject instance = Instantiate(
            managerPrefab
            );
        instance.SetActive(true);
        instance.GetComponent<ShowManager>().EnterOneShow(isTutorial);
        Disable();

    }

    public void Enable()
    {
        //设置开头的状态
        if (lvlsDisplay.activeSelf)
        {
            ShowLevels();
        }
        coinDisplay.text = GlobalManager.instance.getCurCoinAmount().ToString();
        levelDisplay.text = GlobalManager.instance.currentLevelIndex + " / 8";
        GetComponent<Canvas>().enabled = true;

        /*
        bool isAllAnimalUnlocked = true;
        foreach (animalProperty animal in GlobalManager.instance.allAnimals.properies)
        {
            if (!GlobalManager.instance.isAnimalUnlocked[animal.animalName])
            {
                isAllAnimalUnlocked = false;
                break;
            }
        }
        shopButton.SetActive(isAllAnimalUnlocked);
        */

        TryUnlockShopButton();
    }

    public void TryUnlockShopButton()
    {
        if (GlobalManager.instance.currentLevelIndex >= 2)
        {
            shopButton.GetComponent<Button>().enabled = true;
            shopButton.transform.GetChild(0).gameObject.SetActive(false);
            shopButton.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            shopButton.GetComponent<Button>().enabled = false;
            shopButton.transform.GetChild(0).gameObject.SetActive(true);
            shopButton.transform.GetChild(1).gameObject.SetActive(true);
            CanvasMain.instance.ShowPopUp(shopButton.transform.GetChild(0).GetComponent<Image>(), "Mr. Shop will not be present until two shows are completed.");
        }
    }

    public void Disable()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void ShowLevels()
    {
        if (lvlsDisplay.activeSelf)
        {
            lvlsDisplay.SetActive(false);
        }
        else
        {
            lvlsDisplay.SetActive(true);
        }

    }

    public void CallSound()
    {
        GameObject audioObj = GameObject.FindWithTag("audio manager");
        audioObj.GetComponent<AudioManagerScript>().PlayUISound(audioObj.GetComponent<AudioManagerScript>().UI[0]);
    }

    public void BGMSwap(string name)
    {
        GameObject audioObj = GameObject.FindWithTag("audio manager");
        AudioManagerScript audioScript = audioObj.GetComponent<AudioManagerScript>();

        //audioScript.FadeOutEnvironment(2);

        switch (name)
        {
            case "battle":
                audioScript.PlayEnvironmentSound(audioScript.Environment[0]);
                break;

            case "idle":
                audioScript.PlayEnvironmentSound(audioScript.Environment[1]);
                break;

        }
    }

    public void LoadGlobalSaveData(GlobalSaveData globalSaveData)
    {
        coinDisplay.text = GlobalManager.instance.getCurCoinAmount().ToString();
        TryUnlockShopButton();
    }
}
