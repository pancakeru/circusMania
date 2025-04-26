using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTutorialManager : MonoBehaviour
{
    [SerializeField] private ShowTutorial showTutorial;
    //private enum State { WaitingForClickToProceed, WaitingForProceedConditionToBeFulfilled }

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

    [Header("Animal Introduction")]
    [SerializeField] private Image animalIntroductionImage;
    [SerializeField] private TextMeshProUGUI animalIntroductionTMP;

    private int dialogueIndex = -1;
    private bool isProceedConditionNull = false;
    private delegate bool ProceedConditionCheck();
    private ProceedConditionCheck IsProceedConditionFulfilled;

    private void Start()
    {
        ProceedTutorialDialouge();
    }

    private void Update()
    {
        if (isProceedConditionNull)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                ProceedTutorialDialouge();
            }
        }
        else
        {
            if (IsProceedConditionFulfilled())
            {
                ProceedTutorialDialouge();
            }
        }
    }

    private void ProceedTutorialDialouge()
    {
        if (dialogueIndex + 1 < showTutorial.tutorialDialogueList.Length)
        {
            dialogueIndex++;
            TutorialDialogue currentTutorialDialogue = showTutorial.tutorialDialogueList[dialogueIndex];

            if (!currentTutorialDialogue.isAnimalIntroduction)
            {
                speakerImage.gameObject.SetActive(true);
                speakerImage.sprite = currentTutorialDialogue.speakerSprite;
                speakerDialogue.gameObject.SetActive(true);
                speakerDialogue.text = currentTutorialDialogue.speakerDialogue;

                animalIntroductionImage.gameObject.SetActive(false);
                animalIntroductionTMP.gameObject.SetActive(false);
            }
            else
            {
                animalIntroductionImage.gameObject.SetActive(true);
                animalIntroductionImage.sprite = currentTutorialDialogue.animalIntroductionSprite;
                animalIntroductionTMP.gameObject.SetActive(true);
                animalIntroductionTMP.text = currentTutorialDialogue.animalIntroductionString;

                speakerImage.gameObject.SetActive(false);
                speakerDialogue.gameObject.SetActive(false);
            }

            if (currentTutorialDialogue.audioToBePlayed != null)
            {
                AudioManagerScript.Instance.uiSource.PlayOneShot(currentTutorialDialogue.audioToBePlayed);
            }

            if (currentTutorialDialogue.isWholeMask)
            {
                ShowWholeMask();
            }
            else
            {
                ShowPartialMask(currentTutorialDialogue.maskSizeDelta, currentTutorialDialogue.maskAnchoredPosition);
            }

            switch (currentTutorialDialogue.proceedCondition)
            {
                case "N/A":
                    isProceedConditionNull = true;
                    break;
                case "FourMonkeysOnStage":
                    isProceedConditionNull = false;
                    IsProceedConditionFulfilled = IsFourMonkeysOnStage;
                    break;
                case "ShowStart":
                    isProceedConditionNull = false;
                    IsProceedConditionFulfilled = IsShowStarted;
                    break;
            }
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

    #region Proceed Conditions
    private bool IsFourMonkeysOnStage()
    {
        int monkeyCount = 0;

        foreach (GameObject animal in ShowManager.instance.onStage)
        {
            if (animal != null)
            {
                if (animal.TryGetComponent(out AnimalControlMonkey animalControlMonkey))
                {
                    monkeyCount++;
                }
            }
        }

        if (monkeyCount == 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsShowStarted()
    {
        if (ShowManager.instance.ifToShow)
        {
            gameObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
