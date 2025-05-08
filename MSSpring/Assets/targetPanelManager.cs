using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class targetPanelManager : MonoBehaviour
{
	[Header("修改分数")]
	[SerializeField] private TextMeshProUGUI turnText;
	[SerializeField] private TextMeshProUGUI reputationText;
	[SerializeField] private TextMeshProUGUI redText;
	[SerializeField] private TextMeshProUGUI yellowText;
	[SerializeField] private TextMeshProUGUI blueText;

	[Header("控制分数显示开关")]
	[SerializeField] private GameObject redGroup;
	[SerializeField] private GameObject blueGroup;
	[SerializeField] private GameObject yellowGroup;
	public GameObject popularityGroup;

	public void ChangeLevelState(int curTurn, float curRepu, float[] targets, int totalTurn)
	{
		//Debug.Log("现在的repu是" + curRepu);
		turnText.text = curTurn.ToString() + "/" + totalTurn.ToString();
		float[] targetScoreArray = targets;
		redText.text = targetScoreArray[0].ToString();
		yellowText.text = targetScoreArray[1].ToString();
		blueText.text = targetScoreArray[2].ToString();
		reputationText.text = ((int)curRepu).ToString();
	}

	public void ChangeLevelTargetUiDisplayStatus(bool ifShowRed, bool ifShowYellow, bool ifShowBlue, bool ifShowPopularity)
	{
		redGroup.SetActive(ifShowRed);
		blueGroup.SetActive(ifShowBlue);
		yellowGroup.SetActive(ifShowYellow);
		popularityGroup.SetActive(ifShowPopularity);
	}
}
