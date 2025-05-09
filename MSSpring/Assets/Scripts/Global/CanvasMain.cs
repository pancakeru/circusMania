using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

    List<GraphicRaycaster> raycasters = new List<GraphicRaycaster>();

    [SerializeField] GameObject messageBox;
    [SerializeField] GameObject popUp;
    [SerializeField] GameObject cutScenePrefab;

    [Header("CHEAT")]
    [SerializeField] bool isCheatEnabled;
    [SerializeField] GameObject cheatPrefab;
    GameObject myCheat;

    GraphicRaycaster raycaster;
    GameObject myPopUp;
    RectTransform myPopUpRect;
    Dictionary<Image, string> popUpTargets = new Dictionary<Image, string>();

    Canvas canvasMenu;
    Canvas canvasTroupe;
    Canvas canvasMrShop;
    Canvas canvasStartScren;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        raycaster = GetComponent<GraphicRaycaster>();
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

        canvasMenu = FindObjectOfType<MenuController>().GetComponent<Canvas>();
        canvasTroupe = TroupeController.instance.GetComponent<Canvas>();
        canvasMrShop = MrShopManager.instance.GetComponent<Canvas>();
        canvasStartScren = FindObjectOfType<StartScreenManager>().GetComponent<Canvas>();

        raycasters.Add(canvasMenu.GetComponent<GraphicRaycaster>());
        raycasters.Add(canvasTroupe.GetComponent<GraphicRaycaster>());
        raycasters.Add(canvasMrShop.GetComponent<GraphicRaycaster>());
        raycasters.Add(canvasStartScren.GetComponent<GraphicRaycaster>());

        //CheckFonts();
    }

    void Update()
    {
        bool isHovering = false;
        var pointer = new PointerEventData(EventSystem.current) { position = Input.mousePosition };

        List<RaycastResult> allHits = new List<RaycastResult>();
        foreach (var raycaster in raycasters)
        {
            if (raycaster == null)
            {
                raycasters.Remove(raycaster);
                continue;
            }

            var hits = new List<RaycastResult>();
            raycaster.Raycast(pointer, hits);
            allHits.AddRange(hits);
        }

        // Sort all hits manually by sorting order (higher goes first)
        allHits = allHits
            .OrderByDescending(hit => hit.gameObject.GetComponentInParent<Canvas>()?.sortingOrder ?? 0)
            .ToList();

        foreach (var hit in allHits)
        {
            //Debug.Log("[Tooltip Hover] First UI hit: " + hit.gameObject.name, hit.gameObject);
            // Skip tooltip itself
            if (hit.gameObject == myPopUp || hit.gameObject.transform.IsChildOf(myPopUp.transform))
                continue;

            //Only process images
            if (hit.gameObject.GetComponent<Image>() == null)
                continue;

            var img = hit.gameObject.GetComponent<Image>();
            if (img != null && popUpTargets.TryGetValue(img, out string text))
            {
                myPopUp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
                myPopUp.SetActive(true);

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    myPopUpRect.parent as RectTransform,
                    Input.mousePosition,
                    null,
                    out Vector2 localPos
                );
                myPopUpRect.anchoredPosition = localPos;

                isHovering = true;
                break;
            }
            else
            {
                // Some UI is on top, but it's not a tooltip target
                break;
            }
        }

        if (!isHovering)
            myPopUp.SetActive(false);

        if (Input.GetKeyUp(KeyCode.BackQuote))
            myCheat.SetActive(!myCheat.activeSelf);
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

    public void ShowPopUp(Image image, string text, GraphicRaycaster raycaster)
    {
        if (!raycasters.Contains(raycaster)) raycasters.Add(raycaster);

        if (popUpTargets.ContainsKey(image)) popUpTargets[image] = text;
        else popUpTargets.Add(image, text);
    }

    public void ShowCutScene()
    {
        Instantiate(cutScenePrefab, transform);
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
