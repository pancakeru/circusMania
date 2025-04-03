using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAnimalBallPassTimesCounter : MonoBehaviour
{
	public int monkey; //public for testing animalTroupe
	private int elephant;
	private int bear;
	private int lion;
	private int giraffe;
	private int snake;
	private int fox;
	private int seal;
	private int ostrich;
	private int kangaroo;
	private int buffalo;
	private int goat;
	private int lizard;

	private void Start()
	{
		PerformAnimalControl.OnAnyPassingBall += PerformAnimalControl_OnAnyPassingBall;
	}

	private void PerformAnimalControl_OnAnyPassingBall(object sender, PerformAnimalControl.OnAnyPassingBallEventArgs e)
	{
		switch (e.animalName.ToLower()) {
			case "monkey":
				monkey++;
				break;
			case "elephant":
				elephant++;
				break;
			case "bear":
				bear++;
				break;
			case "lion":
				lion++;
				break;
			case "giraffe":
				giraffe++;
				break;
			case "snake":
				snake++;
				break;
			case "fox":
				fox++;
				break;
			case "seal":
				seal++;
				break;
			case "ostrich":
				ostrich++;
				break;
			case "kangaroo":
				kangaroo++;
				break;
			case "buffalo":
				buffalo++;
				break;
			case "goat":
				goat++;
				break;
			case "lizard":
				lizard++;
				break;
		}
	}

	public AnimalBallPassTimes GenerateAnimalBallPassTimes()
	{
		return new AnimalBallPassTimes(monkey, elephant, bear, lion, giraffe, snake, fox, seal, ostrich, kangaroo, buffalo, goat, lizard);
	}

	public void ResetAnimalBallPassTimes(string animalName)
	{
        switch (animalName.ToLower())
        {
            case "monkey":
                monkey = 0;
                break;
            case "elephant":
                elephant = 0;
                break;
            case "bear":
                bear = 0;
                break;
            case "lion":
                lion = 0;
                break;
            case "giraffe":
                giraffe = 0;
                break;
            case "snake":
                snake = 0;
                break;
            case "fox":
                fox = 0;
                break;
            case "seal":
                seal = 0;
                break;
            case "ostrich":
                ostrich = 0;
                break;
            case "kangaroo":
                kangaroo = 0;
                break;
            case "buffalo":
                buffalo = 0;
                break;
            case "goat":
                goat = 0;
                break;
            case "lizard":
                lizard = 0;
                break;
        }
    }
}

public struct AnimalBallPassTimes
{
	public int monkey;
	public int elephant;
	public int bear;
	public int lion;
	public int giraffe;
	public int snake;
	public int fox;
	public int seal;
	public int ostrich;
	public int kangaroo;
	public int buffalo;
	public int goat;
	public int lizard;

	public AnimalBallPassTimes(int monkey, int elephant, int bear, int lion, int giraffe, int snake, int fox, int seal, int ostrich, int kangaroo, int buffalo, int goat, int lizard)
	{
		this.monkey = monkey;
		this.elephant = elephant;
		this.bear = bear;
		this.lion = lion;
		this.giraffe = giraffe;
		this.snake = snake;
		this.fox = fox;
		this.seal = seal;
		this.ostrich = ostrich;
		this.kangaroo = kangaroo;
		this.buffalo = buffalo;
		this.goat = goat;
		this.lizard = lizard;
	}
}
