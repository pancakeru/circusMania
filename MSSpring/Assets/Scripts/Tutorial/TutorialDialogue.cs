using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialDialogue
{
    [Header("Speaker")]
    public Sprite speakerSprite;
    [TextArea(5, 20)] public string speakerDialogue;

    [Header("Audio")]
    public AudioClip audioToBePlayed;

    [Header("Mask")]
    public bool isWholeMask;
    public Vector2 maskSizeDelta = Vector2.zero;
    public Vector2 maskAnchoredPosition = Vector2.zero;

    [Header("Animal Introduction")]
    public bool isAnimalIntroduction = false;
    public Sprite animalIntroductionSprite;
    [TextArea(5, 20)] public string animalIntroductionString;

    [Header("Proceed Condition")]
    public string proceedCondition = "N/A";
}
