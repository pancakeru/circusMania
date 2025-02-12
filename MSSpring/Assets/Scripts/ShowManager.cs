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
    public Transform canvasTransform;

    private float y;
    private float yStart;
    private float x;
    private float offset;

    public bool holding = false;

    private List<animalProperty> testList;
    public List<GameObject> animalPerformancePrefabs;

    void Start()
    {
        testList = new List<animalProperty>();
        x = -750;
        offset = 300;
        yStart = -600;


        for (int i = 0; i < 12; i++) {
            GameObject temp = Instantiate(animalIcon, canvasTransform);
            temp.GetComponent<iconAnimal>().Initialize("monkey");
            temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(x + offset*i, yStart);
        }

        //待会换成for loop
        //取animal property list之后用 for loop 去做图片

        /*
        GameObject temp = Instantiate(animalIcon, canvasTransform);
        temp.GetComponent<iconAnimal>().Initialize("monkey");
        temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(x, y);

        GameObject temp2 = Instantiate(animalIcon, canvasTransform);
        temp2.GetComponent<iconAnimal>().Initialize("giraffe");
        temp2.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(x + offset*1, y);

        GameObject temp3 = Instantiate(animalIcon, canvasTransform);
        temp3.GetComponent<iconAnimal>().Initialize("bear");
        temp3.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(x + offset*2, y);

        GameObject temp4 = Instantiate(animalIcon, canvasTransform);
        temp4.GetComponent<iconAnimal>().Initialize("snake");
        temp4.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(x + offset*3, y);

        GameObject temp5 = Instantiate(animalIcon, canvasTransform);
        temp5.GetComponent<iconAnimal>().Initialize("lion");
        temp5.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(x + offset*4, y);

        GameObject temp6 = Instantiate(animalIcon, canvasTransform);
        temp6.GetComponent<iconAnimal>().Initialize("elephant");
        temp6.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(x + offset*5, y);
        */

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
    public void AnimalFactory(string name) {
        switch (name) {
            case "monkey":
                Instantiate(animalPerformancePrefabs[0], new Vector3(0, 0, 0), Quaternion.identity);
            break;
        }
    }

}
