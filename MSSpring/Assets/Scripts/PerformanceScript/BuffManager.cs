using System;
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

		//buffsGiveExtraWhenScore.Add(new BuffFox());
		//buffsGiveExtraWhenScore.Add(new BuffKangaroo());
		//buffsChangeBaseWhenScore.Add(new BuffBuffalo());

		holdCounter = 0;
	}

	public void AddGiveExtraBuff(BuffGiveExtra giveExtra)
	{
		//未来可能要有buff顺序问题，需要添加时排序，现在没有。
		if(giveExtra is BuffFox)//Debug.Log("添加了狐狸buff");
		buffsGiveExtraWhenScore.Add(giveExtra);
	}

	public void AddChangeBaseBuff(BuffChangeBase changeBase)
	{
		buffsChangeBaseWhenScore.Add(changeBase);

    }

	public void RemoveGiveExtraBuff(BuffGiveExtra toRemove)
	{
		if (buffsGiveExtraWhenScore.Contains(toRemove))
			buffsGiveExtraWhenScore.Remove(toRemove);
		else
			Debug.LogWarning("Remove a GiveExtra Buff while it doesn't exist");
	}

	public void RemoveChangeBaseBuff(BuffChangeBase toRemove)
	{
		if (buffsChangeBaseWhenScore.Contains(toRemove))
			buffsChangeBaseWhenScore.Remove(toRemove);
		else
			Debug.LogError("Remove a ChangeBase Buff while it doesn't exist");
	}

	public List<float[]> BuffInteractionWhenScore(AnimalInfoPack animalInfo, List<BuffExtraMessage> messages)
	{
		List<float[]> returnScoreList = new List<float[]>();

		float[] myBaseScore = new float[] { animalInfo.redScore,
											animalInfo.yellowScore,
											animalInfo.blueScore };

		returnScoreList.Add(BuffInteractionWhenScoreChangeBase(myBaseScore, animalInfo.performAnimalControl,messages));

		foreach (BuffGiveExtra buff in buffsGiveExtraWhenScore) {
			//Debug.Log("正在检查buff"+buff.GetType().ToString());
			if (buff.Check(myBaseScore, animalInfo.performAnimalControl,messages)) {
				//Debug.Log(buff.GetType().ToString()+"应用成功");
				//Debug.Log("是soul吗？"+ animalInfo.performAnimalControl == null);
                List<float[]> toAdd = BuffInteractionWhenScore(new AnimalInfoPack(animalInfo.performAnimalControl.animalBrain.soul, animalInfo.performAnimalControl)
																{
																	redScore = buff.Apply()[0],
																	yellowScore = buff.Apply()[1],
																	blueScore = buff.Apply()[2]
																}, buff.GetMessages());
				//AnimalInfoPack's parameter has changed.
                foreach (float[] toA in toAdd)
					returnScoreList.Add(toA);
			}

		}

		return returnScoreList;
	}

	float[] BuffInteractionWhenScoreChangeBase(float[] baseScore, PerformAnimalControl performAnimalControl, List<BuffExtraMessage> messages)
	{
		//                                  {Red, Yellow, Blue}
		float[] buffScorePlus = new float[] { 0, 0, 0 };
		float[] buffScoreMult = new float[] { 1, 1, 1 };
		float[] buffScoreTotal = new float[] { 0, 0, 0 };

		foreach (BuffChangeBase buff in buffsChangeBaseWhenScore) {
			var checkResult = buff.Check(baseScore, performAnimalControl,messages);
			if (checkResult.isValid) {
				for (int i = 0; i < 3; i++) {
					if (checkResult.isMult) buffScoreMult[i] *= buff.Apply()[i];
					else buffScorePlus[i] += buff.Apply()[i];
				}
			}
		}

		for (int i = 0; i < 3; i++) {
			buffScoreTotal[i] = (baseScore[i] + buffScorePlus[i]) * buffScoreMult[i];
		}

		return buffScoreTotal;
	}
}

public abstract class BuffChangeBase
{
	protected PerformAnimalControl fromAnimal;//提交buff的animal
	public BuffChangeBase(PerformAnimalControl _from)
	{
		fromAnimal = _from;
	}
	public abstract (bool isValid, bool isMult) Check(float[] baseScore, PerformAnimalControl performAnimalControl,List<BuffExtraMessage> messages);
	public abstract float[] Apply(); //if isMult, the base return value is (1, 1, 1) since 0 erases all mult values.
}

public abstract class BuffGiveExtra
{

    protected PerformAnimalControl fromAnimal;//提交buff的animal
    public BuffGiveExtra(PerformAnimalControl _from)
    {
        fromAnimal = _from;
    }
    public abstract bool Check(float[] baseScore, PerformAnimalControl performAnimalControl, List<BuffExtraMessage> messages);
	public abstract float[] Apply(); //the return value becomes a new baseScore and runs BuffInteractionWhenScore().
	public abstract List<BuffExtraMessage> GetMessages();
}

public class BuffFox : BuffGiveExtra //when neighbours pass a ball and generate Red, +4 Red 
{
	public BuffFox(PerformAnimalControl _from) : base(_from)
	{
	}

	public override bool Check(float[] baseScore, PerformAnimalControl performAnimalControl, List<BuffExtraMessage> messages)
	{
		PerformAnimalControl[] animalsOnStage = BuffManager.instance.performUnit.GetAllAnimalsInShow(false);
		int myIndex = Array.IndexOf(animalsOnStage, performAnimalControl);
		//Debug.Log(performAnimalControl.gameObject.name+"的index是"+myIndex);
		PerformAnimalControl leftAnimal = animalsOnStage[myIndex - 1 < 0 ? 0 : myIndex - 1];
		PerformAnimalControl rightAnimal = animalsOnStage[myIndex + 1 >= animalsOnStage.Length ? animalsOnStage.Length - 1 : myIndex + 1];

        //首先检测自身index是否在范围
        //第二个检测左右是否为from animal
        /*
		Debug.Log("左右是否为提交动物: "+(((leftAnimal != null) && (leftAnimal == fromAnimal))
            || ((rightAnimal != null) && (rightAnimal == fromAnimal))));
		*/
        //if(leftAnimal != null )Debug.Log("左侧为"+leftAnimal.gameObject.name+", index为"+ (myIndex - 1 < 0 ? 0 : myIndex - 1));
        //if (rightAnimal != null) Debug.Log("右侧为" + rightAnimal.gameObject.name+", index为"+(myIndex + 1 >= animalsOnStage.Length ? animalsOnStage.Length - 1 : myIndex + 1));
        //Debug.Log("From为" + fromAnimal.gameObject.name);

        //第三个检测是否加红分
        //Debug.Log("是否为红分: " + (baseScore[0] != 0));

        //第四个检测是否为额外加分
        //Debug.Log("是否是额外分？" + !messages.Contains(BuffExtraMessage.extraRed));
        if ((myIndex >= 0) && (myIndex < animalsOnStage.Length - 1) &&
			((leftAnimal != null && leftAnimal == fromAnimal)
			|| (rightAnimal != null && rightAnimal == fromAnimal))
			&& baseScore[0] != 0&& !messages.Contains(BuffExtraMessage.extraRed))
			return true;
		return false;
	}

	public override float[] Apply()
	{
		return new float[] { 4, 0, 0 };
	}

	public override List<BuffExtraMessage> GetMessages()
	{
		return new List<BuffExtraMessage>{BuffExtraMessage.extraRed};
    }
}

public class BuffKangaroo : BuffGiveExtra //Excited(7): when any animal generate Red, +0.2 blue
{
    public BuffKangaroo(PerformAnimalControl _from) : base(_from)
    {
    }
    public override bool Check(float[] baseScore, PerformAnimalControl performAnimalControl, List<BuffExtraMessage> messages)
	{
		if (baseScore[0] > 0) {
			PerformAnimalControl[] animalsOnStage = BuffManager.instance.performUnit.GetAllAnimalsInShow(false);
			foreach (PerformAnimalControl performAnimal in animalsOnStage) {
				if (performAnimal == null)
					continue;
				AbstractSpecialAnimal performAnimalBrain = performAnimal.animalBrain;
				if (performAnimalBrain.soul.animalName == "Kangaroo") {
					if (performAnimalBrain.animalInfo.excited > 0) {
						return true;
					}
				}
			}
		}
		return false;
	}

	public override float[] Apply()
	{
		return new float[] { 0, 0, 8f };
	}

    public override List<BuffExtraMessage> GetMessages()
    {
		return new List<BuffExtraMessage>();
    }
}

public class BuffBuffalo : BuffChangeBase //When generate blue, blue +0.3
{
    public BuffBuffalo(PerformAnimalControl _from) : base(_from)
    {
    }
    public override (bool isValid, bool isMult) Check(float[] baseScore, PerformAnimalControl performAnimalControl, List<BuffExtraMessage> messages)
	{
		foreach (PerformAnimalControl animalOnStage in BuffManager.instance.performUnit.GetAllAnimalsInShow(false)) {
			if (animalOnStage != null && animalOnStage.animalBrain.soul.animalName == "Buffalo"
				&& baseScore[2] != 0)
				return (true, false);
		}
		return (false, false);
	}

	public override float[] Apply()
	{
		return new float[] { 0, 0, 5f };
	}
}

public enum BuffExtraMessage
{
	extraRed,
	extraYellow,
	extraBlue
}