using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public UnlockLoader unlockLoader;
    public PriceLoader priceLoader;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public List<string[]> ParseCSV(string csvText)
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
