using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class iconAnimal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    //换动物图片
    private SpriteRenderer mySprite;
    private Image uiImage;
    private string animalType;
    private RectTransform myPosition;
    public int yGoal = -350;
    public animalProperty selfProperty;

    //idle state animation
    private Vector2 originalPosition;
    private Vector2 hoverPosition;
    private Vector2 halfPosition;
    private Vector2 dragOffset;
    private bool isHovered = false;
    private float hoverSpeed = 15f;
    private bool isDragging = false;
    private Canvas canvas;
    private GameObject showManager;
    private ShowManager showScript;

    private Vector2 lastMousePosition;
    private Vector2 velocity;
    private float smoothingFactor = 10f;
    private float friction = 0.3f;
    private float minVelocityThreshold = 0.1f; // Threshold to stop sliding

    //动物图片
    public List<Sprite> spriteList;
    public List<string> typeList;
    private Vector3 worldPosition;

    //查位置
    private GameObject[] areaDetectors;
    public int myIndex;
    public GameObject myNeighbor;
    public GameObject myOtherNeighbor;

    private float leftThreshold = -750;
    private float rightThreshold = 800;
    private Vector2 targetPosition;
    public float destinationX;
    public float otherDestinationX;
    public float lerpUpTime = 0.2f;
    private Vector2 UpPos;
    private float tempE = 0;
    private Vector2 startPos;
    private float downY;
    private float upY;
    private bool firstTime = true;

    public enum iconState {
        appear,
        selected,
        idle,
        half,
        sliding,
        disappear,
        callFactory,
        moving,
        newInsert,
        movingUp
    }

    private enum macroState
    {
        down,
        drag
    }

    private iconState currentState;

    void Start()
    {
        myPosition = this.GetComponentInChildren<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        showManager = GameObject.FindWithTag("showManager");
        showScript = showManager.GetComponent<ShowManager>();
        areaDetectors = GameObject.FindGameObjectsWithTag("areaTag");

        if (myIndex > 0 && myIndex < showScript.myHand.Count) {
            myNeighbor = showScript.myHand[myIndex - 1];

            if (myIndex == showScript.myHand.Count - 1) {
                myOtherNeighbor = null;
            } else {
                myOtherNeighbor = showScript.myHand[myIndex + 1];
            }
         } else {
            myNeighbor = null;
            myOtherNeighbor = showScript.myHand[myIndex + 1];
         }
    }

    //新的constructor，直接写动物种类
    public void Initialize(animalProperty property, bool insert)
    {
        selfProperty = property;
        animalType = property.name; //动物种类
        uiImage = GetComponentInChildren<Image>();
        uiImage.sprite = selfProperty.animalCoreImg;
        /*
        for (int i = 0; i < typeList.Count; i++) {
            if (animalType != null && typeList[i] == animalType) {
                uiImage.sprite = spriteList[i];
                break;
            } 
        }*/

        if (!insert) {
            currentState = iconState.appear;
        } else {
            currentState = iconState.newInsert;
        }
    }

    void Update()
    {
        switch (currentState) {
            case iconState.appear:
                //出现行为，从下面上来
                //Debug.Log("I am here");
                if (myPosition.anchoredPosition.y <= yGoal) {
                    myPosition.anchoredPosition += Vector2.up * 500 * Time.deltaTime;
                } else {
                    myPosition.anchoredPosition = new Vector2(myPosition.anchoredPosition.x,yGoal);
                    UpdateAnchors();
                    if (firstTime)
                    {
                        downY = myPosition.anchoredPosition.y - 200;
                        upY = myPosition.anchoredPosition.y;
                        firstTime = false;
                        
                    }
                    canBeSelect = true;
                    EnterState(iconState.idle);
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
                showScript.stopMoving = false;

                /*
                if (myIndex == 0 && myOtherNeighbor == null) {
                    myOtherNeighbor = showScript.myHand[1];
                }

                if (myIndex == showScript.myHand.Count - 1 && myPosition.anchoredPosition.x < 750 && showScript.myHand.Count >= 6) {
                    otherDestinationX = 750;
                    showScript.FixRightSpacing(myIndex);
                 }
                */
                
                if (isHovered) {
                    //myPosition.anchoredPosition = Vector2.Lerp(myPosition.anchoredPosition, hoverPosition, hoverSpeed * Time.deltaTime);
                    if (Input.GetKey(KeyCode.Mouse0)) {
                        //currentState = iconState.selected;
                    } 
                } else {
                    //myPosition.anchoredPosition = Vector2.Lerp(myPosition.anchoredPosition, originalPosition, hoverSpeed * Time.deltaTime);
                    if (Input.GetKey(KeyCode.Mouse0)) {
                        if (showScript.holding) {
                            //currentState = iconState.half;
                        } else {
                            //currentState = iconState.sliding;
                        }
                    } 
                }
                break;

            case iconState.half:
                //往下藏一半
                //TODO:把halfPos开放，之后可以调
                myPosition.anchoredPosition = Vector2.Lerp(myPosition.anchoredPosition, halfPosition, hoverSpeed * Time.deltaTime);
                if (Input.GetKeyUp(KeyCode.Mouse0)) {
                    currentState = iconState.idle;
                } 
                break;

            case iconState.sliding:
                if (Input.GetKey(KeyCode.Mouse0)) {
                    if (!showScript.holding) {
            // Dragging left/right
                    Vector2 mouseDelta = (Vector2)Input.mousePosition - lastMousePosition;

                    if(!showScript.stopMoving) {
                        velocity = Vector2.Lerp(velocity, mouseDelta, Time.deltaTime * smoothingFactor);
                    } else {
                        velocity = Vector2.zero;
                    }
    
                     }
                      } else {
                     velocity *= friction;
                            if (Mathf.Abs(velocity.x) < minVelocityThreshold) {
                         UpdateAnchors();
                         velocity = Vector2.zero;
                         currentState = iconState.idle;
                          }
                    }
    
        // Calculate new position before applying it
            Vector2 newPosition = myPosition.anchoredPosition + new Vector2(velocity.x, 0) * Time.deltaTime * 300f;
    
            // check if movement should be restricted
                 if (myIndex == 0 && velocity.x > 0 && newPosition.x > leftThreshold) {
                  newPosition.x = leftThreshold;
                    velocity.x = 0;
                    showScript.stopMoving = true;
                    }
                if (myIndex == showScript.myHand.Count - 1 && velocity.x < 0 && newPosition.x < rightThreshold) {
                  newPosition.x = rightThreshold;
                 velocity.x = 0;
                 showScript.stopMoving = true;
                 }

                myPosition.anchoredPosition = newPosition;
                break;

            case iconState.callFactory:
                showScript.AnimalFactory(animalType, worldPosition);
                showScript.myHand.Remove(showScript.myHand[myIndex]);
                Destroy(gameObject);
            break;

            case iconState.moving:

            myPosition.anchoredPosition = Vector2.Lerp(myPosition.anchoredPosition, targetPosition, hoverSpeed * Time.deltaTime);

        if (Vector2.Distance(myPosition.anchoredPosition, targetPosition) < 1f) {
            myPosition.anchoredPosition = targetPosition; 
            UpdateAnchors();
            currentState = iconState.idle;
        }

            break;

            case iconState.disappear:


            break;


            case iconState.newInsert:


            break;

            case iconState.movingUp:
                tempE += Time.deltaTime;
                float t = tempE / lerpUpTime;
                myPosition.anchoredPosition = Vector2.Lerp(startPos, UpPos, t);
                if (t >= 1)
                {
                    myPosition.anchoredPosition = UpPos;
                    EnterState(iconState.idle);
                }
                break;
        }

        lastMousePosition = Input.mousePosition;
    }

    public void StartMove(RectTransform target) {
        float targetX = target.anchoredPosition.x + showScript.offset;
        float targetY = yGoal;
         targetPosition = new Vector2(targetX, targetY);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        /*
        isDragging = true;
        showScript.holding = true;
        currentState = iconState.selected;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out dragOffset);
        */
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
        /*
    bool detected = false;

    foreach (GameObject area in areaDetectors)
    {
        RectTransform r = area.GetComponentInChildren<RectTransform>();
        
        if (r != null && RectTransformUtility.RectangleContainsScreenPoint(r, Input.mousePosition, canvas.worldCamera) && showScript.onStage[area.GetComponent<areaReport>().spotNum] == null)
        {
            worldPosition = area.GetComponent<areaReport>().myPosition;
            area.GetComponent<areaReport>().Report(animalType);
            currentState = iconState.callFactory;
            detected = true;
            break;
        } 

        if (!detected) {
            currentState = iconState.idle;
        }

    }
    
    isDragging = false;
    showScript.holding = false;
        */
}

    void UpdateAnchors() {
        originalPosition = myPosition.anchoredPosition;
        hoverPosition = originalPosition + Vector2.up * 100;
        halfPosition = originalPosition + Vector2.down * 200;

        if (myNeighbor != null) {
            destinationX = myNeighbor.GetComponent<RectTransform>().anchoredPosition.x;
        } else if (myIndex == 0 && myPosition.anchoredPosition.x > -750) {
            destinationX = -750;
        } else {
            destinationX = myPosition.anchoredPosition.x;
        }

        if (myOtherNeighbor != null) {
            otherDestinationX = myOtherNeighbor.GetComponent<RectTransform>().anchoredPosition.x;
        } else if (myIndex == showScript.myHand.Count - 1 && myPosition.anchoredPosition.x < 750) {
            otherDestinationX = 750;
        } else {
            otherDestinationX = myPosition.anchoredPosition.x;
        }
    }

public void UpdateDistance(float x, int i) {
    //targetPosition = new Vector2(x + i*showScript.offset, yGoal);
    targetPosition = new Vector2(destinationX, yGoal);
  //  Debug.Log($"Setting targetPosition for {myIndex}: {targetPosition}: {destinationX}");

    currentState = iconState.moving;
}

public void UpdateRight() {

    if (myOtherNeighbor != null) {
            otherDestinationX = myOtherNeighbor.GetComponent<RectTransform>().anchoredPosition.x;
        } else if (myIndex == showScript.myHand.Count - 1 && myPosition.anchoredPosition.x < 750) {
            otherDestinationX = 750;
        } else {
            otherDestinationX = myPosition.anchoredPosition.x;
        }

    targetPosition = new Vector2(otherDestinationX, yGoal);
    currentState = iconState.moving;
}

    void OnDestroy()
    {
        Debug.Log("Removed: " + myIndex);
        //showScript.UpdateHand(myIndex);
    }

    public void EnterState(iconState newState)
    {
        switch (currentState)
        {
            case iconState.half:
                UpPos = new Vector2(myPosition.anchoredPosition.x, upY);
                break;
        }
        switch (newState)
        {
            case iconState.idle:
                if (firstTime)
                {
                    downY = myPosition.anchoredPosition.y - 200;
                    upY = myPosition.anchoredPosition.y;
                    firstTime = false;
                }
                break;

            case iconState.half:
                if (firstTime)
                {
                    downY = myPosition.anchoredPosition.y - 200;
                    upY = myPosition.anchoredPosition.y;
                    firstTime = false;
                }
                halfPosition = new Vector2(myPosition.anchoredPosition.x, downY);
                break;

            case iconState.movingUp:
                UpPos = new Vector2(myPosition.anchoredPosition.x, upY);
                tempE = 0;
                
                startPos = myPosition.anchoredPosition;
                break;
        }
        currentState = newState;
    }

    public void SetSelectState(bool ifSelect)
    {
        uiImage.color = ifSelect ? Color.gray : Color.white;
        canGen = !ifSelect;
    }

    bool canBeSelect = false;
    public bool CanBeSelect()
    {
        return canBeSelect;
    }

    private bool canGen = true;
    public bool ifCanGenerate()
    {
        return canGen;
    }

    

}
