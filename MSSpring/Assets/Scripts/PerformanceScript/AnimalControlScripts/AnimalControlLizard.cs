
public class AnimalControlLizard : AbstractSpecialAnimal
{
	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange,animalBody);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		animalInfo.yellowScore = soul.baseYellowChange * animalInfo.power;

		GenerateScore(animalInfo);

		controlUnit.InvokeOnExcitementEvent(animalInfo);

		animalInfo.power += 1;

		animalBody.ifReadyToInteract = false;
	}

	public override void EnterRest(bool ifDirect)
	{
		animalBody.ifInRest = true;
		if (!ifDirect)
			animalBody.FlipSprite(2, false, () => { animalBody.ChangeRestCount(soul.restTurn + animalInfo.power - 1); });
		else
			animalBody.curRestTurn = soul.restTurn + animalInfo.power - 1;

    }

    public override void ResetWhenBackToInitial()
    {
        base.ResetWhenBackToInitial();
		animalInfo.power = 1;
    }
}
