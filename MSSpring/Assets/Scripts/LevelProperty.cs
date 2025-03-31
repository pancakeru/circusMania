
using UnityEngine;
[CreateAssetMenu(fileName = "NewLevelInfo", menuName = "LevelProperty")]
public class LevelProperty : ScriptableObject
{
	public float targetScore;
	public int allowedTurn;

	public int totalN;
	public float redA;
	public float redB;
	public float yellowA;
	public float yellowB;
	public float blueA;
	public float blueB;
}
