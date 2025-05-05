using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject balloon;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Enable()
    {
        canvas.enabled = true;
        balloon.GetComponent<BalloonController>().Enable();
        CanvasMain.instance.isStartScreenCanvasEnabled = true;
    }

    public void Disable()
    {
        canvas.enabled = false;
        CanvasMain.instance.isStartScreenCanvasEnabled = false;
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit(); 
#endif
    }
}
