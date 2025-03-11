
public class AnimalInfoPack
{
	public PerformAnimalControl performAnimalControl;

	public float redScore;
	public float yellowScore;
	public float blueScore;

	public int power;
	public int warmUp;
	public int excited;
    public MechanicNumberType mechanicNumberType;
    public int mechanicActiveNum;

    public AnimalInfoPack(animalProperty soul, PerformAnimalControl control)
	{
		performAnimalControl = control;
		redScore = soul.baseRedChange;
		yellowScore = soul.baseYellowChange;
		blueScore = soul.baseBlueChange;

		power = 1;
		warmUp = 0;
		excited = 0;
		mechanicNumberType = soul.mechanicNumberType;
        mechanicActiveNum = soul.mechanicActiveNum;
    }
}
