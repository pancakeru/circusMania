using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelPreviewController : MonoBehaviour
{
    float scoreFillMax = 1000f;

    [SerializeField] private Image nextSpotImage;
    [SerializeField] private TextMeshProUGUI nextSpotText;
    [SerializeField] private Image funImageFill;
    [SerializeField] private TextMeshProUGUI funText;
    [SerializeField] private Image skillImageFill;
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private Image noveltyImageFill;
    [SerializeField] private TextMeshProUGUI noveltyText;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        GlobalLevel currentGlobalLevel = GlobalManager.instance.GetCurrentGlobalLevel();
        LevelProperty currentLevelProperty = currentGlobalLevel.levelProperty;

        nextSpotImage.sprite = currentGlobalLevel.showVisual;
        nextSpotText.text = GlobalManager.instance.GetCurrentGlobalLevel().levelIndex.ToString() + "/" + GlobalManager.instance.GetGlobalLevelArray().Length.ToString();

        float funTargetScore = currentLevelProperty.totalN / 3 * currentLevelProperty.redA + currentLevelProperty.redB;
        float skillTargetScore = currentLevelProperty.totalN / 3 * currentLevelProperty.yellowA + currentLevelProperty.yellowB;
        float noveltyTargetScore = currentLevelProperty.totalN / 3 * currentLevelProperty.blueA + currentLevelProperty.blueB;

        funText.text = "~" + funTargetScore.ToString();
        funImageFill.fillAmount = GetNormalizedFillAmount(funTargetScore);
        skillText.text = "~" + skillTargetScore.ToString();
        skillImageFill.fillAmount = GetNormalizedFillAmount(skillTargetScore);
        noveltyText.text = "~" + noveltyTargetScore.ToString();
        noveltyImageFill.fillAmount = GetNormalizedFillAmount(noveltyTargetScore);
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
}
