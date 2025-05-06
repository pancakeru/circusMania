using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenController : MonoBehaviour
{
    Animator animator;
    int clickTimes = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        clickTimes++;

        if (clickTimes == 7)
        {
            MrShopManager.instance.AchievementUnlocked(1);
        }

        animator.speed = 0.5f;
        animator.Play("anim_chicken", 0, 0f);
    }

    public void StopAnimation()
    {
        animator.speed = 0;
    }
}
