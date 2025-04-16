
using System.Runtime.ConstrainedExecution;
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
    public ScoreColor skillScoreColor;

    public string textScoreSpecial3; //+10 * n Red
    public string textScoreSpecial5; //and banana
    public string textSkillSpecial;
    public string textPowerSpecial3;
    public string textExcitedSpecial3;

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
    string formatScore = "<b><color=#{0}>+{1} {2} {3}.</color>{4} {5}</b>";
    string formatSkillBasic = "<color=#{0}>{1}</color>";
    string formatPower = "<b><color=#{0}>POWER</color></b>: +1 POWER per ball passed.{1}{2}.";
    string formatWarmUp = "<b><color=#{0}>WARM UP</color></b>: +1 WARM UP per ball passed. When WARM UP reaches <b>{1}</b>, {2}, then deactives until next round.";
    string formatExcited = "<b><color=#{0}>EXCITED</color></b>: <b>{1}</b> EXCITED when ball passed. When EXCITED, -1 EXCITED and {2} {3}.";

    string colorHexFun = "D3458F";
    string colorHexSkill = "E4CF7B";
    string colorHexNovelty = "45A9D2";

    public string ReturnAllExplanation()
    {
        string textExplanation = $"{ReturnBaseScore()}\n{ReturnSkillBasic()}\n{ReturnSkillMechanic()}";
        return textExplanation;
    }

    string ReturnBaseScore()
    {
        (string textColor, string textScore, string textScoreName) 
                         = baseRedChange != 0 ? (colorHexFun, baseRedChange.ToString(), "FUN")
                         : baseYellowChange != 0 ? (colorHexSkill, baseYellowChange.ToString(), "SKILL")
                         : baseBlueChange != 0 ? (colorHexNovelty, baseBlueChange.ToString(), "NOVELTY") 
                         : ("ERROR", "ERROR", "ERROR");

        string extraSkillNum = string.IsNullOrEmpty(textScoreSpecial5) ? "" : $"and +{skillNum.ToString()}";

        return string.Format
        (
            formatScore,
            textColor, textScore, textScoreName, textScoreSpecial3, extraSkillNum, textScoreSpecial5
        );
    }

    string ReturnSkillBasic()
    {
        string textColor = "FFFFFF";

        string textSkillScore = skillNum == 0 ? "" : ReturnSkillScore();

        return string.Format
        (
            formatSkillBasic,
            textColor, textSkillSpecial, textSkillScore
        );
    }

    string ReturnSkillScore()
    {
        /*
        if (!(skillNum == 0 && skillScoreColor == ScoreColor.None || skillNum != 0 && skillScoreColor != ScoreColor.None))
        {
            Debug.LogError($"ASSIGN THE CORRECT VARIABLE TO {animalName}"); 
            return "ERROR";
        }
        */

        (string textColor, string textScore, string textScoreName)
                         = skillScoreColor == ScoreColor.Red ? (colorHexFun, skillNum.ToString(), "FUN")
                         : skillScoreColor == ScoreColor.Yellow ? (colorHexSkill, skillNum.ToString(), "SKILL")
                         : skillScoreColor == ScoreColor.Blue ? (colorHexNovelty, skillNum.ToString(), "NOVELTY") 
                         : ("ERROR", "ERROR", "ERROR");

        return string.Format
        (
            formatScore,
            textColor, textScore, textScoreName
        );
    }

    string ReturnSkillMechanic()
    {
        if (mechanicNumberType == MechanicNumberType.None) return "";

        string formatMechanic = mechanicNumberType == MechanicNumberType.Power ? formatPower
                              : mechanicNumberType == MechanicNumberType.WarmUp ? formatWarmUp
                              : mechanicNumberType == MechanicNumberType.Excited ? formatExcited
                              : "ERROR";

        string textColor = "FFFFFF";
        string textCondition = string.IsNullOrEmpty(mechanicActiveNum.ToString()) ? "" : mechanicActiveNum.ToString();
        string textSkill = string.IsNullOrEmpty(textSkillSpecial) ? ReturnSkillScore() : textSkillSpecial;

        return string.Format
        (
            formatMechanic,
            textColor, textCondition, textSkill
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

public enum ScoreColor
{
    None,
    Red,
    Yellow,
    Blue,
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

[CustomPropertyDrawer(typeof(ScoreColor))]
public class ScoreColorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.enumValueIndex = (int)(ScoreColor)EditorGUI.EnumPopup(position, label, (ScoreColor)property.enumValueIndex);
    }
}
#endif
