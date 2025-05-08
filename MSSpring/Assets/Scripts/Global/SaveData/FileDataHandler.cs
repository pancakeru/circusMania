using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GlobalSaveData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GlobalSaveData loadedGlobalSaveData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                //Load the serialized data from the file
                string dataToLoad = File.ReadAllText(fullPath);

                //Deserialize the data from Json back into the C# Global Save Data
                loadedGlobalSaveData = JsonConvert.DeserializeObject<GlobalSaveData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file: " + fullPath + "\n" + e);
            }
        }
        return loadedGlobalSaveData;
    }

    public void Save(GlobalSaveData globalSaveData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            //Create the directory the file will be written to if it hasn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //Serialize the C# Global Save Data into Json
            string dataToStore = JsonConvert.SerializeObject(globalSaveData, Formatting.Indented);

            //Write the serialized data to the file
            File.WriteAllText(fullPath, dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}
