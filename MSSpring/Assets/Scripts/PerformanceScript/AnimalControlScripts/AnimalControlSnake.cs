
public class AnimalControlSnake : AbstractSpecialAnimal
{
	private int[] ballToArray = { 1, 3, 5 };
	private int ballToArrayIndex = 0;

	public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, ballToArray[ballToArrayIndex % ballToArray.Length]);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;
		//animalManager.Instance.changeScore(interactionYellowScore, interactionRedScore, interactionBlueScore, selfIndex);
		//TODO:分数展示和改变逻辑
		if (soul.baseBlueChange != 0) {
			controlUnit.ChangeBlueScore(soul.baseBlueChange);
			animalBody.generator.RequestTextEffect(soul.baseBlueChange, ScoreTextEffectGenerator.ScoreType.Blue);
			ballToArrayIndex += 1;
		}
		animalBody.ifReadyToInteract = false;
	}
}
