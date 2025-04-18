
using System.Runtime.ConstrainedExecution;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

[CreateAssetMenu(fileName = "NewAnimalInfo", menuName = "Animal System/AnimalProperty")]
public class animalProperty : ScriptableObject
{
    [Header("Must Set In Inspector")]

    public string animalName;
    public Sprite animalCoreImg;
    public Sprite explainImg;

    public int baseBallChange;

    public MechanicNumberType mechanicNumberType;
    public ScoreColor skillScoreColor;

    public string textSkill;
    public string textMechanicScore;
    public string textMechanicExtra;

    [Header("Must Not Set In Inspector")]

    [Header("     For Ball passing")]
    public float baseRedChange;
    public float baseYellowChange;
    public float baseBlueChange;
    public int restTurn;

    [Header("     For Mechanic & Skill")]
    public int mechanicActiveNum;
    public int skillNum;

    [Header("     For explain")]
    string formatScore = "<b><color=#{0}>+{1} {2}</color></b>";

    string formatPower = "<b><color=#{0}>POWER</color></b>: +1 POWER per ball passed.{1}{2}.";
    string formatWarmUp = "<b><color=#{0}>WARM UP</color></b>: +1 WARM UP per ball passed. When WARM UP is <b>{1}</b>, {2}, then deactives until next round.";
    string formatExcited = "<b><color=#{0}>EXCITED</color></b>: <b>{1}</b> EXCITED when ball passed. When EXCITED, -1 EXCITED per ball passed by other animals and {2} {3}.";

    string colorHexFun = "D3458F";
    string colorHexSkill = "E4CF7B";
    string colorHexNovelty = "45A9D2";

    public string ReturnAllExplanation()
    {
        string banana = animalName == "Giraffe" ? $" and {ReturnBanana()}" : "";
        string skill = string.IsNullOrEmpty(textSkill) ? "" : "\n" + ReturnSkillScore();
        textSkill = string.IsNullOrEmpty(textSkill) ? "" : textSkill + ".";
        string mechanic = mechanicNumberType == MechanicNumberType.None || mechanicNumberType == MechanicNumberType.Power ? "" : "\n" + ReturnSkillMechanic();

        string finalExplanation = $"{ReturnScore()}{banana} per ball passed.{skill}{textSkill}{mechanic}";
        return ColorKeyWord(finalExplanation);
    }

    string ReturnScore()
    {
        (string textColor, string textScore, string textScoreName)
                         = baseRedChange != 0 ? (colorHexFun, baseRedChange.ToString(), "FUN")
                         : baseYellowChange != 0 ? (colorHexSkill, baseYellowChange.ToString(), "SKILL")
                         : baseBlueChange != 0 ? (colorHexNovelty, baseBlueChange.ToString(), "NOVELTY")
                         : ("ERROR", "ERROR", "ERROR");

        return string.Format
        (
            formatScore,
            textColor, textScore, textScoreName
        );
    }

    string ReturnBanana()
    {
        string textColor = "FFFFFF";
        string textScore = skillNum.ToString();
        string textScoreName = "Banana";

        return string.Format
        (
            formatScore,
            textColor, textScore, textScoreName
        );
    }

    string ReturnSkillScore()
    {
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

        string color = "FFFFFF";
        string condition = mechanicActiveNum.ToString();
        string mechanicScore = textMechanicScore == "useSkillNum" ? ReturnSkillScore().ToString() : textMechanicScore;
        string mechanicExtra = textMechanicExtra;

        return string.Format
        (
            formatMechanic,
            color, condition, mechanicScore, mechanicExtra
        );
    }

    string ColorKeyWord(string theString)
    {
        if (theString.Contains("FUN"))
        {
            string formatColored = $"<b><color=#{colorHexFun}>FUN</color></b>";
            theString = theString.Replace("FUN", formatColored);
        }

        if (theString.Contains("SKILL"))
        {
            string formatColored = $"<b><color=#{colorHexSkill}>Skill</color></b>";
            theString = theString.Replace("SKILL", formatColored);
        }

        if (theString.Contains("NOVELTY"))
        {
            string formatColored = $"<b><color=#{colorHexNovelty}>NOVELTY</color></b>";
            theString = theString.Replace("NOVELTY", formatColored);
        }

        return theString;
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
