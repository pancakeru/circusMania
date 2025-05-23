using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class MrShopManager : MonoBehaviour, ISaveData
{
    public static MrShopManager instance;

    [SerializeField] GameObject mrShopItemPrefab;
    [SerializeField] List<Sprite> ballSprites = new List<Sprite>();
    [SerializeField] GameObject notification;

    [HideInInspector] public List<GameObject> mrShopItems = new List<GameObject>();
    [HideInInspector] public Sprite myBallSprite;
    [HideInInspector] public List<BallInfo> ballInfos;

    MenuController menuController;
    Vector2 mrShopItemStartPos = new Vector2(-750, 50);
    Vector2 mrShopItemPosOffset = new Vector2(250, -100);

    private bool hasInstantiated = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void Enable()
    {
        GetComponent<Canvas>().enabled = true;

        SetBallsInfo();
    }

    public void SetBallsInfo()
    {
        for (int i = 0; i < mrShopItems.Count; i++)
        {
            mrShopItems[i].GetComponent<MrShopItem>().SetBallInfo(ballInfos[i]);
        }
    }

    public void Disable()
    {
        AudioManagerScript.Instance.PlayUISound(AudioManagerScript.Instance.UI[0]);
        GetComponent<Canvas>().enabled = false;
        menuController.Enable();
    }

    public void AchievementUnlocked(int achievementIndex)
    {
        if (!ballInfos[achievementIndex].isUnlocked)
        {
            ballInfos[achievementIndex].isUnlocked = true;
            GameObject newNotification = Instantiate(notification, CanvasMain.instance.transform);
            newNotification.GetComponent<NotificationController>().ballImage.sprite = ballInfos[achievementIndex].ballSprite;

            GlobalManager.instance.SetBallInfoList(ballInfos);

            if (achievementIndex == 1) ballInfos[achievementIndex].unlockRequirement = "Interact with the chicken for 7 times.";
        }
    }

    public void LoadGlobalSaveData(GlobalSaveData globalSaveData)
    {
        if (globalSaveData.ballInfoList.Count > 0)
        {
            ballInfos = globalSaveData.ballInfoList;
            for (int i = 0; i < ballInfos.Count; i++)
            {
                BallInfo ballInfo = ballInfos[i];
                ballInfo.ballSprite = ballSprites[i];
            }
            globalSaveData.ballInfoList = ballInfos;
        }
        else
        {
            ballInfos = new List<BallInfo>(){
                new BallInfo("Circus Ball", true, "You always have this.", ballSprites[0]),
                new BallInfo("Basketball", false, "???", ballSprites[1]),
                new BallInfo("Pixel Ball", false, "Finish the tutorial.", ballSprites[2]),
                new BallInfo("Tennis Ball", false, "Finish the level 5.", ballSprites[3]),
                new BallInfo("Yarn Ball", false, "Reach 1,200 Popularity in a level.", ballSprites[4]),
                new BallInfo("Beach Ball", false, "Have all kinds of animals.", ballSprites[5]),
                new BallInfo("Chip Ball", false, "Upgrade 7 animals to max level.", ballSprites[6]),
            };
            GlobalManager.instance.SetBallInfoList(ballInfos);
        }

        menuController = FindAnyObjectByType<MenuController>();

        myBallSprite = ballSprites[GlobalManager.instance.GetMyBallIndex()];

        if (!hasInstantiated)
        {
            for (int i = 0; i < ballInfos.Count; i++)
            {
                GameObject newMrShopItem = Instantiate(mrShopItemPrefab, transform);
                newMrShopItem.GetComponent<RectTransform>().anchoredPosition = mrShopItemStartPos + new Vector2(mrShopItemPosOffset.x * i, mrShopItemPosOffset.y * (1 - (i % 2)));
                mrShopItems.Add(newMrShopItem);
            }
            hasInstantiated = true;
        }

        SetBallsInfo();
    }
}

[System.Serializable]
public class BallInfo
{
    public string ballName;
    public bool isUnlocked;
    public string unlockRequirement;
    [JsonIgnore] public Sprite ballSprite;

    public BallInfo(string ballName, bool isUnlocked, string unlockRequirement, Sprite ballSprite)
    {
        this.ballName = ballName;
        this.isUnlocked = isUnlocked;
        this.unlockRequirement = unlockRequirement;
        this.ballSprite = ballSprite;
    }
}
