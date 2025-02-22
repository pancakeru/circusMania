using System.Collections.Generic;

public class AnimalControlLion : AbstractSpecialAnimal
{
	private AnimalInfoPack animalInfo;

	public override void InitAnimalInfo()
	{
		animalInfo = new AnimalInfoPack(animalBody);
	}

	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		List<float[]> scoresAfterBuff = BuffManager.instance.BuffInteractionWhenScore(animalInfo);
		foreach (float[] inputScore in scoresAfterBuff) {
			Scoring(inputScore);
		}

		animalInfo.warmUp += 1;

		animalBody.ifReadyToInteract = false;

		if (animalInfo.warmUp == (soul as WarmUpAnimalProperty).warmUpRequireTime) {
			WarmUp();
		}
	}

	private void WarmUp()
	{
		AnimalInfoPack warmUpAnimalInfo = new AnimalInfoPack(animalBody);
		warmUpAnimalInfo.redScore = 0;
		warmUpAnimalInfo.yellowScore = (soul as WarmUpAnimalProperty).warmUpScore;

		List<float[]> scoresAfterBuff = BuffManager.instance.BuffInteractionWhenScore(warmUpAnimalInfo);
		foreach (float[] inputScore in scoresAfterBuff) {
			Scoring(inputScore);
		}
	}
}
