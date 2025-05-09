using UnityEngine;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] GameObject balloon;
    [SerializeField] GameObject decidePanel;

    public void Enable()
    {
        canvas.enabled = true;
        balloon.GetComponent<BalloonController>().Enable();
    }

    public void StartButton()
    {
        if (SaveDataManager.Instance.HasSaveDataExisted())
        {
            decidePanel.SetActive(true);
        }
        else
        {
            GlobalManager.instance.NewGame();
            canvas.enabled = false;
        }
    }

    public void LoadButton()
    {
        GlobalManager.instance.LoadGame();
        canvas.enabled = false;
    }

    public void DecideConfirm()
    {
        GlobalManager.instance.NewGame();
        canvas.enabled = false;
    }

    public void CreditButton()
    {
        CanvasMain.instance.ShowCredit();
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
