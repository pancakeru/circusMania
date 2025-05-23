
public class AnimalControlSnake : AbstractSpecialAnimal
{
	public float warmUpScore = 5;

    public override void DoWhenShowStart()
    {
		animalInfo.warmUp = animalInfo.mechanicActiveNum;
    }

    public override void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange,animalBody);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;


        if (animalInfo.warmUp > 1)
        {
            animalInfo.warmUp -= 1;
        }
        else if (animalInfo.warmUp == 1)
        {
            animalInfo.warmUp = 0;
            WarmUp();
            animalBody.mechanicNumberUI.StartEffectImpact(true);
        }
        GenerateScore(animalInfo);
        
		controlUnit.InvokeOnExcitementEvent(animalInfo);
        animalInfo.yellowScore = 0;


        animalBody.ifReadyToInteract = false;
        BallSound();
    }

	private void WarmUp()
	{
		AnimalInfoPack warmUpAnimalInfo = new AnimalInfoPack(new animalProperty(),animalBody);
        warmUpAnimalInfo.yellowScore = soul.skillNum;

        //GenerateScore(warmUpAnimalInfo);
        animalInfo.yellowScore = soul.skillNum;
    }
}
