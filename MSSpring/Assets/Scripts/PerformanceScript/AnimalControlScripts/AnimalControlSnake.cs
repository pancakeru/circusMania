
public class AnimalControlSnake : AbstractSpecialAnimal
{
	float warmUpScore = 5;

    public override void DoWhenShowStart()
    {
		animalInfo.warmUp = animalInfo.mechanicActiveNum;
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

        if (animalInfo.warmUp > 1)
		{
            animalInfo.warmUp -= 1;
        }
        else if (animalInfo.warmUp == 1)
        {
            animalInfo.warmUp = 0;
            WarmUp();
            animalBody.mechanicNumberUI.Deactive();
        }

        animalBody.ifReadyToInteract = false;
    }

	private void WarmUp()
	{
		AnimalInfoPack warmUpAnimalInfo = new AnimalInfoPack(new animalProperty());
		warmUpAnimalInfo.yellowScore = warmUpScore;

		GenerateScore(warmUpAnimalInfo);
    }
}
