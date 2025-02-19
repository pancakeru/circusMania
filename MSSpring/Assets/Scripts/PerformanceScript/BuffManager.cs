using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum ActionTag
{
    Red, 
    Yellow, 
    Blue, 
    ExtraRed, 
    ExtraYellow, 
    ExtraBlue,
}

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;

    public PerformUnit performUnit;
    List<Buff> buffsWhenScore = new List<Buff>();
    List<Buff> buffsWhenDoSth = new List<Buff>();

    public int holdCounter; //some buff happens later (Lizard)

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        buffsWhenScore.Add(new BuffFox());
        buffsWhenScore.Add(new BuffKangaroo());
        buffsWhenScore.Add(new BuffPorcupine());

        holdCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float[] BuffInteractionWhenScore(PerformAnimalControl performAnimalControl, List<ActionTag> tags, out List<Buff> toApply)
    {
        float[] returnScore = new float[] { 0, 0, 0 };
        toApply = new List<Buff>();

        foreach (Buff buff in buffsWhenScore)
        {
            if (buff.Check(performAnimalControl, tags)) toApply.Add(buff);
        }

        return returnScore;
    }

    /*
    public float[] BuffInteractionWhenDoSth(PerformAnimalControl performAnimalControl, List<ActionTag> tags, out List<Buff> toApply)
    {

    }
    */
}

public abstract class Buff
{
    public abstract bool Check(PerformAnimalControl performAnimalControl, List<ActionTag> tags);
    public abstract float[] Apply(PerformAnimalControl performAnimalControl); 
}

public class BuffFox : Buff //when neighbours pass a ball and generate Red, +4 Red 
{
    public override bool Check(PerformAnimalControl performAnimalControl, List<ActionTag> tags)
    {
        PerformAnimalControl[] animalsOnStage = BuffManager.instance.performUnit.testAnimals;
        int myIndex = Array.IndexOf(animalsOnStage, performAnimalControl);

        if ((myIndex > 0 && animalsOnStage[myIndex - 1].animalBrain.soul.animalName == "Fox"
            || myIndex < animalsOnStage.Length - 1 && animalsOnStage[myIndex + 1].animalBrain.soul.animalName == "Fox")
            && tags.Contains(ActionTag.Red)
            && !tags.Contains(ActionTag.ExtraRed)) return true;
        return false;
    }

    public override void Apply(PerformAnimalControl performAnimalControl)
    {
        List<ActionTag> tags = new List<ActionTag> { ActionTag.ExtraRed };
        List<Buff> toApply = new List<Buff>();
        float totalScore = BuffManager.instance.BuffInteractionWhenScore(performAnimalControl, tags, out toApply);

        foreach(Buff buff in toApply)
        {
            buff.Apply(performAnimalControl);
        }
    }
}

public class BuffKangaroo : Buff //when any animal generate Red, +0.2 blue
{
    public override bool Check(PerformAnimalControl performAnimalControl, List<ActionTag> tags)
    {
        foreach (PerformAnimalControl animalOnStage in BuffManager.instance.performUnit.testAnimals)
        {
            if (animalOnStage.animalBrain.soul.animalName == "Kangaroo" && tags.Contains(ActionTag.Red)) return true;
        }
        return false;
    }

    public override float[] Apply(PerformAnimalControl performAnimalControl)
    {
        float[] returnScore = BuffManager.instance.BuffInteractionWhenScore(performAnimalControl, tags, out toApply);

        return returnScore;
    }
}

public class BuffPorcupine : Buff //When generate blue, generate 0.3 extra
{
    public override bool Check(PerformAnimalControl performAnimalControl)
    {
        if (animalProp.animalName == "Porcupine") return true;
        return false;
    }

    public override float[] Apply(PerformAnimalControl performAnimalControl)
    {
        return new float[] { 0, 0, 0 };
    }
}