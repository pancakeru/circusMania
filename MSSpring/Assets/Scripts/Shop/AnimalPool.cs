using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAnimalPool", menuName = "Animal System/Animal Pool")]
public class AnimalPool : ScriptableObject
{
    [Serializable]
    public class AnimalEntry
    {
        public int count; // 该动物在池子中的数量
        public animalProperty animalProperty; // 动物的属性
    }

    public List<AnimalEntry> animals = new List<AnimalEntry>();

    
}