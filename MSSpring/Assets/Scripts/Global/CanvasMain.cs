using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    [SerializeField] GameObject popUp;

    [Header("CHEAT")]
    [SerializeField] bool isCheatEnabled;
    [SerializeField] GameObject cheatPrefab;
    GameObject myCheat;

    GameObject myPopUp;
    RectTransform myPopUpRect;
    Dictionary<Image, string> popUpTargets = new Dictionary<Image, string>();

    public bool isStartScreenCanvasEnabled;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        myPopUp = Instantiate(popUp, transform);
        myPopUp.SetActive(false);
        myPopUpRect = myPopUp.GetComponent<RectTransform>();

        if (isCheatEnabled) myCheat = Instantiate(cheatPrefab, transform);
        myCheat.SetActive(false);
    }

    void Start()
    {
        //OnUIInteractionEnabled += ShowManager.instance.EnableCanvas;
        //OnUIInteractionDisabled += ShowManager.instance.DisableCanvas;

        OnUIInteractionEnabled += UnlockMouse;
        OnUIInteractionDisabled += LockMouse;

        //CheckFonts();
    }

    void Update()
    {
        bool hovering = false;
        foreach (var pair in popUpTargets)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(pair.Key.rectTransform, Input.mousePosition) && !isStartScreenCanvasEnabled)
            {
                myPopUp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = pair.Value;

                myPopUp.SetActive(true);

                RectTransformUtility.ScreenPointToLocalPointInRectangle(myPopUpRect.parent as RectTransform, Input.mousePosition, null, out Vector2 pos);
                myPopUpRect.anchoredPosition = pos;
                hovering = true;
                break;
            }
        }

        if (!hovering)
        {
            myPopUp.SetActive(false);
        }

        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            if (myCheat.activeSelf) myCheat.SetActive(false);
            else myCheat.SetActive(true);
        }
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

    public void ShowPopUp(Image image, string text)
    {
        if (popUpTargets.ContainsKey(image)) popUpTargets[image] = text;
        else popUpTargets.Add(image, text);
    }

    void CheckFonts()
    {
        Dictionary<TMP_FontAsset, int> fontUsage = new Dictionary<TMP_FontAsset, int>();
        TMP_FontAsset defaultFont = TMP_Settings.defaultFontAsset;
        int fixedCount = 0;

        foreach (var tmp in FindObjectsOfType<TextMeshProUGUI>(true))
        {
            if (tmp.font == null)
            {
                if (defaultFont != null)
                {
                    tmp.font = defaultFont;
                    Debug.LogWarning($"[AutoFix] Assigned default font to: {tmp.gameObject.name}", tmp.gameObject);
                    fixedCount++;
                }
                continue;
            }

            if (!fontUsage.ContainsKey(tmp.font))
                fontUsage[tmp.font] = 0;

            fontUsage[tmp.font]++;
        }

        foreach (var pair in fontUsage)
        {
            Debug.Log($"Font: {pair.Key.name} - Used {pair.Value} times");
        }

        if (fixedCount > 0)
            Debug.Log($"Fixed {fixedCount} TMP texts with missing fonts.");
    }
}
