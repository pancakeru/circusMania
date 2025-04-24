using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalloonController : MonoBehaviour
{
    Image image;
    Animator animator;

    [SerializeField] float floatSpeed;
    [SerializeField] float floatHeight;
    [SerializeField] float driftSpeed;
    [SerializeField] float driftRange;

    RectTransform rectTransform;
    Vector2 startPos;
    float timeOffset;

    void Start()
    {
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();

        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
        timeOffset = Random.Range(0f, 100f);

        Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Sin((Time.time + timeOffset) * floatSpeed) * floatHeight;
        float x = Mathf.Sin((Time.time + timeOffset) * driftSpeed) * driftRange;

        rectTransform.anchoredPosition = startPos + new Vector2(x, y);
    }

    public void BallonPop()
    {
        image.raycastTarget = false;
        animator.Play("anim_balloonPop", -1, 0f);
    }

    public void Enable()
    {
        image.enabled = true;
        image.raycastTarget = true;
        animator.Play("anim_balloonIdle", -1, 0f);
    }

    public void Disable()
    {
        image.enabled = false;
    }
}
