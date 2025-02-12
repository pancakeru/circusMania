using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour, IGeneralManager
{
    public static GlobalManager instance;

    [Header("For test")]
    public List<animalProperty> testProperties;
    public bool ifDoTest;
    public TestAction testAction;
    public bool ifTestAction = false;
    public animalProperty toTestAdd;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (ifDoTest)
        {
            foreach (animalProperty apt in testProperties)
                addAnAnimal(apt);
        }
    }

    private void Update()
    {
        if (ifTestAction)
        {
            ifTestAction = false;
            switch (testAction)
            {
                case TestAction.Add:
                    addAnAnimal(toTestAdd);
                    break;

                case TestAction.LogInfo:
                    string infoMessage = "";
                    foreach (animalProperty apt in animals)
                    {
                        infoMessage += apt.name;
                        infoMessage += ", ";
                    }

                    Debug.Log("现在仓库有"+ infoMessage);
                    break;
            }
            
        }
    }

    // 动物管理
    #region 动物管理
    private List<animalProperty> animals = new List<animalProperty>();

    public List<animalProperty> getAllAnimals()
    {
        return animals;
    }

    public void addAnAnimal(animalProperty newAnimal)
    {
        animals.Add(newAnimal);
    }
    #endregion
    // 金币管理
    #region 金币管理
    private int curCoinAmount = 0;

    public int getCurCoinAmount()
    {
        return curCoinAmount;
    }

    public bool checkIfCanChangeCoin(int n)
    {
        return curCoinAmount + n >= 0;
    }

    public bool changeCoinAmount(int n)
    {
        if (checkIfCanChangeCoin(n))
        {
            curCoinAmount += n;
            return true;
        }
        return false;
    }
    #endregion

    public enum TestAction
    {
        Add,
        LogInfo
    }
}
