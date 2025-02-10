using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class iconAnimal : MonoBehaviour
{
    //换动物图片
    private SpriteRenderer mySprite;
    private Image uiImage;
    private string animalType;
    private RectTransform myPosition;
    private int yGoal = -350;

    //idle state animation
    private Vector2 originalPosition;
    private Vector2 hoverPosition;
    private bool isHovered = false;
    private float hoverSpeed = 200f;

    //动物图片
    public List<Sprite> spriteList;
    public List<string> typeList;

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
        myPosition = this.GetComponentInChildren<RectTransform>();
    }

    //新的constructor，直接写动物种类
    public void Initialize(string type)
    {
        animalType = type; //动物种类

      //  mySprite = this.GetComponent<SpriteRenderer>();
        uiImage = GetComponentInChildren<Image>();

        for (int i = 0; i < typeList.Count; i++) {
            if (animalType != null && typeList[i] == animalType) {
               // mySprite.sprite = spriteList[i];
                uiImage.sprite = spriteList[i];
                break;
                //Debug.Log(spriteList[i]);
            } else {
                Debug.Log("animal type is: " + animalType);
            }
        }

        currentState = iconState.appear;

        
    }

    void Update()
    {
        switch (currentState) {
            case iconState.appear:
                //出现行为，从下面上来
                if (myPosition.anchoredPosition.y <= yGoal) {
                    myPosition.anchoredPosition += Vector2.up * 500 * Time.deltaTime;
                } else {
                    originalPosition = myPosition.anchoredPosition;
                    hoverPosition = originalPosition + Vector2.up * 10;
                    this.currentState = iconState.idle;
                }

            break;

            case iconState.selected:
                //跟着mouse，查位置




            break;

            case iconState.idle:
                //animation
                //玩着还没选
                if (isHovered) {
                    myPosition.anchoredPosition = Vector2.Lerp(myPosition.anchoredPosition, hoverPosition, hoverSpeed * Time.deltaTime);
                } else {
                    myPosition.anchoredPosition = Vector2.Lerp(myPosition.anchoredPosition, originalPosition, hoverSpeed * Time.deltaTime);
                }

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
        isHovered = true;
        Debug.Log("hovered");
    }

    void OnMouseExit() {
        //animation (idle state)
        isHovered = false;
        Debug.Log("not hovered");
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("hovered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("no hover");
    }

    void OnMouseDrag() {
        //这个跟着mouse (selected state)
    }

    void OnMouseUp() {
        //查位置
        //make prefab or reset list
    }

}
