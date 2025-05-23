using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using TMPro;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI cheatList;

    Dictionary<string, Action> cheatCodes;

    void Start()
    {
        cheatCodes = new Dictionary<string, Action>
        {
            { "infiniteMoney", InfiniteMoney },
            { "unlock", Unlock },
            { "unlockAll", UnlockAll },
            { "end", ForceEnd },
            { "balls", Unlockballs },
        };

        cheatList.text = "Cheat will not be applied until refreshing UI. \nCheat List: \n";
        foreach (string key in cheatCodes.Keys)
        {
            cheatList.text += "\n" + key;
        }

        inputField.onEndEdit.AddListener(CheckCheat);
    }

    void CheckCheat(string text)
    {
        if (cheatCodes.TryGetValue(text, out Action cheatAction))
        {
            cheatAction.Invoke();
        }
        inputField.onValueChanged.RemoveListener(CheckCheat);
        gameObject.SetActive(false);
    }

    void InfiniteMoney()
    {
        GlobalManager.instance.setCoinAmount(999999999);
    }

    void UnlockAll()
    {
        GlobalManager.instance.currentLevelIndex = 7;
        GlobalManager.instance.UnlockAnimal();
        GlobalManager.instance.SetSaveDataCurrentLevelIndex();
    }

    void Unlock()
    {
        GlobalManager.instance.currentLevelIndex++;
        GlobalManager.instance.UnlockAnimal();
        GlobalManager.instance.SetSaveDataCurrentLevelIndex();
    }

    void ForceEnd()
    {
        FindObjectOfType<summaryScript>().SummaryLevel();
        FindObjectOfType<winLoseScript>().SetBasedOnLevelIndex(FindObjectOfType<winLoseScript>().testN);
    }

    void Unlockballs()
    {
        for (int i = 0; i < MrShopManager.instance.ballInfos.Count; i++)
        {
            MrShopManager.instance.AchievementUnlocked(i);
        }
    }
}
