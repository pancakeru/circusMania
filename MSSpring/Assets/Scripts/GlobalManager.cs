using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

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
			
			InitAnimalUnlock();
            InitAnimalPrice();
			InitAnimalLevel();
        }

		//Screen.SetResolution(1920,1080,FullScreenMode.ExclusiveFullScreen);
    }

	private void Start()
	{
        OnNextGlobalLevel?.Invoke(globalLevelArray[0]);
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

					Debug.Log("现在仓库有" + infoMessage);
					break;

				case TestAction.LogCoin:
					Debug.Log("现在金币是" + curCoinAmount);
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
        foreach (string animalName in DataManager.instance.unlockLoader.allUnlockData[currentLevelIndex].animalToUnlock)
		{
			if (isAnimalUnlocked.ContainsKey(animalName))
			{
				isAnimalUnlocked[animalName] = true;
			}
		}
    }

    public Dictionary<string, int> animalPrices = new Dictionary<string, int>();
    public Dictionary<string, int> animalInitPrice = new Dictionary<string, int>();
    public int maxPrice = 99;
    public void InitAnimalPrice()
	{
        foreach (UnlockData dataRow in DataManager.instance.unlockLoader.allUnlockData)
        {
            foreach (string animalName in dataRow.animalToUnlock)
            {
                if (dataRow.animalToUnlock.Contains(animalName))
                {
                    animalInitPrice[animalName] = dataRow.initialPrice;
                }
            }
        }

        foreach (animalProperty animal in allAnimals.properies)
        {
			if (animalInitPrice.ContainsKey(animal.animalName)) animalPrices[animal.animalName] = animalInitPrice[animal.animalName];
            else Debug.LogError("ERROR");
        }
    }

    public void CalculateAnimalPrice()
    {
        AnimalBallPassTimes allAnimalBallPassTimes = ShowManager.instance.GetComponent<ShowAnimalBallPassTimesCounter>().GenerateAnimalBallPassTimes();

        foreach (animalProperty animal in allAnimals.properies)
        {
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

			Debug.Log($"{animal.animalName}: {myBallPassTimes}");

            if (myBallPassTimes <= 10) UpdatePrice(animal.animalName, (int)(animalPrices[animal.animalName] * (1f - 0.5f * (myBallPassTimes / 10f))));
			else UpdatePrice(animal.animalName, animalPrices[animal.animalName] + myBallPassTimes/2 );

            ShowManager.instance.GetComponent<ShowAnimalBallPassTimesCounter>().ResetAnimalBallPassTimes(animal.animalName);
        }
    }

    public void UpdatePrice(string animalName, int updateAmount)
	{
		//animalPrices[animalName] = initPrice + updateAmount;
        //animalPrices[animalName] = Math.Clamp(animalPrices[animalName], initPrice, maxPrice);
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
