using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraMover : MonoBehaviour
{
    [Header("Move Module")]
    public float moveDuration = 1.0f; // 移动时间
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 运动曲线

    public List<UnityEvent> onMoveReachs; // 可在 Inspector 中设置的 UnityEvent
    public int triggerIndex = 0; // 触发事件的索引

    private bool isMoving = false; // 是否在移动
    private Vector3 startPosition; // 起始位置
    private Vector3 targetPosition; // 目标位置
    private float elapsedTime = 0; // 计时器

    private Camera mainCamera; // 摄像机引用

    private float startScale; // 初始摄像机缩放
    private float targetScale; // 目标摄像机缩放
    private bool isScaling = false; // 是否需要缩放

    void Awake()
    {
        mainCamera = Camera.main; // 获取主摄像机
    }

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    /// <summary>
    /// 移动摄像机到指定位置，可选择是否调整摄像机缩放
    /// </summary>
    /// <param name="destination">目标位置 (Vector3)</param>
    /// <param name="newScale">目标缩放 (float?)，可选参数，不填则不缩放</param>
    /// <param name="i">触发索引 (int)</param>
    public void MoveTo(Vector3 destination, float? newScale = null, int i = 0)
    {
        if (mainCamera == null)
        {
            Debug.LogError("未找到 Camera 组件！");
            return;
        }

        startPosition = mainCamera.transform.position;
        targetPosition = destination;
        elapsedTime = 0;
        isMoving = true;
        triggerIndex = i;

        // 处理缩放
        if (newScale.HasValue)
        {
            startScale = mainCamera.orthographicSize;
            targetScale = newScale.Value;
            isScaling = true;
        }
        else
        {
            isScaling = false;
        }
    }

    /// <summary>
    /// 移动摄像机到指定位置，并使用新的移动曲线、时间，可选择是否调整缩放
    /// </summary>
    /// <param name="destination">目标位置 (Vector3)</param>
    /// <param name="newCurve">新的动画曲线 (AnimationCurve)</param>
    /// <param name="newT">新的移动时间 (float)</param>
    /// <param name="newScale">目标缩放 (float?)，可选参数，不填则不缩放</param>
    /// <param name="i">触发索引 (int)</param>
    public void MoveTo(Vector3 destination, AnimationCurve newCurve, float newT, float? newScale = null, int i = 0)
    {
        if (mainCamera == null)
        {
            Debug.LogError("未找到 Camera 组件！");
            return;
        }

        startPosition = mainCamera.transform.position;
        targetPosition = destination;
        elapsedTime = 0;
        isMoving = true;
        moveCurve = newCurve;
        moveDuration = newT;
        triggerIndex = i;

        // 处理缩放
        if (newScale.HasValue)
        {
            startScale = mainCamera.orthographicSize;
            targetScale = newScale.Value;
            isScaling = true;
        }
        else
        {
            isScaling = false;
        }
    }

    /// <summary>
    /// 平滑移动到目标点，并根据需要调整缩放
    /// </summary>
    private void MoveTowardsTarget()
    {
        if (!isMoving) return;

        // 更新时间
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / moveDuration); // 归一化时间 [0, 1]

        // 计算曲线插值
        float curveValue = moveCurve.Evaluate(t);
        mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);

        // 处理缩放
        if (isScaling)
        {
            mainCamera.orthographicSize = Mathf.Lerp(startScale, targetScale, curveValue);
        }

        // 结束检测
        if (t >= 1.0f)
        {
            mainCamera.transform.position = targetPosition; // 确保到达目标位置
            if (isScaling)
            {
                mainCamera.orthographicSize = targetScale; // 确保缩放到最终值
            }
            isMoving = false; // 停止移动
            isScaling = false; // 停止缩放
            if (onMoveReachs != null && (triggerIndex >= 0) && (triggerIndex < onMoveReachs.Count))
                onMoveReachs[triggerIndex]?.Invoke();
        }
    }

    public void SetTo(Vector3 destination, float newScale )
    {
        mainCamera.transform.position = destination;
        mainCamera.orthographicSize = newScale;
    }
}
