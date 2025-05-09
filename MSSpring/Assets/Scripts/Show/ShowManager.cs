using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class ShowManager : MonoBehaviour, IReportReceiver
{
	public static ShowManager instance;

	//State Machine
	private enum ShowStates
	{
		SelectAnimal,
		Animation,
		Performance,
		EndCheck
	}


	public enum DecideScreenState
	{
		empty,
		slide,
		choose,
		moveAnimal,
		tutorial
	}

	#region 从外部获取的变量
	[Header("手中动物的起始x")]
	[SerializeField] private float xStart;
	[Header("手中动物的offset")]
	[SerializeField] private float offset;
	[Header("检测为手牌区的下方比例")]
	[SerializeField] private float downRatio = 0.2f;

	[Header("预制体")]
	public GameObject animalIcon;
	public GameObject areaPrefab;

	[Header("动物的预制体")]
	public List<GameObject> animalPerformancePrefabs;

	[Header("获取组件")]
	[SerializeField] private GraphicRaycaster uiRaycaster;
	[SerializeField] private Transform handPanelTransform;
	[SerializeField] private Transform handPanelBackground;
	[SerializeField] private Transform stagePanelTransform;
	[SerializeField] private UiMover scorePanelMover;
	public PerformUnit totalPerformanceControl;
	[SerializeField] private targetPanelManager targetDisplayManager;
	[SerializeField] private UiMover showBanana;
	[SerializeField] private UiMover startButtonMover;
	[SerializeField] private UiMover bananaUiMover;
	[SerializeField] private BananaThrower thrower;
	[SerializeField] private UiMover targetPanelMover;
	[SerializeField] private GameObject turnRelatedGMO;
	public GameObject pauseShow;
	#endregion

	#region 记录变量
	//State变量
	private ShowStates currentState;
	[HideInInspector] public LevelProperty curLevel;
	private float curScore;
	[HideInInspector] public int curTurn;
	private float curRepu;
	private bool inDown = false;
	private int moveFromStageIndex;
	private GameObject holdingAnimalObj;
	private GameObject firstDetect;
	private Vector2 lastMousePosition;
	private EventSystem eventSystem;
	private CameraMover camMover;
	private int soundIndex;
	private MenuController menu;
	private ShowScoreManager scoreManager;
	public bool ifToShow { get; private set; } = false;
	private TutorialRelatedFunctionContainer tContainer;
	private ShowInteractionStateRecorder interContainer;
	private bool ifExplainEnabled = true;

	//icon的开始y
	private float yStart;
	//移动时改变的量，加上initial pos就是结果
	private float leftAnchorX;

	//private bool enterInteraction = false;
	//private bool canBeMovedOrSelected = true;
	//private bool sliding = false;
	//public bool holding = false; zoe的
	//public bool stopMoving = false; zoe的
	//private List<animalProperty> testList;
	#endregion

	#region 每次表演的记录变量
	private List<GameObject> myHand;//需要重置
	public GameObject[] onStage { get; private set; }//需要重置
	private List<Vector2> initialPos = new List<Vector2>();//需要重置
	private List<iconAnimal> myHandControls = new List<iconAnimal>();//需要重置
	private areaReport[] posRecord;//需要重置
	private BiDictionary<iconAnimal, GameObject> iconToOnStage = new BiDictionary<iconAnimal, GameObject>();//需要重置
	#endregion

	#region 表演的设置变量
	[Header("表演设置")]
	[SerializeField] private float repuRatio = 0.1f;
	private int speedRatio = 1;
	#endregion

	#region 选人界面和表演界面切换用到的变量
	private UiMover handPanelMover;
	private UiMover stagePanelMover;
	[Header("舞台切换")]
	[SerializeField]
	private RectTransform handPanelUpPos;
	[SerializeField]
	private RectTransform handPanelDownPos;
	[SerializeField]
	private RectTransform stagePanelDownPos;
	[SerializeField]
	private RectTransform stagePanelUpPos;
	[SerializeField]
	private RectTransform scorePanelUpPos;
	[SerializeField]
	private RectTransform scorePanelDownPos;
	[SerializeField]
	private Transform CamInDecition;
	[SerializeField]
	private Transform CamInShow;
	[SerializeField] private RectTransform StartButtonDown;
	[SerializeField] private RectTransform StartButtonUp;
	[SerializeField] private float DecideCamScale;
	[SerializeField] private float ShowCamScale;
	[SerializeField] private RectTransform BanannaInDecision;
	[SerializeField] private RectTransform BanannaInShow;
	[SerializeField] private RectTransform ShowBanannaInDecision;
	[SerializeField] private RectTransform ShowBanannaInShow;
	#endregion

	#region EndScreen 相关变量
	[Header("结算界面")]
	[SerializeField] private Transform canvasTrans;
	[SerializeField] private GameObject EndScreenPrefab;
	[SerializeField] private RectTransform endScreenUpPos;
	[SerializeField] private RectTransform endScreenDownPos;
	[SerializeField] private tempBlackManager blacker;
	private GameObject curEndScreen;
	#endregion

	#region 动物解释相关
	[Header("动物解释相关")]
	//private tempShowExplain explainer;
	[SerializeField] private GameObject explainingCardPrefab;
	ExplainingCardController myExplainingCard;
	#endregion

	#region 测试相关变量
	[Header("For test")]
	//[SerializeField]
	private bool ifTest;
	//[SerializeField]
	private RectTransform tarTrans;
	//[SerializeField]
	private UiMover mover;
	[SerializeField] private LevelProperty testLevel;
	//public animalProperty testProperty;

	[Header("测试替换")]
	[SerializeField] private AnimalStart switchhand;
	[Header("测试添加")]
	[SerializeField] private animalProperty addhand;
	#endregion

	[Header("Tutorial")]
	[SerializeField] private GameObject regularTutorial;
	[SerializeField] private ShowTutorialManager showTutorialManager;

	[Header("Others")]
	[SerializeField] Image speedUpButton;
    public ShowStressManager showStressManager;

    //这个类的作用是方便管理一些功能的开关
    private class TutorialRelatedFunctionContainer
	{
		private ShowManager father;

		public bool isTutorial = false;

		public TutorialRelatedFunctionContainer(ShowManager manager)
		{
			father = manager;
		}

		public bool ifBananaEnabled = true;
		public bool ifTurnEnabled = true;
		public bool ifChangePrice = true;
		public bool ifUpdatePopularity = true;


		private int tempCountOfMover = 0;

		private bool ifNewHand = false;
		private List<animalProperty> hand;

		public void DoBananaMoverAction(UiMover mover, Vector2 tarPos, int triggerindex = 0)
		{
			if (!ifBananaEnabled)
				return;
			tempCountOfMover += 1;
			mover.MoveTo(tarPos, triggerindex);
		}

		public int TakeCount()
		{
			int realCount = tempCountOfMover;
			tempCountOfMover = 0;
			return realCount;
		}

		public void ChangeBananaDuringShow(int toAdd)
		{
			if (!ifBananaEnabled)
				return;
			father.thrower.addBanana(toAdd);
		}

		public void DoTurnAddAction()
		{
			if (ifTurnEnabled)
				father.curTurn += 1;
		}

		public void ChangeHand(bool ifNewHand, List<animalProperty> newHand)
		{
			this.ifNewHand = ifNewHand;
			hand = newHand;
		}

		public List<animalProperty> GetCurrentHand()
		{
			return ifNewHand ? hand : GlobalManager.instance.getAllAnimals();
		}

		public void EndTurnScoreChange()
		{
			if (ifUpdatePopularity)
				father.scoreManager.EndTurn();
		}

	}

	private void Awake()
	{
		if (instance == null) instance = this;
		else Destroy(gameObject);

		menu = FindAnyObjectByType<MenuController>();
		handPanelMover = handPanelTransform.GetComponent<UiMover>();
		stagePanelMover = stagePanelTransform.GetComponent<UiMover>();
		camMover = Camera.main.GetComponent<CameraMover>();
		//GlobalManager_OnNextGlobalLevel();
		GlobalManager.OnNextGlobalLevel += GlobalManager_OnNextGlobalLevel;
		scoreManager = GetComponent<ShowScoreManager>();
		CanvasMain.OnUIInteractionEnabled += EnableCanvas;
		CanvasMain.OnUIInteractionDisabled += DisableCanvas;
		Camera.main.GetComponent<moveReporter>().reportReceiver = gameObject;
		pauseShow.SetActive(false);

		myExplainingCard = Instantiate(explainingCardPrefab, canvasTrans).GetComponent<ExplainingCardController>();
		CanvasMain.instance.ShowPopUp(targetDisplayManager.popularityGroup.GetComponent<Image>(), "You need a positive Popularity score to pass the level.", targetDisplayManager.transform.parent.GetComponent<Canvas>().GetComponent<GraphicRaycaster>());
	}

	void Start()
	{

		//SetUpAnLevel(testLevel);
		//testList = new List<animalProperty>();


		//GlobalManager做完后把这个搬到 SelectAnimal
		//取animalProperty list 的 animalName
		/*
        for (int i = 0; i < 12; i++) {
            GameObject temp = Instantiate(animalIcon, canvasTransform);
            myHand.Add(temp);
           // Debug.Log($"Added object to myHand. Current count: {myHand.Count}");
            temp.GetComponent<iconAnimal>().Initialize(testProperty, false);
            temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(x + offset*i, yStart);
            temp.GetComponent<iconAnimal>().myIndex = i;
            //TODO:把这个目标位置整合
            initialPos.Add(new Vector2(x + offset * i, -350));
            myHandControls.Add(temp.GetComponent<iconAnimal>());
        }*/
	}

	#region 调整show设置的函数
	private SceneSetUpInfoContainer infoContainer = new SceneSetUpInfoContainer();

	void SetShowPositionNum(int n)
	{
		infoContainer.SetPosNum(n);
	}

	void SetShowPositionNumDuringShow(int n)
	{
		foreach (GameObject an in onStage)
		{
			if (an != null)
				SetUnSelectIconInHand(an);
		}
		foreach (areaReport report in posRecord)
			if (report != null) Destroy(report.gameObject);
		yStart = -600;
		infoContainer.SetPosNum(n);
		onStage = new GameObject[6];
		posRecord = new areaReport[6];
		iconToOnStage = new BiDictionary<iconAnimal, GameObject>();
		totalPerformanceControl.InitShow(infoContainer.posNum, Enumerable.Range(0, 6)
							 .Select(i => infoContainer.GetEmptyPosLocalX(i))
							 .ToArray(), Enumerable.Range(0, 6)
							 .Select(i => infoContainer.GetStageLocalX(i))
							 .ToArray());
		for (int i = 0; i < infoContainer.posNum; i++)
		{
			GameObject temp = Instantiate(areaPrefab, stagePanelTransform);
			temp.GetComponent<areaReport>().spotNum = i;
			temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-5 + infoContainer.areaOffset * i + infoContainer.centerOffset, 0);
			posRecord[i] = temp.GetComponent<areaReport>();
		}

	}

	void SetScoreEnableState(bool ifRed, bool ifYellow, bool ifBlue, bool ifPopularity)
	{
		//首先是选人界面的不显示
		targetDisplayManager.ChangeLevelTargetUiDisplayStatus(ifRed, ifYellow, ifBlue, ifPopularity);

		//还要在结算禁用，这里先不搞了。
	}

	void SetBananaEnableState(bool ifBanana)
	{
		//首先依然是ui禁用
		showBanana.gameObject.SetActive(ifBanana);
		bananaUiMover.gameObject.SetActive(ifBanana);
		tContainer.ifBananaEnabled = ifBanana;
	}

	void SetTurnEnableState(bool ifTurn)
	{
		//禁用ui
		turnRelatedGMO.SetActive(ifTurn);
		//设置逻辑
		tContainer.ifTurnEnabled = ifTurn;
	}

	public bool IfChangeReputation()
	{
		return tContainer.ifUpdatePopularity;
	}

	public bool GetIfBananaEnabled()
	{
		return tContainer.ifBananaEnabled;
	}

	void SetHandAnimal(bool ifNewHand, List<animalProperty> newHand)
	{
		tContainer.ChangeHand(ifNewHand, newHand);
	}

	void SetAndInitializeHandAnimal(bool ifNewHand, List<animalProperty> newHand)
	{
		SetHandAnimal(ifNewHand, newHand);
		foreach (GameObject an in onStage)
		{
			if (an != null)
				SetUnSelectIconInHand(an);
		}
		foreach (GameObject handIcon in myHand)
		{
			Destroy(handIcon);
		}
		onStage = new GameObject[6];
		iconToOnStage = new BiDictionary<iconAnimal, GameObject>();
		myHandControls = new List<iconAnimal>();
		myHand = new List<GameObject>();
		initialPos = new List<Vector2>();
		InitializeHand(tContainer.GetCurrentHand());
	}

	public void SetIfChangeTroupePrice(bool ifChange)
	{
		tContainer.ifChangePrice = ifChange;
	}

	void SetIfUpdatePopularity(bool ifPopularity)
	{
		tContainer.ifUpdatePopularity = ifPopularity;
	}

	void SetDicideStateInteractionEnabled(bool ifCanInteract)
	{
		StartDecideState(ifCanInteract ? DecideScreenState.empty : DecideScreenState.tutorial);
	}

	public void AddAnimalToHand(animalProperty animal)
	{
		// 检查是否已经存在这个动物（按名字）
		bool alreadyExists = false;

		foreach (iconAnimal icon in myHandControls)
		{
			if (icon.selfProperty.animalName == animal.animalName)
			{
				icon.AddNum(1);
				alreadyExists = true;
				break;
			}
		}

		if (alreadyExists)
		{
			return; // ✅ 已经有这个动物，只是更新数量即可
		}

		// ✅ 否则是一个新动物：创建新 icon
		GameObject temp = Instantiate(animalIcon, handPanelTransform.position, Quaternion.identity, handPanelBackground);
		myHand.Add(temp);

		int index = myHand.Count - 1;
		iconAnimal iconComp = temp.GetComponent<iconAnimal>();
		iconComp.Initialize(animal, 1, false);
		iconComp.myIndex = index;

		RectTransform rt = temp.GetComponentInChildren<RectTransform>();
		rt.anchoredPosition = new Vector2(xStart + offset * index, yStart);

		initialPos.Add(new Vector2(xStart + offset * index, iconComp.yGoal));
		myHandControls.Add(iconComp);
	}

	public void BanAllInteraction() => interContainer.DisableAll();
	public void EnableAllInteraction() => interContainer.EnableAll();
	public void SetSlideAnimalEnableState(bool ifEnable) => interContainer.ifSlide = ifEnable;
	public void SetSelectAnimalInDownEnableState(bool ifEnable) => interContainer.ifSelectDown = ifEnable;
	public void SetSelectAnimalInUpEnableState(bool ifEnable) => interContainer.ifSelectUp = ifEnable;

	void SwitchExplainEnableState(bool enable) => ifExplainEnabled = enable;

	#endregion

	#region Fuctions
	public void EnterOneShow(bool isTutorial = false)
	{
		if (!isTutorial)
		{
			//Debug.Log("我是SHowManager，我是的obj是" + gameObject.name);
			//x = -750;
			//offset = 300;
			yStart = -600;
			tContainer = new TutorialRelatedFunctionContainer(this);
			interContainer = new ShowInteractionStateRecorder();
			//areaOffset = 2;
			//SetShowPositionNum(3);
			//SetScoreEnableState(true, false, false, false);
			//SetBananaEnableState(false);
			//SetTurnEnableState(false);
			//SetHandAnimal(false, new List<animalProperty>(switchhand.properies));
			//SetIfChangeTroupePrice(false);
			//SetIfUpdatePopularity(false);
			//BanAllInteraction();
			SetSelectAnimalInDownEnableState(true);
			curTurn = 1;
			blacker.Initial();
			onStage = new GameObject[6];
			posRecord = new areaReport[6];
			iconToOnStage = new BiDictionary<iconAnimal, GameObject>();
			myHandControls = new List<iconAnimal>();
			myHand = new List<GameObject>();
			initialPos = new List<Vector2>();
			SetUpAnLevel(GlobalManager.instance.GetCurrentGlobalLevel().levelProperty);
			totalPerformanceControl.InitShow(infoContainer.posNum, Enumerable.Range(0, 6)
							 .Select(i => infoContainer.GetEmptyPosLocalX(i))
							 .ToArray(), Enumerable.Range(0, 6)
							 .Select(i => infoContainer.GetStageLocalX(i))
							 .ToArray());
			//tContainer.ChangeBananaDuringShow(10);
			//是要设置香蕉
			thrower.changeCount(10);
			//位置 GameObject


			for (int i = 0; i < infoContainer.posNum; i++)
			{
				GameObject temp = Instantiate(areaPrefab, stagePanelTransform);
				temp.GetComponent<areaReport>().spotNum = i;
				temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-5 + infoContainer.areaOffset * i + infoContainer.centerOffset, 0);
				posRecord[i] = temp.GetComponent<areaReport>();
			}
			InitializeHand(tContainer.GetCurrentHand());
			currentState = ShowStates.SelectAnimal;
			stagePanelMover.gameObject.SetActive(true);
			//TODO:改成到地方再可以交互
			startButtonMover.GetComponent<Button>().interactable = true;
			camMover.SetTo(CamInDecition.position, DecideCamScale);

			regularTutorial.SetActive(true);
			showTutorialManager.gameObject.SetActive(false);
		}
		else
		{
			yStart = -600;
			tContainer = new TutorialRelatedFunctionContainer(this);
			interContainer = new ShowInteractionStateRecorder();
			tContainer.isTutorial = true;
			SetShowPositionNum(4);
			SetScoreEnableState(true, false, false, false);
			SetBananaEnableState(false);
			SetTurnEnableState(false);
			SetHandAnimal(true, new List<animalProperty>(showTutorialManager.switchHand.properies));
			SetIfChangeTroupePrice(false);
			SetIfUpdatePopularity(false);
			curTurn = 1;
			blacker.Initial();
			onStage = new GameObject[6];
			posRecord = new areaReport[6];
			iconToOnStage = new BiDictionary<iconAnimal, GameObject>();
			myHandControls = new List<iconAnimal>();
			myHand = new List<GameObject>();
			initialPos = new List<Vector2>();
			SetUpAnLevel(GlobalManager.instance.GetTutorialLevel());
			totalPerformanceControl.InitShow(infoContainer.posNum, Enumerable.Range(0, 6)
							 .Select(i => infoContainer.GetEmptyPosLocalX(i))
							 .ToArray(), Enumerable.Range(0, 6)
							 .Select(i => infoContainer.GetStageLocalX(i))
							 .ToArray());
			thrower.changeCount(10);

			for (int i = 0; i < infoContainer.posNum; i++)
			{
				GameObject temp = Instantiate(areaPrefab, stagePanelTransform);
				temp.GetComponent<areaReport>().spotNum = i;
				temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-5 + infoContainer.areaOffset * i + infoContainer.centerOffset, 0);
				posRecord[i] = temp.GetComponent<areaReport>();
			}
			InitializeHand(tContainer.GetCurrentHand());
			currentState = ShowStates.SelectAnimal;
			stagePanelMover.gameObject.SetActive(true);
			startButtonMover.GetComponent<Button>().interactable = true;
			camMover.SetTo(CamInDecition.position, DecideCamScale);

			regularTutorial.SetActive(false);
			showTutorialManager.gameObject.SetActive(true);
		}
	}

	private void SetUpAnLevel(LevelProperty level)
	{
		curLevel = level;
		//Debug.Log("关卡是" + level.name + ",回合数是" + level.allowedTurn);
		curScore = 0;
		curTurn = 1;
		GetComponent<ShowScoreManager>().StartTurn(curLevel);
		curRepu = GetComponent<ShowScoreManager>().currentReputation;
		targetDisplayManager.ChangeLevelState(curTurn, curRepu, scoreManager.GetTargetScore(), level.allowedTurn);
	}

	private void InitializeHand(List<animalProperty> properties)
	{
		Dictionary<string, int> nameToCount = new Dictionary<string, int>();
		List<animalProperty> uniqueProperties = new List<animalProperty>();

		// 统计数量 + 构造不重复的新列表
		foreach (var prop in properties)
		{
			string name = prop.animalName;
			if (nameToCount.ContainsKey(name))
			{
				nameToCount[name]++;
			}
			else
			{
				nameToCount[name] = 1;
				uniqueProperties.Add(prop); // 第一次出现才加进去
			}
		}

		for (int i = 0; i < uniqueProperties.Count; i++)
		{
			//Debug.Log(properties[i].animalName);
			GameObject temp = Instantiate(animalIcon, handPanelTransform.position, Quaternion.identity, handPanelBackground);
			myHand.Add(temp);

			temp.GetComponent<iconAnimal>().Initialize(uniqueProperties[i], nameToCount[uniqueProperties[i].animalName], false);
			temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(xStart + offset * i, yStart);
			temp.GetComponent<iconAnimal>().myIndex = i;
			//TODO:把这个目标位置整合
			initialPos.Add(new Vector2(xStart + offset * i, temp.GetComponent<iconAnimal>().yGoal));
			myHandControls.Add(temp.GetComponent<iconAnimal>());
		}
	}

	private void ChangeLevelStatus(int _curTurn, bool ifChangeTurnDisplay)
	{
		curRepu = GetComponent<ShowScoreManager>().currentReputation;

		targetDisplayManager.ChangeLevelState(_curTurn - (ifChangeTurnDisplay ? 0 : 1), curRepu, scoreManager.GetTargetScore(), curLevel.allowedTurn);
	}

	public void ChangeTargetPanelDisplay(float reputation)
	{
		tContainer.EndTurnScoreChange();
		scoreManager.StartTurn(curLevel);
		targetDisplayManager.ChangeLevelState(curTurn, (int)reputation, scoreManager.GetTargetScore(), curLevel.allowedTurn);
	}


	private void GlobalManager_OnNextGlobalLevel(GlobalLevel level)
	{
		testLevel = level.levelProperty;
		//Debug.Log("当前level是" + testLevel.name);
	}

	void Update()
	{

		if (ifTest)
		{
			ifTest = false;
			//mover.MoveTo(tarTrans.anchoredPosition);
			/*
			if (Time.timeScale != 1)
				Time.timeScale = 1;
			else
				Time.timeScale *= 2;
			*/
		}
		/*
        if (Input.GetKeyDown(KeyCode.U))
        {
            //EnterOneShow();
            blacker.Fade();
            curEndScreen = Instantiate(EndScreenPrefab, canvasTrans);
            curEndScreen.GetComponent<RectTransform>().anchoredPosition = endScreenDownPos.anchoredPosition;
            curEndScreen.GetComponent<UiMover>().MoveTo(endScreenUpPos.anchoredPosition);
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            
        }*/
		//查现在是哪个State


		switch (currentState)
		{

			case ShowStates.SelectAnimal:
				//选动物
				UpdateDecideState();
				break;

			case ShowStates.Animation:
				//换State
				if (moveCounter.TakeResult())
				{
					if (ifToShow)
						StartShow();
					else
						StartDecide(true);
				}
				break;

			case ShowStates.Performance:
				//表演
				if (Input.GetKeyDown(KeyCode.E))
				{
					SpeedUp();

				}
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					if (Time.timeScale > 0)
					{
						Time.timeScale = 0;
						pauseShow.SetActive(true);
						thrower.SwitchThrowEnableWhenPause(false);

					}
					else
					{
						PauseResume();
					}
				}

				break;
		}





	}

	public void SpeedUp()
	{
        if (speedRatio == 1) speedRatio = 2;
        else if (speedRatio == 2) speedRatio = 5;
        else if (speedRatio == 5) speedRatio = 1;

        Time.timeScale = speedRatio;

		int speedRatioToSpriteIndex = speedRatio == 1 ? 0 : speedRatio == 2 ? 1 : 2;
        speedUpButton.sprite = speedUpButton.transform.parent.GetComponent<ScoreUIDisplay>().speedUpButtonSprites[speedRatioToSpriteIndex];
    }

	public void PauseResume()
	{
		Time.timeScale = speedRatio;
		pauseShow.SetActive(false);
		thrower.SwitchThrowEnableWhenPause(true);
		SwitchExplainEnableState(true);
	}


	public void StartMoveToShow()
	{
		ifToShow = true;
		currentState = ShowStates.Animation;
		handPanelMover.MoveTo(handPanelDownPos.anchoredPosition);
		//stagePanelMover.MoveTo(stagePanelDownPos.anchoredPosition);
		stagePanelMover.gameObject.SetActive(false);
		scorePanelMover.MoveTo(scorePanelDownPos.anchoredPosition);
		camMover.MoveTo(CamInShow.position, ShowCamScale);
		startButtonMover.MoveTo(StartButtonUp.anchoredPosition);
		startButtonMover.GetComponent<Button>().interactable = false;
		targetPanelMover.MoveTo(scorePanelUpPos.anchoredPosition);

		int count = 5;
		tContainer.DoBananaMoverAction(bananaUiMover, BanannaInShow.anchoredPosition);
		tContainer.DoBananaMoverAction(showBanana, ShowBanannaInShow.anchoredPosition);
		totalPerformanceControl.InitShow(curRepu);
		moveCounter.SetUpCount(count + tContainer.TakeCount());
		var toGive = from x in onStage
					 let control = x?.GetComponent<PerformAnimalControl>() // 先获取组件，避免重复调用
					 select control; // 直接返回 control（如果 x 是 null，control 也会是 null）
		totalPerformanceControl.GetInfoFromShowManager(toGive.ToArray(), this);

		AudioManagerScript.Instance.PlayUISound(AudioManagerScript.Instance.UI[0]);

		if (tContainer.isTutorial)
		{
			showTutorialManager.turotialShowTurn++;
			if (showTutorialManager.turotialShowTurn == 5 || showTutorialManager.turotialShowTurn == 6)
			{
				showTutorialManager.isRehearsalGoalActive = true;
			}
		}

        showStressManager.Initialize();
    }

	void StartShow()
	{
		Time.timeScale = speedRatio;
		totalPerformanceControl.StartState(showState.showStart);
		currentState = ShowStates.Performance;

		//GetComponent<ShowScoreManager>().StartTurn();
	}

	public void EndMoveToDecide(float score)
	{
		//时间速率相关
		Time.timeScale = 1;

		ifToShow = false;
		currentState = ShowStates.Animation;
		handPanelMover.MoveTo(handPanelUpPos.anchoredPosition);
		//stagePanelMover.MoveTo(stagePanelUpPos.anchoredPosition);
		scorePanelMover.MoveTo(scorePanelUpPos.anchoredPosition);
		camMover.MoveTo(CamInDecition.position, DecideCamScale);
		startButtonMover.MoveTo(StartButtonDown.anchoredPosition);
		targetPanelMover.MoveTo(scorePanelDownPos.anchoredPosition);
		int count = 5;
		tContainer.DoBananaMoverAction(bananaUiMover, BanannaInDecision.anchoredPosition);
		tContainer.DoBananaMoverAction(showBanana, ShowBanannaInDecision.anchoredPosition);
		moveCounter.SetUpCount(count + tContainer.TakeCount());

		GetComponent<ShowAnimalBallPassTimesCounter>().DoWhenTurnEnds();

		if (tContainer.isTutorial)
		{
			switch (showTutorialManager.turotialShowTurn)
			{
				case 1:
					SetScoreEnableState(true, true, false, false);
					AddAnimalToHand(showTutorialManager.addHandElephant);
					break;
				case 2:
					SetBananaEnableState(true);
					break;
				case 3:
					SetShowPositionNumDuringShow(6);
					SetScoreEnableState(true, true, true, false);
					AddAnimalToHand(showTutorialManager.addHandLion);
					break;
				case 4:
					showTutorialManager.bananaHitTimes = 0;
					break;
				case 5:
					if (showTutorialManager.bananaHitTimes >= 3)
					{
						showTutorialManager.isRehearsalGoalActive = false;
						GetComponent<ShowScoreManager>().SetReputation(10f);
						SetScoreEnableState(true, true, true, true);
						SetIfUpdatePopularity(true);
					}
					else
					{
						showTutorialManager.turotialShowTurn--;
					}
					break;
				case 6:
					if (GetComponent<ShowScoreManager>().currentReputation >= 0)
					{
						showTutorialManager.isRehearsalGoalActive = false;
						SetTurnEnableState(true);
						showTutorialManager.ChangeGoalTickVisual(true);
					}
					else
					{
						GetComponent<ShowScoreManager>().SetReputation(10f);
						showTutorialManager.turotialShowTurn--;
					}
					break;
			}
			showTutorialManager.content.gameObject.SetActive(true);
		}
	}

	public void StartDecide(bool ifBreak)
	{
		tContainer.DoTurnAddAction();
		if (curTurn <= curLevel.allowedTurn)
		{
			//Debug.Log("开始decide");
			currentState = ShowStates.SelectAnimal;
			stagePanelMover.gameObject.SetActive(true);
			startButtonMover.GetComponent<Button>().interactable = true;
			tContainer.ChangeBananaDuringShow(10);

			ChangeLevelStatus(curTurn, true);
		}
		else
		{
			ChangeLevelStatus(curTurn, false);
			currentState = ShowStates.EndCheck;
			blacker.Fade();
			curEndScreen = Instantiate(EndScreenPrefab, canvasTrans);
			curEndScreen.GetComponent<EndScreenScript>().InitialScore( GetComponent<ShowScoreManager>().containers, (int)curRepu, 100,ifBreak);
			curEndScreen.GetComponent<RectTransform>().anchoredPosition = endScreenDownPos.anchoredPosition;
			curEndScreen.GetComponent<UiMover>().MoveTo(endScreenUpPos.anchoredPosition);

			if (ifBreak && GlobalManager.instance.currentLevelIndex == 5) MrShopManager.instance.AchievementUnlocked(3);

            //LeaveShow();
        }
	}

	public void LeaveShow(int curMoneyEarned)
	{
		if (tContainer.ifChangePrice)
		{
			GlobalManager.instance.changeCoinAmount(curMoneyEarned);
			GlobalManager.instance.CalculateAnimalPrice();
			GlobalManager.instance.UnlockAnimal();
			GlobalManager.instance.SetMaxBallPassTimes(GetComponent<ShowAnimalBallPassTimesCounter>().GetTotalBallPassTimesListPerShow());
			GlobalManager.instance.ToNextGlobalLevel();
		}
		else
		{
			GlobalManager.instance.DirectResetAnimalBallPassTime();
			GlobalManager.instance.temporaryPointsByAnimal.Clear();
		}
		foreach (GameObject an in onStage)
		{
			if (an != null)
				SetUnSelectIconInHand(an);
		}
		foreach (GameObject handIcon in myHand)
		{
			Destroy(handIcon);
		}
		gameObject.SetActive(false);
		Destroy(curEndScreen);
		menu.Enable();
		GetComponent<ShowScoreManager>().ResetReputation();
		Destroy(gameObject);

	}

	private Counter moveCounter = new Counter();
	public void reportFinish()
	{
		moveCounter.CountDown(1);
	}

	//创动物prefab
	public GameObject AnimalFactory(string name, Vector3 position)
	{
		switch (name)
		{
			case "Monkey":
				soundIndex = 8;
				return Instantiate(animalPerformancePrefabs[0], position, Quaternion.identity, transform);

			case "Elephant":
				soundIndex = 2;
				return Instantiate(animalPerformancePrefabs[1], position, Quaternion.identity, transform);

			case "Bear":
				soundIndex = 0;
				return Instantiate(animalPerformancePrefabs[2], position, Quaternion.identity, transform);

			case "Lion":
				soundIndex = 6;
				return Instantiate(animalPerformancePrefabs[3], position, Quaternion.identity, transform);

			case "Giraffe":
				soundIndex = 4;
				return Instantiate(animalPerformancePrefabs[4], position, Quaternion.identity, transform);

			case "Snake":
				soundIndex = 12;
				return Instantiate(animalPerformancePrefabs[5], position, Quaternion.identity, transform);

			case "Fox":
				soundIndex = 3;
				return Instantiate(animalPerformancePrefabs[6], position, Quaternion.identity, transform);

			case "Seal":
				soundIndex = 14;
				return Instantiate(animalPerformancePrefabs[7], position, Quaternion.identity, transform);

			case "Ostrich":
				soundIndex = 9;
				return Instantiate(animalPerformancePrefabs[8], position, Quaternion.identity, transform);

			case "Kangaroo":
				soundIndex = 5;
				return Instantiate(animalPerformancePrefabs[9], position, Quaternion.identity, transform);

			case "Buffalo":
				soundIndex = 1;
				return Instantiate(animalPerformancePrefabs[10], position, Quaternion.identity, transform);

			case "Goat":
				soundIndex = 13;
				return Instantiate(animalPerformancePrefabs[11], position, Quaternion.identity, transform);

			case "Lizard":
				soundIndex = 7;
				return Instantiate(animalPerformancePrefabs[12], position, Quaternion.identity, transform);
		}
		return null;
	}

	public void CallSound()
	{
		AudioManagerScript.Instance.PlayUISound(AudioManagerScript.Instance.AnimalSounds[soundIndex]);
	}



	void SlideAnimalsInHand(float changeX)
	{
		//TODO:限制左右
		//if(changeX!= 0)Debug.Log("改变的x是"+changeX);
		if (Mathf.Abs(changeX) > 500)
			return;
		leftAnchorX += changeX;
		float rightLimit = Screen.width / 10;
		float leftLimit = -Math.Max((myHand.Count) * offset * (Application.platform == RuntimePlatform.WebGLPlayer ? 2 : 1) - Screen.width, 0) - Screen.width / 10;

		// 限制左右移动范围
		leftAnchorX = Mathf.Clamp(leftAnchorX, leftLimit, rightLimit);
		for (int i = 0; i < myHand.Count; i++)
		{
			GameObject gmo = myHand[i];
			gmo.GetComponentInChildren<RectTransform>().anchoredPosition = initialPos[i] + new Vector2(leftAnchorX, 0);
		}

	}

	bool CheckIfRayCastElementWithTag(string targetTag, out GameObject first)
	{
		if (eventSystem == null)
		{
			eventSystem = FindObjectOfType<EventSystem>();
		}

		// 创建一个 PointerEventData 来存储射线检测信息
		PointerEventData eventData = new PointerEventData(eventSystem);
		eventData.position = Input.mousePosition; // 设置射线的起点（鼠标位置）

		// 存储射线检测到的 UI 组件
		List<RaycastResult> results = new List<RaycastResult>();

		// 进行 UI 射线检测
		uiRaycaster.Raycast(eventData, results);

		// 遍历检测到的 UI 组件
		foreach (RaycastResult result in results)
		{
			// 检查 GameObject 是否有目标 Tag
			if (result.gameObject.CompareTag(targetTag))
			{
				first = result.gameObject;
				return true; // 找到匹配的对象，返回 true
			}
		}
		first = null;
		return false; // 没有找到匹配的对象
	}

	bool CheckIfRayCastWorldObject2DWithTag(string targetTag, out GameObject first)
	{
		first = null;

		// 获取鼠标在世界空间的 2D 位置
		Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		// 进行 2D 射线检测
		RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

		// 检测 Tag 是否匹配
		if (hit.collider != null && hit.collider.CompareTag(targetTag))
		{
			first = hit.collider.gameObject;
			return true; // 找到匹配的物体
		}

		return false; // 没找到
	}

	Vector3 GetMouseWorldPositionAtZeroZ()
	{
		// 获取鼠标在屏幕中的位置
		Vector3 mouseScreenPosition = Input.mousePosition;

		// 设定鼠标的世界 Z 位置为 0
		mouseScreenPosition.z = Mathf.Abs(Camera.main.transform.position.z);

		// 转换为世界坐标
		Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

		// 确保 Z 轴为 0
		worldPosition.z = 0;

		return worldPosition;
	}


	DecideScreenState curDecideState = DecideScreenState.empty;
	public void StartDecideState(DecideScreenState newState)
	{
		EndDecideState(curDecideState);
		switch (newState)
		{
			case DecideScreenState.slide:
				lastMousePosition = Input.mousePosition;
				break;

			case DecideScreenState.moveAnimal:
				inDown = true;
				break;

		}
		curDecideState = newState;
	}

	void EndDecideState(DecideScreenState lastState)
	{
		switch (lastState)
		{
			case DecideScreenState.slide:
				break;

			case DecideScreenState.moveAnimal:
				if (inDown)
				{
					foreach (iconAnimal animal in myHandControls)
					{
						animal.EnterState(iconAnimal.iconState.movingUp);
					}
				}
				break;

			case DecideScreenState.empty:
				myExplainingCard.DoneExplain();
				break;
		}
	}



	void UpdateDecideState()
	{
		switch (curDecideState)
		{
			case DecideScreenState.empty:

				if (Input.GetMouseButtonDown(0) && !pauseShow.activeInHierarchy)
				{
					//enterInteraction = true;

					if (CheckIfRayCastWorldObject2DWithTag("animalTag", out firstDetect) && interContainer.ifSelectUp)
					{
						//选择到了表演小动物
						holdingAnimalObj = firstDetect;
						moveFromStageIndex = Array.IndexOf(onStage, firstDetect);
						FreePosOnStage(firstDetect);
						StartDecideState(DecideScreenState.moveAnimal);

					}
					else if ((!CheckIfRayCastElementWithTag("showAnimalInHand", out firstDetect) || !DetectMouseInDownArea(downRatio)) && interContainer.ifSlide)
					{

						StartDecideState(DecideScreenState.slide);

						//进入滑动
					}
					else if (CheckIfRayCastElementWithTag("showAnimalInHand", out firstDetect) && firstDetect.GetComponentInParent<iconAnimal>().CanBeSelect() && interContainer.ifSelectDown)
					{
						//进入上下
						//Debug.Log(firstDetect.transform.parent.name);
						foreach (iconAnimal animal in myHandControls)
						{
							if (animal.gameObject != firstDetect.transform.parent.gameObject)
							{
								animal.EnterState(iconAnimal.iconState.half);
							}
							else
							{
								holdingAnimalObj = RegisterAndCreateNewAnimal(animal);
								animal.TryMinus(1);

							}
						}
						StartDecideState(DecideScreenState.choose);
					}

				}
				else
				{
					//解释动物
					if (CheckIfRayCastElementWithTag("showAnimalInHand", out firstDetect) && ifExplainEnabled)
					{
						if (firstDetect != null)
							myExplainingCard.StartExplain(firstDetect.GetComponent<RectTransform>(), true, firstDetect.GetComponentInParent<iconAnimal>().selfProperty);
					}
					else if (CheckIfRayCastWorldObject2DWithTag("animalTag", out firstDetect) && ifExplainEnabled)
					{
						if (firstDetect != null)
							myExplainingCard.StartExplain(firstDetect.transform.position, false, iconToOnStage.GetKeyByValue(firstDetect).selfProperty);
						//Debug.Log(iconToOnStage.GetByValue(firstDetect).selfProperty.animalName);
					}
					else
					{
						myExplainingCard.DoneExplain();
					}
					/*
					 * } else if (CheckIfRayCastElementWithTag("mechanicExplain", out firstDetect)) {
						if (firstDetect != null)
							explainer.StartMechanicExplain(firstDetect.GetComponent<RectTransform>());
					 */
				}

				if (Input.GetKeyDown(KeyCode.Escape))
				{
					if (Time.timeScale > 0)
					{
						Time.timeScale = 0;
						pauseShow.SetActive(true);
						SwitchExplainEnableState(false);
					}
					else
					{
						PauseResume();
					}
				}
				//if (Input.GetKeyDown(KeyCode.P))
				//SetDicideStateInteractionEnabled(false);
				break;

			case DecideScreenState.slide:
				if (Input.GetMouseButton(0))
				{
					Vector2 currentMousePosition = Input.mousePosition;
					float changeX = currentMousePosition.x - lastMousePosition.x; // 计算滑动距离
					SlideAnimalsInHand(changeX);
					lastMousePosition = currentMousePosition; // 更新鼠标位置
				}
				if (Input.GetMouseButtonUp(0))
				{
					//enterInteraction = false;
					StartDecideState(DecideScreenState.empty);
				}
				break;

			case DecideScreenState.choose:
				if (Input.GetMouseButton(0))
				{

					holdingAnimalObj.transform.position = GetMouseWorldPositionAtZeroZ();
				}
				if (Input.GetMouseButtonUp(0))
				{
					foreach (iconAnimal animal in myHandControls)
					{
						if (animal.gameObject != firstDetect.transform.parent.gameObject)
						{
							animal.EnterState(iconAnimal.iconState.movingUp);
						}
						//canBeMovedOrSelected = false;
						//Invoke("ResetCanBeMoveOrSelect", 0.3f);
					}
					//enterInteraction = false;
					GameObject Rect;
					if (CheckIfRayCastElementWithTag("areaTag", out Rect))
					{
						//区分目标位置是否有动物
						areaReport rectReport = Rect.GetComponentInParent<areaReport>();

						GameObject atTar = onStage[Array.IndexOf(posRecord, rectReport)];
						if (atTar != null)
						{
							SetUnSelectIconInHand(atTar);
						}

						MoveObjToIndexOnStage(-1, Array.IndexOf(posRecord, rectReport), holdingAnimalObj);
						CallSound();
						//SetSelectIconInHand(holdingAnimalObj);
						holdingAnimalObj = null;
					}
					else
					{
						UnRegisterPerformAnimal(holdingAnimalObj);
						Destroy(holdingAnimalObj);
					}
					StartDecideState(DecideScreenState.empty);

				}
				break;

			case DecideScreenState.moveAnimal:
				if (Input.GetMouseButton(0))
				{
					holdingAnimalObj.transform.position = GetMouseWorldPositionAtZeroZ();
					if (DetectMouseInDownArea(downRatio))
					{
						if (!inDown)
						{
							inDown = true;
							foreach (iconAnimal animal in myHandControls)
							{
								if (animal != iconToOnStage.GetKeyByValue(holdingAnimalObj))
								{
									animal.EnterState(iconAnimal.iconState.half);
								}
							}
						}
					}
					else
					{
						if (inDown)
						{
							inDown = false;
							foreach (iconAnimal animal in myHandControls)
							{
								if (animal != iconToOnStage.GetKeyByValue(holdingAnimalObj))
								{
									animal.EnterState(iconAnimal.iconState.movingUp);
								}
							}
						}
					}
				}
				if (Input.GetMouseButtonUp(0))
				{

					//enterInteraction = false;
					GameObject Rect;
					if (CheckIfRayCastElementWithTag("areaTag", out Rect))
					{
						areaReport rectReport = Rect.GetComponentInParent<areaReport>();

						GameObject atTar = onStage[Array.IndexOf(posRecord, rectReport)];
						if (atTar != null)
						{
							MoveObjToIndexOnStage(Array.IndexOf(posRecord, rectReport), moveFromStageIndex, atTar);
						}
						MoveObjToIndexOnStage(-1, Array.IndexOf(posRecord, rectReport), holdingAnimalObj);
						holdingAnimalObj = null;
					}
					else if (DetectMouseInDownArea(downRatio))
					{
						SetUnSelectIconInHand(holdingAnimalObj);
						holdingAnimalObj = null;
					}
					else
					{
						MoveObjToIndexOnStage(moveFromStageIndex, moveFromStageIndex, holdingAnimalObj);

					}
					StartDecideState(DecideScreenState.empty);

				}
				break;

			case DecideScreenState.tutorial:
				//if (Input.GetKeyDown(KeyCode.P))
				//SetDicideStateInteractionEnabled(true);
				break;
		}
	}

	private bool DetectMouseInDownArea(float percentage) // 默认是屏幕下方 30%
	{
		float screenHeight = Screen.height; // 获取屏幕高度
		float thresholdY = screenHeight * percentage; // 计算下方区域的 Y 轴临界值

		return Input.mousePosition.y <= thresholdY; // 如果鼠标 Y 轴位置在这个范围内，则返回 true
	}



	/// <summary>
	/// 将对象移动到 `onStage` 位置索引 `to`，并设置其位置。
	/// </summary>
	/// <param name="from">对象当前所在的索引，`-1` 表示不关心原位置（例如鼠标悬停的情况）。</param>
	/// <param name="to">对象要移动到的目标索引。</param>
	/// <param name="toMove">要移动的 `GameObject`。</param>
	private void MoveObjToIndexOnStage(int from, int to, GameObject toMove)
	{
		// 如果 `from` 不是 -1，则清空原位置
		if (from != -1)
			onStage[from] = null;

		// 在目标索引 `to` 处放置 `toMove`
		onStage[to] = toMove;

		// 让 `toMove` 平滑移动到 `posRecord[to]` 记录的位置
		toMove.GetComponent<dragBack>().SetToStagePos(posRecord[to].myPosition);

		AudioManagerScript.Instance.PlayUISound(AudioManagerScript.Instance.Battle[6]);

		// 直接瞬移到目标位置的备用代码（已注释）
		// toMove.transform.position = posRecord[to].myPosition;
	}

	private void SetUnSelectIconInHand(GameObject obj)
	{
		FreePosOnStage(obj);
		UnRegisterPerformAnimal(obj);
		Destroy(obj);
	}

	private void SetSelectIconInHand(GameObject obj)
	{

		iconToOnStage.GetKeyByValue(obj).SetSelectState(true);
	}

	private GameObject RegisterAndCreateNewAnimal(iconAnimal chooseAnimal)
	{
		GameObject create = AnimalFactory(chooseAnimal.selfProperty.animalName, GetMouseWorldPositionAtZeroZ());
		iconToOnStage.Add(chooseAnimal, create);
		return create;
	}

	private void UnRegisterPerformAnimal(GameObject choosePerformAnimal)
	{
		iconToOnStage.GetKeyByValue(choosePerformAnimal).AddNum(1);
		iconToOnStage.RemoveByValue(choosePerformAnimal);

	}

	private void FreePosOnStage(GameObject obj)
	{
		int index = Array.IndexOf(onStage, obj);
		if (index != -1)
		{
			onStage[index] = null;
		}
	}

	public void EnableCanvas()
	{
		uiRaycaster.enabled = true;
	}

	public void DisableCanvas()
	{
		uiRaycaster.enabled = false;
	}

	private void OnDestroy()
	{
		CanvasMain.OnUIInteractionEnabled -= EnableCanvas;
		CanvasMain.OnUIInteractionDisabled -= DisableCanvas;
	}
	private class ShowInteractionStateRecorder
	{
		public bool ifSlide = true;
		public bool ifSelectDown = true;
		public bool ifSelectUp = true;

		public void DisableAll()
		{
			ifSlide = false;
			ifSelectDown = false;
			ifSelectUp = false;

		}

		public void EnableAll()
		{
			ifSlide = true;
			ifSelectDown = true;
			ifSelectUp = true;
		}
	}
}

public class BiDictionary<TKey, TValue>
{
	private Dictionary<TKey, List<TValue>> forward = new Dictionary<TKey, List<TValue>>();
	private Dictionary<TValue, TKey> reverse = new Dictionary<TValue, TKey>();

	public void Add(TKey key, TValue value)
	{
		if (reverse.ContainsKey(value))
			throw new ArgumentException("This value already exists and is bound to a key.");

		if (!forward.ContainsKey(key))
			forward[key] = new List<TValue>();

		forward[key].Add(value);
		reverse[value] = key;
	}

	public bool TryGetValuesByKey(TKey key, out List<TValue> values)
	{
		return forward.TryGetValue(key, out values);
	}

	public bool TryGetKeyByValue(TValue value, out TKey key)
	{
		return reverse.TryGetValue(value, out key);
	}

	public List<TValue> GetValuesByKey(TKey key) => forward[key];

	public TKey GetKeyByValue(TValue value) => reverse[value];

	public bool RemoveByKey(TKey key)
	{
		if (forward.TryGetValue(key, out List<TValue> values))
		{
			foreach (var v in values)
				reverse.Remove(v);

			forward.Remove(key);
			return true;
		}
		return false;
	}

	public bool RemoveByValue(TValue value)
	{
		if (reverse.TryGetValue(value, out TKey key))
		{
			reverse.Remove(value);
			if (forward.TryGetValue(key, out List<TValue> list))
			{
				list.Remove(value);
				if (list.Count == 0)
					forward.Remove(key);
			}
			return true;
		}
		return false;
	}

	public void Clear()
	{
		forward.Clear();
		reverse.Clear();
	}

	public int Count => reverse.Count; // value 是唯一的，reverse 代表总项数
}

public class Counter
{
	int n;

	bool beenSet = false;

	public void SetUpCount(int _n)
	{
		n = _n;
		beenSet = true;
	}

	public void CountDown(int toCount)
	{
		if (!beenSet)
			Debug.LogError("先设置计数器才可以Count");
		n -= toCount;
	}

	public bool TakeResult()
	{
		if (!beenSet)
			Debug.LogError("先设置计数器才可以take");
		if (n <= 0)
		{
			beenSet = false;
			return true;
		}
		return false;
	}
}

public class SceneSetUpInfoContainer
{
	public int posNum { get; private set; }
	public float areaOffset { get; private set; }
	public float centerOffset { get; private set; }

	public SceneSetUpInfoContainer()
	{
		posNum = 6;
		areaOffset = 1.3f;
		centerOffset = 1.8f;
	}

	public void SetPosNum(int n)
	{
		posNum = Mathf.Clamp(n, 1, 6);
		centerOffset = 1.8f + (6 - posNum) * areaOffset / 2;
		//要改配套emptypos 和台子pos
	}

	public float GetStageLocalX(int n)
	{
		if (n >= 0 && n < 6)
		{
			return -6.05f + n * 2.7f + (6 - posNum) * 2.7f / 2;
		}
		else
		{
			Debug.LogWarning("尝试获取非法的舞台位置");
			return 0;
		}
	}

	public float GetEmptyPosLocalX(int n)
	{
		if (n >= 0 && n < 6)
		{
			return -6.65f + n * 2.95f + (6 - posNum) * 2.95f / 2;
		}
		else
		{
			Debug.LogWarning("尝试获取非法的舞台位置");
			return 0;
		}
	}
	#endregion
}