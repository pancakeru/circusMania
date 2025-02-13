using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowManager : MonoBehaviour
{
    //State Machine
    private enum ShowStates {
        SelectAnimal,
        Animation,
        Performance
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

    private List<animalProperty> testList;
    public List<GameObject> animalPerformancePrefabs;

    public List<GameObject> myHand;

    void Start()
    {
        testList = new List<animalProperty>();
        x = -750;
        offset = 300;
        yStart = -600;
        areaOffset = 2;

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
            temp.GetComponent<iconAnimal>().Initialize("monkey");
            temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(x + offset*i, yStart);
            temp.GetComponent<iconAnimal>().myIndex = i;
        }

        //FOR ADDING TO PERFORMANCE
        //Make a list for iconAnimals
        //Upon instantiation, assign list pos to iconAnimal
        //Assign neighbor objects to each iconAnimal
        //if pos is 0 or lenght-1, check for neighbor on one side only
        //track distance between self and neighbors, and lerp to adjust position if increment is bigger than a certain amount
        //When iconAnimal is destroyed, update position num of all iconAnimals in list
        //check distance between neighbors, and lerp to reposition

        //FOR ADDING BACK TO DECK
        //List.Insert(index, obj)
        //when adding back to deck, check the two neighboring iconAnimal objs via collision detection
        //when detected, get larger index position for insert
        //for each iconAnimal on the smaller index and less, move distance left
        //for each iconAnimal on the larger index and more, move distance right
        //when pointer is let go in a valid spot, Insert the obj into the list and update all obj positions and Indexes
        //if not in valid spot, go back to previous pos in performance

        //SCROLLING CLAMP
        //create a var to store which direction the mouse is currently swiping in
        //create a bool for clamping
        //depending on direction, check iconAnimal at pos 0 or pos length-1
        //if either reaches the clamp threshold, activate clamping bool and stop all swiping

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
    public void AnimalFactory(string name, Vector3 position) {
        switch (name) {
            case "monkey":
                Instantiate(animalPerformancePrefabs[0], position, Quaternion.identity);
            break;
        }
    }


    public void UpdateHand() {
        for (int i = 0; i < myHand.Count; i++) {
            GameObject icon = myHand[i];
            iconAnimal script = icon.GetComponent<iconAnimal>();
            script.myIndex = i;

            if (script.myIndex > 0) {
                script.myNeighbor = myHand[script.myIndex - 1];
                script.UpdateDistance();
            }
        }
    }
}
