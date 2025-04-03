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
    public AnimalStart allAnimals;

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

    #region 动物价格/等级
    public Dictionary<string, int> animalPrices = new Dictionary<string, int>();
    public int initPrice = 1;
    public void InitAnimalPrice()
	{
		foreach(animalProperty animal in allAnimals.properies)
		{
            animalPrices[animal.animalName] = initPrice;
        }
	}

	public void UpdatePrice(string animalName, int updateAmount)
	{
		animalPrices[animalName] += updateAmount;
		Math.Clamp(animalPrices[animalName], 1, 99);
    }

    public Dictionary<string, int> animalLevels = new Dictionary<string, int>();
    int initLevel = 1;
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
        Math.Clamp(animalLevels[animalName], 1, 99);
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
