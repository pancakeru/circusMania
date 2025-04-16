
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimalInfo", menuName = "Animal System/AnimalProperty")]
public class animalProperty : ScriptableObject
{
    [Header("Must Set In Inspector")]
    public string animalName;
    public Sprite animalCoreImg;
    public Sprite explainImg;

    public MechanicNumberType mechanicNumberType;

    [Header("Must Not Set In Inspector")]

    [Header("     For Ball passing")]
    public float baseRedChange;
    public float baseYellowChange;
    public float baseBlueChange;
    public int baseBallChange;
    public int restTurn;

    [Header("     For Mechanic & Skill")]
    public int mechanicActiveNum;
    public int skillNum;

    [Header("     For explain")]
    
    public string ballAction1;

    public string scoreAction1;
    public string scoreAction2;
    public string scoreAction3;

    string formatScore = "<b><color=#{0}>+{1} {2}.</color></b>";
    string formatSkill;

    string colorHexFun = "D3458F";
    string colorHexSkill = "E4CF7B";
    string colorHexNovelty = "45A9D2";

    public string returnBallAction()
    {
        return string.Format
        (
            "Throw ball to {0},",
            string.IsNullOrEmpty(ballAction1) ? "" : ballAction1
        );
    }

    public string ReturnScore()
    {
        (string textColor, string textScore, string textScoreName) 
                         = baseRedChange != 0 ? (colorHexFun, baseRedChange.ToString(), "FUN")
                         : baseYellowChange != 0 ? (colorHexSkill, baseYellowChange.ToString(), "SKILL")
                         : baseBlueChange != 0 ? (colorHexNovelty, baseBlueChange.ToString(), "NOVELTY") : ("ERROR", "ERROR", "ERROR");

        Debug.Log(formatScore);

        return string.Format
        (
            formatScore,
            textColor, textScore, textScoreName
        );
    }

    public string returnBallAction1Only()
    {
        return string.Format
        (
            "{0}",
            string.IsNullOrEmpty(ballAction1) ? "" : ballAction1
        );
    }

    public string returnScoreActionNoRest()
    {
        return string.Format
        (
            "Gain {0}. \n{1}",
            string.IsNullOrEmpty(scoreAction1) ? "" : scoreAction1,
            string.IsNullOrEmpty(scoreAction3) ? "" : scoreAction3
        );
    }

}

public enum MechanicNumberType
{
    None,
    Power,
    WarmUp,
    Excited,
}

//Dropdown menu in hierachy
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MechanicNumberType))]
public class MechanicNumberTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.enumValueIndex = (int)(MechanicNumberType)EditorGUI.EnumPopup(position, label, (MechanicNumberType)property.enumValueIndex);
    }
}
#endif
