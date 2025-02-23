using System.Collections.Generic;

public class AnimalControlSeal : AbstractSpecialAnimal
{
	private AnimalInfoPack animalInfo;

	private int excitedTurnAmount = 7;
	private int ballChangeWhenExcited = -1;
	private int scoringTimesWhenExcited = 3;

	public override void InitAnimalInfo()
	{
		animalInfo = new AnimalInfoPack(animalBody);
	}

	public override void InteractWithBall()
	{
		if (animalInfo.excited == 0) {
			animalBody.ball.gameObject.SetActive(true);
			animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange);
			animalBody.FlipSprite(1, false);
			animalBody.ifJustInteract = true;
			animalBody.ifHaveBall = false;

			List<float[]> scoresAfterBuff = BuffManager.instance.BuffInteractionWhenScore(animalInfo);
			foreach (float[] inputScore in scoresAfterBuff) {
				Scoring(inputScore);
			}

			animalInfo.excited = excitedTurnAmount;

			animalBody.ifReadyToInteract = false;
		} else if (animalInfo.excited > 0) {
			animalBody.ball.gameObject.SetActive(true);
			animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + ballChangeWhenExcited);
			animalBody.FlipSprite(1, false);
			animalBody.ifJustInteract = true;
			animalBody.ifHaveBall = false;

			for (int i = 0; i < scoringTimesWhenExcited; i++) {
				List<float[]> scoresAfterBuff = BuffManager.instance.BuffInteractionWhenScore(animalInfo);
				foreach (float[] inputScore in scoresAfterBuff) {
					Scoring(inputScore);
				}
			}

			animalInfo.excited -= 1;

			animalBody.ifReadyToInteract = false;
		}
	}
}
