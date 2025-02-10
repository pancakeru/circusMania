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

    void Start()
    {
        GameObject temp = Instantiate(animalIcon, canvasTransform);
        temp.GetComponent<iconAnimal>().Initialize("monkey");
        temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(0, 0);

        GameObject temp2 = Instantiate(animalIcon, canvasTransform);
        temp2.GetComponent<iconAnimal>().Initialize("giraffe");
        temp2.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(250, 0);

        GameObject temp3 = Instantiate(animalIcon, canvasTransform);
        temp3.GetComponent<iconAnimal>().Initialize("bear");
        temp3.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(500, 0);

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
    public void AnimalFactory() {

    }

}
