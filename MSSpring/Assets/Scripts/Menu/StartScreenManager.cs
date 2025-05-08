using UnityEngine;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject balloon;

    public void Enable()
    {
        canvas.enabled = true;
        balloon.GetComponent<BalloonController>().Enable();
    }

    public void StartButton()
    {
        GlobalManager.instance.NewGame();
        canvas.enabled = false;
    }

    public void LoadButton()
    {
        GlobalManager.instance.LoadGame();
        canvas.enabled = false;
    }

    public void Exit()
    {
        GlobalManager.instance.SaveGlobalSaveData();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit(); 
#endif
    }
}
