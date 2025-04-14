public class AnimalControlKangaroo : AbstractSpecialAnimal
{
    private BuffKangaroo selfBuff;
	private bool haveBuffAdded = false;
    
	

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
		if (haveBuffAdded)
		{
			BuffManager.instance.RemoveGiveExtraBuff(selfBuff);
		}
        selfBuff = new BuffKangaroo(animalBody,soul.skillNum);
        BuffManager.instance.AddGiveExtraBuff(selfBuff);
		haveBuffAdded = true;
        animalBody.mechanicNumberUI.StartEffectImpact(false);

		animalBody.ifReadyToInteract = false;

		BallSound();
	}

	private void PerformUnit_OnExcitement(object sender, PerformUnit.OnExcitementEventArgs e)
	{
		if (animalInfo.excited > 0)
		{
			/*
			if (e.animalInfo.redScore > 0)
			{
				GenerateScore(animalInfo);
			}*/
			animalInfo.excited -= 1;
			if (animalInfo.excited <= 0)
			{
                BuffManager.instance.RemoveGiveExtraBuff(selfBuff);
				haveBuffAdded = false;
            }
			animalBody.mechanicNumberUI.UpdateText();
			if (animalInfo.excited > 0) animalBody.mechanicNumberUI.StartEffectImpact(false);
			else animalBody.mechanicNumberUI.StartEffectImpact(true);
        }
	}

    public override void ResetWhenBackToInitial()
    {
        base.ResetWhenBackToInitial();
		animalInfo.excited = 0;
		if (haveBuffAdded)
		{
            BuffManager.instance.RemoveGiveExtraBuff(selfBuff);
			haveBuffAdded = false;
        }
		animalBody.mechanicNumberUI.StartEffectDeath();
    }
}
