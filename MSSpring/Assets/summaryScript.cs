using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class summaryScript : MonoBehaviour
{

    public static summaryScript instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); // 或者改成 DontDestroyOnLoad(gameObject); 看你需求
    }
    public winLoseScript wl;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SummaryLevel()
    {
        GlobalSaveData data = GlobalManager.instance.GetGlobalData();
        List<animalProperty> topProperty;
        List<int> topCounts;

        animalProperty topScorer;
        animalProperty topPasser;
        int topScore;
        int topPass;
        CalculateTop5PositiveGrowth(data.animalPropertyListByLevel, out topProperty, out topCounts);
        GetMaxAnimalScoreProperty(data.pointsByAnimal, out topScorer, out topScore);
        GetTopPassAnimal(data.animalBallPassTimes, out topPasser,out topPass);

        SummaryRequireInfo info = new SummaryRequireInfo(GlobalManager.instance.currentLevelIndex, topCounts.ToArray(), topPass, topScore, data.maxBallPassTimes, data.currentCoin + data.coinUsedForUpgrade, topProperty.ToArray(),topPasser , topScorer);
        wl.gameObject.SetActive(true);
        wl.SetData(info);
    }


    public static void CalculateTop5PositiveGrowth(
    List<List<animalProperty>> animalPropertyListByLevel,
    out List<animalProperty> topAnimals,
    out List<int> topCounts)
    {
        var countsByRound = new List<Dictionary<animalProperty, int>>();
        var positiveDiffAccumulated = new Dictionary<animalProperty, int>();
        var previous = new Dictionary<animalProperty, int>();

        foreach (var round in animalPropertyListByLevel)
        {
            Dictionary<animalProperty, int> currentCount = new();

            foreach (var animal in round)
            {
                if (currentCount.ContainsKey(animal))
                    currentCount[animal]++;
                else
                    currentCount[animal] = 1;
            }

            countsByRound.Add(currentCount);

            foreach (var kvp in currentCount)
            {
                var animal = kvp.Key;
                int current = kvp.Value;
                int last = previous.ContainsKey(animal) ? previous[animal] : 0;

                int diff = current - last;
                if (diff > 0)
                {
                    if (positiveDiffAccumulated.ContainsKey(animal))
                        positiveDiffAccumulated[animal] += diff;
                    else
                        positiveDiffAccumulated[animal] = diff;
                }
            }

            previous = new Dictionary<animalProperty, int>(currentCount);
        }

        // 取前五
        var sorted = positiveDiffAccumulated
            .OrderByDescending(pair => pair.Value)
            .Take(5)
            .ToList();

        topAnimals = sorted.Select(pair => pair.Key).ToList();
        topCounts = sorted.Select(pair => pair.Value).ToList();
    }

    public static void GetMaxAnimalScoreProperty(Dictionary<string, int> pointsByAnimal, out animalProperty maxProperty, out int maxScore)
    {
        string maxName = null;
        maxScore = int.MinValue;

        foreach (var pair in pointsByAnimal)
        {
            if (pair.Value > maxScore)
            {
                maxName = pair.Key;
                maxScore = pair.Value;
            }
        }

        // 查找对应 animalProperty
        maxProperty = default;

        foreach (var prop in GlobalManager.instance.allAnimals.properies)
        {
            if (prop.animalName == maxName)
            {
                maxProperty = prop;
                break;
            }

        }

    }

    public static void GetTopPassAnimal(AnimalBallPassTimes data, out animalProperty topProperty, out int topCount)
    {
        string topName = null;
        topCount = int.MinValue;
        topProperty = default;

        var type = typeof(AnimalBallPassTimes);
        var fields = type.GetFields();

        foreach (var field in fields)
        {
            if (field.FieldType == typeof(int))
            {
                int value = (int)field.GetValue(data);
                if (value > topCount)
                {
                    topCount = value;
                    topName = char.ToUpper(field.Name[0]) + field.Name.Substring(1);
                }
            }
        }

        if (topName == null)
        {
            Debug.LogWarning("未找到最大传球动物");
            return;
        }

        foreach (var prop in GlobalManager.instance.allAnimals.properies)
        {
            if (prop.animalName == topName)
            {
                topProperty = prop;
                return;
            }
        }

        Debug.LogWarning($"未找到名为 {topName} 的 animalProperty");
    }
}
