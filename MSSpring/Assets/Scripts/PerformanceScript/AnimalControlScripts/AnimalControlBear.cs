
public class AnimalControlBear : AbstractSpecialAnimal
{
	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange * animalInfo.power);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		animalInfo.redScore = soul.baseRedChange * animalInfo.power;

		GenerateScore(animalInfo);

		controlUnit.InvokeOnExcitementEvent(animalInfo);

		animalInfo.power += 1;

		animalBody.ifReadyToInteract = false;
	}

    public override void ResetWhenBackToInitial()
    {
        base.ResetWhenBackToInitial();
		animalInfo.power = 1;
    }
}
