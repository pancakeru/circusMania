using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopDisplayUnit : MonoBehaviour
{
	[SerializeField] private Image image;
	[SerializeField] private TextMeshProUGUI priceText;
	[SerializeField] private Button buyButton;
	[SerializeField] private GameObject outOfStock;
	[SerializeField] private HoverExplainSystem hoverExplainSystem;

	private ShopAnimal shopAnimal;

	[HideInInspector] public int displayIndex;

	public void SetUp(ShopAnimal shopAnimal)
	{
		this.shopAnimal = shopAnimal;
		Debug.Log(shopAnimal== null);
		image.sprite = shopAnimal.GetAnimalProperty().animalCoreImg;
		priceText.text = "$ " + shopAnimal.GetAnimalProperty().animalPrice.ToString();
		outOfStock.SetActive(false);

		hoverExplainSystem.OnHover += HoverExplainSystem_OnHover;
	}

	public void Buy(GameObject gameObject)
	{
		if (ShopManager.Instance.Buy(gameObject)) {
			outOfStock.SetActive(true);
			buyButton.gameObject.SetActive(false);
			image.GetComponent<HoverExplainSystem>().enabled = false;
		}
	}

	private void HoverExplainSystem_OnHover(object sender, HoverExplainSystem.OnHoverEventArgs e)
	{
		int displayingListCount = ShopManager.Instance.GetDisplayingListCount();
		e.hoverExplanation.GetComponent<ShopHoverExplanationSingle>().SetUp(this);
		if (displayIndex <= displayingListCount / 2 - 1) {
			e.hoverExplanation.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition + new Vector2(-400, 0);
		} else {
			e.hoverExplanation.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition + new Vector2(300, 0);
		}
	}

	public ShopAnimal GetShopAnimal()
	{
		return shopAnimal;
	}
}
