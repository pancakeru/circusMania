using UnityEngine;
using TMPro;

public class ScoreTextEffect : MonoBehaviour
{
    public AnimationCurve moveCurve;  // 文字上浮的曲线
    public AnimationCurve fadeCurve;  // 透明度渐变曲线
    public float effectDuration = 1f; // 文字显示时间

    private TextMeshPro textMesh;
    private SpriteRenderer sprite;
    private Vector3 moveStart;
    private Vector3 moveEnd;
    private Color originalColor;
    private float elapsedTime = 0f;
    private bool ifStartMoving = false;

    void Start()
    {
        if (TryGetComponent(out TextMeshPro textMeshPro))
        {
            textMesh = textMeshPro;
        }
        else if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            sprite = spriteRenderer;
        }

        if (textMesh != null)
        {
            originalColor = textMesh.color;
        }
        else if (sprite != null)
        {
            originalColor = sprite.color;
        }
    }

    void Update()
    {
        if (ifStartMoving)
        {
            if (elapsedTime < effectDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / effectDuration); // 归一化时间

                // 文字移动（根据曲线 Lerp 计算）
                float moveT = moveCurve.Evaluate(t);
                transform.position = Vector3.Lerp(moveStart, moveEnd, moveT);

                // 透明度渐变
                float alpha = fadeCurve.Evaluate(t);
                if (textMesh != null)
                {
                    textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                }
                else if (sprite != null)
                {
                    sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                }
            }
            else
            {
                Destroy(gameObject); // 效果结束后销毁对象
            }
        }
    }

    /// <summary>
    /// 初始化并开始移动文字
    /// </summary>
    public void MoveText(Vector3 start, Vector3 end)
    {
        moveStart = start;
        moveEnd = end;
        transform.position = start;
        ifStartMoving = true;
    }
}