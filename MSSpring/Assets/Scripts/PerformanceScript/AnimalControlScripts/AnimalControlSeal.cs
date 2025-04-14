
public class AnimalControlSeal : AbstractSpecialAnimal
{
	private int excitedTurnAmount = 7;
	private int ballChangeWhenExcited = -1;
	private int scoringTimesWhenExcited = 2;

	public override void InitOnExcitementEventListener()
	{
		controlUnit.OnExcitement += PerformUnit_OnExcitement;
	}

	public override void InteractWithBall()
	{
		if (animalInfo.excited == 0) {
			animalBody.ball.gameObject.SetActive(true);
			animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange,animalBody);
			animalBody.FlipSprite(1, false);
			animalBody.ifJustInteract = true;
			animalBody.ifHaveBall = false;

			GenerateScore(animalInfo);

			controlUnit.InvokeOnExcitementEvent(animalInfo);

			animalInfo.excited = soul.mechanicActiveNum;

			animalBody.ifReadyToInteract = false;
		} else if (animalInfo.excited > 0) {
			animalBody.ball.gameObject.SetActive(true);
			animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + ballChangeWhenExcited,animalBody);
			animalBody.FlipSprite(1, false);
			animalBody.ifJustInteract = true;
			animalBody.ifHaveBall = false;

			GenerateScore(animalInfo);

			controlUnit.InvokeOnExcitementEvent(animalInfo);

			animalBody.ifReadyToInteract = false;
		}
        animalBody.mechanicNumberUI.StartEffectImpact(false);
		BallSound();
    }

	private void PerformUnit_OnExcitement(object sender, PerformUnit.OnExcitementEventArgs e)
	{
		if (animalInfo.excited > 0) {
			for (int i = 0; i < scoringTimesWhenExcited; i++) {
				e.animalInfo.performAnimalControl.animalBrain.GenerateScore(e.animalInfo);
			}
			animalInfo.excited -= 1;
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
