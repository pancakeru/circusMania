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
        //#文本修改
        mainText.text = "想经历一场酣畅淋漓的教程吗:)";
        subText.gameObject.SetActive(false);
        isTutorial = true;
    }

    public void ButtonShow()
    {
        gameObject.SetActive(true);
        //#文本修改
        mainText.text = "准备好开始表演了吗?";
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
