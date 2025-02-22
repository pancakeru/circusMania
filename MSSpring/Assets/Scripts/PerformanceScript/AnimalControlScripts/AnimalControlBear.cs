using System.Collections.Generic;

public class AnimalControlBear : AbstractSpecialAnimal
{
	private AnimalInfoPack animalInfo;

	public override void InitAnimalInfo()
	{
		animalInfo = new AnimalInfoPack(animalBody);
	}

	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange * animalInfo.power);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		animalInfo.redScore *= animalInfo.power;

		List<float[]> scoresAfterBuff = BuffManager.instance.BuffInteractionWhenScore(animalInfo);
		foreach (float[] inputScore in scoresAfterBuff) {
			Scoring(inputScore);
		}

		animalInfo.power += 1;

		animalBody.ifReadyToInteract = false;
	}
}
