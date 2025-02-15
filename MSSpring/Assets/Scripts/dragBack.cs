using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragBack : MonoBehaviour
{
    private GameObject showManager;
    private ShowManager showScript;
    public GameObject iconPrefab;
    private SpriteRenderer mySprite;

    void Start()
    {
        showManager = GameObject.FindWithTag("showManager");
        showScript = showManager.GetComponent<ShowManager>();
        mySprite = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        
    }

}
