using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialDialogue
{
    public Sprite speakerSprite;
    [TextArea(5, 20)] public string speakerDialogue;

    public bool isWholeMask;
    public Vector2 maskSizeDelta = Vector2.zero;
    public Vector2 maskAnchoredPosition = Vector2.zero;

    public string proceedCondition = "N/A";
}

[CreateAssetMenu(fileName = "NewShowTutorial", menuName = "Tutorial/ShowTutorial")]
public class ShowTutorial : ScriptableObject
{
    public TutorialDialogue[] tutorialDialogueList;
}
