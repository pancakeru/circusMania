using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;

    public PerformUnit performUnit;
    List<BuffChangeBase> buffsChangeBaseWhenScore = new List<BuffChangeBase>();
    List<BuffGiveExtra> buffsGiveExtraWhenScore = new List<BuffGiveExtra>();

    public int holdCounter; //some buff happens later (Lizard)

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        buffsGiveExtraWhenScore.Add(new BuffFox());
        buffsChangeBaseWhenScore.Add(new BuffPorcupine());

        holdCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<float[]> BuffInteractionWhenScore(PerformAnimalControl performAnimalControl)
    {
        List<float[]> returnScoreList = new List<float[]>();

        float[] myBaseScore = new float[] { performAnimalControl.animalBrain.soul.baseRedChange,
                                            performAnimalControl.animalBrain.soul.baseYellowChange,
                                            performAnimalControl.animalBrain.soul.baseBlueChange };

        returnScoreList.Add(BuffInteractionWhenScoreChangeBase(myBaseScore, performAnimalControl));

        foreach (BuffGiveExtra buff in buffsGiveExtraWhenScore)
        {
            if (buff.Check(performAnimalControl))
            {
                returnScoreList.Add(BuffInteractionWhenScoreChangeBase(buff.Apply(), performAnimalControl));
            }
                
        }

        return returnScoreList;
    }

    float[] BuffInteractionWhenScoreChangeBase(float[] baseScore, PerformAnimalControl performAnimalControl)
    {
        //                                  {Red, Yellow, Blue}
        float[] buffScorePlus = new float[] { 0, 0, 0 };
        float[] buffScoreMult = new float[] { 1, 1, 1 };
        float[] buffScoreTotal = new float[] { 0, 0, 0 };

        foreach (BuffChangeBase buff in buffsChangeBaseWhenScore)
        {
            var checkResult = buff.Check(performAnimalControl);
            if (checkResult.isValid)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (checkResult.isMult) buffScoreMult[i] *= buff.Apply()[i];
                    else buffScorePlus[i] += buff.Apply()[i];
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            buffScoreTotal[i] = (baseScore[i] + buffScorePlus[i]) * buffScoreMult[i];
        }

        return buffScoreTotal;
    }
}

public abstract class BuffChangeBase
{
    public abstract (bool isValid, bool isMult) Check(PerformAnimalControl performAnimalControl);
    public abstract float[] Apply(); //if isMult, the base return value is (1, 1, 1) since 0 erases all mult values.
}

public abstract class BuffGiveExtra
{
    public abstract bool Check(PerformAnimalControl performAnimalControl);
    public abstract float[] Apply(); //the return value becomes a new baseScore and runs BuffInteractionWhenScore().
}

public class BuffFox : BuffGiveExtra //when neighbours pass a ball and generate Red, +4 Red 
{
    public override bool Check(PerformAnimalControl performAnimalControl)
    {
        PerformAnimalControl[] animalsOnStage = BuffManager.instance.performUnit.testAnimals;
        int myIndex = Array.IndexOf(animalsOnStage, performAnimalControl);

        if ((myIndex > 0 && animalsOnStage[myIndex - 1].animalBrain.soul.animalName == "Fox"
            || myIndex < animalsOnStage.Length - 1 && animalsOnStage[myIndex + 1].animalBrain.soul.animalName == "Fox")
            && performAnimalControl.animalBrain.soul.baseRedChange != 0)
            return true;
        return false;
    }

    public override float[] Apply()
    {
        return new float[] { 4, 0, 0 };
    }
}

/*
public class BuffKangaroo : BuffChangeBase //when any animal generate Red, +0.2 blue
{
    public override (bool isValid, bool isMult) Check(PerformAnimalControl performAnimalControl)
    {
        foreach (PerformAnimalControl animalOnStage in BuffManager.instance.performUnit.testAnimals)
        {
            if (animalOnStage.animalBrain.soul.animalName == "Kangaroo" 
                && performAnimalControl.animalBrain.soul.baseRedChange != 0) 
                return (true, false);
        }
        return (false, false);
    }

    public override float[] Apply()
    {
        return new float[] { 0, 0, 0.2f };
    }
}
*/

public class BuffPorcupine : BuffChangeBase //When generate blue, blue +0.3
{
    public override (bool isValid, bool isMult) Check(PerformAnimalControl performAnimalControl)
    {
        foreach (PerformAnimalControl animalOnStage in BuffManager.instance.performUnit.testAnimals)
        {
            if (animalOnStage.animalBrain.soul.animalName == "Porcupine" 
                && animalOnStage.animalBrain.soul.baseBlueChange != 0) 
                return (true, false);
        }
        return (false, false);
    }

    public override float[] Apply()
    {
        return new float[] { 0, 0, 0.3f };
    }
}