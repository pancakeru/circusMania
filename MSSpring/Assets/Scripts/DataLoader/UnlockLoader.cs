using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UnlockData
{
    public int level;
    public List<string> animalToUnlock = new List<string>();
}

public class UnlockLoader : MonoBehaviour
{
    public List<UnlockData> allUnlockData = new List<UnlockData>();
    public void Load()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("UnlockDataCVS");
        List<string[]> rows = DataManager.instance.ParseCSV(csvFile.text);

        for (int i = 1; i < rows.Count; i++) // Skip header
        {
            string[] fields = rows[i];

            int level = int.Parse(fields[0]);
            string[] animals = fields[1].Split(new[] { ", " }, System.StringSplitOptions.None);

            UnlockData data = new UnlockData();
            data.level = level;
            data.animalToUnlock = new List<string>(animals);

            allUnlockData.Add(data);
        }
    }
}
