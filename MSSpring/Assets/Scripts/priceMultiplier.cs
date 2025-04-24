using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPriceMultiplier", menuName = "Game Data/Price Multiplier")]
public class priceMultiplier : ScriptableObject
{
    public List<float> multipliers = new List<float>();
}
