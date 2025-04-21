using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Resume()
    {
        ShowManager.instance.PauseResume();
    }

    public void ButtonReturnToBackstage()
    {
        CanvasMain.instance.DisplaySelection("Are you sure you want to return to backstage?", ReturnToBackstage);
    }

    public void ReturnToBackstage()
    {
        Time.timeScale = 1;
        ShowManager.instance.curTurn = ShowManager.instance.curLevel.allowedTurn;
        ShowManager.instance.SetIfChangeTroupePrice(false);
        ShowManager.instance.StartDecide();

        gameObject.SetActive(false);
    }
}
