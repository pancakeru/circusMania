using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
	public static ShopManager Instance { get; private set; }

	private string animalPropertiesFolder = "animalProperties";
	private animalProperty[] animalPropertyArray;
	private List<ShopAnimal> animalInventory;
	private int animalInventoryCount = 12;
	private List<ShopAnimal> displayingList;
	private List<ShopAnimal> boughtList;
	private int displayColumnNumber = 4;

	private int coin = 114514;
	private int rollPrice = 3;

	[SerializeField] GameObject shopDisplayUnitContainer;
	[SerializeField] GameObject shopDisplayUnitPrefab;
	[SerializeField] TextMeshProUGUI coinText;

	private void Awake()
	{
		if (Instance != null) {
			Debug.LogError("There is more than one ShopManager!" + transform + "-" + Instance);
			Destroy(gameObject);
			return;
		}
		Instance = this;

		animalPropertyArray = Resources.LoadAll<animalProperty>(animalPropertiesFolder);

		animalInventory = new List<ShopAnimal>();
		for (int i = 0; i < animalInventoryCount; i++) {
			int randomAnimal = Random.Range(0, animalPropertyArray.Length);
			animalInventory.Add(new ShopAnimal(animalPropertyArray[randomAnimal]));
		}

		displayingList = new List<ShopAnimal>();
		boughtList = new List<ShopAnimal>();
		Display();

		coinText.text = coin.ToString();
	}

	private void Display()
	{
		displayingList.Clear();
		if (animalInventory.Count >= displayColumnNumber) {
			List<int> randomDisplayIndex = GenerateUniqueRandomNumbers(0, animalInventory.Count - 1, displayColumnNumber);
			foreach (int i in randomDisplayIndex) {
				displayingList.Add(animalInventory[i]);
			}
		} else {
			displayingList.AddRange(animalInventory);
		}

		foreach (Transform shopDisplayUnitPrefabTransform in shopDisplayUnitContainer.transform) {
			Destroy(shopDisplayUnitPrefabTransform.gameObject);
		}
		if (displayingList != null) {
			foreach (ShopAnimal shopAnimal in displayingList) {
				GameObject shopDisplayUnit = Instantiate(shopDisplayUnitPrefab, shopDisplayUnitContainer.transform);
				shopDisplayUnit.GetComponent<ShopDisplayUnit>().SetUp(shopAnimal);
			}
		}
	}

	public void Buy(GameObject gameObject)
	{
		ShopAnimal shopAnimal = gameObject.GetComponent<ShopDisplayUnit>().GetShopAnimal();
		int shopAnimalPrice = shopAnimal.GetAnimalProperty().animalPrice;
		if (coin >= shopAnimalPrice) {
			coin -= shopAnimalPrice;
			coinText.text = coin.ToString();
			animalInventory.Remove(shopAnimal);
			boughtList.Add(shopAnimal);
		} else {
			Debug.Log("You do not have sufficient coins to buy this!");
		}
	}

	public void Roll()
	{
		if (coin >= rollPrice) {
			coin -= rollPrice;
			coinText.text = coin.ToString();
			Display();
		}
	}

	public List<int> GenerateUniqueRandomNumbers(int min, int max, int count)
	{
		if (max - min + 1 < count) {
			return null;//The range is too small to generate the required number of unique values
		}
		HashSet<int> numbers = new HashSet<int>();
		while (numbers.Count < count) {
			int randomNumber = Random.Range(min, max + 1);//Random.Range is inclusive for integers
			numbers.Add(randomNumber);
		}
		return new List<int>(numbers);
	}
}
