using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowScoreManager : MonoBehaviour
{
	private enum ScoreType { Red, Yellow, Blue }

	private ShowManager showManager;

	private float targetRedScore;
	private float targetYellowScore;
	private float targetBlueScore;
	private float defaultReputation = 50f;
	public float currentReputation { get; private set; }
	private float reputationConversionRate = 0.2f;
	public Queue repuChanges;

	private GlobalLevel currentGlobalLevel;
	private LevelProperty currentLevelProperty;
	public List<ScoreContainer> containers;

	private void Awake()
	{
		ResetReputation();
		containers = new List<ScoreContainer>();
	}

	private void Start()
	{
		showManager = FindFirstObjectByType<ShowManager>();
	}

	public void StartTurn(LevelProperty curProperty)
	{
		//currentGlobalLevel = GlobalManager.instance.GetCurrentGlobalLevel();
		currentLevelProperty = curProperty;
		int[] nArray = GenerateNArray(curProperty.totalN);
		targetRedScore = CalculateTargetScore(ScoreType.Red, nArray[0]);
		targetYellowScore = CalculateTargetScore(ScoreType.Yellow, nArray[1]);
		targetBlueScore = CalculateTargetScore(ScoreType.Blue, nArray[2]);
	}

	public float[] GetTargetScore()
	{
		return new float[] { targetRedScore, targetYellowScore, targetBlueScore };
	}

	public void EndTurn()
	{
		float lastRepu = currentReputation;
        float[] sArray = showManager.totalPerformanceControl.GetCurrentScoreArray();
        ScoreContainer newContainer = new ScoreContainer(targetRedScore, targetYellowScore, targetBlueScore, sArray[0], sArray[1], sArray[2]);
		containers.Add(newContainer);
		float[] respectiveEndTurnReputationChangeArray = CalculateRespectiveEndTurnReputationChange();
		for (int i = 0; i < 3; i++)
		{
			currentReputation += respectiveEndTurnReputationChangeArray[i];
		}
		repuChanges.Enqueue((int)(currentReputation - lastRepu));
	}

	public float[] CalculateRespectiveEndTurnReputationChange()
	{
		float[] endScreenScoreArray = showManager.totalPerformanceControl.GetCurrentScoreArray();
		float redChange = 0f;
		float yellowChange = 0f;
		float blueChange = 0f;
		for (int i = 0; i < endScreenScoreArray.Length; i++)
		{
			switch (i)
			{
				case 0:
					if (endScreenScoreArray[i] > targetRedScore)
					{
						redChange = (endScreenScoreArray[i] - targetRedScore) * reputationConversionRate;
					}
					else
					{
						redChange = -(targetRedScore - endScreenScoreArray[i]);
					}
					break;
				case 1:
					if (endScreenScoreArray[i] > targetYellowScore)
					{
						yellowChange = (endScreenScoreArray[i] - targetYellowScore) * reputationConversionRate;
					}
					else
					{
						yellowChange = -(targetYellowScore - endScreenScoreArray[i]);
					}
					break;
				case 2:
					if (endScreenScoreArray[i] > targetBlueScore)
					{
						blueChange = (endScreenScoreArray[i] - targetBlueScore) * reputationConversionRate;
					}
					else
					{
						blueChange = -(targetBlueScore - endScreenScoreArray[i]);
					}
					break;
			}
		}
		float totalRepuationChange = currentReputation + redChange + yellowChange + blueChange;
		return new float[] { redChange, yellowChange, blueChange, totalRepuationChange };
	}

	public void ResetReputation()
	{
		currentReputation = defaultReputation;
		repuChanges = new Queue();
        containers = new List<ScoreContainer>();
    }

	public void SetReputation(float newReputation)
	{
		currentReputation = newReputation;
		repuChanges = new Queue();
        containers = new List<ScoreContainer>();
    }

	private float CalculateTargetScore(ScoreType scoreType, int n)
	{
		switch (scoreType)
		{
			case ScoreType.Red:
				return currentLevelProperty.redA * n + currentLevelProperty.redB;
			case ScoreType.Yellow:
				return currentLevelProperty.yellowA * n + currentLevelProperty.yellowB;
			case ScoreType.Blue:
				return currentLevelProperty.blueA * n + currentLevelProperty.blueB;
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
		if ((totalN - a - maxPerPart) > 1)
		{
			if ((totalN - a) < maxPerPart)
			{
				b = UnityEngine.Random.Range(totalN - a - maxPerPart, totalN - a);
			}
			else
			{
				b = UnityEngine.Random.Range(totalN - a - maxPerPart, maxPerPart);
			}
		}
		else
		{
			if ((totalN - a) < maxPerPart)
			{
				b = UnityEngine.Random.Range(1, totalN - a);
			}
			else
			{
				b = UnityEngine.Random.Range(1, maxPerPart);
			}
		}
		c = totalN - a - b;

		return new[] { a, b, c };
	}
}
