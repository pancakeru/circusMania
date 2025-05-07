using System.Collections.Generic;

public class GlobalSaveData
{
    public List<List<animalProperty>> animalPropertyListByLevel;
    public int currentLevelIndex;
    public List<Dictionary<string, int>> pointsByAnimal;
    public AnimalBallPassTimes animalBallPassTimes;
    public int currentCoin;
    public int coinUsedForUpgrade;
    public int maxBallPassTimes;
    public List<Dictionary<string, int>> animalLevelList;
    public List<Dictionary<string, int>> animalPriceList;
    public List<BallInfo> ballInfoList;

    public GlobalSaveData(List<List<animalProperty>> animalPropertyListByLevel, int currentLevelIndex, List<Dictionary<string, int>> pointsByAnimal, AnimalBallPassTimes animalBallPassTimes, int currentCoin, int coinUsedForUpgrade, int maxBallPassTimes, List<Dictionary<string, int>> animalLevelList, List<Dictionary<string, int>> animalPriceList, List<BallInfo> ballInfoList)
    {
        this.animalPropertyListByLevel = animalPropertyListByLevel;
        this.currentLevelIndex = currentLevelIndex;
        this.pointsByAnimal = pointsByAnimal;
        this.animalBallPassTimes = animalBallPassTimes;
        this.currentCoin = currentCoin;
        this.coinUsedForUpgrade = coinUsedForUpgrade;
        this.maxBallPassTimes = maxBallPassTimes;
        this.animalLevelList = animalLevelList;
        this.animalPriceList = animalPriceList;
        this.ballInfoList = ballInfoList;
    }
}
