using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;

    public PerformUnit performUnit;
    List<Buff> buffs = new List<Buff>();

    public int holdCounter; //some buff happens later (Lizard)

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        buffs.Add(new BuffBear());
        buffs.Add(new BuffLion());
        buffs.Add(new BuffFox());
        buffs.Add(new BuffBuffalo());
        buffs.Add(new BuffKangaroo());
        buffs.Add(new BuffPorcupine());
        buffs.Add(new BuffLizard());
        buffs.Add(new BuffRhino());

        holdCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Buff> BuffOnInteractWithBall(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        List<Buff> validBuffs = new List<Buff>();

        foreach (Buff buff in buffs)
        {
            if (buff.Check(animalProp, performAnimalControl)) validBuffs.Add(buff);
        }

        return validBuffs;
    }
}

public abstract class Buff
{
    public abstract bool Check(animalProperty animalProp, PerformAnimalControl performAnimalControl);
    public abstract float[] Apply(animalProperty animalProp, PerformAnimalControl performAnimalControl); //float[] {Red, Yellow, Blue}
}

public class BuffBear : Buff
{
    public override bool Check(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        if (animalProp.animalName == "Bear") return true;
        return false;
    }

    public override float[] Apply(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        animalProp.power += 1;

        return new float[] { 0, 0, 0 };
    }
}

public class BuffLion : Buff
{
    public override bool Check(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        if (animalProp.animalName == "Lion" && animalProp.warmup == 3) return true;
        return false;
    }

    public override float[] Apply(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        animalProp.power += 1;

        return new float[] { 0, 5, 0 };
    }
}

public class BuffFox : Buff
{
    public override bool Check(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        PerformAnimalControl[] animalsOnStage = BuffManager.instance.performUnit.testAnimals;
        int myIndex = Array.IndexOf(animalsOnStage, performAnimalControl);

        if ((myIndex > 0 && animalsOnStage[myIndex - 1].animalBrain.soul.animalName == "Fox"
            || myIndex < animalsOnStage.Length - 1 && animalsOnStage[myIndex + 1].animalBrain.soul.animalName == "Fox")
            && animalProp.baseRedChange > 0) return true;
        return false;
    }

    public override float[] Apply(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        animalProp.power += 1;

        return new float[] { 4, 0, 0 };
    }
}

public class BuffBuffalo : Buff //Need Update
{
    public override bool Check(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        if (animalProp.animalName == "Buffalo" && animalProp.excited >= 7) return true;
        return false;
    }

    public override float[] Apply(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        //What does "Gain 2 more times" mean? 
        return new float[] { 0, 0, 0 };
    }
}

public class BuffKangaroo : Buff
{
    public override bool Check(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        foreach (PerformAnimalControl animalOnStage in BuffManager.instance.performUnit.testAnimals)
        {
            if (animalOnStage.animalBrain.soul.animalName == "Kangaroo" && animalOnStage.animalBrain.soul.excited >= 7) return true;
        }
        return false;
    }

    public override float[] Apply(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        //What does "Gain 2 more times" mean? 
        return new float[] { 0, 0, 0.2f };
    }
}

public class BuffPorcupine : Buff //Need Update
{
    public override bool Check(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        if (animalProp.animalName == "Porcupine") return true;
        return false;
    }

    public override float[] Apply(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        return new float[] { 0, 0, 0 };
    }
}

public class BuffLizard : Buff
{
    public override bool Check(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        if (animalProp.animalName == "Lizard" && BuffManager.instance.holdCounter == 0)
        {
            BuffManager.instance.holdCounter = 1;
        }
        return true;
    }

    public override float[] Apply(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        int powerGot = animalProp.power - 1;
        if (powerGot < 0) powerGot = 0;
        animalProp.power = 1;

        if (BuffManager.instance.holdCounter == 0) return new float[] { 0, 0, 0.5f * powerGot };
        BuffManager.instance.holdCounter--;
        return new float[] { 0, 0, 0 };
    }
}

public class BuffRhino : Buff
{
    public override bool Check(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        if (animalProp.animalName == "Rhino") return true;
        return false;
    }

    public override float[] Apply(animalProperty animalProp, PerformAnimalControl performAnimalControl)
    {
        animalProp.power += 1;

        return new float[] { 0, 0, 0 };
    }
}