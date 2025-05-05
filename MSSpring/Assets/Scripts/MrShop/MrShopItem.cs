using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MrShopItem : MonoBehaviour
{
    public Image ballImage;
    public TextMeshProUGUI ballName;
    public TextMeshProUGUI ballRequirement;

    public GameObject locked;
    public Image ballImageSelected;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBallInfo(BallInfo ballInfo)
    {
        ballImageSelected.enabled = (MrShopManager.instance.myBallSprite == ballImage.sprite);
        locked.gameObject.SetActive(!ballInfo.isUnlocked);
        ballImage.sprite = ballInfo.ballSprite;
        ballName.text = ballInfo.ballName;
        ballRequirement.text = ballInfo.unlockRequirement;
    }

    public void SetMyBall()
    {
        MrShopManager.instance.myBallSprite = ballImage.sprite;
        MrShopManager.instance.SetBallsInfo();
    }
}
