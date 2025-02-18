
public class AnimalControlLion : AbstractSpecialAnimal
{
	private int warmupPoints = 0;
	private bool hasWarmuped = false;

	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;
		//animalManager.Instance.changeScore(interactionYellowScore, interactionRedScore, interactionBlueScore, selfIndex);
		//TODO:分数展示和改变逻辑
		if (soul.baseRedChange != 0) {
			controlUnit.ChangeRedScore(soul.baseRedChange);
			animalBody.generator.RequestTextEffect(soul.baseRedChange, ScoreTextEffectGenerator.ScoreType.Red);
			warmupPoints += 1;
		}
		animalBody.ifReadyToInteract = false;
		if (warmupPoints >= 3) {
			if (!hasWarmuped) {
				WarmUp();
			}
		}
	}

	private void WarmUp()
	{
		controlUnit.ChangeYellowScore(5);
	}
}
