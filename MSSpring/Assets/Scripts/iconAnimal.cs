using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class iconAnimal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
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
    private Vector2 halfPosition;
    private Vector2 dragOffset;
    public bool isHovered = false;
    private float hoverSpeed = 15f;
    private bool isDragging = false;
    private Canvas canvas;

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
        canvas = GetComponentInParent<Canvas>();
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
                    hoverPosition = originalPosition + Vector2.up * 100;
                    halfPosition = originalPosition + Vector2.down * 200;
                    this.currentState = iconState.idle;
                }
                break;

            case iconState.selected:
                //跟着mouse，查位置
                if (isDragging)
                {
                    Vector2 localPoint;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out localPoint);
                    myPosition.anchoredPosition = localPoint;
                }
                break;

            case iconState.idle:
                //animation
                //玩着还没选
                if (isHovered) {
                    myPosition.anchoredPosition = Vector2.Lerp(myPosition.anchoredPosition, hoverPosition, hoverSpeed * Time.deltaTime);
                    if (Input.GetKey(KeyCode.Mouse0)) {
                        currentState = iconState.selected;
                    } 
                } else {
                    myPosition.anchoredPosition = Vector2.Lerp(myPosition.anchoredPosition, originalPosition, hoverSpeed * Time.deltaTime);
                    if (Input.GetKey(KeyCode.Mouse0)) {
                        currentState = iconState.half;
                    } 
                }
                break;

            case iconState.half:
                //往下藏一半
                myPosition.anchoredPosition = Vector2.Lerp(myPosition.anchoredPosition, halfPosition, hoverSpeed * Time.deltaTime);
                if (Input.GetKeyUp(KeyCode.Mouse0)) {
                    currentState = iconState.idle;
                } 
                break;

            case iconState.disappear:
                //不见了
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        //Debug.Log("hovered");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
       // Debug.Log("no hover");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        currentState = iconState.selected;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out dragOffset);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == iconState.selected)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out localPoint);
            myPosition.anchoredPosition = localPoint - dragOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        currentState = iconState.idle;
    }
}
