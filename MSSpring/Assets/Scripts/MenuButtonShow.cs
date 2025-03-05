using UnityEngine;
using UnityEngine.UI;

public class MenuButtonShow : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Image>().sprite = GlobalManager.instance.GetCurrentGlobalLevel().showVisual;
	}
}
