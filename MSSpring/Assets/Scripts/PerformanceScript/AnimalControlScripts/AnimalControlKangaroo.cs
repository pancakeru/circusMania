public class AnimalControlKangaroo : AbstractSpecialAnimal
{
	public override void InitOnExcitementEventListener()
	{
		controlUnit.OnExcitement += PerformUnit_OnExcitement;
	}

	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange, animalBody);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		GenerateScore(animalInfo);

		controlUnit.InvokeOnExcitementEvent(animalInfo);

        animalInfo.excited = animalInfo.mechanicActiveNum;
        animalBody.mechanicNumberUI.StartEffectImpact(false);

		animalBody.ifReadyToInteract = false;
	}

	private void PerformUnit_OnExcitement(object sender, PerformUnit.OnExcitementEventArgs e)
	{
		if (animalInfo.excited > 0)
		{
			if (e.animalInfo.redScore > 0)
			{
				GenerateScore(animalInfo);
			}
			animalInfo.excited -= 1;

			animalBody.mechanicNumberUI.UpdateText();
			if (animalInfo.excited > 0) animalBody.mechanicNumberUI.StartEffectImpact(false);
			else animalBody.mechanicNumberUI.StartEffectImpact(true);
        }
	}

    public override void ResetWhenBackToInitial()
    {
        base.ResetWhenBackToInitial();
		animalInfo.excited = 0;

		animalBody.mechanicNumberUI.StartEffectDeath();
    }
}
