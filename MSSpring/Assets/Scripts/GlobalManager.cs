using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour, IGeneralManager
{
	public static GlobalManager instance;

	public static event Action OnNextGlobalLevel;

	private string globalLevelFolderPath = "GlobalLevels";
	private GlobalLevel[] globalLevelArray;
	public int currentLevelIndex = 0;

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
    }

	private void Start()
	{
		
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
			OnNextGlobalLevel?.Invoke();
		}
	}

	// 动物管理
	#region 动物管理
	private List<animalProperty> animals = new List<animalProperty>();

	public List<animalProperty> getAllAnimals()
	{
		Debug.Log("动物list有这些: " + animals.Count);
		return animals;
	}

	public void addAnAnimal(animalProperty newAnimal)
	{
		animals.Add(newAnimal);
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

	public enum TestAction
	{
		Add,
		LogInfo,
		LogCoin
	}
}
