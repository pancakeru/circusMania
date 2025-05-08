
public class AnimalControlBuffalo : AbstractSpecialAnimal
{
	private BuffBuffalo selfBuff;
    private bool haveBuffAdded = false;


    public override void InitOnExcitementEventListener()
    {
        controlUnit.OnExcitement += PerformUnit_OnExcitement;
    }

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
        animalInfo.excited = animalInfo.mechanicActiveNum;
        if (haveBuffAdded)
        {
            BuffManager.instance.RemoveChangeBaseBuff(selfBuff);
        }
        selfBuff = new BuffBuffalo(animalBody,soul.skillNum);
        BuffManager.instance.AddChangeBaseBuff(selfBuff);
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
                BuffManager.instance.RemoveChangeBaseBuff(selfBuff);
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
            BuffManager.instance.RemoveChangeBaseBuff(selfBuff);
            haveBuffAdded = false;
        }
        animalBody.mechanicNumberUI.StartEffectDeath();
    }


    
}
