using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = CanvasMain.instance.transform;
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
        CanvasMain.instance.DisplaySelection("你希望返回后台重新开始这次表演吗?", ReturnToBackstage);
    }

    public void ReturnToBackstage()
    {
        Time.timeScale = 1;
        ShowManager.instance.curTurn = ShowManager.instance.curLevel.allowedTurn;
        ShowManager.instance.SetTurnEnableState(true);
        ShowManager.instance.SetIfChangeTroupePrice(false);
        ShowManager.instance.StartDecide(false);

        gameObject.SetActive(false);
    }
}
