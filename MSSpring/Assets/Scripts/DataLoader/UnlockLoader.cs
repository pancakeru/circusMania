using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UnlockData
{
    public int level;
    public List<string> animalToUnlock = new List<string>();
    public int initialPrice;
}

public class UnlockLoader : MonoBehaviour
{
    public List<UnlockData> allUnlockData = new List<UnlockData>();
    void Awake()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("UnlockDataCVS");
        List<string[]> rows = ParseCSV(csvFile.text);

        for (int i = 1; i < rows.Count; i++) // Skip header
        {
            string[] fields = rows[i];

            int level = int.Parse(fields[0]);
            string[] animals = fields[1].Split(new[] { ", " }, System.StringSplitOptions.None);
            int price = int.Parse(fields[2]);

            UnlockData data = new UnlockData();
            data.level = level;
            data.animalToUnlock = new List<string>(animals);
            data.initialPrice = price;

            allUnlockData.Add(data);
        }
    }

    List<string[]> ParseCSV(string csvText)
    {
        var lines = new List<string[]>();
        var reader = new StringReader(csvText);
        string line;

        while ((line = reader.ReadLine()) != null)
        {
            lines.Add(ParseCSVLine(line));
        }

        return lines;
    }

    string[] ParseCSVLine(string line)
    {
        var values = new List<string>();
        bool inQuotes = false;
        string current = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '\"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                values.Add(current.Trim());
                current = "";
            }
            else
            {
                current += c;
            }
        }

        values.Add(current.Trim());
        return values.ToArray();
    }
}
