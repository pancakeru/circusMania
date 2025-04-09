using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class GlobalManager : MonoBehaviour, IGeneralManager
{
	public static GlobalManager instance;

	public static event Action<GlobalLevel> OnNextGlobalLevel;

	private string globalLevelFolderPath = "GlobalLevels";
	private GlobalLevel[] globalLevelArray;
	public int currentLevelIndex = 0;
    public AnimalStart allAnimals; //all kinds of animal

    public AnimalBallPassTimes animalBallPassTimes;

    [Header("Show Score Effect Color")]
	public Color redEffect;
	public Color blueEffect;
	public Color yellowEffect;

	[Header("For test")]
	[SerializeField]
	private AnimalStart testProperties;
	[SerializeField]
	private int testCoinNum;
	[SerializeField]
	private bool ifDoTestInitialize;
	[SerializeField]
	private TestAction testAction;
	[SerializeField]
	private bool ifTestAction = false;
	[SerializeField]
	private animalProperty toTestAdd;

	[Header("Others")]
	[SerializeField] GameObject messageBox;


    private void Awake()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			Destroy(gameObject);
		}

		globalLevelArray = Resources.LoadAll<GlobalLevel>(globalLevelFolderPath);
		if (globalLevelArray.Length > 1) {
			Array.Sort(globalLevelArray, (a, b) => a.levelIndex.CompareTo(b.levelIndex));
		}

        if (ifDoTestInitialize)
        {

            foreach (animalProperty apt in testProperties.properies)
                addAnAnimal(apt);
            curCoinAmount = testCoinNum;
			
			
        }

        //Screen.SetResolution(1920,1080,FullScreenMode.ExclusiveFullScreen);
    }

	private void Start()
	{
        DataManager.instance.animalLoader.Load();
        DataManager.instance.unlockLoader.Load();
        DataManager.instance.priceLoader.Load();

        OnNextGlobalLevel?.Invoke(globalLevelArray[0]);
        InitAnimalUnlock();
        InitAnimalPrice();
        InitAnimalLevel();
    }

	private void Update()
	{
		if (ifTestAction) {
			ifTestAction = false;
			switch (testAction) {
				case TestAction.Add:
					addAnAnimal(toTestAdd);
					break;

				case TestAction.LogInfo:
					string infoMessage = "";
					foreach (animalProperty apt in animals) {
						infoMessage += apt.name;
						infoMessage += ", ";
					}

					//Debug.Log("现在仓库有" + infoMessage);
					break;

				case TestAction.LogCoin:
					//Debug.Log("现在金币是" + curCoinAmount);
					break;
			}

		}
	}

	public GlobalLevel GetCurrentGlobalLevel()
	{
		return globalLevelArray[currentLevelIndex];
	}

	public void ToNextGlobalLevel()
	{
		if (currentLevelIndex + 1 < globalLevelArray.Length) {
			currentLevelIndex += 1;
			OnNextGlobalLevel?.Invoke(globalLevelArray[currentLevelIndex]);
		}
	}

	// 动物管理
	#region 动物管理
	private List<animalProperty> animals = new List<animalProperty>();

	public List<animalProperty> getAllAnimals()
	{
		//Debug.Log("动物list有这些: " + animals.Count);
		return animals;
	}

	public void addAnAnimal(animalProperty newAnimal)
	{
		animals.Add(newAnimal);
	}

	public void removeAnAnimal(animalProperty theAnimal)
	{
		animals.Remove(theAnimal);
	}


	#endregion
	// 金币管理
	#region 金币管理
	private int curCoinAmount = 0;

	public int getCurCoinAmount()
	{
		return curCoinAmount;
	}

	public bool checkIfCanChangeCoin(int n)
	{
		return curCoinAmount + n >= 0;
	}

	public bool changeCoinAmount(int n)
	{
		if (checkIfCanChangeCoin(n)) {
			curCoinAmount += n;
			return true;
		}
		return false;
	}

	public void setCoinAmount(int n)
	{
		curCoinAmount = n;
	}
    #endregion

    #region 动物解锁/价格/等级

    public Dictionary<string, bool> isAnimalUnlocked = new Dictionary<string, bool>();
    public void InitAnimalUnlock()
    {
        foreach (animalProperty animal in allAnimals.properies)
        {
            isAnimalUnlocked[animal.animalName] = false;
        }
		UnlockAnimal();
    }

	public void UnlockAnimal()
	{
		if (DataManager.instance.unlockLoader.unlockData.Count > currentLevelIndex)
		{
			foreach (string animalName in DataManager.instance.unlockLoader.unlockData[currentLevelIndex].animalToUnlock)
			{
				if (isAnimalUnlocked.ContainsKey(animalName))
				{
					isAnimalUnlocked[animalName] = true;
				}
			}
		}
    }

    public Dictionary<string, int> animalPrices = new Dictionary<string, int>();
    public Dictionary<string, int> animalPriceLevel = new Dictionary<string, int>();
    public Dictionary<string, int> animalBasePrice = new Dictionary<string, int>();
    public Dictionary<string, int> animalPricePerLv = new Dictionary<string, int>();

    public int maxPrice = 99;
    public void InitAnimalPrice()
	{
        foreach (PriceData dataRow in DataManager.instance.priceLoader.priceData)
        {
			animalPriceLevel[dataRow.animalName] = 5;
			animalBasePrice[dataRow.animalName] = dataRow.basePrice;
			animalPricePerLv[dataRow.animalName] = dataRow.pricePerLv;
			UpdatePrice(dataRow.animalName);
        }
    }

    public void CalculateAnimalPrice()
    {
        AnimalBallPassTimes allAnimalBallPassTimes = ShowManager.instance.GetComponent<ShowAnimalBallPassTimesCounter>().GenerateAnimalBallPassTimes();

        foreach (animalProperty animal in allAnimals.properies)
        {
			bool isNumberZero = true;
			foreach(animalProperty checkAnimal in getAllAnimals())
			{
				if (checkAnimal.animalName == animal.animalName) isNumberZero = false;

            }
			if (isNumberZero) return;

            int myBallPassTimes = 0;
            switch (animal.animalName.ToLower())
            {
                case "monkey": myBallPassTimes = allAnimalBallPassTimes.monkey; break;
                case "elephant": myBallPassTimes = allAnimalBallPassTimes.elephant; break;
                case "bear": myBallPassTimes = allAnimalBallPassTimes.bear; break;
                case "lion": myBallPassTimes = allAnimalBallPassTimes.lion; break;
                case "giraffe": myBallPassTimes = allAnimalBallPassTimes.giraffe; break;
                case "snake": myBallPassTimes = allAnimalBallPassTimes.snake; break;
                case "fox": myBallPassTimes = allAnimalBallPassTimes.fox; break;
                case "seal": myBallPassTimes = allAnimalBallPassTimes.seal; break;
                case "ostrich": myBallPassTimes = allAnimalBallPassTimes.ostrich; break;
                case "kangaroo": myBallPassTimes = allAnimalBallPassTimes.kangaroo; break;
                case "buffalo": myBallPassTimes = allAnimalBallPassTimes.buffalo; break;
                case "goat": myBallPassTimes = allAnimalBallPassTimes.goat; break;
                case "lizard": myBallPassTimes = allAnimalBallPassTimes.lizard; break;
                default:
                    Debug.LogWarning($"Bug Found"); break;
            }

			Debug.Log($"{animal.animalName}'s BPT: {myBallPassTimes}");

			UpdatePriceLevel(animal.animalName, myBallPassTimes);
            UpdatePrice(animal.animalName);

            Debug.Log($"{animal.animalName}'s Price: {animalPrices[animal.animalName]}");

            ShowManager.instance.GetComponent<ShowAnimalBallPassTimesCounter>().ResetAnimalBallPassTimes(animal.animalName);
        }
    }

    void UpdatePrice(string animalName)
    {
        animalPrices[animalName] = animalBasePrice[animalName] + animalPricePerLv[animalName] * animalPriceLevel[animalName];
    }

	void UpdatePriceLevel(string animalName, int ballPassTimes)
	{
		int basePriceLv = -2;
		int priceLvUpPerPass = 3;
		int maxPriceLvUpAmount = 3;
		animalPriceLevel[animalName] += basePriceLv + Math.Clamp(ballPassTimes / priceLvUpPerPass, 0, maxPriceLvUpAmount);
	}

    public Dictionary<string, int> animalLevels = new Dictionary<string, int>();
    int initLevel = 1;
    int maxLevel = 99;
    public void InitAnimalLevel()
    {
        foreach (animalProperty animal in allAnimals.properies)
        {
            animalLevels[animal.animalName] = initLevel;
        }
    }

    public void UpdateLevel(string animalName, int updateAmount)
    {
        animalLevels[animalName] += updateAmount;
        animalLevels[animalName] = Math.Clamp(animalLevels[animalName], initLevel, maxLevel);
    }

    #endregion

	public void ShowMessageBox(string text)
	{
		GameObject newMessageBox = Instantiate(messageBox, CanvasMain.instance.transform);
		newMessageBox.GetComponent<MessageBoxController>().uiText.text = text;
	}

    public enum TestAction
	{
		Add,
		LogInfo,
		LogCoin
	}
}
