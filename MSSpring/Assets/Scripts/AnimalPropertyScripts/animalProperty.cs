
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimalInfo", menuName = "Animal System/AnimalProperty")]
public class animalProperty : ScriptableObject
{
    public string animalName;
    public Sprite animalCoreImg;
    public Sprite explainImg;

    [Header("For Ball passing")]
    public float baseYellowChange;
    public float baseRedChange;
    public float baseBlueChange;
    public int baseBallChange;
    public int restTurn;

    [Header("For Mechanic")]
    public MechanicNumberType mechanicNumberType;
    public int mechanicActiveNum; 

    [Header("For explain")]
    /// <summary>
    /// 得分动作的文本模板，例如 "得分 {0} 分，并额外获得 {1} 点奖励"
    /// </summary>
    
    [SerializeField]
    private string ballAction1;
    
    /// <summary>
    /// 传球动作的文本模板，例如 "向{0}传球，进入{1}回合冷却"
    /// </summary>
    [SerializeField]
    private string scoreAction1;
    [SerializeField]
    private string scoreAction2;
    [SerializeField]
    private string scoreAction3;

    /// <summary>
    /// 生成得分动作的最终文本
    /// </summary>
    /// <returns>格式化后的得分描述字符串</returns>

    public string returnBallAction()
    {
        return string.Format(
        "Throw ball to {0},",
        string.IsNullOrEmpty(ballAction1) ? "" : ballAction1
    );
    }

    public string returnScoreAction()
    {
        return string.Format(
        "Gain {0}, Rest {1}. \n{2}",
        string.IsNullOrEmpty(scoreAction1) ? "" : scoreAction1,
        string.IsNullOrEmpty(scoreAction2) ? "" : scoreAction2,
        string.IsNullOrEmpty(scoreAction3) ? "" : scoreAction3
    );
    }

    public string returnBallAction1Only()
    {
        return string.Format(
        "{0}",
        string.IsNullOrEmpty(ballAction1) ? "" : ballAction1
    );
    }

    public string returnScoreActionNoRest()
    {
        return string.Format(
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
