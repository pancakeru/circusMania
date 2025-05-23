using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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

    [Header("test Level")]
    public LevelProperty tutorialProperty;

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

    [SerializeField] LevelPreviewController levelPreviewController;

    [Header("Global Save Data")]
    [SerializeField, ReadOnly] private GlobalSaveData globalSaveData;
    private bool hasSaveData = false;
    private List<ISaveData> saveDataObjectList;
    public Dictionary<string, int> temporaryPointsByAnimal;
    [HideInInspector] public AnimalBallPassTimes temporaryAnimalBallPassTimes;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        globalLevelArray = Resources.LoadAll<GlobalLevel>(globalLevelFolderPath);
        if (globalLevelArray.Length > 1)
        {
            Array.Sort(globalLevelArray, (a, b) => a.levelIndex.CompareTo(b.levelIndex));
        }

        temporaryPointsByAnimal = new Dictionary<string, int>();
        //Screen.SetResolution(1920,1080,FullScreenMode.ExclusiveFullScreen);
    }

    private void Update()
    {
        if (ifTestAction)
        {
            ifTestAction = false;
            switch (testAction)
            {
                case TestAction.Add:
                    addAnAnimal(toTestAdd);
                    break;

                case TestAction.LogInfo:
                    string infoMessage = "";
                    foreach (animalProperty apt in animals)
                    {
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

    public GlobalLevel[] GetGlobalLevelArray()
    {
        return globalLevelArray;
    }

    public GlobalLevel GetCurrentGlobalLevel()
    {
        return globalLevelArray[currentLevelIndex];
    }

    public LevelProperty GetTutorialLevel()
    {
        return tutorialProperty;
    }

    public void ToNextGlobalLevel()
    {
        if (ShowManager.win)
        {
            if (currentLevelIndex + 1 < globalLevelArray.Length)
            {
                currentLevelIndex += 1;
                OnNextGlobalLevel?.Invoke(globalLevelArray[currentLevelIndex]);

                UpdateGlobalSaveDataOnNextGlobalLevel();
            }
            else
            {
                currentLevelIndex += 1;
                UpdateGlobalSaveDataOnNextGlobalLevel();
                summaryScript.instance.SummaryLevel();
            }
        }
        else
        {
            UpdateGlobalSaveDataOnNextGlobalLevel();
            summaryScript.instance.SummaryLevel();
        }

        //TODO:这里要加一再触发结束
    }

    #region Global Save Data
    public GlobalSaveData GetGlobalData()
    {
        return globalSaveData;
    }

    public void SaveGlobalSaveData()
    {
        SaveDataManager.Instance.SaveGame(globalSaveData);
    }

    public void NewGame()
    {
        globalSaveData = SaveDataManager.Instance.NewGame();
        StartInitialization(true);
        hasSaveData = true;
        CanvasMain.instance.ShowCutScene();
    }

    public void LoadGame()
    {
        if (SaveDataManager.Instance.HasSaveDataExisted())
        {
            globalSaveData = SaveDataManager.Instance.LoadGame();
            StartInitialization(false);
            hasSaveData = true;
        }
        else
        {
            NewGame();
        }
    }

    private void StartInitialization(bool isNewGame)
    {
        DataManager.instance.animalLoader.Load();
        DataManager.instance.unlockLoader.Load();
        DataManager.instance.priceLoader.Load();

        currentLevelIndex = globalSaveData.currentLevelIndex;
        OnNextGlobalLevel?.Invoke(globalLevelArray[currentLevelIndex]);

        if (isNewGame)
        {
            if (ifDoTestInitialize)
            {
                curCoinAmount = testCoinNum;
                globalSaveData.currentCoin = curCoinAmount;

                if (animals.Count == 0)
                {
                    foreach (animalProperty apt in testProperties.properies)
                    {
                        addAnAnimal(apt);
                    }
                    globalSaveData.animals = animals;
                }
            }

            InitAnimalUnlock();

            InitAnimalPriceOnNew();
            globalSaveData.animalPriceLevel = animalPriceLevel;
            globalSaveData.animalPrices = animalPrices;

            InitAnimalLevel();
            globalSaveData.animalLevels = animalLevels;
        }
        else
        {
            curCoinAmount = globalSaveData.currentCoin;

            animals = globalSaveData.animals;
            foreach (animalProperty animal in animals)
            {
                foreach (animalProperty animalStart in allAnimals.properies)
                {
                    if (animal.animalName == animalStart.animalName)
                    {
                        animal.animalCoreImg = animalStart.animalCoreImg;
                        animal.explainImg = animalStart.explainImg;
                    }
                }
            }
            globalSaveData.animals = animals;

            isAnimalUnlocked = globalSaveData.isAnimalUnlocked;

            InitAnimalPriceOnLoad();
            animalPriceLevel = globalSaveData.animalPriceLevel;
            animalPrices = globalSaveData.animalPrices;

            maxLevel = 5;
            animalLevels = globalSaveData.animalLevels;
        }

        SetAnimalProperty();

        TroupeController.instance.Initialize();

        saveDataObjectList = FindAllSaveDataObjects();
        foreach (ISaveData saveDataObject in saveDataObjectList)
        {
            saveDataObject.LoadGlobalSaveData(globalSaveData);
        }
    }

    private void UpdateGlobalSaveDataOnNextGlobalLevel()
    {
        globalSaveData.animalPropertyListByLevel.Add(animals);

        globalSaveData.currentLevelIndex = currentLevelIndex;

        foreach (string animal in temporaryPointsByAnimal.Keys)
        {
            if (globalSaveData.pointsByAnimal.ContainsKey(animal))
            {
                globalSaveData.pointsByAnimal[animal] += temporaryPointsByAnimal[animal];
            }
            else
            {
                globalSaveData.pointsByAnimal.Add(animal, temporaryPointsByAnimal[animal]);
            }
        }
        temporaryPointsByAnimal.Clear();

        globalSaveData.animalBallPassTimes += temporaryAnimalBallPassTimes;
        temporaryAnimalBallPassTimes = new AnimalBallPassTimes();

        globalSaveData.currentCoin = curCoinAmount;

        globalSaveData.animals = animals;
    }

    public void SetSaveDataCurrentLevelIndex()
    {
        globalSaveData.currentLevelIndex = currentLevelIndex;
    }

    public void AddPointsToTemporaryPointsByAnimal(string animal, int points)
    {
        if (temporaryPointsByAnimal.ContainsKey(animal))
        {
            temporaryPointsByAnimal[animal] += points;
        }
        else
        {
            temporaryPointsByAnimal.Add(animal, points);
        }
    }

    public void SetCoinUsedForUpgrade(int coinUsedForUpgrade)
    {
        globalSaveData.currentCoin = curCoinAmount;
        if (coinUsedForUpgrade != 0)
        {
            globalSaveData.coinUsedForUpgrade += coinUsedForUpgrade;
        }
        globalSaveData.animals = animals;
    }

    public void SetMaxBallPassTimes(List<int> totalBallPassTimesListPerShow)
    {
        foreach (int ballPassTimes in totalBallPassTimesListPerShow)
        {
            if (ballPassTimes > globalSaveData.maxBallPassTimes)
            {
                globalSaveData.maxBallPassTimes = ballPassTimes;
            }
        }
    }

    private void SetAnimalLevelList(Dictionary<string, int> animalLevels)
    {
        globalSaveData.animalLevels = animalLevels;
    }

    private void SetAnimalPrice(Dictionary<string, int> animalPriceLevel, Dictionary<string, int> animalPrices)
    {
        globalSaveData.animalPriceLevel = animalPriceLevel;
        globalSaveData.animalPrices = animalPrices;
    }

    public void SetBallInfoList(List<BallInfo> ballInfoList)
    {
        globalSaveData.ballInfoList = ballInfoList;
    }

    public int GetMyBallIndex()
    {
        return globalSaveData.myBallIndex;
    }

    public void SetMyBallIndex(int index)
    {
        globalSaveData.myBallIndex = index;
    }

    private void SetIsAnimalUnlocked()
    {
        globalSaveData.isAnimalUnlocked = isAnimalUnlocked;
    }

    public Dictionary<string, List<int>> GetAnimalPriceChanges()
    {
        if (globalSaveData.animalPriceChanges != null)
        {
            return globalSaveData.animalPriceChanges;
        }
        else
        {
            return null;
        }
    }

    public void SetAnimalPriceChanges(Dictionary<string, List<int>> animalPriceChanges)
    {
        globalSaveData.animalPriceChanges = animalPriceChanges;
    }

    private List<ISaveData> FindAllSaveDataObjects()
    {
        IEnumerable<ISaveData> saveDataObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveData>();
        return new List<ISaveData>(saveDataObjects);
    }

    public void ClearGlobalManagerSaveData()
    {
        animals.Clear();
        globalSaveData = new GlobalSaveData(new List<List<animalProperty>>(), 0, new Dictionary<string, int>(), new AnimalBallPassTimes(), 0, 0, 0, new List<animalProperty>(), new Dictionary<string, bool>(), new Dictionary<string, int>(), new Dictionary<string, int>(), new Dictionary<string, List<int>>(), new Dictionary<string, int>(), new List<BallInfo>(), 0);
        hasSaveData = false;
    }

    private void OnApplicationQuit()
    {
        if (hasSaveData)
        {
            SaveGlobalSaveData();
        }
    }
    #endregion

    // 动物管理
    #region 动物管理
    public List<animalProperty> animals { get; private set; } = new List<animalProperty>();

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

    void SetAnimalProperty()
    {
        foreach (animalProperty animal in allAnimals.properies)
        {
            AnimalData animalData = DataManager.instance.animalLoader.animalData[animal.animalName];
            animal.baseRedChange = animalData.scoreColor == "Red Score" ? animalData.score[animalLevels[animal.animalName]] : 0;
            animal.baseYellowChange = animalData.scoreColor == "Yellow Score" ? animalData.score[animalLevels[animal.animalName]] : 0;
            animal.baseBlueChange = animalData.scoreColor == "Blue Score" ? animalData.score[animalLevels[animal.animalName]] : 0;
            animal.restTurn = animalData.restTurn[animalLevels[animal.animalName]];
            animal.mechanicActiveNum = animalData.skillCondition.Count > 1 ? animalData.skillCondition[animalLevels[animal.animalName]] : animalData.skillCondition[0];
            animal.skillNum = animalData.skillNumber.Count > 1 ? animalData.skillNumber[animalLevels[animal.animalName]] : animalData.skillNumber[0];

            //Debug.Log($"{animal.animalName}: {animalData.skillCondition.Count}");
        }
    }

    #endregion
    // 金币管理
    #region 金币管理
    public int curCoinAmount { get; private set; } = 0;

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
        if (checkIfCanChangeCoin(n))
        {
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
        SetIsAnimalUnlocked();
    }

    public void UnlockAnimal()
    {
        //Debug.Log("解锁中，当前level index是"+currentLevelIndex);
        foreach (UnlockData entry in DataManager.instance.unlockLoader.unlockData)
        {
            if (entry.level == currentLevelIndex)
            {
                foreach (string animalName in entry.animalToUnlock)
                {
                    //Debug.Log("解锁中，当前dongwu是" +animalName);
                    if (isAnimalUnlocked.ContainsKey(animalName))
                    {
                        isAnimalUnlocked[animalName] = true;
                    }
                    else
                    {
                        Debug.LogWarning($"动物名 {animalName} 不在解锁字典中！");
                    }
                }
                SetIsAnimalUnlocked();
                return; // 找到了就退出
            }
        }

    }

    public Dictionary<string, int> animalPrices = new Dictionary<string, int>();
    public Dictionary<string, int> animalPriceLevel = new Dictionary<string, int>();
    public Dictionary<string, int> animalBasePrice = new Dictionary<string, int>();
    public Dictionary<string, int> animalPricePerLv = new Dictionary<string, int>();

    public int maxPrice = 40;
    private void InitAnimalPriceOnNew()
    {
        foreach (PriceData dataRow in DataManager.instance.priceLoader.priceData)
        {
            animalPriceLevel[dataRow.animalName] = 5;
            animalBasePrice[dataRow.animalName] = dataRow.basePrice;
            animalPricePerLv[dataRow.animalName] = dataRow.pricePerLv;
            UpdatePrice(dataRow.animalName);
        }
    }

    private void InitAnimalPriceOnLoad()
    {
        foreach (PriceData dataRow in DataManager.instance.priceLoader.priceData)
        {
            animalBasePrice[dataRow.animalName] = dataRow.basePrice;
            animalPricePerLv[dataRow.animalName] = dataRow.pricePerLv;
        }
    }

    public void CalculateAnimalPrice()
    {
        AnimalBallPassTimes allAnimalBallPassTimes = ShowManager.instance.GetComponent<ShowAnimalBallPassTimesCounter>().GenerateAnimalBallPassTimes();
        temporaryAnimalBallPassTimes = allAnimalBallPassTimes;

        foreach (animalProperty animal in allAnimals.properies)
        {
            bool isNumberZero = true;
            foreach (animalProperty checkAnimal in getAllAnimals())
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

            //Debug.Log($"{animal.animalName}'s BPT: {myBallPassTimes}");

            UpdatePriceLevel(animal.animalName, myBallPassTimes);
            UpdatePrice(animal.animalName);
            SetAnimalPrice(animalPriceLevel, animalPrices);

            //Debug.Log($"{animal.animalName}'s Price: {animalPrices[animal.animalName]}");

            ShowManager.instance.GetComponent<ShowAnimalBallPassTimesCounter>().ResetAnimalBallPassTimes(animal.animalName);
        }
    }


    public void DirectResetAnimalBallPassTime()
    {
        foreach (animalProperty animal in allAnimals.properies)
        {
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
        animalPriceLevel[animalName] = Math.Clamp(animalPriceLevel[animalName], 1, 9);
    }

    public Dictionary<string, int> animalLevels = new Dictionary<string, int>();
    int initLevel = 1;
    public int maxLevel = 5;
    public void InitAnimalLevel()
    {
        maxLevel = 5;

        foreach (animalProperty animal in allAnimals.properies)
        {
            animalLevels[animal.animalName] = initLevel;
        }
    }

    public void UpdateLevel(string animalName, int updateAmount)
    {
        animalLevels[animalName] += updateAmount;
        animalLevels[animalName] = Math.Clamp(animalLevels[animalName], initLevel, maxLevel);
        SetAnimalProperty();
        TroupeController.instance.DisplayCardDetail(TroupeController.instance.troupeCardSelected);

        SetAnimalLevelList(animalLevels);
    }

    #endregion

    public enum TestAction
    {
        Add,
        LogInfo,
        LogCoin
    }
}

public class ReadOnlyAttribute : PropertyAttribute
{

}
/*
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}*/
