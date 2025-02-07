using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveController : MonoBehaviour
{
    public UiMover mover;
    // Start is called before the first frame update
    void Start()
    {
        mover.MoveTo(GetComponent<RectTransform>().anchoredPosition+new Vector2(200,0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
