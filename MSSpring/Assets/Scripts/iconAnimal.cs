using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;

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
    private bool isHovered = false;
    private float hoverSpeed = 15f;
    private bool isDragging = false;
    private Canvas canvas;
    public GameObject showManager;
    private ShowManager showScript;

    private Vector2 lastMousePosition;
    private Vector2 velocity;
    private float smoothingFactor = 10f;
    private float friction = 0.3f;
    private float minVelocityThreshold = 0.1f; // Threshold to stop sliding

    //动物图片
    public List<Sprite> spriteList;
    public List<string> typeList;

    private enum iconState {
        appear,
        selected,
        idle,
        half,
        sliding,
        disappear
    }
    private iconState currentState;

    void Start()
    {
        myPosition = this.GetComponentInChildren<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        showScript = showManager.GetComponent<ShowManager>();
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
                    UpdateAnchors();
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
                        if (showScript.holding) {
                            currentState = iconState.half;
                        } else {
                            currentState = iconState.sliding;
                        }
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

            case iconState.sliding:
                if (Input.GetKey(KeyCode.Mouse0)) {
                    if (!showScript.holding) {
                        // Dragging left right
                        Vector2 mouseDelta = (Vector2)Input.mousePosition - lastMousePosition;
                        velocity = Vector2.Lerp(velocity, mouseDelta, Time.deltaTime * smoothingFactor);
                    }
                } else {
                    // friction to stop
                    velocity *= friction;
                    if (Mathf.Abs(velocity.x) < minVelocityThreshold) {
                        UpdateAnchors();
                        velocity = Vector2.zero;
                        currentState = iconState.idle;
                    }
                }
                myPosition.anchoredPosition += new Vector2(velocity.x, 0) * Time.deltaTime * 300f;
                break;
        }

        lastMousePosition = Input.mousePosition;
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
        showScript.holding = true;
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
        showScript.holding = false;
        currentState = iconState.idle;
    }

    void UpdateAnchors() {
        originalPosition = myPosition.anchoredPosition;
        hoverPosition = originalPosition + Vector2.up * 100;
        halfPosition = originalPosition + Vector2.down * 200;
    }
}
