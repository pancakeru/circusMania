using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformancePrefabReference : MonoBehaviour
{

    public static PerformancePrefabReference instance;
    public List<GameObject> prefabs;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

   
}
