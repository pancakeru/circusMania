
public class AnimalControlSnake : AbstractSpecialAnimal
{
	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange,animalBody);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		GenerateScore(animalInfo);

		controlUnit.InvokeOnExcitementEvent(animalInfo);

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

		GenerateScore(warmUpAnimalInfo);
	}
}
