using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopDisplayUnit : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private TextMeshProUGUI priceText;
	[SerializeField] private Button buyButton;
	[SerializeField] private GameObject outOfStock;

	private ShopAnimal shopAnimal;

	public void SetUp(ShopAnimal shopAnimal)
	{
		this.shopAnimal=shopAnimal;
		image.sprite = shopAnimal.GetAnimalProperty().animalCoreImg;
		priceText.text = shopAnimal.GetAnimalProperty().animalPrice.ToString();
		outOfStock.SetActive(false);
	}

	public void Buy(GameObject gameObject){
		//TODO:修改购买逻辑，先检测是否可以购买再进行购买
		ShopManager.Instance.Buy(gameObject);
		outOfStock.SetActive(true);
		buyButton.gameObject.SetActive(false);
	}

	public ShopAnimal GetShopAnimal(){
		return shopAnimal;
	}
}
