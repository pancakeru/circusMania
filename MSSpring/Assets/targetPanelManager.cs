using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class targetPanelManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI turnText;
	[SerializeField] private TextMeshProUGUI reputationText;
	[SerializeField] private TextMeshProUGUI redText;
	[SerializeField] private TextMeshProUGUI yellowText;
	[SerializeField] private TextMeshProUGUI blueText;

	public void ChangeLevelState(int curTurn, float curRepu, float totalTargetScore, int totalTurn)
	{
		//Debug.Log("现在的Target是" + totalTargetScore);
		turnText.text = curTurn.ToString() + "/" + totalTurn.ToString();
		float[] targetScoreArray = FindFirstObjectByType<ShowScoreManager>().GetTargetScore();
		redText.text = targetScoreArray[0].ToString();
		yellowText.text = targetScoreArray[1].ToString();
		blueText.text = targetScoreArray[2].ToString();
		reputationText.text = ((int)curRepu).ToString();
	}
}
