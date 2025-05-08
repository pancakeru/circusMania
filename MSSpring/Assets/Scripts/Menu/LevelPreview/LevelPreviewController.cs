using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPreviewController : MonoBehaviour
{
    private float scoreFillMax = 500f;

    private RectTransform backgroundRectTransform;

    [Header("Next Spot")]
    [SerializeField] private Image nextSpotImage;
    [SerializeField] private TextMeshProUGUI nextSpotName;
    [SerializeField] private TextMeshProUGUI nextSpotTurnText;

    [Header("Fun - Skill - Novelty")]
    [SerializeField] private Image funImageFill;
    [SerializeField] private TextMeshProUGUI funText;
    [SerializeField] private Image skillImageFill;
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private Image noveltyImageFill;
    [SerializeField] private TextMeshProUGUI noveltyText;

    private TourIconController[] tourIconArray;
    [Header("Tour Icon")]
    [SerializeField] private GameObject tourSchedule;
    private Color redColor;
    private Color yellowColor;
    private Color blueColor;

    private bool isLetsGoButtonPressed = false;
    [Header("Miscellaneous")]
    [SerializeField] private DecidePanelController decidePanel;
    [SerializeField] private GameObject container;

    private void Awake()
    {
        backgroundRectTransform = GetComponent<RectTransform>();

        redColor = new Color(0.827451f, 0.2745098f, 0.5607843f);
        yellowColor = new Color(0.8941177f, 0.8117647f, 0.4823529f);
        blueColor = new Color(0.2745098f, 0.6627451f, 0.8235294f);

        tourIconArray = tourSchedule.GetComponentsInChildren<TourIconController>();
        Array.Sort(tourIconArray, (a, b) => a.index.CompareTo(b.index));
        SetTourIconsVisualAtStart();
    }

    private void Update()
    {
        if (!isLetsGoButtonPressed)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Vector2 localMousePosition = backgroundRectTransform.InverseTransformPoint(Input.mousePosition);
                if (!backgroundRectTransform.rect.Contains(localMousePosition))
                {
                    container.SetActive(false);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                container.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        UpdateVisual();
    }

    private void OnDisable()
    {
        isLetsGoButtonPressed = false;
    }

    private void SetTourIconsVisualAtStart()
    {
        GlobalLevel[] globalLevelArray = GlobalManager.instance.GetGlobalLevelArray();
        foreach (GlobalLevel globalLevel in globalLevelArray)
        {
            foreach (TourIconController tourIcon in tourIconArray)
            {
                if (globalLevel.levelIndex == tourIcon.index)
                {
                    float[] targetScoreArray = CalculateTargetScore(globalLevel.levelProperty);
                    float highestTargetScore = Mathf.Max(targetScoreArray);
                    if (highestTargetScore == targetScoreArray[0])
                    {
                        tourIcon.gameObject.GetComponent<Image>().color = redColor;
                        tourIcon.selectedVisual.color = redColor;
                    }
                    else if (highestTargetScore == targetScoreArray[1])
                    {
                        tourIcon.gameObject.GetComponent<Image>().color = yellowColor;
                        tourIcon.selectedVisual.color = yellowColor;
                    }
                    else if (highestTargetScore == targetScoreArray[2])
                    {
                        tourIcon.gameObject.GetComponent<Image>().color = blueColor;
                        tourIcon.selectedVisual.color = blueColor;
                    }
                }
            }
        }
    }

    private void SetSelectedTourIconVisual(GlobalLevel currentGlobalLevel)
    {
        foreach (TourIconController tourIcon in tourIconArray)
        {
            if (tourIcon.index == currentGlobalLevel.levelIndex)
            {
                tourIcon.selectedVisual.gameObject.SetActive(true);
            }
            else
            {
                tourIcon.selectedVisual.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateVisual()
    {
        GlobalLevel currentGlobalLevel = GlobalManager.instance.GetCurrentGlobalLevel();
        LevelProperty currentLevelProperty = currentGlobalLevel.levelProperty;

        nextSpotImage.sprite = currentGlobalLevel.showVisual;
        nextSpotName.text = currentGlobalLevel.levelName;
        nextSpotTurnText.text = (GlobalManager.instance.GetCurrentGlobalLevel().levelIndex + 1).ToString() + "/" + GlobalManager.instance.GetGlobalLevelArray().Length.ToString();

        float[] targetScoreArray = CalculateTargetScore(currentLevelProperty);
        float funTargetScore = targetScoreArray[0];
        float skillTargetScore = targetScoreArray[1];
        float noveltyTargetScore = targetScoreArray[2];

        SetSelectedTourIconVisual(currentGlobalLevel);

        funText.text = "~" + funTargetScore.ToString();
        funImageFill.fillAmount = GetNormalizedFillAmount(funTargetScore);
        skillText.text = "~" + skillTargetScore.ToString();
        skillImageFill.fillAmount = GetNormalizedFillAmount(skillTargetScore);
        noveltyText.text = "~" + noveltyTargetScore.ToString();
        noveltyImageFill.fillAmount = GetNormalizedFillAmount(noveltyTargetScore);
    }

    private float[] CalculateTargetScore(LevelProperty currentLevelProperty)
    {
        float funTargetScore = currentLevelProperty.totalN / 3 * currentLevelProperty.redA + currentLevelProperty.redB;
        float skillTargetScore = currentLevelProperty.totalN / 3 * currentLevelProperty.yellowA + currentLevelProperty.yellowB;
        float noveltyTargetScore = currentLevelProperty.totalN / 3 * currentLevelProperty.blueA + currentLevelProperty.blueB;

        return new float[] { funTargetScore, skillTargetScore, noveltyTargetScore };
    }

    private float GetNormalizedFillAmount(float targetScore)
    {
        if (targetScore != 0f)
        {
            return targetScore / scoreFillMax;
        }
        else
        {
            return 0f;
        }
    }

    public void LetsGoButtonPressed()
    {
        isLetsGoButtonPressed = true;
        decidePanel.ButtonShow();
    }

    public void DecidePanelCancelled()
    {
        isLetsGoButtonPressed = false;
        decidePanel.Cancel();
    }
}
