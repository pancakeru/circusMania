using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
	public static ShopManager Instance { get; private set; }

	private List<ShopAnimal> animalInventory;

	private List<ShopAnimal> displayingList;
	private List<ShopAnimal> boughtList;
	private int displayColumnNumber = 4;

	private int coin;
	[SerializeField]private int rollPrice = 1;

	[SerializeField] GameObject shopDisplayUnitContainer;
	[SerializeField] GameObject shopDisplayUnitPrefab;
	[SerializeField] TextMeshProUGUI coinText;

	MenuController menuController;

	private GameObject audioObj;
	private AudioManagerScript audioScript;

	private void Awake()
	{
		if (Instance != null) {
			Debug.LogError("There is more than one ShopManager!" + transform + "-" + Instance);
			Destroy(gameObject);
			return;
		}
		Instance = this;

		menuController = FindAnyObjectByType<MenuController>();
        GlobalManager.OnNextGlobalLevel += GlobalManager_OnNextGlobalLevel;
    }

	private void Start()
	{
		//GlobalManager_OnNextGlobalLevel();
		audioObj = GameObject.FindWithTag("audio manager");
		audioScript = audioObj.GetComponent<AudioManagerScript>();
	}

	private void GlobalManager_OnNextGlobalLevel(GlobalLevel level)
	{
		animalInventory = new List<ShopAnimal>();
		foreach (AnimalPool.AnimalEntry animalEntry in level.animalPool.animals) {
			int count = animalEntry.count;
			for (int i = 0; i < count; i++) {
				animalInventory.Add(new ShopAnimal(animalEntry.animalProperty));
			}
		}

		displayingList = new List<ShopAnimal>();
		boughtList = new List<ShopAnimal>();
		Display();
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
				shopDisplayUnit.GetComponent<ShopDisplayUnit>().displayIndex = displayingList.IndexOf(shopAnimal);
				shopDisplayUnit.GetComponent<ShopDisplayUnit>().SetUp(shopAnimal);
			}
		}
	}

	public int GetDisplayingListCount()
	{
		return displayingList.Count;
	}

	public bool Buy(GameObject gameObject)
	{
		ShopAnimal shopAnimal = gameObject.GetComponent<ShopDisplayUnit>().GetShopAnimal();
		//int shopAnimalPrice = shopAnimal.GetAnimalProperty().animalPrice;
		int shopAnimalPrice = 10;
        if (coin >= shopAnimalPrice) {
			coin -= shopAnimalPrice;
            DisplayCoin();
            animalInventory.Remove(shopAnimal);
			boughtList.Add(shopAnimal);
			return true;
		} else {
			//Debug.Log("You do not have sufficient coins to buy this!");
			return false;
		}
	}

	public void Roll()
	{
		if (coin >= rollPrice) {
			coin -= rollPrice;
            DisplayCoin();
            Display();
		} else {
			//Debug.Log("You do not have sufficient coins to roll!");
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

	public void Enable()
	{
		transform.parent.GetComponent<Canvas>().enabled = true;
		coin = GlobalManager.instance.getCurCoinAmount();
		DisplayCoin();
	}

	private void DisplayCoin()
	{
        coinText.text = "Coin: " + coin.ToString();
    }

	public void Disable()
	{
		//结算购买动物
		foreach (ShopAnimal an in boughtList) {
			GlobalManager.instance.addAnAnimal(an.GetAnimalProperty());
		}
		boughtList.Clear();

		GlobalManager.instance.setCoinAmount(coin);

		transform.parent.GetComponent<Canvas>().enabled = false;
		menuController.Enable();
	}

	public void CallUISound() {
		audioScript.PlayUISound(audioScript.UI[0]);
	}
}
