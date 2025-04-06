using System;
using UnityEngine;
using System.Collections;

public class ShowScoreManager : MonoBehaviour
{
	private enum ScoreType { Red, Yellow, Blue }

	private ShowManager showManager;

	private float targetRedScore;
	private float targetYellowScore;
	private float targetBlueScore;
	private float defaultReputation = 50f;
	public float currentReputation { get; private set; }
	private float reputationConversionRate = 0.5f;
	public Queue repuChanges;

	private GlobalLevel currentGlobalLevel;

	private void Awake()
	{
		ResetReputation();
	}

	private void Start()
	{
		showManager = FindFirstObjectByType<ShowManager>();
	}

	public void StartTurn()
	{
		currentGlobalLevel = GlobalManager.instance.GetCurrentGlobalLevel();

		int[] nArray = GenerateNArray(currentGlobalLevel.levelProperty.totalN);
		targetRedScore = CalculateTargetScore(ScoreType.Red, nArray[0]);
		targetYellowScore = CalculateTargetScore(ScoreType.Yellow, nArray[1]);
		targetBlueScore = CalculateTargetScore(ScoreType.Blue, nArray[2]);

		//Debug.Log("Current Reputation: " + currentReputation);
		//Debug.Log("Current Turn Target Score: " + targetRedScore + ", " + targetYellowScore + ", " + targetBlueScore);
	}

	public float[] GetTargetScore()
	{
		return new float[] { targetRedScore, targetYellowScore, targetBlueScore };
	}

	public void EndTurn()
	{
		float lastRepu = currentReputation;
		float[] endScreenScoreArray = showManager.totalPerformanceControl.GetCurrentScoreArray();
		for (int i = 0; i < endScreenScoreArray.Length; i++) {
			switch (i) {
				case 0:
					//Debug.Log("红分是" + endScreenScoreArray[i]);
					//Debug.Log("目标红分时"+ targetRedScore);
					if (endScreenScoreArray[i] > targetRedScore) {
						currentReputation += (endScreenScoreArray[i] - targetRedScore) * reputationConversionRate;
					} else {
						currentReputation -= targetRedScore - endScreenScoreArray[i];
					}
					break;
				case 1:
                    //Debug.Log("黄分是" + endScreenScoreArray[i]);
                    //Debug.Log("目标黄分时" + targetYellowScore);
                    if (endScreenScoreArray[i] > targetYellowScore) {
						currentReputation += (endScreenScoreArray[i] - targetYellowScore) * reputationConversionRate;
					} else {
						currentReputation -= targetYellowScore - endScreenScoreArray[i];
					}
					break;
				case 2:
                    //Debug.Log("蓝分是" + endScreenScoreArray[i]);
                    //Debug.Log("目标蓝分时" + targetBlueScore);
                    if (endScreenScoreArray[i] > targetBlueScore) {
						currentReputation += (endScreenScoreArray[i] - targetBlueScore) * reputationConversionRate;
					} else {
						currentReputation -= targetBlueScore - endScreenScoreArray[i];
					}
					break;
			}
		}
		//Debug.Log("Current Reputation: " + currentReputation);
		repuChanges.Enqueue((int)(currentReputation-lastRepu));
	}

	public void ResetReputation()
	{
		currentReputation = defaultReputation;
		repuChanges = new Queue();
	}

	private float CalculateTargetScore(ScoreType scoreType, int n)
	{
		switch (scoreType) {
			case ScoreType.Red:
				return currentGlobalLevel.levelProperty.redA * n + currentGlobalLevel.levelProperty.redB;
			case ScoreType.Yellow:
				return currentGlobalLevel.levelProperty.yellowA * n + currentGlobalLevel.levelProperty.yellowB;
			case ScoreType.Blue:
				return currentGlobalLevel.levelProperty.blueA * n + currentGlobalLevel.levelProperty.blueB;
			default:
				Debug.LogError(this + ": Unknown Score Type!");
				return -1;
		}
	}

	private int[] GenerateNArray(int totalN)
	{
		if (totalN < 3)
			throw new ArgumentException("Total must be at least 3.");

		int maxPerPart = totalN * 3 / 5;//60%
		if (maxPerPart < 1)
			maxPerPart = 1;

		int a, b, c;

		a = UnityEngine.Random.Range(1, maxPerPart);
		if ((totalN - a - maxPerPart) > 1) {
			if ((totalN - a) < maxPerPart) {
				b = UnityEngine.Random.Range(totalN - a - maxPerPart, totalN - a);
			} else {
				b = UnityEngine.Random.Range(totalN - a - maxPerPart, maxPerPart);
			}
		} else {
			if ((totalN - a) < maxPerPart) {
				b = UnityEngine.Random.Range(1, totalN - a);
			} else {
				b = UnityEngine.Random.Range(1, maxPerPart);
			}
		}
		c = totalN - a - b;

		return new[] { a, b, c };
	}
}
