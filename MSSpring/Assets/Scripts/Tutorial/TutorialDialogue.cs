using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialDialogue
{
    public Sprite speakerSprite;
    [TextArea(5, 20)] public string speakerDialogue;

    public bool isWholeMask;
}

[CreateAssetMenu(fileName = "NewShowTutorial", menuName = "Tutorial/ShowTutorial")]
public class ShowTutorial : ScriptableObject
{
    public TutorialDialogue[] tutorialDialogueList;
}
