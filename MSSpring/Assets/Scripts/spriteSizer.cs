using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteSizer : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite lastSprite; // 记录上一次的Sprite
    public float fixedWidth = 1f; // 设定的固定宽度（单位：世界单位）

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("未找到 SpriteRenderer 组件！");
        }

        lastSprite = spriteRenderer.sprite; // 初始化上一次的Sprite
        AdjustScale(); // 初始调整一次
    }

    void Update()
    {
        /*
        if (spriteRenderer.sprite != lastSprite) // 检测Sprite变化
        {
            lastSprite = spriteRenderer.sprite; // 更新记录
            AdjustScale(); // 重新调整Scale
        }*/
    }

    /// <summary>
    /// 调整物体的缩放，使 Sprite 宽度匹配 fixedWidth
    /// </summary>
    void AdjustScale()
    {
        if (spriteRenderer.sprite == null) return;

        // 获取Sprite的原始像素尺寸
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        if (spriteWidth > 0)
        {
            float newScaleX = fixedWidth / spriteWidth; // 计算X缩放
            
            
            transform.localScale = new Vector3(newScaleX, newScaleX, 1f);
            Debug.Log(transform.localScale);
        }
    }
}
