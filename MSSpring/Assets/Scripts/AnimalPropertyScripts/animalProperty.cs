
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

[CreateAssetMenu(fileName = "NewAnimalInfo", menuName = "Animal System/AnimalProperty")]
[System.Serializable]
public class animalProperty : ScriptableObject
{
    [Header("Must Set In Inspector")]

    public string animalName;
    [JsonIgnore] public Sprite animalCoreImg;
    [JsonIgnore] public Sprite explainImg;

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

    string formatPower = "<b><color=#{0}>“力量”</color></b>: +1 “力量”（每次传球时）。\n{1}{2} {3}。";
    string formatWarmUp = "<b><color=#{0}>“热身”</color></b>: +1 “热身”（每次传球时）。\n当热身为 <b>{1}</b>, {2}{3}, 随后“热身”失效至下一次行动。";
    string formatExcited = "<b><color=#{0}>“兴奋”</color></b>: <b>{1}</b> “兴奋”（每次传球时）。 \n当“兴奋”时, -1 “兴奋”（每次其他动物传球时）并{2}{3}。";

    string formatPowerSimple = "<b><color=#{0}>力量</color></b>: {1}{2} {3}.";
    string formatWarmUpSimple = "<b><color=#{0}>热身 ({1})</color></b>: {2} {3}.";
    string formatExcitedSimple = "<b><color=#{0}>兴奋 ({1})</color></b>: {2} {3}.";

    //Original format
    /*
    string formatPower = "<b><color=#{0}>POWER</color></b>: +1 POWER per ball passed.{1}{2} {3}.";
    string formatWarmUp = "<b><color=#{0}>WARM UP</color></b>: +1 WARM UP per ball passed. When WARM UP is <b>{1}</b>, {2}{3}, then deactives until next act.";
    string formatExcited = "<b><color=#{0}>EXCITED</color></b>: <b>{1}</b> EXCITED when ball passed. When EXCITED, -1 EXCITED per ball passed by other animals and {2}{3}.";

    string formatPowerSimple = "<b><color=#{0}>POWER</color></b>: {1}{2} {3}.";
    string formatWarmUpSimple = "<b><color=#{0}>WARM UP ({1})</color></b>: {2} {3}.";
    string formatExcitedSimple = "<b><color=#{0}>EXCITED ({1})</color></b>: {2} {3}.";
    */

    string colorHexRed = "D3458F";
    string colorHexYellow = "BF8B00"; //"E4CF7B";
    string colorHexBlue = "45A9D2";

    string scoreRedName = "JOY";
    string scoreYellowName = "SKILL";
    string scoreBlueName = "NOVELTY";

    public string ReturnAllExplanation()
    {
        string banana = animalName == "Giraffe" ? $" and {ReturnBanana()}" : "";
        string skill = string.IsNullOrEmpty(textSkill) ? "" : "\n" + ReturnSkillScore();
        textSkill = string.IsNullOrEmpty(textSkill) ? "" : textSkill;
        string mechanic = mechanicNumberType == MechanicNumberType.None ? "" : "\n" + ReturnSkillMechanic(false);

        textSkill = ForceLocalization("textSkill");

        string finalExplanation = $"每次传球时{ReturnScore()}{banana}。{skill}{textSkill}{mechanic}";
        //string finalExplanation = $"{ReturnScore()}{banana} per ball passed.{skill}{textSkill}{mechanic}";
        return ColorKeyWord(finalExplanation);
    }

    public string ReturnSimpleExplanation()
    {
        string banana = animalName == "Giraffe" ? $" and {ReturnBanana()}" : "";
        string skill = string.IsNullOrEmpty(textSkill) ? "" : "\n" + ReturnSkillScore();
        textSkill = string.IsNullOrEmpty(textSkill) ? "" : textSkill;
        string mechanic = mechanicNumberType == MechanicNumberType.None ? "" : "\n" + ReturnSkillMechanic(true);

        string finalExplanation = $"{ReturnScore()}{banana}.{skill}{textSkill}{mechanic}";

        textSkill = ForceLocalization("textSkill");

        finalExplanation = Regex.Replace(finalExplanation, @"<color=(#FFFFFF|white)>", "<color=#000000>", RegexOptions.IgnoreCase);

        return ColorKeyWord(finalExplanation);
    }


    string ReturnScore()
    {
        (string textColor, string textScore, string textScoreName)
                         = baseRedChange != 0 ? (colorHexRed, baseRedChange.ToString(), scoreRedName)
                         : baseYellowChange != 0 ? (colorHexYellow, baseYellowChange.ToString(), scoreYellowName)
                         : baseBlueChange != 0 ? (colorHexBlue, baseBlueChange.ToString(), scoreBlueName)
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
        string textScoreName = "香蕉";

        return string.Format
        (
            formatScore,
            textColor, textScore, textScoreName
        );
    }

    string ReturnSkillScore()
    {
        (string textColor, string textScore, string textScoreName)
                         = skillScoreColor == ScoreColor.Red ? (colorHexRed, skillNum.ToString(), scoreRedName)
                         : skillScoreColor == ScoreColor.Yellow ? (colorHexYellow, skillNum.ToString(), scoreYellowName)
                         : skillScoreColor == ScoreColor.Blue ? (colorHexBlue, skillNum.ToString(), scoreBlueName)
                         : ("ERROR", "ERROR", "ERROR");

        if (textColor == "ERROR") return "";

        return string.Format
        (
            formatScore,
            textColor, textScore, textScoreName
        );
    }

    string ReturnSkillMechanic(bool isSimple)
    {
        if (mechanicNumberType == MechanicNumberType.None) return "";

        string formatMechanic = "";

        if (isSimple)
        {
            formatMechanic = mechanicNumberType == MechanicNumberType.Power ? formatPowerSimple
                           : mechanicNumberType == MechanicNumberType.WarmUp ? formatWarmUpSimple
                           : mechanicNumberType == MechanicNumberType.Excited ? formatExcitedSimple
                           : "ERROR";
        }
        else
        {
            formatMechanic = mechanicNumberType == MechanicNumberType.Power ? formatPower
                           : mechanicNumberType == MechanicNumberType.WarmUp ? formatWarmUp
                           : mechanicNumberType == MechanicNumberType.Excited ? formatExcited
                           : "ERROR";
        }

        string color = "FFFFFF";
        string condition = mechanicNumberType == MechanicNumberType.Power ? "" : mechanicActiveNum.ToString();
        string mechanicScore = textMechanicScore == "useSkillNum" ? ReturnSkillScore().ToString() : textMechanicScore;
        string mechanicExtra = textMechanicExtra;

        mechanicExtra = ForceLocalization("mechanicExtra");

        return string.Format
        (
            formatMechanic,
            color, condition, mechanicScore, mechanicExtra
        );
    }

    string ColorKeyWord(string theString)
    {
        if (theString.Contains(scoreRedName))
        {
            string formatColored = $"<b><color=#{colorHexRed}>{scoreRedName}</color></b>";
            theString = theString.Replace(scoreRedName, formatColored);
        }

        if (theString.Contains(scoreYellowName))
        {
            string formatColored = $"<b><color=#{colorHexYellow}>{scoreYellowName}</color></b>";
            theString = theString.Replace(scoreYellowName, formatColored);
        }

        if (theString.Contains(scoreBlueName))
        {
            string formatColored = $"<b><color=#{colorHexBlue}>{scoreBlueName}</color></b>";
            theString = theString.Replace(scoreBlueName, formatColored);
        }

        return theString;
    }

    public override bool Equals(object obj)
    {
        if (obj is not animalProperty other) return false;
        return this.animalName == other.animalName; // 用合适的唯一标识
    }

    string ForceLocalization(string textKind)
    {
        string returnText = "";
        switch (textKind)
        {
            case "textSkill":

                if (animalName == "Fox") returnText = "每当相邻动物传球并产出JOY时。";
                else if (animalName == "Goat") returnText = "有“力量”的动物接球时，使他的“力量”为 1 并使获得消除的“力量”相应倍数的NOVELTY。";
                else if (animalName == "Lion") returnText = "狮子每次传球到第一位，第三位，第五位。";

                break;

            case "mechanicExtra":

                if (animalName == "Bear") returnText = "“力量”越高投掷距离越长";
                else if (animalName == "Buffalo") returnText = "每当任何动物产出NOVELTY";
                else if (animalName == "Kangaroo") returnText = "每当任何动物产出JOY";
                else if (animalName == "Lizard") returnText = "产出的SKILL与自身“力量”相乘";
                else if (animalName == "Seal") returnText = "翻倍所有动物产出分数并减少自身投掷距离";

                break;
        }
        return returnText;
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
