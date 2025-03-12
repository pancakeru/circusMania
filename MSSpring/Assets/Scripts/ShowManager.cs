using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class ShowManager : MonoBehaviour, IReportReceiver
{
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
		moveAnimal
	}
	//State变量
	private ShowStates currentState;

	public GameObject animalIcon;
	public GameObject areaPrefab;
	public Transform handPanelTransform;
	[SerializeField] private Transform handPanelBackground;
	public Transform stagePanelTransform;
	public UiMover scorePanelMover;
	public CameraMover camMover;
	public PerformUnit totalPerformanceControl;
	public UiMover startButtonMover;
	public UiMover bananaUiMover;
	public BananaThrower thrower;
	public UiMover targetPanelMover;
	[SerializeField] private targetPanelManager targetDisplayManager;
	[SerializeField] private UiMover showBanana;

	private float y;
	private float yStart;
	private float x;
	public float offset;
	public float areaOffset = 1.5f;
	public float centerOffset = 4;

	public bool holding = false;
	public bool stopMoving = false;

	private List<animalProperty> testList;
	public List<GameObject> animalPerformancePrefabs;

	public List<GameObject> myHand;//需要重置
	private GameObject[] onStage;//需要重置

	private float leftAnchorX;
	private List<Vector2> initialPos = new List<Vector2>();//需要重置

	public GraphicRaycaster uiRaycaster;
	[SerializeField] private float downRatio = 0.2f;
	private EventSystem eventSystem;
	private bool sliding = false;
	private Vector2 lastMousePosition;
	private List<iconAnimal> myHandControls = new List<iconAnimal>();//需要重置
	private GameObject firstDetect;
	private bool canBeMovedOrSelected = true;
	private bool enterInteraction = false;
	private GameObject holdingAnimalObj;
	private areaReport[] posRecord;//需要重置
	private BiDictionary<iconAnimal, GameObject> iconToOnStage = new BiDictionary<iconAnimal, GameObject>();//需要重置
	private int moveFromStageIndex;
	private bool inDown = false;
	private LevelProperty curLevel;
	private float curScore;
	private int curTurn;
	private float curRepu;

	//表演速率和状态
	private int speedRatio = 1;

	private float recordScoreFromLastTime;
	[Header("For Show Setting")]
	[SerializeField] private float repuRatio = 0.1f;

	//for general uiControl
	private UiMover handPanelMover;
	private UiMover stagePanelMover;
	[Header("For stage switch")]
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

	[Header("End Screen")]
	[SerializeField] private Transform canvasTrans;
	[SerializeField] private GameObject EndScreenPrefab;
	[SerializeField] private RectTransform endScreenUpPos;
	[SerializeField] private RectTransform endScreenDownPos;
	[SerializeField] private tempBlackManager blacker;
	private GameObject curEndScreen;
	private int[] recordScore;

	[Header("Explain")]
	public tempShowExplain explainer;


	private MenuController menu;

	public animalProperty testProperty;

	[Header("For test")]
	[SerializeField]
	private bool ifTest;
	[SerializeField]
	private RectTransform tarTrans;
	[SerializeField]
	private UiMover mover;
	[SerializeField] private LevelProperty testLevel;

	private bool ifToShow = false;
	private void Awake()
	{
		menu = FindAnyObjectByType<MenuController>();
		handPanelMover = handPanelTransform.GetComponent<UiMover>();
		stagePanelMover = stagePanelTransform.GetComponent<UiMover>();
		camMover = Camera.main.GetComponent<CameraMover>();
        //GlobalManager_OnNextGlobalLevel();
        GlobalManager.OnNextGlobalLevel += GlobalManager_OnNextGlobalLevel;
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

		//FOR ADDING BACK TO DECK
		//instantiate a new iconAnimal prefab on the performance animal
		//hide the sprite of the perfomance animal and only destroy obj if add condition is valid
		//List.Insert(index, obj)
		//when adding back to deck, check the two neighboring iconAnimal objs via collision detection
		//when detected, get larger index position for insert
		//for each iconAnimal on the smaller index and less, move distance left
		//for each iconAnimal on the larger index and more, move distance right
		//when pointer is let go in a valid spot, Insert the obj into the list and update all obj positions and Indexes
		//if not in valid spot, go back to previous pos in performance

	}

	//Functions
	public void EnterOneShow()
	{
		x = -750;
		//offset = 300;
		yStart = -600;
		//areaOffset = 2;
		curTurn = 1;
		blacker.Initial();
		onStage = new GameObject[6];
		posRecord = new areaReport[6];
		iconToOnStage = new BiDictionary<iconAnimal, GameObject>();
		myHandControls = new List<iconAnimal>();
		myHand = new List<GameObject>();
		initialPos = new List<Vector2>();
		SetUpAnLevel(testLevel);
		totalPerformanceControl.InitShow();
		recordScore = new int[curLevel.allowedTurn];
		thrower.changeCount(10);
		//位置 GameObject
		for (int i = 0; i < 6; i++) {
			GameObject temp = Instantiate(areaPrefab, stagePanelTransform);
			temp.GetComponent<areaReport>().spotNum = i;
			temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-5 + areaOffset * i + centerOffset, 0);
			posRecord[i] = temp.GetComponent<areaReport>();
		}
		InitializeHand(GlobalManager.instance.getAllAnimals());
		currentState = ShowStates.SelectAnimal;
		stagePanelMover.gameObject.SetActive(true);
		//TODO:改成到地方再可以交互
		startButtonMover.GetComponent<Button>().interactable = true;
		camMover.SetTo(CamInDecition.position, DecideCamScale);
	}

	private void SetUpAnLevel(LevelProperty level)
	{
		curLevel = level;
		curScore = 0;
		curTurn = 1;
		curRepu = 1;
		targetDisplayManager.ChangeLevelState(curScore, curTurn, curRepu, level.targetScore, level.allowedTurn);
	}

	private void InitializeHand(List<animalProperty> properties)
	{
		for (int i = 0; i < properties.Count; i++) {
			Debug.Log(properties[i].animalName);
			GameObject temp = Instantiate(animalIcon, handPanelTransform.position, Quaternion.identity, handPanelBackground);
			myHand.Add(temp);

			temp.GetComponent<iconAnimal>().Initialize(properties[i], false);
			temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(x + offset * i, yStart);
			temp.GetComponent<iconAnimal>().myIndex = i;
			//TODO:把这个目标位置整合
			initialPos.Add(new Vector2(x + offset * i, temp.GetComponent<iconAnimal>().yGoal));
			myHandControls.Add(temp.GetComponent<iconAnimal>());
		}
	}

	private void ChangeLevelStatus(int _curTurn, float _curScore, float reputationRatio, bool ifChangeTurnDisplay)
	{
		recordScore[_curTurn - 2] = (int)_curScore;
		curRepu = _curScore * reputationRatio;
		curScore += _curScore;
		targetDisplayManager.ChangeLevelState(curScore, _curTurn - (ifChangeTurnDisplay?0:1), curRepu, curLevel.targetScore, curLevel.allowedTurn);
	}

	private void GlobalManager_OnNextGlobalLevel(GlobalLevel level)
	{
		testLevel = level.levelProperty;
		Debug.Log("当前level是"+testLevel.name);
	}

	void Update()
	{
		if (ifTest) {
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
		switch (currentState) {

			case ShowStates.SelectAnimal:
				//选动物
				UpdateDecideState();
				break;

			case ShowStates.Animation:
				//换State
				if (moveCounter.TakeResult()) {
					if (ifToShow)
						StartShow();
					else
						StartDecide();
				}
				break;

			case ShowStates.Performance:
				//表演
				if (Input.GetKeyDown(KeyCode.E))
				{
                    if (speedRatio != 1)
                        speedRatio = 1;
                    else
                        speedRatio *= 2;

					Time.timeScale = speedRatio;
                    
                }
				break;
		}

		//TODO:把这部分结合进statemachine
		/*
        if (Input.GetMouseButtonDown(0)&& canBeMovedOrSelected)
        {
            //Debug.Log(CheckIfRayCastElementWithTag("showAnimalInHand"));
            
            if (!CheckIfRayCastElementWithTag("showAnimalInHand",out firstDetect))
            {
                sliding = true;
                lastMousePosition = Input.mousePosition;
                //进入滑动
            }
            else
            {
                //进入上下
                Debug.Log(firstDetect.transform.parent.name);
                foreach (iconAnimal animal in myHandControls)
                {
                    if (animal.gameObject != firstDetect.transform.parent.gameObject)
                    {
                        animal.EnterState(iconAnimal.iconState.half);
                    }
                    else
                    {
                        //生成一个小动物
                        holdingAnimalObj = AnimalFactory(animal.selfProperty.name,GetMouseWorldPositionAtZeroZ());
                    }
                }
            }
            enterInteraction = true;
        }

        

        if (Input.GetMouseButtonUp(0)&& enterInteraction)
        {
            Debug.Log("触发了");
            if (!sliding)
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
            }
            sliding = false;
            enterInteraction = false;
        }*/

	}

	void ResetCanBeMoveOrSelect()
	{
		canBeMovedOrSelected = true;
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
		bananaUiMover.MoveTo(BanannaInShow.anchoredPosition);
		targetPanelMover.MoveTo(scorePanelUpPos.anchoredPosition);
		showBanana.MoveTo(ShowBanannaInShow.anchoredPosition);
		totalPerformanceControl.InitShow(Mathf.Max(1, curRepu));
		moveCounter.SetUpCount(7);
        var toGive = from x in onStage
                     let control = x?.GetComponent<PerformAnimalControl>() // 先获取组件，避免重复调用
                     select control; // 直接返回 control（如果 x 是 null，control 也会是 null）
        totalPerformanceControl.GetInfoFromShowManager(toGive.ToArray(), this);


	}

	void StartShow()
	{
        Time.timeScale = speedRatio;
        totalPerformanceControl.StartState(showState.showStart);
		currentState = ShowStates.Performance;
	}

	public void EndMoveToDecide(float score)
	{
		//时间速率相关
		Time.timeScale = 1;

		ifToShow = false;
		recordScoreFromLastTime = score;
		currentState = ShowStates.Animation;
		handPanelMover.MoveTo(handPanelUpPos.anchoredPosition);
		//stagePanelMover.MoveTo(stagePanelUpPos.anchoredPosition);
		scorePanelMover.MoveTo(scorePanelUpPos.anchoredPosition);
		camMover.MoveTo(CamInDecition.position, DecideCamScale);
		startButtonMover.MoveTo(StartButtonDown.anchoredPosition);
		bananaUiMover.MoveTo(BanannaInDecision.anchoredPosition);
		targetPanelMover.MoveTo(scorePanelDownPos.anchoredPosition);
		showBanana.MoveTo(ShowBanannaInDecision.anchoredPosition);
		moveCounter.SetUpCount(7);
	}

	void StartDecide()
	{
		curTurn += 1;
		if (curTurn <= curLevel.allowedTurn) {
			Debug.Log("开始decide");
			currentState = ShowStates.SelectAnimal;
			stagePanelMover.gameObject.SetActive(true);
			startButtonMover.GetComponent<Button>().interactable = true;
			thrower.addBanana(10);

			ChangeLevelStatus(curTurn, recordScoreFromLastTime, repuRatio,true);
		} else {
			ChangeLevelStatus(curTurn, recordScoreFromLastTime, repuRatio,false);
			currentState = ShowStates.EndCheck;
			blacker.Fade();
			curEndScreen = Instantiate(EndScreenPrefab, canvasTrans);
			curEndScreen.GetComponent<EndScreenScript>().InitialScore((int)curLevel.targetScore, recordScore, (int)curScore, 15);
			curEndScreen.GetComponent<RectTransform>().anchoredPosition = endScreenDownPos.anchoredPosition;
			curEndScreen.GetComponent<UiMover>().MoveTo(endScreenUpPos.anchoredPosition);
			//LeaveShow();
		}
	}

	public void LeaveShow()
	{
		foreach (GameObject an in onStage) {
			if (an != null)
				SetUnSelectIconInHand(an);
		}
		foreach (GameObject handIcon in myHand) {
			Destroy(handIcon);
		}
		gameObject.SetActive(false);
		Destroy(curEndScreen);
		menu.Enable();
	}

	private Counter moveCounter = new Counter();
	public void reportFinish()
	{
		moveCounter.CountDown(1);
	}

	//创动物prefab
	public GameObject AnimalFactory(string name, Vector3 position)
	{
		switch (name) {
			case "Monkey":
				return Instantiate(animalPerformancePrefabs[0], position, Quaternion.identity, transform);

			case "Elephant":
				return Instantiate(animalPerformancePrefabs[1], position, Quaternion.identity, transform);

			case "Bear":
				return Instantiate(animalPerformancePrefabs[2], position, Quaternion.identity, transform);

			case "Lion":
				return Instantiate(animalPerformancePrefabs[3], position, Quaternion.identity, transform);

			case "Giraffe":
				return Instantiate(animalPerformancePrefabs[4], position, Quaternion.identity, transform);

			case "Snake":
				return Instantiate(animalPerformancePrefabs[5], position, Quaternion.identity, transform);

			case "Fox":
				return Instantiate(animalPerformancePrefabs[6], position, Quaternion.identity, transform);

			case "Seal":
				return Instantiate(animalPerformancePrefabs[7], position, Quaternion.identity, transform);

			case "Ostrich":
				return Instantiate(animalPerformancePrefabs[8], position, Quaternion.identity, transform);

			case "Kangaroo":
				return Instantiate(animalPerformancePrefabs[9], position, Quaternion.identity, transform);

			case "Buffalo":
				return Instantiate(animalPerformancePrefabs[10], position, Quaternion.identity, transform);

			case "Goat":
				return Instantiate(animalPerformancePrefabs[11], position, Quaternion.identity, transform);

			case "Lizard":
				return Instantiate(animalPerformancePrefabs[12], position, Quaternion.identity, transform);
		}
		return null;
	}

	/*
    public void UpdateHand(int index) {
        for (int i = 0; i < myHand.Count; i++) {
            GameObject icon = myHand[i];
            iconAnimal script = icon.GetComponent<iconAnimal>();
            script.myIndex = i;

            if (script.myIndex > 0 && script.myIndex >= index && index != myHand.Count) {
                script.myNeighbor = myHand[script.myIndex - 1];

                float neighborX = script.myNeighbor.GetComponent<RectTransform>().anchoredPosition.x;

                if (Mathf.Abs(icon.GetComponent<RectTransform>().anchoredPosition.x - neighborX) > offset) {
                    script.UpdateDistance(neighborX, 1);
                  // Debug.Log(script.myIndex + "'s NeighborX: " + (neighborX + offset));
                } else {
                   // script.destinationX = neighborX - offset;
                    script.UpdateDistance(neighborX - offset, 1);
                   // Debug.Log(script.myIndex + "'s !! NeighborX: " + (neighborX + offset));
                }

            } else if (script.myIndex == 0 && script.GetComponent<RectTransform>().anchoredPosition.x > -750) {
                script.UpdateDistance(x, 0);
            } else if (index == myHand.Count){
                if (script.myIndex == index - 1) {
                    script.myOtherNeighbor = null;
                } else {
                    script.myOtherNeighbor = myHand[script.myIndex + 1];
                }

                script.UpdateRight();
            }
        }
    }

    public void FixRightSpacing(int index) {

        for (int i = 0; i < myHand.Count; i++) {
            GameObject icon = myHand[i];
            iconAnimal script = icon.GetComponent<iconAnimal>();
            script.myIndex = i;

     
            if (script.myIndex == myHand.Count - 1) {
                script.myOtherNeighbor = null;
                script.otherDestinationX = 750;
            } else {
                 script.myOtherNeighbor = myHand[script.myIndex + 1];
             }

            script.UpdateRight();


        }
    }*/

	void SlideAnimalsInHand(float changeX)
	{
		//TODO:限制左右
		//if(changeX!= 0)Debug.Log("改变的x是"+changeX);
		if (Mathf.Abs(changeX) > 500)
			return;
		leftAnchorX += changeX;
		float rightLimit = Screen.width / 10;
		float leftLimit = -Math.Max(myHand.Count * offset - Screen.width, 0) - Screen.width / 10;

		// 限制左右移动范围
		leftAnchorX = Mathf.Clamp(leftAnchorX, leftLimit, rightLimit);
		for (int i = 0; i < myHand.Count; i++) {
			GameObject gmo = myHand[i];
			gmo.GetComponentInChildren<RectTransform>().anchoredPosition = initialPos[i] + new Vector2(leftAnchorX, 0);
		}

	}

	bool CheckIfRayCastElementWithTag(string targetTag, out GameObject first)
	{
		if (eventSystem == null) {
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
		foreach (RaycastResult result in results) {
			// 检查 GameObject 是否有目标 Tag
			if (result.gameObject.CompareTag(targetTag)) {
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
		if (hit.collider != null && hit.collider.CompareTag(targetTag)) {
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
		switch (newState) {
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
		switch (lastState) {
			case DecideScreenState.slide:
				break;

			case DecideScreenState.moveAnimal:
				if (inDown) {
					foreach (iconAnimal animal in myHandControls) {
						animal.EnterState(iconAnimal.iconState.movingUp);
					}
				}
				break;

			case DecideScreenState.empty:
				explainer.DownExplain();
				break;
		}
	}



	void UpdateDecideState()
	{
		switch (curDecideState) {
			case DecideScreenState.empty:


				if (Input.GetMouseButtonDown(0) && canBeMovedOrSelected) {
					enterInteraction = true;
					//Debug.Log(CheckIfRayCastElementWithTag("showAnimalInHand"));
					if (CheckIfRayCastWorldObject2DWithTag("animalTag", out firstDetect)) {
						//选择到了表演小动物
						Debug.Log(firstDetect.name);
						holdingAnimalObj = firstDetect;
						moveFromStageIndex = Array.IndexOf(onStage, firstDetect);
						FreePosOnStage(firstDetect);
						StartDecideState(DecideScreenState.moveAnimal);

					} else if (!CheckIfRayCastElementWithTag("showAnimalInHand", out firstDetect) || !DetectMouseInDownArea(downRatio)) {
                        
                        StartDecideState(DecideScreenState.slide);
                        
                        //进入滑动
                    } else if (firstDetect.GetComponentInParent<iconAnimal>().CanBeSelect()) {
						//进入上下
						//Debug.Log(firstDetect.transform.parent.name);
						foreach (iconAnimal animal in myHandControls) {
							if (animal.gameObject != firstDetect.transform.parent.gameObject) {
								animal.EnterState(iconAnimal.iconState.half);
							} else {
								GameObject tryGet;
								//区分是否已经生成
								if (iconToOnStage.TryGetByKey(animal, out tryGet)) {
									//如果已经创建
									holdingAnimalObj = tryGet;
									//释放onstage里
									FreePosOnStage(tryGet);
								} else {
									//生成一个小动物
									holdingAnimalObj = RegisterAndCreateNewAnimal(animal);
								}
							}
						}
						StartDecideState(DecideScreenState.choose);
					}

				} else {
					//解释动物
					if (CheckIfRayCastElementWithTag("showAnimalInHand", out firstDetect)) {
						if (firstDetect != null)
							explainer.StartExplain(firstDetect.GetComponent<RectTransform>(), true, firstDetect.GetComponentInParent<iconAnimal>().selfProperty);
					} else if (CheckIfRayCastWorldObject2DWithTag("animalTag", out firstDetect)) {
						if (firstDetect != null)
							explainer.StartExplain(firstDetect.transform.position, false, iconToOnStage.GetByValue(firstDetect).selfProperty);
						//Debug.Log(iconToOnStage.GetByValue(firstDetect).selfProperty.animalName);
					} else if (CheckIfRayCastElementWithTag("mechanicExplain", out firstDetect)) {
						if (firstDetect != null)
							explainer.StartMechanicExplain(firstDetect.GetComponent<RectTransform>());
                    } else {
						explainer.DownExplain();
					}
				}
				break;

			case DecideScreenState.slide:
				if (Input.GetMouseButton(0)) {
					Vector2 currentMousePosition = Input.mousePosition;
					float changeX = currentMousePosition.x - lastMousePosition.x; // 计算滑动距离
					SlideAnimalsInHand(changeX);
					lastMousePosition = currentMousePosition; // 更新鼠标位置
				}
				if (Input.GetMouseButtonUp(0)) {

					sliding = false;
					enterInteraction = false;
					StartDecideState(DecideScreenState.empty);
				}
				break;

			case DecideScreenState.choose:
				if (Input.GetMouseButton(0)) {

					holdingAnimalObj.transform.position = GetMouseWorldPositionAtZeroZ();
				}
				if (Input.GetMouseButtonUp(0)) {
					foreach (iconAnimal animal in myHandControls) {
						if (animal.gameObject != firstDetect.transform.parent.gameObject) {
							animal.EnterState(iconAnimal.iconState.movingUp);
						}
						//canBeMovedOrSelected = false;
						//Invoke("ResetCanBeMoveOrSelect", 0.3f);
					}
					enterInteraction = false;
					GameObject Rect;
					if (CheckIfRayCastElementWithTag("areaTag", out Rect)) {
						//区分目标位置是否有动物
						areaReport rectReport = Rect.GetComponentInParent<areaReport>();

						GameObject atTar = onStage[Array.IndexOf(posRecord, rectReport)];
						if (atTar != null) {
							SetUnSelectIconInHand(atTar);
						}

						MoveObjToIndexOnStage(-1, Array.IndexOf(posRecord, rectReport), holdingAnimalObj);

						SetSelectIconInHand(holdingAnimalObj);
						holdingAnimalObj = null;
					} else {
						UnRegisterPerformAnimal(holdingAnimalObj);
						Destroy(holdingAnimalObj);
					}
					StartDecideState(DecideScreenState.empty);

				}
				break;

			case DecideScreenState.moveAnimal:
				if (Input.GetMouseButton(0)) {
					holdingAnimalObj.transform.position = GetMouseWorldPositionAtZeroZ();
					if (DetectMouseInDownArea(downRatio)) {
						if (!inDown) {
							inDown = true;
							foreach (iconAnimal animal in myHandControls) {
								if (animal != iconToOnStage.GetByValue(holdingAnimalObj)) {
									animal.EnterState(iconAnimal.iconState.half);
								}
							}
						}
					} else {
						if (inDown) {
							inDown = false;
							foreach (iconAnimal animal in myHandControls) {
								if (animal != iconToOnStage.GetByValue(holdingAnimalObj)) {
									animal.EnterState(iconAnimal.iconState.movingUp);
								}
							}
						}
					}
				}
				if (Input.GetMouseButtonUp(0)) {

					enterInteraction = false;
					GameObject Rect;
					if (CheckIfRayCastElementWithTag("areaTag", out Rect)) {
						areaReport rectReport = Rect.GetComponentInParent<areaReport>();

						GameObject atTar = onStage[Array.IndexOf(posRecord, rectReport)];
						if (atTar != null) {
							MoveObjToIndexOnStage(Array.IndexOf(posRecord, rectReport), moveFromStageIndex, atTar);
						}
						MoveObjToIndexOnStage(-1, Array.IndexOf(posRecord, rectReport), holdingAnimalObj);
						holdingAnimalObj = null;
					} else if (DetectMouseInDownArea(downRatio)) {
						SetUnSelectIconInHand(holdingAnimalObj);
						holdingAnimalObj = null;
					} else {
						MoveObjToIndexOnStage(moveFromStageIndex, moveFromStageIndex, holdingAnimalObj);

					}
					StartDecideState(DecideScreenState.empty);

				}
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
		iconToOnStage.GetByValue(obj).SetSelectState(true);
	}

	private GameObject RegisterAndCreateNewAnimal(iconAnimal chooseAnimal)
	{
		GameObject create = AnimalFactory(chooseAnimal.selfProperty.animalName, GetMouseWorldPositionAtZeroZ());
		iconToOnStage.Add(chooseAnimal, create);
		return create;
	}

	private void UnRegisterPerformAnimal(GameObject choosePerformAnimal)
	{
		iconToOnStage.GetByValue(choosePerformAnimal).SetSelectState(false);
		iconToOnStage.RemoveByValue(choosePerformAnimal);

	}

	private void FreePosOnStage(GameObject obj)
	{
		int index = Array.IndexOf(onStage, obj);
		if (index != -1) {
			onStage[index] = null;
		}
	}
}

public class BiDictionary<TKey, TValue>
{
	private Dictionary<TKey, TValue> forward = new Dictionary<TKey, TValue>();
	private Dictionary<TValue, TKey> reverse = new Dictionary<TValue, TKey>();

	public void Add(TKey key, TValue value)
	{
		if (forward.ContainsKey(key) || reverse.ContainsKey(value)) {
			throw new ArgumentException("Key or Value already exists in BiDictionary");
		}

		forward[key] = value;
		reverse[value] = key;
	}

	public bool TryGetByKey(TKey key, out TValue value) => forward.TryGetValue(key, out value);

	public bool TryGetByValue(TValue value, out TKey key) => reverse.TryGetValue(value, out key);

	public TValue GetByKey(TKey key) => forward[key];

	public TKey GetByValue(TValue value) => reverse[value];

	public bool RemoveByKey(TKey key)
	{
		if (forward.TryGetValue(key, out TValue value)) {
			forward.Remove(key);
			reverse.Remove(value);
			return true;
		}
		return false;
	}

	public bool RemoveByValue(TValue value)
	{
		if (reverse.TryGetValue(value, out TKey key)) {
			reverse.Remove(value);
			forward.Remove(key);
			return true;
		}
		return false;
	}

	public void Clear()
	{
		forward.Clear();
		reverse.Clear();
	}

	public int Count => forward.Count;
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
		if (n <= 0) {
			beenSet = false;
			return true;
		}
		return false;
	}
}