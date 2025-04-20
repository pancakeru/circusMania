using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceData
{
    public string animalName;
    public int basePrice;
    public int pricePerLv;
}

public class PriceLoader : MonoBehaviour
{
    public List<PriceData> priceData = new List<PriceData>();
    public void Load()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("PriceDataCVS");
        List<string[]> rows = DataManager.instance.ParseCSV(csvFile.text);

        for (int i = 1; i < rows.Count; i++) // Skip header
        {
            PriceData data = new PriceData();
            data.animalName = rows[i][0];
            data.basePrice = int.Parse(rows[i][1]);
            data.pricePerLv = int.Parse(rows[i][2]);

            priceData.Add(data);
            //Debug.Log($"{priceData[i - 1].animalName}: {priceData[i - 1].basePrice}, {priceData[i - 1].pricePerLv}");
        }
    }
}
