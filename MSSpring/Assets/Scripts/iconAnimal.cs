using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iconAnimal : MonoBehaviour
{
    //换动物图片
    private SpriteRenderer mySprite;
    public Image uiImage;
    private string animalType;

    private enum iconState {
        appear,
        selected,
        idle,
        half,
        disappear
    }
    private iconState currentState;


    void Start()
    {
        mySprite = this.GetComponent<SpriteRenderer>();

    }

    //新的constructor，直接写动物种类
    public void Initialize(string type)
    {
        animalType = type; //动物种类
        LoadSprite(type);
    }

    //换成适合的图片
    private void LoadSprite(string type)
    {
        /*
        Sprite loadedSprite = //type 的图片
        mySprite.sprite = loadedSprite;
        */
    }

    void Update()
    {
        switch (currentState) {
            case iconState.appear:
                //出现行为，从下面上来
            break;

            case iconState.selected:
                //跟着mouse，查位置
            break;

            case iconState.idle:
                //animation
                //玩着还没选
            break;

            case iconState.half:
                //往下藏一半
            break;

            case iconState.disappear:
                //不见了
            break;
        }
    }

    void OnMouseEnter() {
        //animation (idle state)
    }

    void OnMouseExit() {
        //animation (idle state)
    }

    void OnMouseDrag() {
        //这个跟着mouse (selected state)
    }

    void OnMouseUp() {
        //查位置
        //make prefab or reset list
    }

}
