using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTutorialManager : MonoBehaviour
{
    [SerializeField] private ShowTutorial showTutorial;

    [Header("Hand")]
    public AnimalStart switchHand;
    public animalProperty addHand;

    [Header("Mask")]
    [SerializeField] private Image mask;
    [SerializeField] private Sprite wholeMask;
    [SerializeField] private Sprite partialMask9Sliced;
    [SerializeField] private Image[] raycastImageArray;

    [Header("Speaker")]
    [SerializeField] private Image speakerImage;
    [SerializeField] private TextMeshProUGUI speakerDialogue;

    private int dialogueIndex = -1;

    private void Start()
    {
        ProceedTutorialDialouge();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ProceedTutorialDialouge();
        }
    }

    private void ProceedTutorialDialouge()
    {
        dialogueIndex++;
        TutorialDialogue currentTutorialDialogue = showTutorial.tutorialDialogueList[dialogueIndex];

        speakerImage.sprite = currentTutorialDialogue.speakerSprite;
        speakerDialogue.text = currentTutorialDialogue.speakerDialogue;
        if (currentTutorialDialogue.isWholeMask)
        {
            ShowWholeMask();
        }
        else
        {
            ShowPartialMask();
        }
    }

    private void ShowWholeMask()
    {
        mask.gameObject.SetActive(true);
        mask.sprite = wholeMask;
        mask.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
        mask.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        mask.raycastTarget = true;
        foreach (Image raycastImage in raycastImageArray)
        {
            raycastImage.gameObject.SetActive(false);
        }
    }

    private void ShowPartialMask()
    {
        mask.gameObject.SetActive(true);
        mask.sprite = partialMask9Sliced;
        mask.raycastTarget = false;
        for (int i = 0; i < raycastImageArray.Length; i++)
        {
            Image raycastImage = raycastImageArray[i];
            if (i == 0)
            {
                raycastImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 540f - mask.GetComponent<RectTransform>().anchoredPosition.y - mask.GetComponent<RectTransform>().sizeDelta.y / 2f);
                raycastImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, mask.GetComponent<RectTransform>().sizeDelta.y / 2f);
            }
            else if (i == 2)
            {
                raycastImage.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 540f + mask.GetComponent<RectTransform>().anchoredPosition.y - mask.GetComponent<RectTransform>().sizeDelta.y / 2f);
                raycastImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -Mathf.Abs(mask.GetComponent<RectTransform>().anchoredPosition.y) - mask.GetComponent<RectTransform>().sizeDelta.y / 2f);
            }
        }
    }
}
