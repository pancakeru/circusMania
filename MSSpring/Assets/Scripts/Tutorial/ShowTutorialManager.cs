using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowTutorialManager : MonoBehaviour
{
    [SerializeField] private ShowTutorial showTutorial;
    public GameObject content;
    public int turotialShowTurn = 0;

    [Header("Hand")]
    public AnimalStart switchHand;
    public animalProperty addHandElephant;
    public animalProperty addHandLion;

    [Header("Mask")]
    [SerializeField] private Image mask;
    [SerializeField] private Sprite wholeMask;
    [SerializeField] private Sprite partialMask9Sliced;
    [SerializeField] private Image[] raycastImageArray;

    [Header("Speaker")]
    [SerializeField] private Image speakerImage;
    [SerializeField] private Image speakerShadow;
    [SerializeField] private TextMeshProUGUI speakerDialogue;
    [SerializeField] private Image speakerBubble;
    private bool isTyping = false;

    [Header("Animal Introduction")]
    [SerializeField] private Image animalIntroductionImage;
    [SerializeField] private TextMeshProUGUI animalIntroductionTMP;

    private int dialogueIndex = -1;
    private bool isProceedConditionNull = false;
    private delegate bool ProceedConditionCheck();
    private ProceedConditionCheck IsProceedConditionFulfilled;
    private bool isProceedCoroutineRunning = false;

    private bool isAudioPlayed = false;

    //Banana
    public int bananaHitTimes { get; private set; } = 0;
    [HideInInspector] public bool isRehearsalGoalActive = false;

    private void Start()
    {
        BananaScript.OnAnyBananaHitsBall += BananaScript_OnAnyBananaHitsBall;

        ShowManager.instance.BanAllInteraction();
        ProceedTutorialDialouge();
    }

    private void Update()
    {
        if (isTyping && speakerDialogue.gameObject.GetComponent<TypewriterEffect>().isDoneTyping)
        {
            isTyping = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isTyping)
            {
                speakerDialogue.gameObject.GetComponent<TypewriterEffect>().StopAllCoroutines();
                speakerDialogue.gameObject.GetComponent<TypewriterEffect>().isDoneTyping = true;
                speakerDialogue.text = speakerDialogue.gameObject.GetComponent<TypewriterEffect>().fullText;
                isTyping = false;
            }
            else
            {
                if (isProceedConditionNull)
                {
                    if (!isProceedCoroutineRunning)
                    {
                        isProceedCoroutineRunning = true;
                        StartCoroutine(WaitUntilContentIsActiveAndProceed());
                    }
                }
            }
        }

        if (!isProceedConditionNull && IsProceedConditionFulfilled())
        {
            if (!isProceedCoroutineRunning)
            {
                isProceedCoroutineRunning = true;
                StartCoroutine(WaitUntilContentIsActiveAndProceed());
            }
        }
    }

    private void ProceedTutorialDialouge()
    {
        if (dialogueIndex + 1 < showTutorial.tutorialDialogueList.Length && !isRehearsalGoalActive)
        {
            dialogueIndex++;
            TutorialDialogue currentTutorialDialogue = showTutorial.tutorialDialogueList[dialogueIndex];

            if (currentTutorialDialogue.speakerDialogue == "END")
            {
                content.gameObject.SetActive(false);
                Time.timeScale = 1;
                ShowManager.instance.curTurn = ShowManager.instance.curLevel.allowedTurn;
                ShowManager.instance.SetIfChangeTroupePrice(false);
                ShowManager.instance.StartDecide();
            }
            else
            {
                if (!currentTutorialDialogue.isAnimalIntroduction)
                {
                    speakerImage.gameObject.SetActive(true);
                    speakerImage.sprite = currentTutorialDialogue.speakerSprite;
                    speakerShadow.gameObject.SetActive(true);
                    if (currentTutorialDialogue.speakerDialogue == " ")
                    {
                        speakerBubble.gameObject.SetActive(false);
                    }
                    else
                    {
                        speakerBubble.gameObject.SetActive(true);
                        speakerDialogue.gameObject.GetComponent<TypewriterEffect>().fullText = currentTutorialDialogue.speakerDialogue;
                        speakerDialogue.gameObject.GetComponent<TypewriterEffect>().StartTyping();
                        isTyping = true;
                    }

                    SetAnimalIntroductionActiveSelf(false);
                }
                else
                {
                    SetAnimalIntroductionActiveSelf(true);
                    animalIntroductionImage.sprite = currentTutorialDialogue.animalIntroductionSprite;
                    animalIntroductionTMP.text = currentTutorialDialogue.animalIntroductionString;

                    SetSpeakerActiveSelf(false);
                }

                if (currentTutorialDialogue.audioToBePlayed != null)
                {
                    AudioManagerScript.Instance.uiSource.PlayOneShot(currentTutorialDialogue.audioToBePlayed);
                    isAudioPlayed = true;
                }

                if (currentTutorialDialogue.isWholeMask)
                {
                    ShowWholeMask();
                }
                else if (currentTutorialDialogue.isWholeImage)
                {
                    SetSpeakerActiveSelf(false);
                    ShowWholeImage(currentTutorialDialogue.imageSprite);
                }
                else
                {
                    ShowPartialMask(currentTutorialDialogue.maskSizeDelta, currentTutorialDialogue.maskAnchoredPosition);
                }

                switch (currentTutorialDialogue.proceedCondition)
                {
                    case "AudioPlayed":
                        isProceedConditionNull = false;
                        IsProceedConditionFulfilled = IsAudioPlayed;
                        break;
                    case "FourMonkeysOnStage":
                        ShowManager.instance.SetSelectAnimalInDownEnableState(true);
                        isProceedConditionNull = false;
                        IsProceedConditionFulfilled = IsFourMonkeysOnStage;
                        break;
                    case "ShowStart":
                        isProceedConditionNull = false;
                        IsProceedConditionFulfilled = IsShowStarted;
                        break;
                    case "ElephantOn4thPosition":
                        ShowManager.instance.SetSelectAnimalInDownEnableState(true);
                        isProceedConditionNull = false;
                        IsProceedConditionFulfilled = IsElephantOn4thPosition;
                        break;
                    case "OneLionOnStage":
                        ShowManager.instance.SetSelectAnimalInDownEnableState(true);
                        isProceedConditionNull = false;
                        IsProceedConditionFulfilled = IsOneLionOnStage;
                        break;
                    default:
                        isProceedConditionNull = true;
                        break;
                }
            }
        }
    }

    private void ShowWholeMask()
    {
        mask.gameObject.SetActive(true);
        mask.GetComponent<ImageFade>().enabled = false;
        mask.sprite = wholeMask;
        mask.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
        mask.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        mask.color = new Color(0, 0, 0, 0.7843137f);
        mask.raycastTarget = true;
        foreach (Image raycastImage in raycastImageArray)
        {
            raycastImage.gameObject.SetActive(false);
        }
    }

    private void ShowPartialMask(Vector2 maskSizeDelta, Vector2 maskAnchoredPosition)
    {
        mask.gameObject.SetActive(true);
        mask.GetComponent<ImageFade>().enabled = false;
        mask.sprite = partialMask9Sliced;
        mask.color = new Color(0, 0, 0, 0.7843137f);
        RectTransform maskTransform = mask.GetComponent<RectTransform>();
        maskTransform.sizeDelta = maskSizeDelta;
        maskTransform.anchoredPosition = maskAnchoredPosition;
        mask.raycastTarget = false;
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

    private void ShowWholeImage(Sprite imageSprite)
    {
        mask.gameObject.SetActive(true);
        mask.GetComponent<ImageFade>().enabled = true;
        mask.sprite = imageSprite;
        mask.GetComponent<RectTransform>().sizeDelta = new Vector2(1920, 1080);
        mask.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        mask.color = Color.white;
        mask.raycastTarget = true;
        foreach (Image raycastImage in raycastImageArray)
        {
            raycastImage.gameObject.SetActive(false);
        }
    }

    private void SetSpeakerActiveSelf(bool isActive)
    {
        speakerImage.gameObject.SetActive(isActive);
        speakerShadow.gameObject.SetActive(isActive);
        speakerBubble.gameObject.SetActive(isActive);
    }

    private void SetAnimalIntroductionActiveSelf(bool isActive)
    {
        animalIntroductionImage.gameObject.SetActive(isActive);
        animalIntroductionTMP.gameObject.SetActive(isActive);
    }

    private IEnumerator WaitUntilContentIsActiveAndProceed()
    {
        while (!content.gameObject.activeSelf)
        {
            yield return null;
        }
        isProceedCoroutineRunning = false;
        ProceedTutorialDialouge();
    }

    private void BananaScript_OnAnyBananaHitsBall()
    {
        bananaHitTimes++;
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
            ShowManager.instance.BanAllInteraction();
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
            content.gameObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsAudioPlayed()
    {
        if (isAudioPlayed && !AudioManagerScript.Instance.uiSource.isPlaying)
        {
            isAudioPlayed = false;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsElephantOn4thPosition()
    {
        bool isAnimalsOnPosition = true;
        for (int i = 0; i < ShowManager.instance.onStage.Length; i++)
        {
            GameObject animal = ShowManager.instance.onStage[i];
            if (i == 3)
            {
                if (animal != null)
                {
                    if (!animal.TryGetComponent(out AnimalControlElephant animalControlElephant))
                    {
                        isAnimalsOnPosition = false;
                    }
                }
            }
            else
            {
                if (animal != null)
                {
                    if (!animal.TryGetComponent(out AnimalControlMonkey animalControlMonkey))
                    {
                        isAnimalsOnPosition = false;
                    }
                }
            }
        }
        if (isAnimalsOnPosition)
        {
            ShowManager.instance.BanAllInteraction();
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsOneLionOnStage()
    {
        bool isOneLionOnStage = false;
        int animalCount = 0;

        foreach (GameObject animal in ShowManager.instance.onStage)
        {
            if (animal != null)
            {
                animalCount++;
                if (animal.TryGetComponent(out AnimalControlLion animalControlLion))
                {
                    isOneLionOnStage = true;
                }
            }
        }

        if (isOneLionOnStage && animalCount == 6)
        {
            ShowManager.instance.BanAllInteraction();
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
