using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public TMP_InputField inputField;

    Dictionary<string, Action> cheatCodes;

    void Start()
    {
        cheatCodes = new Dictionary<string, Action>
        {
            { "infiniteMoney", InfiniteMoney },
            { "unlockAll", UnlockAll }
        };

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
    }
}
