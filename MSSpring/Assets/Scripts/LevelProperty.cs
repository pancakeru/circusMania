
using UnityEngine;
[CreateAssetMenu(fileName = "NewLevelInfo", menuName = "LevelProperty")]
public class LevelProperty : ScriptableObject
{
    public float targetScore;
    public int allowedTurn;
}
