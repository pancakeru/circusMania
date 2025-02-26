
public struct AnimalInfoPack
{
	public PerformAnimalControl performAnimalControl;

	public float redScore;
	public float yellowScore;
	public float blueScore;

	public int power;
	public int warmUp;
	public int excited;

	public AnimalInfoPack(PerformAnimalControl performAnimalControl)
	{
		this.performAnimalControl = performAnimalControl;

		redScore = performAnimalControl.animalBrain.soul.baseRedChange;
		yellowScore = performAnimalControl.animalBrain.soul.baseYellowChange;
		blueScore = performAnimalControl.animalBrain.soul.baseBlueChange;

		power = 1;
		warmUp = 0;
		excited = 0;
	}

	public AnimalInfoPack(PerformAnimalControl performAnimalControl, float[] baseScore)
	{
        this.performAnimalControl = performAnimalControl;

        power = 1;
        warmUp = 0;
        excited = 0;
        redScore = baseScore[0];
		yellowScore = baseScore[1];
		blueScore = baseScore[2];
    }
}
