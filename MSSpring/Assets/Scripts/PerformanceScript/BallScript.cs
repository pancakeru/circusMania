using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{

	// 现在没有用
	public List<Transform> points;
	// y 轴方向的动画曲线（决定球的运动轨迹）
	public AnimationCurve yCurve;
	// 基础高度
	public float baseY;
	// 标志球是否正在移动
	private bool isMoving = false;
	// 目标索引
	private int toIndex;
	private int fromIndex;
	// 判断球是否向右移动
	private bool ifRight = true;

	public float maxAllowHeight = 10;

	[Header("开头掉落")]
	public AnimationCurve dropCurve; // 下降动画曲线
	public float dropT; // 下降持续时间
	public float height; // 初始掉落高度

	[Header("测试用")]
	public int testStartIndex;
	public int testEndIndex;
	public float testT;
	public float testh;
	public bool testBtn;

	public SpriteRenderer spriteRenderer; // 目标 SpriteRenderer
	public float fadeDuration = 1.0f; // 渐变持续时间

	public float initialYVperUnit = 3f; // 每单位的初始y方向速度
	public float baseYV = 2f; // 基础y方向速度
	float gravity = 9.8f; // 重力加速度

	private PerformUnit showUnit;

	private Coroutine curPara; // 当前抛物线运动协程
	private bool ifDropped = false; // 标记球是否已经掉落

	private PerformAnimalControl passer = null;//传球的动物
	private PerformAnimalControl catcher;//接球的动物

	void Update()
	{
		/*
        if (testBtn)
        {
            if (animalManager.Instance != null)
                animalManager.Instance.ballToIndex(this, 0);
            testBtn = false;
        }*/
	}

	// 处理香蕉投掷逻辑
	public void takeBanana()
	{
		if (ifDropped) return;
		toIndex += (ifRight ? 1 : -1);

		if (toIndex < 0 || toIndex >= points.Count) {
			//这里是投出场外
			isMoving = true;
			if (curPara != null) StopCoroutine(curPara);
			curPara = StartCoroutine(MoveInParabola(transform.position, showUnit.GetPositionWhenThrowToEmpty(toIndex), baseYV, gravity, null, showUnit));

            //curPara = StartCoroutine(MoveInParabola(transform.position, showUnit.ReturnDropOutPos(!(toIndex < 0)), baseYV, gravity, null, showUnit));
			return;
		}

		isMoving = true;
		Vector3 pos2;
		PerformAnimalControl an;
		if (showUnit.CheckAndGetAnimalThrowAcceptPos(toIndex, false, out pos2) && showUnit.CheckAndGetAnimalBasedOnIndex(toIndex, out an)) {
			if (curPara != null) StopCoroutine(curPara);
			curPara = StartCoroutine(MoveInParabola(transform.position, pos2,Mathf.Min(maxAllowHeight, baseYV + initialYVperUnit * Mathf.Abs(pos2.x - transform.position.x) * 0.6f), gravity, an, showUnit));
		} else {
			if (curPara != null) StopCoroutine(curPara);
			curPara = StartCoroutine(MoveInParabola(transform.position, showUnit.GetPositionWhenThrowToEmpty(toIndex), baseYV, gravity, null, showUnit));
		}
	}

	// 启动抛物线运动
	public IEnumerator MoveInParabola(Vector3 start, Vector3 end, float initialYVelocity, float gravity, PerformAnimalControl toCatch, PerformUnit callBack = null)
	{
		float totalTime = CalculateParabolaTime(start.y, end.y, initialYVelocity, gravity);
		if (totalTime <= 0) {
			Debug.LogError("无效的抛物线时间");
			yield break;
		}

		Vector3 horizontalVelocity = new Vector3(
			(end.x - start.x) / totalTime,
			0,
			(end.z - start.z) / totalTime
		);

		float elapsedTime = 0f;
		Vector3 currentPosition = start;

		while (elapsedTime < totalTime) {
			float currentYVelocity = initialYVelocity - gravity * elapsedTime;
			float currentY = start.y + initialYVelocity * elapsedTime - 0.5f * gravity * elapsedTime * elapsedTime;
			Vector3 currentHorizontal = start + horizontalVelocity * elapsedTime;
			currentPosition = new Vector3(currentHorizontal.x, currentY, currentHorizontal.z);
			transform.position = currentPosition;
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.position = end;
		if (toCatch != null) {
			toCatch.TakeBall(this);
			Debug.Log("球成功传递");
		} else {
			DoDrop();
		}
		isMoving = false;
		if (callBack != null) callBack.ReportMoveFinish(this);
	}

	// 计算抛物线运动的总时间
	private float CalculateParabolaTime(float startY, float endY, float initialYVelocity, float gravity)
	{
		float a = -0.5f * gravity;
		float b = initialYVelocity;
		float c = startY - endY;

		float discriminant = b * b - 4 * a * c;
		if (discriminant < 0) {
			Debug.LogError("无有效解的抛物线时间计算");
			return -1f;
		}

		float t1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
		float t2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);

		return Mathf.Max(t1, t2);
	}

	// 球逐渐淡出并销毁
	private IEnumerator FadeOut()
	{
		Color originalColor = spriteRenderer.color;
		float elapsedTime = 0f;

		while (elapsedTime < fadeDuration) {
			elapsedTime += Time.deltaTime;
			float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / fadeDuration);
			spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
			yield return null;
		}

		spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
		Destroy(gameObject);
	}

	public void DoDrop()
	{
		Debug.Log("球掉落");
		/*
		GameObject audioObj = GameObject.FindWithTag("audio manager");
		audioObj.GetComponent<AudioManagerScript>().PlayBattleSound(audioObj.GetComponent<AudioManagerScript>().Battle[2]);
		*/
		ifDropped = true;
		showUnit.ReportDrop(this);
		StartCoroutine(FadeOut());
	}

	public void DoInitialDrop(Vector3 toPos, PerformAnimalControl toAnimal, PerformUnit curUnit)
	{
		passer = null;
		// 计算初始位置：目标位置上方 height 的位置
		Vector3 startPosition = new Vector3(toPos.x, toPos.y + height, toPos.z);

		// 将对象设置到初始位置，并激活
		transform.position = startPosition;
		gameObject.SetActive(true);
		showUnit = curUnit;

		// 启动协程进行移动
		StartCoroutine(MoveToPosition(toPos, dropCurve, dropT, toAnimal, showUnit));
	}

	public IEnumerator MoveToPosition(Vector3 toPos, AnimationCurve curve, float duration, PerformAnimalControl toCatch = null, PerformUnit machine = null)
	{
		float elapsedTime = 0f; // 已经过的时间
		Vector3 startPosition = transform.position; // 初始位置

		while (elapsedTime < duration) {
			// 计算当前进度（归一化时间 [0, 1]）
			float normalizedTime = elapsedTime / duration;

			// 根据曲线计算当前位置的 Y 值
			float curveValue = curve.Evaluate(normalizedTime);
			float currentY = Mathf.Lerp(startPosition.y, toPos.y, curveValue);

			// 更新位置（保持 X 和 Z 不变）
			transform.position = new Vector3(toPos.x, currentY, toPos.z);

			// 增加经过的时间
			elapsedTime += Time.deltaTime;

			// 等待下一帧
			yield return null;
		}

		// 确保最后的位置完全对齐
		transform.position = toPos;
		if (toCatch != null)
		{
			toCatch.TakeBall(this);
			catcher = toCatch;
		}
		else
		{
			DoDrop();
		}
		if (machine != null)
			machine.ReportMoveFinish(this);
	}

	public void MoveBall(int from, int to, PerformAnimalControl _passer)
	{
		Debug.Log("从" + from + "到" + to);
		fromIndex = from;
		passer = _passer;
		// Validate indices
		if (points == null || points.Count == 0) {
			Debug.LogError("Points list is empty or null.");
			return;
		}

		if (from < 0 || from >= points.Count) {
			Debug.LogError($"Start index {from} is out of range.");
			return;
		}

		if (to < 0 || to >= points.Count) {
			Debug.Log("这里是投出场地");
			//Debug.LogError($"End index {endIndex} is out of range.");
			isMoving = true; // Set the flag to indicate movement has started
			if (curPara != null)
				StopCoroutine(curPara);
			curPara = StartCoroutine(MoveInParabola(transform.position, showUnit.GetPositionWhenThrowToEmpty(to), Mathf.Min(baseYV+ initialYVperUnit * (Mathf.Abs((to<0?-1:6) - from)),maxAllowHeight), gravity, null, showUnit));
			ifRight = !(to<0);
			return;
		}

		if (isMoving) {
			Debug.LogWarning("Already moving. Wait until current movement is finished.");
			return;
		}

		isMoving = true; // Set the flag to indicate movement has started
		toIndex = to;
		if (to - from > 0)
			ifRight = true;
		else
			ifRight = false;

		if (catcher != null) {
			passer = catcher;
			catcher = null;
		}//The catcher of the last turn becomes the passer of this turn

		//StartCoroutine(MoveCoroutine(points[startIndex], points[endIndex], t, h));
		Vector3 pos1;
		Vector3 pos2;
		PerformAnimalControl an;
		if (showUnit.CheckAndGetAnimalThrowAcceptPos(from, true, out pos1) && showUnit.CheckAndGetAnimalThrowAcceptPos(to, false, out pos2) && showUnit.CheckAndGetAnimalBasedOnIndex(to, out an)) {
			Debug.Log("这里是有人接");
			if (curPara != null)
				StopCoroutine(curPara);
			curPara = StartCoroutine(MoveInParabola(pos1, pos2, Mathf.Min(baseYV + initialYVperUnit * (Mathf.Abs(to - from)),maxAllowHeight), gravity, an, showUnit));
		} else {
			Debug.Log("这里是没有人接,目前有问题");
			if (curPara != null)
				StopCoroutine(curPara);
			curPara = StartCoroutine(MoveInParabola(transform.position, showUnit.GetPositionWhenThrowToEmpty(to), Mathf.Min(baseYV + initialYVperUnit * (Mathf.Abs(to - from)), maxAllowHeight), gravity, null, showUnit));
		}
	}

	public PerformAnimalControl GetPasser()
	{
		return passer;
	}
}
