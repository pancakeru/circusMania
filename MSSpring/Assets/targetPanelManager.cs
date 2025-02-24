using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class targetPanelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI reputationText;
    [SerializeField] private Slider processSlider;


    public void ChangeLevelState(float curTotalScore, int curTurn, float curRepu, float totalTargetScore, int totalTurn)
    {
        scoreText.text = ((int)(totalTargetScore - curTotalScore)).ToString();
        processSlider.value = (curTotalScore / totalTargetScore);
        turnText.text = curTurn.ToString() + "/" + totalTurn.ToString();
        reputationText.text = ((int)curRepu).ToString();
    }
}
