using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ShowManager : MonoBehaviour
{
    //State Machine
    private enum ShowStates {
        SelectAnimal,
        Animation,
        Performance
    }


    public enum DecideScreenState
    {
        empty,
        slide,
        choose
    }
    //State变量
    private ShowStates currentState;

    public GameObject animalIcon;
    public GameObject areaPrefab;
    public Transform canvasTransform;

    private float y;
    private float yStart;
    private float x;
    public float offset;
    private float areaOffset;

    public bool holding = false;
    public bool stopMoving = false;

    private List<animalProperty> testList;
    public List<GameObject> animalPerformancePrefabs;

    public List<GameObject> myHand;
    public String[] onStage;

    private float leftAnchorX;
    private List<Vector2> initialPos = new List<Vector2>();

    public GraphicRaycaster uiRaycaster;
    private EventSystem eventSystem;
    private bool sliding = false;
    private Vector2 lastMousePosition;
    private List<iconAnimal> myHandControls = new List<iconAnimal>();
    private GameObject firstDetect;
    private bool canBeMovedOrSelected = true;
    private bool enterInteraction = false;
    private GameObject holdingAnimalObj;

    public animalProperty testProperty;
    void Start()
    {
        testList = new List<animalProperty>();
        x = -750;
        offset = 300;
        yStart = -600;
        areaOffset = 2;

        onStage = new String[6];

        //位置 GameObject
        for (int i = 0; i < 6; i++) {
            GameObject temp = Instantiate(areaPrefab, canvasTransform);
            temp.GetComponent<areaReport>().spotNum = i;
            temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-5 + areaOffset*i, 0);
        }

        //GlobalManager做完后把这个搬到 SelectAnimal
        //取animalProperty list 的 animalName
        for (int i = 0; i < 12; i++) {
            GameObject temp = Instantiate(animalIcon, canvasTransform);
            myHand.Add(temp);
           // Debug.Log($"Added object to myHand. Current count: {myHand.Count}");
            temp.GetComponent<iconAnimal>().Initialize(testProperty, false);
            temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(x + offset*i, yStart);
            temp.GetComponent<iconAnimal>().myIndex = i;
            //TODO:把这个目标位置整合
            initialPos.Add(new Vector2(x + offset * i, -350));
            myHandControls.Add(temp.GetComponent<iconAnimal>());
        }

        //FOR ADDING BACK TO DECK
        //instantiate a new iconAnimal prefab on the performance animal
        //hide the sprite of the perfomance animal and only destroy obj if add condition is valid
        //List.Insert(index, obj)
        //when adding back to deck, check the two neighboring iconAnimal objs via collision detection
        //when detected, get larger index position for insert
        //for each iconAnimal on the smaller index and less, move distance left
        //for each iconAnimal on the larger index and more, move distance right
        //when pointer is let go in a valid spot, Insert the obj into the list and update all obj positions and Indexes
        //if not in valid spot, go back to previous pos in performance

    }

    void Update()
    {
        
        //查现在是哪个State
        switch (currentState) {

            case ShowStates.SelectAnimal:
            //选动物
            break;

            case ShowStates.Animation:
            //换State
            break;

            case ShowStates.Performance:
            //表演
            break;
        }

        //TODO:把这部分结合进statemachine
        /*
        if (Input.GetMouseButtonDown(0)&& canBeMovedOrSelected)
        {
            //Debug.Log(CheckIfRayCastElementWithTag("showAnimalInHand"));
            
            if (!CheckIfRayCastElementWithTag("showAnimalInHand",out firstDetect))
            {
                sliding = true;
                lastMousePosition = Input.mousePosition;
                //进入滑动
            }
            else
            {
                //进入上下
                Debug.Log(firstDetect.transform.parent.name);
                foreach (iconAnimal animal in myHandControls)
                {
                    if (animal.gameObject != firstDetect.transform.parent.gameObject)
                    {
                        animal.EnterState(iconAnimal.iconState.half);
                    }
                    else
                    {
                        //生成一个小动物
                        holdingAnimalObj = AnimalFactory(animal.selfProperty.name,GetMouseWorldPositionAtZeroZ());
                    }
                }
            }
            enterInteraction = true;
        }

        

        if (Input.GetMouseButtonUp(0)&& enterInteraction)
        {
            Debug.Log("触发了");
            if (!sliding)
            {
                foreach (iconAnimal animal in myHandControls)
                {
                    if (animal.gameObject != firstDetect.transform.parent.gameObject)
                    {
                        animal.EnterState(iconAnimal.iconState.movingUp);
                    }
                    //canBeMovedOrSelected = false;
                    //Invoke("ResetCanBeMoveOrSelect", 0.3f);
                }
            }
            sliding = false;
            enterInteraction = false;
        }*/
        UpdateDecideState();
    }

    void ResetCanBeMoveOrSelect()
    {
        canBeMovedOrSelected = true;
    }

    //Functions
    void EnterOneShow() {

    }

    void StartShow() {

    }

    void EndShow() {

    }

    void LeaveShow() {

    }

    //创动物prefab
    public GameObject AnimalFactory(string name, Vector3 position) {
        switch (name) {
            case "monkey":
                return Instantiate(animalPerformancePrefabs[0], position, Quaternion.identity);
        }
        return null;
    }

    /*
    public void UpdateHand(int index) {
        for (int i = 0; i < myHand.Count; i++) {
            GameObject icon = myHand[i];
            iconAnimal script = icon.GetComponent<iconAnimal>();
            script.myIndex = i;

            if (script.myIndex > 0 && script.myIndex >= index && index != myHand.Count) {
                script.myNeighbor = myHand[script.myIndex - 1];

                float neighborX = script.myNeighbor.GetComponent<RectTransform>().anchoredPosition.x;

                if (Mathf.Abs(icon.GetComponent<RectTransform>().anchoredPosition.x - neighborX) > offset) {
                    script.UpdateDistance(neighborX, 1);
                  // Debug.Log(script.myIndex + "'s NeighborX: " + (neighborX + offset));
                } else {
                   // script.destinationX = neighborX - offset;
                    script.UpdateDistance(neighborX - offset, 1);
                   // Debug.Log(script.myIndex + "'s !! NeighborX: " + (neighborX + offset));
                }

            } else if (script.myIndex == 0 && script.GetComponent<RectTransform>().anchoredPosition.x > -750) {
                script.UpdateDistance(x, 0);
            } else if (index == myHand.Count){
                if (script.myIndex == index - 1) {
                    script.myOtherNeighbor = null;
                } else {
                    script.myOtherNeighbor = myHand[script.myIndex + 1];
                }

                script.UpdateRight();
            }
        }
    }

    public void FixRightSpacing(int index) {

        for (int i = 0; i < myHand.Count; i++) {
            GameObject icon = myHand[i];
            iconAnimal script = icon.GetComponent<iconAnimal>();
            script.myIndex = i;

     
            if (script.myIndex == myHand.Count - 1) {
                script.myOtherNeighbor = null;
                script.otherDestinationX = 750;
            } else {
                 script.myOtherNeighbor = myHand[script.myIndex + 1];
             }

            script.UpdateRight();


        }
    }*/

    void SlideAnimalsInHand(float changeX)
    {
        //TODO:限制左右
        leftAnchorX += changeX;
        for (int i = 0; i < myHand.Count; i++)
        {
            GameObject gmo = myHand[i];
            gmo.GetComponentInChildren<RectTransform>().anchoredPosition = initialPos[i] + new Vector2(leftAnchorX, 0);
        }
        
    }

    bool CheckIfRayCastElementWithTag(string targetTag, out GameObject first)
    {
        if (eventSystem == null)
        {
            eventSystem = FindObjectOfType<EventSystem>();
        }

        // 创建一个 PointerEventData 来存储射线检测信息
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = Input.mousePosition; // 设置射线的起点（鼠标位置）

        // 存储射线检测到的 UI 组件
        List<RaycastResult> results = new List<RaycastResult>();

        // 进行 UI 射线检测
        uiRaycaster.Raycast(eventData, results);

        // 遍历检测到的 UI 组件
        foreach (RaycastResult result in results)
        {
            // 检查 GameObject 是否有目标 Tag
            if (result.gameObject.CompareTag(targetTag))
            {
                first = result.gameObject;
                return true; // 找到匹配的对象，返回 true
            }
        }
        first = null;
        return false; // 没有找到匹配的对象
    }

    Vector3 GetMouseWorldPositionAtZeroZ()
    {
        // 获取鼠标在屏幕中的位置
        Vector3 mouseScreenPosition = Input.mousePosition;

        // 设定鼠标的世界 Z 位置为 0
        mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);

        // 转换为世界坐标
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // 确保 Z 轴为 0
        worldPosition.z = 0;

        return worldPosition;
    }


    DecideScreenState curDecideState = DecideScreenState.empty;
    public void StartDecideState(DecideScreenState newState)
    {
        EndDecideState(curDecideState);
        switch (newState)
        {
            case DecideScreenState.slide:
                break;
        }
        curDecideState = newState;
    }

    void EndDecideState(DecideScreenState lastState)
    {
        switch (lastState)
        {
            case DecideScreenState.slide:
                break;
        }
    }

    void UpdateDecideState()
    {
        switch (curDecideState)
        {
            case DecideScreenState.empty:
                if (Input.GetMouseButtonDown(0) && canBeMovedOrSelected)
                {
                    enterInteraction = true;
                    //Debug.Log(CheckIfRayCastElementWithTag("showAnimalInHand"));

                    if (!CheckIfRayCastElementWithTag("showAnimalInHand", out firstDetect))
                    {
                        StartDecideState(DecideScreenState.slide);
                        lastMousePosition = Input.mousePosition;
                        //进入滑动
                    }
                    else
                    {
                        //进入上下
                        Debug.Log(firstDetect.transform.parent.name);
                        foreach (iconAnimal animal in myHandControls)
                        {
                            if (animal.gameObject != firstDetect.transform.parent.gameObject)
                            {
                                animal.EnterState(iconAnimal.iconState.half);
                            }
                            else
                            {
                                //生成一个小动物
                                holdingAnimalObj = AnimalFactory(animal.selfProperty.name, GetMouseWorldPositionAtZeroZ());
                            }
                        }
                        StartDecideState(DecideScreenState.choose);
                    }
                    
                }
                break;

            case DecideScreenState.slide:
                if (Input.GetMouseButton(0))
                {
                    Vector2 currentMousePosition = Input.mousePosition;
                    float changeX = currentMousePosition.x - lastMousePosition.x; // 计算滑动距离
                    SlideAnimalsInHand(changeX);
                    lastMousePosition = currentMousePosition; // 更新鼠标位置
                }
                if (Input.GetMouseButtonUp(0))
                {
                    
                    sliding = false;
                    enterInteraction = false;
                    StartDecideState(DecideScreenState.empty);
                }
                break;

            case DecideScreenState.choose:
                if (Input.GetMouseButton(0))
                {
                    holdingAnimalObj.transform.position = GetMouseWorldPositionAtZeroZ();
                }

                if (Input.GetMouseButtonUp(0))
                {
                        foreach (iconAnimal animal in myHandControls)
                        {
                            if (animal.gameObject != firstDetect.transform.parent.gameObject)
                            {
                                animal.EnterState(iconAnimal.iconState.movingUp);
                            }
                            //canBeMovedOrSelected = false;
                            //Invoke("ResetCanBeMoveOrSelect", 0.3f);
                        }
                    enterInteraction = false;
                    StartDecideState(DecideScreenState.empty);

                }
                break;
        }
    }
}
