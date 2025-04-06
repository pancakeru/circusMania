using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMain : MonoBehaviour
{
    public static CanvasMain instance;

    public static event Action OnUIInteractionEnabled;
    public static event Action OnUIInteractionDisabled;
    
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        OnUIInteractionEnabled += ShowManager.instance.EnableCanvas;
        OnUIInteractionDisabled += ShowManager.instance.DisableCanvas;

        OnUIInteractionEnabled += UnlockMouse;
        OnUIInteractionDisabled += LockMouse;
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
