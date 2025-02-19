using UnityEngine;
using System.Collections.Generic;

public class AnimalControlBear : AbstractSpecialAnimal
{
	private int power = 1;

	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange * power);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;
        //animalManager.Instance.changeScore(interactionYellowScore, interactionRedScore, interactionBlueScore, selfIndex);
        //TODO:分数展示和改变逻辑

        List<float[]> scoresAfterBuff = BuffManager.instance.BuffInteractionWhenScore(animalBody);
        foreach (float[] inputScore in scoresAfterBuff)
        {
            Scoring(inputScore);
        }
		/*
        if (soul.baseRedChange != 0) {
			controlUnit.ChangeRedScore(soul.baseRedChange * power);
			animalBody.generator.RequestTextEffect(soul.baseRedChange, ScoreTextEffectGenerator.ScoreType.Red);
			power += 1;
		}*/
		animalBody.ifReadyToInteract = false;
	}
}
