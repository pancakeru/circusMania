using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dragBack : MonoBehaviour
{
    private GameObject showManager;
    private ShowManager showScript;
    public GameObject iconPrefab;
    private SpriteRenderer mySprite;

    void Start()
    {
        showManager = GameObject.FindWithTag("showManager");
        //showScript = showManager.GetComponent<ShowManager>();
        mySprite = this.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        
    }

    [Header("AnimationWhenSet")]
    public AnimationCurve scaleCurve; // 缩放动画曲线
    public AnimationCurve colorCurve; // 颜色变化曲线
    public float animationTime = 0.5f; // 动画持续时间
    public SpriteRenderer spriteRenderer; // 物体的 Sprite 渲染器

    private Coroutine animationCoroutine; // 记录动画协程

    public void SetToStagePos(Vector3 pos)
    {
        // 如果有正在运行的动画，先停止
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }

        // 直接设置物体位置
        transform.position = pos;

        // 开启新的动画协程
        animationCoroutine = StartCoroutine(PlayAnimation());
    }

    public void StopSet()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
    }

    private IEnumerator PlayAnimation()
    {
        float elapsedTime = 0f;
        Vector3 startScale = new Vector3(0.2f, 0.2f, 1f); // 初始缩放
        Vector3 targetScale = new Vector3(1f, 1f, 1f); // 目标缩放
        Color startColor = spriteRenderer.color; // 记录初始颜色
        Color blackColor = Color.black; // 黑色
        Color whiteColor = Color.white; // 白色

        while (elapsedTime < animationTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animationTime; // 归一化时间 0~1
            float curveValue = scaleCurve.Evaluate(t); // 取缩放曲线值
            float colorValue = colorCurve.Evaluate(t); // 取颜色曲线值

            // XY 轴缩放从 0.2 → 1
            transform.localScale = Vector3.Lerp(startScale, targetScale, curveValue);
            spriteRenderer.color = Color.Lerp(Color.black, whiteColor, colorValue);

            yield return null;
        }

        // 确保最终值正确
        transform.localScale = targetScale;
        spriteRenderer.color = whiteColor;
        animationCoroutine = null;
    }
}
