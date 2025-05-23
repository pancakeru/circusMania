using System.Collections.Generic;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    public static SaveDataManager Instance { get; private set; }

    private GlobalSaveData globalSaveData;
    private FileDataHandler fileDataHandler;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one SaveDataManager!" + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        //Debug.Log(Application.persistentDataPath);
    }

    public GlobalSaveData NewGame()
    {
        globalSaveData = new GlobalSaveData(new List<List<animalProperty>>(), 0, new Dictionary<string, int>(), new AnimalBallPassTimes(), 0, 0, 0, new List<animalProperty>(), new Dictionary<string, bool>(), new Dictionary<string, int>(), new Dictionary<string, int>(), new Dictionary<string, List<int>>(), new Dictionary<string, int>(), new List<BallInfo>(), 0);
        return globalSaveData;
    }

    public void SaveGame(GlobalSaveData globalSaveData)
    {
        this.globalSaveData = globalSaveData;

        //Save the data to a file using the data handler
        fileDataHandler.Save(this.globalSaveData);
    }

    public GlobalSaveData LoadGame()
    {
        //Load any saved data from a file using the data handler
        globalSaveData = fileDataHandler.Load();

        //If no data can be loaded, initialize to a new game
        if (globalSaveData == null)
        {
            Debug.Log("No save data found. Start a new game.");
            NewGame();
        }
        return globalSaveData;
    }

    public bool HasSaveDataExisted()
    {
        GlobalSaveData saveTest = fileDataHandler.Load();
        if (saveTest == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void DeleteSaveData()
    {
        fileDataHandler.DeleteSaveData();
    }
}
