using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void WebGLStorage_Save(string key, string data);

    [DllImport("__Internal")]
    private static extern string WebGLStorage_Load(string key);

    [DllImport("__Internal")]
    private static extern void WebGLStorage_Delete(string key);
#endif

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
#if UNITY_WEBGL && !UNITY_EDITOR
            string dataToLoad = WebGLStorage_Load(fullPath);
            if (!string.IsNullOrEmpty(dataToLoad))
            {
                loadedGlobalSaveData = JsonConvert.DeserializeObject<GlobalSaveData>(dataToLoad);
            }
#else
                if (File.Exists(fullPath))
                {
                    //Load the serialized data from the file
                    string dataToLoad = File.ReadAllText(fullPath);

                    //Deserialize the data from Json back into the C# Global Save Data
                    loadedGlobalSaveData = JsonConvert.DeserializeObject<GlobalSaveData>(dataToLoad);
                }
#endif
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
            //Serialize the C# Global Save Data into Json
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            string dataToStore = JsonConvert.SerializeObject(globalSaveData, settings);

#if UNITY_WEBGL && !UNITY_EDITOR
            WebGLStorage_Save(fullPath, dataToStore);
#else
            //Create the directory the file will be written to if it hasn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //Write the serialized data to the file
            File.WriteAllText(fullPath, dataToStore);
#endif
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public void DeleteSaveData()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            WebGLStorage_Delete(fullPath);
#else
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
#endif
        }
        catch (Exception e)
        {
            Debug.LogError("Error deleting data: " + fullPath + "\n" + e);
        }
    }
}
