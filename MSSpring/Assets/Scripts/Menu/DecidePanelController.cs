using TMPro;
using UnityEngine;

public class DecidePanelController : MonoBehaviour
{
    [SerializeField] private MenuController menuController;
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private TextMeshProUGUI subText;
    private bool isTutorial;

    public void ButtonTutorial()
    {
        gameObject.SetActive(true);
        mainText.text = "Are you sure you want to enter the tutorial?";
        subText.gameObject.SetActive(false);
        isTutorial = true;
    }

    public void ButtonShow()
    {
        gameObject.SetActive(true);
        mainText.text = "Are you ready to start the show?";
        subText.gameObject.SetActive(true);
        isTutorial = false;
    }

    public void Confirm()
    {
        menuController.ButtonShow(isTutorial);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
