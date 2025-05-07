using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager Instance { get; private set; }

    private GlobalSaveData globalSaveData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one SaveDataManager!" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public GlobalSaveData NewGame()
    {
        globalSaveData = new GlobalSaveData(new List<List<animalProperty>>() { GlobalManager.instance.animals }, 0, new List<Dictionary<string, int>>(), new AnimalBallPassTimes(), GlobalManager.instance.curCoinAmount, 0, 0, new List<Dictionary<string, int>>(), new List<Dictionary<string, int>>(), new List<BallInfo>());
        return globalSaveData;
    }

    public void SaveGame(List<List<animalProperty>> animalPropertyListByLevel, int currentLevelIndex, List<Dictionary<string, int>> pointsByAnimal, AnimalBallPassTimes animalBallPassTimes, int currentCoin, int coinUsedForUpgrade, int maxBallPassTimes, List<Dictionary<string, int>> animalLevelList, List<Dictionary<string, int>> animalPriceList, List<BallInfo> ballInfoList)
    {
        globalSaveData = new GlobalSaveData(animalPropertyListByLevel, currentLevelIndex, pointsByAnimal, animalBallPassTimes, currentCoin, coinUsedForUpgrade, maxBallPassTimes, animalLevelList, animalPriceList, ballInfoList);
    }

    public GlobalSaveData LoadGame()
    {
        if (globalSaveData == null)
        {
            Debug.Log("No save data found. Start a new game.");
            NewGame();
        }
        return globalSaveData;
    }
}
