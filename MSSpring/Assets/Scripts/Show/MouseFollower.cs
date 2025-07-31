using UnityEngine;
using System.Collections;

public class MouseFollower : MonoBehaviour
{
    public RectTransform cursorImage; 
    private Vector3 defaultScale;
    private Quaternion defaultRotation;
    private bool isAnimating = false;

    ShowManager myShowManager;
    CanvasMain canvasMain;

    void Start()
    {
        Cursor.visible = false;
        defaultScale = cursorImage.localScale;
        defaultRotation = cursorImage.localRotation;

        myShowManager = transform.parent.transform.parent.gameObject.GetComponent<ShowManager>();
        canvasMain = FindFirstObjectByType<CanvasMain>();
        transform.parent = canvasMain.transform;
    }

    void Update()
    {
        cursorImage.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0) && !isAnimating)
        {
            StartCoroutine(ClickEffect());
        }

        if (myShowManager == null) Destroy(gameObject);
    }

    IEnumerator ClickEffect()
    {
        isAnimating = true;

        float duration = 0.05f;
        float t = 0f;

        Vector3 bigScale = defaultScale * 1.4f;
        Quaternion tilt = Quaternion.Euler(0, 0, 15f);

        // Scale up & tilt
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;
            cursorImage.localScale = Vector3.Lerp(defaultScale, bigScale, progress);
            cursorImage.localRotation = Quaternion.Lerp(defaultRotation, tilt, progress);
            yield return null;
        }

        t = 0f;
        // Return to normal
        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;
            cursorImage.localScale = Vector3.Lerp(bigScale, defaultScale, progress);
            cursorImage.localRotation = Quaternion.Lerp(tilt, defaultRotation, progress);
            yield return null;
        }

        isAnimating = false;
    }
}
