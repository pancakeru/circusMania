using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;

public class MrShopManager : MonoBehaviour
{
    public static MrShopManager instance;

    [SerializeField] GameObject mrShopItemPrefab;
    [SerializeField] List<Sprite> ballSprites = new List<Sprite>();

    [HideInInspector] public List<GameObject> mrShopItems = new List<GameObject>();
    [HideInInspector] public Sprite myBallSprite;
    [HideInInspector] public List<BallInfo> ballInfos;

    MenuController menuController;
    Vector2 mrShopItemStartPos = new Vector2 (-750, 50);
    Vector2 mrShopItemPosOffset = new Vector2 (250, -100);

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        menuController = FindAnyObjectByType<MenuController>();

        ballInfos = new List<BallInfo>()
        {
            new BallInfo("Circus Ball", true, "You always have this.", ballSprites[0]),
            new BallInfo("Basketball", true, "???", ballSprites[1]),
            new BallInfo("Pixel Ball", true, "Finish the tutorial.", ballSprites[2]),
            new BallInfo("Tennis Ball", true, "Complete a level with at least 3 foxes.", ballSprites[3]),
            new BallInfo("Yarn Ball", true, "Complete a level with at least 3 lions.", ballSprites[4]),
            new BallInfo("Beach Ball", true, "Complete a level with at least 3 seals.", ballSprites[5]),
            new BallInfo("Chip Ball", true, "Upgrade 7 animals to max level.", ballSprites[6]),
        };

        myBallSprite = ballSprites[0];

        for (int i = 0; i < ballInfos.Count; i++)
        {
            GameObject newMrShopItem = Instantiate(mrShopItemPrefab, transform);
            newMrShopItem.GetComponent<RectTransform>().anchoredPosition = mrShopItemStartPos + new Vector2(mrShopItemPosOffset.x * i, mrShopItemPosOffset.y * (1 - (i % 2)));
            mrShopItems.Add(newMrShopItem);
        }
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
}

public class BallInfo
{
    public string ballName;
    public bool isUnlocked;
    public string unlockRequirement;
    public Sprite ballSprite;

    public BallInfo(string ballName, bool isUnlocked, string unlockRequirement, Sprite ballSprite)
    {
        this.ballName = ballName;
        this.isUnlocked = isUnlocked;
        this.unlockRequirement = unlockRequirement;
        this.ballSprite = ballSprite;

    }
}
