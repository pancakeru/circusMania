using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum MessageType
{
    Warning, 
    Selection, 
}

public class CanvasMain : MonoBehaviour
{
    public static CanvasMain instance;

    public static event Action OnUIInteractionEnabled;
    public static event Action OnUIInteractionDisabled;

    [SerializeField] GameObject messageBox;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //OnUIInteractionEnabled += ShowManager.instance.EnableCanvas;
        //OnUIInteractionDisabled += ShowManager.instance.DisableCanvas;

        OnUIInteractionEnabled += UnlockMouse;
        OnUIInteractionDisabled += LockMouse;
    }

    public void DisplayWarning(string text)
    {
        GameObject newMessageBox = Instantiate(messageBox, transform);
        newMessageBox.GetComponent<MessageBoxController>().messageType = MessageType.Warning;
        newMessageBox.GetComponent<MessageBoxController>().uiText.text = text;
    }

    public void DisplaySelection(string text, Action action)
    {
        GameObject newMessageBox = Instantiate(messageBox, transform);
        newMessageBox.GetComponent<MessageBoxController>().messageType = MessageType.Selection;
        newMessageBox.GetComponent<MessageBoxController>().action = action;
        newMessageBox.GetComponent<MessageBoxController>().uiText.text = text;
    }

    public static void EnableUIInteraction()
    {
        OnUIInteractionEnabled?.Invoke();
    }

    public static void DisableUIInteraction()
    {
        OnUIInteractionDisabled?.Invoke();
    }

    void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
