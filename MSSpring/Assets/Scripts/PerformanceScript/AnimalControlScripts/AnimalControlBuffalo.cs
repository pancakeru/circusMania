
public class AnimalControlBuffalo : AbstractSpecialAnimal
{
	private BuffBuffalo selfBuff;
	public override void InteractWithBall()
	{
		PerformAnimalControl passer = animalBody.ball.GetPasser();

		animalBody.ball.gameObject.SetActive(true);
        animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange, animalBody);
		/*
        if (passer == null) {
			animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange,animalBody);
		} else {
			animalBody.ball.MoveBall(animalBody.selfIndexInShow, passer.selfIndexInShow + soul.baseBallChange,animalBody);
		}*/
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		GenerateScore(animalInfo);

		controlUnit.InvokeOnExcitementEvent(animalInfo);

		animalBody.ifReadyToInteract = false;
	}

    public override void DoWhenShowStart()
    {
        base.DoWhenShowStart();
		selfBuff = new BuffBuffalo(animalBody);
		BuffManager.instance.AddChangeBaseBuff(selfBuff);
    }

    public override void ResetWhenBackToInitial()
    {
        base.ResetWhenBackToInitial();
		BuffManager.instance.RemoveChangeBaseBuff(selfBuff);
    }
}
