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
        int toReach = ((int)(totalTargetScore - curTotalScore));
        scoreText.text = toReach<= 0? "Complete!":toReach.ToString();
        processSlider.value = Mathf.Clamp((curTotalScore / totalTargetScore),0,1);
        turnText.text = curTurn.ToString() + "/" + totalTurn.ToString();
        reputationText.text = ((int)curRepu).ToString();
    }
}
