using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winLoseScript : MonoBehaviour
{
    public GameObject[] pictures;
    int index = 0;
    float delay = 0.5f;

    void Start()
    {
        

    }

    void Update()
    {
        delay -= Time.deltaTime;

        if (index < pictures.Length && delay <= 0) {
            pictures[index].GetComponent<pictureIconScript>().Appear();
            index += 1;
            delay = 0.5f;
        }
    }
}
