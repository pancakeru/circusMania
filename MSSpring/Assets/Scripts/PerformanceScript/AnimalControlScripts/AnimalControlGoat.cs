
public class AnimalControlGoat : AbstractSpecialAnimal
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

		animalBody.ifReadyToInteract = false;

		BallSound();
	}

	public override void DoWhenShowStart()
	{
        base.DoWhenShowStart();
		BuffManager.instance.Temp_RecordGoatPower(soul.skillNum);
    }

    public override void ResetWhenBackToInitial()
    {
        base.ResetWhenBackToInitial();
        BuffManager.instance.Temp_RecordGoatPower(0);
    }
}
