using System.Collections.Generic;

[System.Serializable]
public class GlobalSaveData
{
    public List<List<animalProperty>> animalPropertyListByLevel;
    public int currentLevelIndex;
    public Dictionary<string, int> pointsByAnimal;
    public AnimalBallPassTimes animalBallPassTimes;
    public int currentCoin;
    public int coinUsedForUpgrade;
    public int maxBallPassTimes;
    public List<animalProperty> animals;
    public Dictionary<string, bool> isAnimalUnlocked;
    public Dictionary<string, int> animalPrices;
    public Dictionary<string, int> animalPriceLevel;
    public Dictionary<string, int> animalLevels;
    public List<BallInfo> ballInfoList;

    public GlobalSaveData(List<List<animalProperty>> animalPropertyListByLevel, int currentLevelIndex, Dictionary<string, int> pointsByAnimal, AnimalBallPassTimes animalBallPassTimes, int currentCoin, int coinUsedForUpgrade, int maxBallPassTimes, List<animalProperty> animals, Dictionary<string, bool> isAnimalUnlocked, Dictionary<string, int> animalPrices, Dictionary<string, int> animalPriceLevel, Dictionary<string, int> animalLevels, List<BallInfo> ballInfoList)
    {
        this.animalPropertyListByLevel = animalPropertyListByLevel;
        this.currentLevelIndex = currentLevelIndex;
        this.pointsByAnimal = pointsByAnimal;
        this.animalBallPassTimes = animalBallPassTimes;
        this.currentCoin = currentCoin;
        this.coinUsedForUpgrade = coinUsedForUpgrade;
        this.maxBallPassTimes = maxBallPassTimes;
        this.animals = animals;
        this.isAnimalUnlocked = isAnimalUnlocked;
        this.animalPrices = animalPrices;
        this.animalPriceLevel = animalPriceLevel;
        this.animalLevels = animalLevels;
        this.ballInfoList = ballInfoList;
    }
}
