using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopHoverExplanationSingle : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private TextMeshProUGUI text;
	private ShopDisplayUnit shopDisplayUnit;

	/*
	private void Awake()
	{
		transform.SetParent(ShopManager.Instance.transform);
	}

	public void SetUp(ShopDisplayUnit shopDisplayUnit)
	{
		this.shopDisplayUnit = shopDisplayUnit;
		image.sprite = shopDisplayUnit.GetShopAnimal().GetAnimalProperty().explainImg;
		//text.text = shopDisplayUnit.GetShopAnimal().GetAnimalProperty().returnScoreAction() + "\n" + shopDisplayUnit.GetShopAnimal().GetAnimalProperty().returnBallAction();
	}
	*/
}
