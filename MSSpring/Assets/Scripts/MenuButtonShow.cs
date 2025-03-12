using UnityEngine;
using TMPro;

public class MenuButtonShow : MonoBehaviour
{
	public TextMeshProUGUI text;
    private void Awake()
    {
        GlobalManager.OnNextGlobalLevel += GlobalManager_OnNextGlobalLevel;
    }
	private void GlobalManager_OnNextGlobalLevel(GlobalLevel nextLevel)
	{
		Debug.Log("代码在这里" + gameObject.name);
		if (text != null)
			text.text = "To Win:" + nextLevel.levelProperty.targetScore;
		//GetComponent<Image>().sprite = GlobalManager.instance.GetCurrentGlobalLevel().showVisual;
	}
}
