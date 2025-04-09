using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalData
{
    public string scoreColor;
    public List<int> score = new List<int>();
    public List<int> restTurn = new List<int>();
    public List<int> skillCondition = new List<int>();
    public List<int> skillNumber = new List<int>();
}

public class AnimalLoader : MonoBehaviour
{
    public Dictionary<string, AnimalData> animalData = new Dictionary<string, AnimalData>();

    public void Load()
    {
        foreach(animalProperty animal in GlobalManager.instance.allAnimals.properies)
        {
            animalData[animal.animalName] = transferData(animal);
        }
    }

    public AnimalData transferData(animalProperty animal)
    {
        AnimalData data = new AnimalData();
        TextAsset csvFile = Resources.Load<TextAsset>("AnimalData/" + "AnimalDataCVS" + animal.animalName);
        List<string[]> rows = DataManager.instance.ParseCSV(csvFile.text);

        for (int i = 1; i < rows.Count; i++) // Skip header
        {
            string[] fields = rows[i];

            data.scoreColor = fields[0];
            for (int j = 1; j < fields.Length; j++)
            {
                if (string.IsNullOrEmpty(fields[j])) continue; 

                if (i == 1) data.score.Add(int.Parse(fields[j]));
                if (i == 2) data.restTurn.Add(int.Parse(fields[j]));
                if (i == 3) data.skillCondition.Add(int.Parse(fields[j]));
                if (i == 4) data.skillNumber.Add(int.Parse(fields[j]));
            }
        }

        return data;
    }
}
