
public class AnimalControlGiraffe : AbstractSpecialAnimal
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

		controlUnit.thrower.addBanana(soul.skillNum);

		animalBody.ifReadyToInteract = false;

		BallSound();
	}
}
