using System;
using System.IO;
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
                string dataToLoad = "";
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        dataToLoad = streamReader.ReadToEnd();
                    }
                }

                //Deserialize the data from Json back into the C# Global Save Data
                loadedGlobalSaveData = JsonUtility.FromJson<GlobalSaveData>(dataToLoad);
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
            string dataToStore = JsonUtility.ToJson(globalSaveData, true);

            //Write the serialized data to the file
            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}
