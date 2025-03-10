using UnityEngine;
using UnityEngine.UI;

public class MenuButtonShow : MonoBehaviour
{
	private void Start()
	{
		GlobalManager_OnNextGlobalLevel();
		GlobalManager.OnNextGlobalLevel += GlobalManager_OnNextGlobalLevel;
	}

	private void GlobalManager_OnNextGlobalLevel()
	{
		GetComponent<Image>().sprite = GlobalManager.instance.GetCurrentGlobalLevel().showVisual;
	}
}
