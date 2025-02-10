using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Transform ThrowPos;
    public Transform AcceptPos;
    internal bool ifHaveBall = false;
    internal bool ifReadyToInteract = true;
    internal bool ifJustInteract = false;
    [SerializeField]
    internal int curRestTurn = -1;
    internal BallScript ball;
    internal int selfIndexInShow;

    private float ratioTimer = 1;
    private Vector3 originalScale;
    private Vector3 baseRatio;

    void Start()
    {

        
        originalScale = transform.localScale; // 记录原始缩放
        baseRatio = transform.localScale;
        
        //StartState(animalSceneState.inShop);
    }

    private void Update()
    {
        transform.localScale = baseRatio * ratioTimer;
    }

    public void ShowStart(PerformUnit controlUnit, int index)
    {
        //这里的逻辑是，这个control是通用的，并不需要performunit去进行思考行动
        //只有animalBrain需要，因为有可能要获取上一个传球的人什么的
        //所以controlUnit并没有在PerformAnimalControl里进行储存，直接传给了Brain
        animalBrain.InitBrain(controlUnit, this);
        selfIndexInShow = index;
    }

    public void DoTurnStart()
    {
        Debug.Log(name + "开始了");
        //如果有球
        if (ifHaveBall)
        {
            animalBrain.InteractWithBall();

        }
        else if (!ifReadyToInteract)
        {
            //如果无球并且在休息状态，就休息
            animalBrain.DoRest();
        }
        //否则就什么都不做
    }

    public void DoTurnEnd()
    {
        if (ifJustInteract)
        {
            FlipSprite(2, false);
            animalBrain.EnterRest();
            ifJustInteract = false;
        }
        else if (curRestTurn <= 1&& !ifReadyToInteract)
        {
            animalBrain.Recover();
        }
    }

    public void TakeBall(BallScript b)
    {
        if (ifHaveBall || (!ifReadyToInteract))
        {
            b.DoDrop();
        }
        else
        {
            ball = b;
            b.gameObject.SetActive(false);
            ifHaveBall = true;
        }
    }

    public void TakeBanana(int n)
    {
        curRestTurn = Mathf.Max(curRestTurn - n, 0);
        animalBrain.ConsumeBanana(n);
        if (curRestTurn < 1)
        {
            animalBrain.Recover();
        }
        else
        {
            ChangeRestCount(curRestTurn);
        }
    }

    public void ChangeRestCount(int num)
    {
        //TODO:实现改变休息count 的逻辑
        curRestTurn = num;
    }

    public void FlipSprite(int state, bool ifDirect)
    {
        if (flipCor != null)
            Debug.LogError("在翻面执行中时再次翻面");
        Debug.Log("触发翻面");
        flipCor = StartCoroutine(FlipSpriteCor(displaySprites[state], ifDirect));
        if (state == 2)
            renderer.color = Color.gray;
        else
            renderer.color = Color.white;
    }

    public void FlipSprite(Sprite newSpr, bool ifDirect)
    {
        if (flipCor != null)
            Debug.LogError("在翻面执行中时再次翻面");

        renderer.color = Color.white;
        flipCor = StartCoroutine(FlipSpriteCor(newSpr, ifDirect));
    }

    private IEnumerator FlipSpriteCor(Sprite toSprite, bool ifDirect)
    {
        float halfDuration = flipDuration/2; 
        
        

        if (ifDirect)
        {
            baseRatio = new Vector3(0, originalScale.y, originalScale.z);
        }
        else
        {
            float elapsedTime = 0f;
            while (elapsedTime < halfDuration)
            {
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
        while (elapsedTime < halfDuration)
        {
            float t = elapsedTime / halfDuration;
            baseRatio = Vector3.Lerp(new Vector3(0, originalScale.y, originalScale.z), originalScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        baseRatio = originalScale;
        flipCor = null;
    }
}

public abstract class AbstractSpecialAnimal: MonoBehaviour
{
    internal PerformUnit controlUnit;
    internal PerformAnimalControl animalBody;

    public int ballPassChangeIndex;
    public int restTurn;

    public void InitBrain(PerformUnit _unit, PerformAnimalControl _body)
    {
        controlUnit = _unit;
        animalBody = _body;
    }
    public virtual void InteractWithBall()
    {
        animalBody.ball.gameObject.SetActive(true);
        animalBody.ball.MoveBall(animalBody.selfIndexInShow, animalBody.selfIndexInShow+ballPassChangeIndex);
        animalBody.FlipSprite(1, false);
        animalBody.ifJustInteract = true;
        animalBody.ifHaveBall = false;
        //animalManager.Instance.changeScore(interactionYellowScore, interactionRedScore, interactionBlueScore, selfIndex);
        //TODO:分数展示和改变逻辑
        animalBody.ifReadyToInteract = false;
    }

    public virtual void DoRest()
    {
        animalBody.curRestTurn = Mathf.Max(animalBody.curRestTurn - 1, 0);
        if (animalBody.curRestTurn < 1)
        {
            Recover();
        }
        else
        {
            animalBody.ChangeRestCount(animalBody.curRestTurn);
        }
    }
    public virtual void Recover()
    {
        animalBody.FlipSprite(0, false);
        animalBody.ChangeRestCount(-1);
        animalBody.ifReadyToInteract = true;
    }

    //TODO：把ChangeRestCount变成纯粹的改变展示，把这个放到进入rest地方
    public virtual void EnterRest()
    {
        animalBody.ChangeRestCount(restTurn);
    }

    public void ConsumeBanana(int n) { }//This is only for special effect
}
