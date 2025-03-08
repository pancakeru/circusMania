
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimalInfo", menuName = "Animal System/AnimalProperty")]
public class animalProperty : ScriptableObject
{
    public string animalName;
    public Sprite animalCoreImg;

    [Header("ForShop")]
    public int animalPrice;

    [Header("For Ball passing")]
    public float baseYellowChange;
    public float baseRedChange;
    public float baseBlueChange;
    public int baseBallChange;
    public int restTurn;

    [Header("For explain")]
    /// <summary>
    /// 得分动作的文本模板，例如 "得分 {0} 分，并额外获得 {1} 点奖励"
    /// </summary>
    string ballActionTemplate = "Throw ball to {0},";
    [SerializeField]
    private int amount1;
    [SerializeField]
    private int amount2;
    [SerializeField]
    private int amount3;
    
    string scoreActionTemplate = "Gain {1}, Rest {2}. \n{3}";
    /// <summary>
    /// 传球动作的文本模板，例如 "向{0}传球，进入{1}回合冷却"
    /// </summary>
    [SerializeField]
    private int amount4;
    [SerializeField]
    private int amount5;
    [SerializeField]
    private int amount6;

    /// <summary>
    /// 生成得分动作的最终文本
    /// </summary>
    /// <returns>格式化后的得分描述字符串</returns>

    public string returnBallAction()
    {
        return string.Format(scoreActionTemplate, amount1,amount2,amount3);
    }

    /// <summary>
    /// 生成传球动作的最终文本
    /// </summary>
    /// <returns>格式化后的传球描述字符串</returns>
    public string returnScoreAction()
    {
        return string.Format(ballActionTemplate, amount4, amount5, amount6);
    }

}
