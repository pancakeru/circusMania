using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

public class PerformAnimalControl : MonoBehaviour
{
	//public enum ShowState { Charge, Actioned, Rest}

	public AbstractSpecialAnimal animalBrain;

	[Header("Flip Related")]
	[SerializeField]
	private SpriteRenderer renderer;
	public List<Sprite> displaySprites;
	public float flipDuration = 0.5f;
	private float elapsedTime = 0; // 累计时间
	private Coroutine flipCor;

	[Header("Performance Related")]
	private bool ifEnSouled = false;
	public Transform ThrowPos;
	public Transform AcceptPos;
	public TextMeshPro restText;
	internal bool ifHaveBall = false;
	internal bool ifReadyToInteract = true;
	internal bool ifJustInteract = false;
	internal int curRestTurn = -1;
	internal BallScript ball;
	internal int selfIndexInShow;
	internal bool ifInRest = false;//这个是为了防止投球时吃香蕉，只有inrest才吃

	[Header("For Score Effect")]
	public ScoreTextEffectGenerator generator;

	private float ratioTimer = 1;
	private Vector3 originalScale;
	private Vector3 baseRatio;

	[Header("MechanicNumber")]
    public GameObject mechanicNumberUIPrefab;
    public MechanicNumberController mechanicNumberUI;

    void Start()
	{


		originalScale = renderer.transform.localScale; // 记录原始缩放
		baseRatio = renderer.transform.localScale;

		//StartState(animalSceneState.inShop);

    }

	private void Update()
	{
		renderer.transform.localScale = baseRatio * ratioTimer;
	}

	public void EnSoul(animalProperty soulProperty)
	{
		ifEnSouled = true;
		animalBrain.EnSoul(soulProperty);
		animalBrain.InitOnExcitementEventListener();
	}

	public void ShowStart(PerformUnit controlUnit, int index)
	{


		//这里的逻辑是，这个control是通用的，并不需要performunit去进行思考行动
		//只有animalBrain需要，因为有可能要获取上一个传球的人什么的
		//所以controlUnit并没有在PerformAnimalControl里进行储存，直接传给了Brain
		animalBrain.InitBrain(controlUnit, this);
		selfIndexInShow = index;

        GameObject myMechanicNumberUI = Instantiate(mechanicNumberUIPrefab, transform.position + new Vector3(0, -2f, 0), Quaternion.identity);
        mechanicNumberUI = myMechanicNumberUI.GetComponent<MechanicNumberController>();
        mechanicNumberUI.myAnimalBrain = animalBrain;
        mechanicNumberUI.Begin();

        if (!ifEnSouled)
			Debug.LogError("Remember to do EnSoul to each animal in show");
	}

	public void DoTurnStart()
	{
		Debug.Log(name + "开始了");
		//如果有球
		if (ifHaveBall) {
            animalBrain.InteractWithBall();

        } else if (!ifReadyToInteract) {
			//如果无球并且在休息状态，就休息
			animalBrain.DoRest();
        }
        mechanicNumberUI.UpdateText();
        //否则就什么都不做
    }

	public void DoTurnEnd()
	{
		Debug.Log("回合结束行动");
		if (ifJustInteract) {

			animalBrain.EnterRest(false);
			ifJustInteract = false;
		} else if (curRestTurn <= 1 && !ifReadyToInteract) {
			animalBrain.Recover();
		}
		animalBrain.TurnEndAction();
	}

	public void TakeBall(BallScript b)
	{
		if (ifHaveBall) {
			b.DoDrop();
		} else
		{
            Debug.Log("拿到球");
            if (!ifReadyToInteract)
			{
				if (ifJustInteract)
					animalBrain.EnterRest(true);
                int realTake = animalBrain.controlUnit.thrower.takeBanana(curRestTurn);
                TakeBanana(realTake);
            }
            ball = b;
            b.gameObject.SetActive(false);

            if (ball.GetPasser() != null)
            {
                if (ball.GetPasser().animalBrain.soul.name == "Goat")
                {
                    if (animalBrain.animalInfo.power > 1)
                    {
                        int powerDifference = animalBrain.animalInfo.power - 1;
                        animalBrain.animalInfo.power = 1;
                        for (int i = 0; i < powerDifference; i++)
                        {
                            animalBrain.Scoring(new float[] { 0, 0, 0.5f });
                        }
                    }
                }
            }
			ifJustInteract = false;
            ifHaveBall = true;
        }
	}

	public void TakeBanana(int n)
	{

		curRestTurn = Mathf.Max(curRestTurn - n, 0);
		animalBrain.ConsumeBanana(n);
		if (ifInRest) {
			if (curRestTurn < 1) {
				animalBrain.Recover();
			} else {
				ChangeRestCount(curRestTurn);
			}
		}
	}

	public void ChangeRestCount(int num)
	{
		//TODO:实现改变休息count 的逻辑
		curRestTurn = num;
		if (curRestTurn > 0) {
			restText.text = curRestTurn.ToString();
			if (!restText.gameObject.activeInHierarchy)
				restText.gameObject.SetActive(true);

		} else {
			restText.gameObject.SetActive(false);
		}
	}

	public void FlipSprite(int state, bool ifDirect, Action doInFlip = null)
	{
		if (flipCor != null) {
			Debug.Log("在翻面执行中时再次翻面");
			StopCoroutine(flipCor);
			//return;
		}

		Debug.Log("触发翻面");
		flipCor = StartCoroutine(FlipSpriteCor(displaySprites[state], ifDirect, doInFlip));
		if (state == 2)
			renderer.color = Color.gray;
		else
			renderer.color = Color.white;
	}

	public void FlipSprite(Sprite newSpr, bool ifDirect, Action doInFlip = null)
	{
		if (flipCor != null)
			Debug.LogError("在翻面执行中时再次翻面");

		renderer.color = Color.white;
		flipCor = StartCoroutine(FlipSpriteCor(newSpr, ifDirect, doInFlip));
	}

	private IEnumerator FlipSpriteCor(Sprite toSprite, bool ifDirect, Action doInFlip)
	{
		float halfDuration = flipDuration / 2;



		if (ifDirect) {
			baseRatio = new Vector3(0, originalScale.y, originalScale.z);
		} else {
			float elapsedTime = 0f;
			while (elapsedTime < halfDuration) {
				float t = elapsedTime / halfDuration;
				baseRatio = Vector3.Lerp(originalScale, new Vector3(0, originalScale.y, originalScale.z), t);
				elapsedTime += Time.deltaTime;
				yield return null;
			}
		}

		baseRatio = new Vector3(0, originalScale.y, originalScale.z);

		// 这里假设有 renderer 和 displaySprites 来支持翻转
		renderer.sprite = toSprite;

		elapsedTime = 0f;
		while (elapsedTime < halfDuration) {
			float t = elapsedTime / halfDuration;
			baseRatio = Vector3.Lerp(new Vector3(0, originalScale.y, originalScale.z), originalScale, t);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		baseRatio = originalScale;
		doInFlip?.Invoke();
		flipCor = null;
	}

	public void BackToInitial()
	{
		Debug.Log(name+"正在back to initial");
		ifInRest = false;
		ifJustInteract = false;
		FlipSprite(0, false);
		ChangeRestCount(-1);
		ifReadyToInteract = true;
		animalBrain.ResetWhenBackToInitial();
	}
}

public abstract class AbstractSpecialAnimal : MonoBehaviour
{
	//记录一些动物的具体数据
	internal animalProperty soul;
	//记录演出的全局管理
	internal PerformUnit controlUnit;
	//记录动物的执行代码
	internal PerformAnimalControl animalBody;

	[Header("For Test")]
	public animalProperty testProperty;

	[HideInInspector] public AnimalInfoPack animalInfo;

	public void InitBrain(PerformUnit _unit, PerformAnimalControl _body)
	{
		controlUnit = _unit;
		animalBody = _body;

		if (soul == null)
			_body.EnSoul(testProperty);

        DoWhenShowStart();
    }

	public void EnSoul(animalProperty newSoul)
	{
		soul = newSoul;
        animalInfo = new AnimalInfoPack(soul);
    }

	public virtual void DoWhenShowStart()
	{

	}

	public virtual void InteractWithBall()
	{
		animalBody.ball.gameObject.SetActive(true);
		animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow + soul.baseBallChange);
		animalBody.FlipSprite(1, false);
		animalBody.ifJustInteract = true;
		animalBody.ifHaveBall = false;

		GenerateScore(animalInfo);

        controlUnit.InvokeOnExcitementEvent(animalInfo);

		animalBody.ifReadyToInteract = false;
	}

	internal void Scoring(float[] inputScore)
	{
		if (inputScore[0] != 0) {
			controlUnit.ChangeRedScore(inputScore[0]);
			animalBody.generator.RequestTextEffect(inputScore[0], ScoreTextEffectGenerator.ScoreType.Red);
		}
		if (inputScore[1] != 0) {
			controlUnit.ChangeYellowScore(inputScore[1]);
			animalBody.generator.RequestTextEffect(inputScore[1], ScoreTextEffectGenerator.ScoreType.Yellow);
		}
		if (inputScore[2] != 0) {
			controlUnit.ChangeBlueScore(inputScore[2]);
			animalBody.generator.RequestTextEffect(inputScore[2], ScoreTextEffectGenerator.ScoreType.Blue);
		}
	}

	public virtual void DoRest()
	{
		animalBody.curRestTurn = Mathf.Max(animalBody.curRestTurn - 1, 0);
		if (animalBody.curRestTurn < 1) {
			Recover();
		} else {
			animalBody.ChangeRestCount(animalBody.curRestTurn);
		}
	}
	public virtual void Recover()
	{
		animalBody.ifInRest = false;
		animalBody.FlipSprite(0, false);
		animalBody.ChangeRestCount(-1);
		animalBody.ifReadyToInteract = true;
	}

	public virtual void TurnEndAction()
	{

	}

	public virtual void EnterRest(bool ifDirect)
	{
		animalBody.ifInRest = true;
		if (!ifDirect)
			animalBody.FlipSprite(2, false, () => { animalBody.ChangeRestCount(soul.restTurn); });
		else
			animalBody.curRestTurn = soul.restTurn;
	}

	public void ConsumeBanana(int n) { }//This is only for special effect

	public virtual void InitOnExcitementEventListener() { }

	public void GenerateScore(AnimalInfoPack animalInfo)
	{
		List<float[]> scoresAfterBuff = BuffManager.instance.BuffInteractionWhenScore(animalInfo);
		foreach (float[] inputScore in scoresAfterBuff) {
			Scoring(inputScore);
		}
	}

	public virtual void ResetWhenBackToInitial()
	{

	}
}