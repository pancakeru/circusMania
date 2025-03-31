
public class AnimalControlLion : AbstractSpecialAnimal
{
	private int[] ballToArray = { 0, 2, 4 };
	private int ballToArrayIndex = 0;

	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, ballToArray[ballToArrayIndex % ballToArray.Length],animalBody);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		GenerateScore(animalInfo);

		controlUnit.InvokeOnExcitementEvent(animalInfo);

		ballToArrayIndex += 1;

		animalBody.ifReadyToInteract = false;

		BallSound();
	}

    public override void ResetWhenBackToInitial()
    {
        base.ResetWhenBackToInitial();
		ballToArrayIndex = 0;
    }
}
