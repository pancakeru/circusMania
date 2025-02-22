using System.Collections.Generic;

public class AnimalControlSnake : AbstractSpecialAnimal
{
	private int[] ballToArray = { 0, 2, 4 };
	private int ballToArrayIndex = 0;

	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, ballToArray[ballToArrayIndex % ballToArray.Length]);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		List<float[]> scoresAfterBuff = BuffManager.instance.BuffInteractionWhenScore(new AnimalInfoPack(animalBody));
		foreach (float[] inputScore in scoresAfterBuff) {
			Scoring(inputScore);
		}

		ballToArrayIndex += 1;

		animalBody.ifReadyToInteract = false;
	}
}
