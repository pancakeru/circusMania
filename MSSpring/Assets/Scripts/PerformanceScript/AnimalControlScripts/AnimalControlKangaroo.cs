
public class AnimalControlKangaroo : AbstractSpecialAnimal
{
	private int excitedTurnAmount = 7;
	private BuffKangaroo selfBuff;
	private bool ifNeedRemove = false;
	public override void InitOnExcitementEventListener()
	{
		controlUnit.OnExcitement += PerformUnit_OnExcitement;
	}

	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		GenerateScore(animalInfo);

		controlUnit.InvokeOnExcitementEvent(animalInfo);

		if (animalInfo.excited == 0) {
			animalInfo.excited = excitedTurnAmount;
			selfBuff = new BuffKangaroo(animalBody);
			BuffManager.instance.AddGiveExtraBuff(selfBuff);
			ifNeedRemove = true;
		}

		animalBody.ifReadyToInteract = false;
	}

	private void PerformUnit_OnExcitement(object sender, PerformUnit.OnExcitementEventArgs e)
	{
		if (animalInfo.excited > 0) {
			animalInfo.excited -= 1;
			if (animalInfo.excited == 0)
			{
                BuffManager.instance.RemoveGiveExtraBuff(selfBuff);
				ifNeedRemove = false;
            }	
		}
	}

	public override void ResetWhenBackToInitial()
	{
		base.ResetWhenBackToInitial();
		animalInfo.excited = 0;
		if (ifNeedRemove)
			BuffManager.instance.RemoveGiveExtraBuff(selfBuff);
	}
}
