using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        
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
