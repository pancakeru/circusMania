using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAnimalBallPassTimesCounter : MonoBehaviour
{
	private int monkey;
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
		switch (e.animalName) {
			case "Monkey":
				monkey++;
				break;
			case "Elephant":
				elephant++;
				break;
			case "Bear":
				bear++;
				break;
			case "Lion":
				lion++;
				break;
			case "Giraffe":
				giraffe++;
				break;
			case "Snake":
				snake++;
				break;
			case "Fox":
				fox++;
				break;
			case "Seal":
				seal++;
				break;
			case "Ostrich":
				ostrich++;
				break;
			case "Kangaroo":
				kangaroo++;
				break;
			case "Buffalo":
				buffalo++;
				break;
			case "Goat":
				goat++;
				break;
			case "Lizard":
				lizard++;
				break;
		}
	}

	public AnimalBallPassTimes GenerateAnimalBallPassTimes()
	{
		return new AnimalBallPassTimes(monkey, elephant, bear, lion, giraffe, snake, fox, seal, ostrich, kangaroo, buffalo, goat, lizard);
	}

	public void ResetAnimalBallPassTimes()
	{
		monkey = 0;
		elephant = 0;
		bear = 0;
		lion = 0;
		giraffe = 0;
		snake = 0;
		fox = 0;
		seal = 0;
		ostrich = 0;
		kangaroo = 0;
		buffalo = 0;
		goat = 0;
		lizard = 0;
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
