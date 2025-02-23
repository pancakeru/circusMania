using UnityEngine;

[CreateAssetMenu(fileName = "NewWarmUpAnimalInfo", menuName = "Animal System/WarmUpAnimalProperty")]
public class WarmUpAnimalProperty: animalProperty
{
    [Header("For WarmUp")]
    public float warmUpScore;
    public int warmUpRequireTime;
}
