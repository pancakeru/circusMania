using UnityEngine;

[CreateAssetMenu(fileName = "NewShowTutorial", menuName = "Tutorial/ShowTutorial")]
public class ShowTutorial : ScriptableObject
{
    public TutorialDialogue[] tutorialDialogueList;
}
