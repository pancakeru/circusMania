using UnityEngine;

[CreateAssetMenu(fileName = "NewGlobalLevel", menuName = "GlobalLevel")]
public class GlobalLevel : ScriptableObject
{
	public int levelIndex;
	public Sprite showVisual;
	public string levelName;
	public LevelProperty levelProperty;
}
