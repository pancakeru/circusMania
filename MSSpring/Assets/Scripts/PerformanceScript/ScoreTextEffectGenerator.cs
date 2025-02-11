using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextEffectGenerator : MonoBehaviour
{
    public enum ScoreType { Red, Yellow, Blue }

    public GameObject textPrefab; // 文字预制体（需要绑定一个 TMP 组件）
    public Vector3 moveOffset;
    public float spawnCooldown = 0.2f; // 生成冷却时间

    private float lastSpawnTime = 0f;
    private Queue<ScoreRequest> scoreQueue = new Queue<ScoreRequest>(); // 伤害数字生成队列

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RequestTextEffect(20, ScoreType.Red);
        }

        ProcessQueue(); // 逐步处理队列
    }

    /// <summary>
    /// 处理伤害文字队列，每次冷却结束后生成一个
    /// </summary>
    private void ProcessQueue()
    {
        if (scoreQueue.Count > 0 && Time.time >= lastSpawnTime + spawnCooldown)
        {
            ScoreRequest request = scoreQueue.Dequeue(); // 取出队列中的请求
            GenerateTextEffect(request.score, request.type);
            lastSpawnTime = Time.time; // 记录上次生成时间
        }
    }

    /// <summary>
    /// 请求生成伤害数字，将其加入队列
    /// </summary>
    public void RequestTextEffect(float score, ScoreType type)
    {
        scoreQueue.Enqueue(new ScoreRequest(score, type));
    }

    /// <summary>
    /// 立即生成伤害数字
    /// </summary>
    private void GenerateTextEffect(float score, ScoreType type)
    {
        GameObject textObj = Instantiate(textPrefab, transform.position, Quaternion.identity);
        TextMeshPro textMesh = textObj.GetComponent<TextMeshPro>();

        // 设置文本内容
        textMesh.text = score.ToString();

        // 设置不同颜色
        switch (type)
        {
            case ScoreType.Yellow:
                textMesh.color = Color.yellow;
                break;
            case ScoreType.Red:
                textMesh.color = Color.red;
                break;
            case ScoreType.Blue:
                textMesh.color = Color.blue;
                break;
            default:
                textMesh.color = Color.white;
                break;
        }

        // 调用文字的移动函数
        textObj.GetComponent<ScoreTextEffect>().MoveText(textObj.transform.position, textObj.transform.position + moveOffset);
    }

    /// <summary>
    /// 伤害数字请求类（用于队列）
    /// </summary>
    private class ScoreRequest
    {
        public float score;
        public ScoreType type;

        public ScoreRequest(float score, ScoreType type)
        {
            this.score = score;
            this.type = type;
        }
    }
}
