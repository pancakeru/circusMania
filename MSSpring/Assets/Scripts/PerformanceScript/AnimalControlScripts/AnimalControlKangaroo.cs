
public class AnimalControlKangaroo : AbstractSpecialAnimal
{
	private int excitedTurnAmount = 7;

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
		}

		animalBody.ifReadyToInteract = false;
	}

	private void PerformUnit_OnExcitement(object sender, PerformUnit.OnExcitementEventArgs e)
	{
		if (animalInfo.excited > 0) {
			if (e.animalInfo.redScore > 0) {
				GenerateScore(animalInfo);
			}
			animalInfo.excited -= 1;
		}
	}
}
