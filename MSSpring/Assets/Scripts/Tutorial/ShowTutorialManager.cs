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
            ShowPartialMask(currentTutorialDialogue.maskSizeDelta, currentTutorialDialogue.maskAnchoredPosition);
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

    private void ShowPartialMask(Vector2 maskSizeDelta, Vector2 maskAnchoredPosition)
    {
        mask.gameObject.SetActive(true);
        mask.sprite = partialMask9Sliced;
        mask.raycastTarget = false;
        RectTransform maskTransform = mask.GetComponent<RectTransform>();
        maskTransform.sizeDelta = maskSizeDelta;
        maskTransform.anchoredPosition = maskAnchoredPosition;
        for (int i = 0; i < raycastImageArray.Length; i++)
        {
            RectTransform raycastImageTransform = raycastImageArray[i].GetComponent<RectTransform>();
            raycastImageTransform.gameObject.SetActive(true);
            if (i == 0)
            {
                raycastImageTransform.sizeDelta = new Vector2(1920f, 540f - maskAnchoredPosition.y - maskSizeDelta.y / 2f);
                raycastImageTransform.anchoredPosition = new Vector2(0f, 270f + maskAnchoredPosition.y / 2f + maskSizeDelta.y / 4f);
            }
            else if (i == 1)
            {
                raycastImageTransform.sizeDelta = new Vector2(960f - maskAnchoredPosition.x - maskSizeDelta.x / 2f, maskSizeDelta.y);
                raycastImageTransform.anchoredPosition = new Vector2(480f + maskAnchoredPosition.x / 2f + maskSizeDelta.x / 4f, maskAnchoredPosition.y);
            }
            else if (i == 2)
            {
                raycastImageTransform.sizeDelta = new Vector2(1920f, 540f + maskAnchoredPosition.y - maskSizeDelta.y / 2f);
                raycastImageTransform.anchoredPosition = new Vector2(0f, -270f + maskAnchoredPosition.y / 2f - maskSizeDelta.y / 4f);
            }
            else if (i == 3)
            {
                raycastImageTransform.sizeDelta = new Vector2(960f + maskAnchoredPosition.x - maskSizeDelta.x / 2f, maskSizeDelta.y);
                raycastImageTransform.anchoredPosition = new Vector2(-480f + maskAnchoredPosition.x / 2f - maskSizeDelta.x / 4f, maskAnchoredPosition.y);
            }
        }
    }
}
