
public class AnimalControlBuffalo : AbstractSpecialAnimal
{
	public override void InteractWithBall()
	{
		PerformAnimalControl passer = animalBody.ball.GetPasser();

		animalBody.ball.gameObject.SetActive(true);
		if (passer == null) {
			animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange);
		} else {
			animalBody.ball.MoveBall(animalBody.selfIndexInShow, passer.selfIndexInShow + soul.baseBallChange);
		}
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		GenerateScore(animalInfo);

		controlUnit.InvokeOnExcitementEvent(animalInfo);

		animalBody.ifReadyToInteract = false;
	}
}
