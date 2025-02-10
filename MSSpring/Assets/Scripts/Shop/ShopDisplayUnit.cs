using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopDisplayUnit : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private TextMeshProUGUI priceText;
	[SerializeField] private Button buyButton;
	[SerializeField] private GameObject outOfStock;
	[SerializeField] private TextMeshProUGUI hoverExplanationText;

	private ShopAnimal shopAnimal;

	public void SetUp(ShopAnimal shopAnimal)
	{
		this.shopAnimal = shopAnimal;
		image.sprite = shopAnimal.GetAnimalProperty().animalCoreImg;
		priceText.text = shopAnimal.GetAnimalProperty().animalPrice.ToString();
		outOfStock.SetActive(false);
		hoverExplanationText.text = shopAnimal.GetAnimalProperty().scoreActionTemplate + "\n" + shopAnimal.GetAnimalProperty().ballActionTemplate;
	}

	public void Buy(GameObject gameObject)
	{
		if (ShopManager.Instance.Buy(gameObject)) {
			outOfStock.SetActive(true);
			buyButton.gameObject.SetActive(false);
			image.GetComponent<HoverExplainSystem>().enabled=false;
		}
	}

	public ShopAnimal GetShopAnimal()
	{
		return shopAnimal;
	}
}
