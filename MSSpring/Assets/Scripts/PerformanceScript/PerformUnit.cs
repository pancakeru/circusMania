using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class PerformUnit : MonoBehaviour
{
	public ShowManager totalManager;
	private showState curState;

	public GameObject ballPrefab;
	public float endTurnWaitTime = 1f;
	public bool ifShowEnd = false;

	[Header("For Banana")]
	public BananaThrower thrower;

	private BallScript curBall;
	private bool ifBallMoveFinish = false;
	private bool ifendTurnAnimationFinish = false;
	private bool gameFail = false;
	private PerformAnimalControl[] allAnimalsInShow;

	[Header("For Score")]
	[SerializeField]
	private ScoreUIDisplay scoreUI;
	private float curYellowScore;
	private float curLastScore;
	private float curRedScore;
	private float curBlueScore;

	[Header("For Empty Throw")]
	[SerializeField] private Transform leftOut;
	[SerializeField] private Transform rightOut;
	[SerializeField] private Transform[] inSequenceEmpty;

	[Header("For Test")]
	public PerformAnimalControl[] testAnimals;
	public bool ifTest;
	private bool ifInitWithTest = false;

	public event EventHandler<OnExcitementEventArgs> OnExcitement;
	public class OnExcitementEventArgs : EventArgs
	{
		public AnimalInfoPack animalInfo;
	}

	// Start is called before the first frame update
	void Awake()
	{
		if (scoreUI == null)
			scoreUI = FindObjectOfType<ScoreUIDisplay>();
		curState = showState.empty;
		
	}

	// Update is called once per frame
	void Update()
	{
		UpdateState();

		if (ifTest) {
			ifTest = false;
			ifInitWithTest = true;
			StartState(showState.showStart);
		}

		if (Input.GetKeyDown(KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	public void GetInfoFromShowManager(PerformAnimalControl[] animals, ShowManager _manager)
	{
		allAnimalsInShow = animals;
		totalManager = _manager;
	}

	

	public void InitShow()
	{
		SetLastScore(1);
		ChangeYellowScore(0,ChangeScoreType.Set);
		ChangeBlueScore(1,ChangeScoreType.Set);
		ChangeRedScore(0,ChangeScoreType.Set);
		float[] targetScoreArray = FindFirstObjectByType<ShowScoreManager>().GetTargetScore();
		scoreUI.UpdateTargetScores(targetScoreArray[0],targetScoreArray[1],targetScoreArray[2]);
	}

	public void InitShow(float last)
	{
		SetLastScore(last);
		ChangeYellowScore(0,ChangeScoreType.Set);
		ChangeBlueScore(1,ChangeScoreType.Set);
		ChangeRedScore(0,ChangeScoreType.Set);
		float[] targetScoreArray = FindFirstObjectByType<ShowScoreManager>().GetTargetScore();
		scoreUI.UpdateTargetScores(targetScoreArray[0],targetScoreArray[1],targetScoreArray[2]);
	}

	void StartShow()
	{
		//allAnimalsInShow = GetAllAnimalsInShow(ifInitWithTest);
		thrower.ShowStart(true);

		for (int i = 0; i < allAnimalsInShow.Length; i++) {
			if (allAnimalsInShow[i] != null) {
				allAnimalsInShow[i].ShowStart(this, i);
			}
		}

		PerformAnimalControl startAnimal = ReturnFirstAnimal();
		if (startAnimal != null)
		{
			curBall = Instantiate(ballPrefab).GetComponent<BallScript>();
			curBall.DoInitialDrop(startAnimal.AcceptPos.position, startAnimal, this);
		}
		else
		{
            curBall = Instantiate(ballPrefab).GetComponent<BallScript>();
			curBall.DoInitialDrop(GetPositionWhenThrowToEmpty(0), null, this);
        }
		//Debug.LogError("没有起始动物");
		
	}

	public void StartState(showState newState)
	{
		EndState(curState);
		switch (newState) {
			case showState.showStart:
				StartShow();
				ifBallMoveFinish = false;
				ifShowEnd = false;
				gameFail = false;
				break;

			case showState.turnStart:
				TurnStart();
				ifBallMoveFinish = false;
				break;


			case showState.turnEnd:
				TurnEnd();
				ifendTurnAnimationFinish = false;

				Invoke("changeAnimationFinishState", endTurnWaitTime);
				break;

			default:
				break;
		}
		curState = newState;
	}

	private void EndState(showState lastState)
	{
		switch (lastState) {
			default:
				break;
		}
	}

	private void UpdateState()
	{
		switch (curState) {
			case showState.showStart:
				if (ifBallMoveFinish) {
					StartState(showState.turnStart);

				}
				break;

			case showState.turnStart:
				if (ifBallMoveFinish) {

					StartState(gameFail ? showState.gameEnd : showState.turnEnd);

				}

				break;

			case showState.turnEnd:
				if (ifendTurnAnimationFinish) {
					StartState(showState.turnStart);

				}

				break;

			default:
				break;
		}
	}

	PerformAnimalControl ReturnFirstAnimal()
	{
		
		foreach (PerformAnimalControl an in allAnimalsInShow)
		{
			if (an != null)
				return an;
		}
		return null;
	}

	void changeAnimationFinishState()
	{
		ifendTurnAnimationFinish = true;
	}

	public void ReportDrop(BallScript ball)
	{
		gameFail = true;

		DoShowEnd();
	}

	void DoShowEnd()
	{
		if (totalManager != null) {
			foreach (PerformAnimalControl control in allAnimalsInShow) {
				if (control != null)
					control.BackToInitial();
			}

			thrower.ShowStart(false);
			Invoke("BackToDecide", 1f);
		}
	}

	void BackToDecide()
	{
		totalManager.EndMoveToDecide(CalculateTotalScore());
	}

	public void BallToIndex(BallScript ball, int index)
	{

	}

	public Vector3 ReturnDropOutPos(bool ifRight)
	{
		return Vector3.zero;
	}

	public void ReportMoveFinish(BallScript ball)
	{
		Debug.Log("我来拉");
		if (ball == curBall)
			ifBallMoveFinish = true;
	}

	public bool CheckAndGetAnimalThrowAcceptPos(int i, bool ifThrowPos, out Vector3 pos)
	{
		if (allAnimalsInShow[i] == null) {
			pos = Vector3.zero;
			return false;
		}
		pos = ifThrowPos ? allAnimalsInShow[i].ThrowPos.position : allAnimalsInShow[i].AcceptPos.position;
		return true;
	}

	public bool CheckAndGetAnimalBasedOnIndex(int n, out PerformAnimalControl animal)
	{

		if (allAnimalsInShow[n] == null) {
			animal = null;
			return false;
		}
		animal = allAnimalsInShow[n];
		return true;
	}

	public Vector3 GetAnimalBasicPos(int n)
	{
		int k = Mathf.Clamp(n, 0, 5);
		return Vector3.zero;
	}

	public void ReportMoveFinsih(BallScript ball)
	{

	}

	public void TurnStart()
	{
		Debug.Log("回合开始了");
		foreach (PerformAnimalControl an in allAnimalsInShow) {
			if (an != null)
				an.DoTurnStart();
		}
	}

	public void TurnEnd()
	{
		Debug.Log("回合结束了");
		foreach (PerformAnimalControl an in allAnimalsInShow) {
			if (an != null)
				an.DoTurnEnd();
		}
	}

	public PerformAnimalControl[] GetAllAnimalsInShow(bool ifTest = true)
	{
		//TODO:接入ShowManager获取到正确的表演
		if (ifTest)
			return testAnimals;
		/*
		foreach (PerformAnimalControl an in allAnimalsInShow)
		{
			if(an!= null)Debug.Log("现在返回所有动物：" + an.gameObject.name);
		}*/
		return allAnimalsInShow;
	}

	public Vector3 GetPositionWhenThrowToEmpty(int index)
	{
		if (index < 0)
			return leftOut.position;
		else if (index >= 6)
			return rightOut.position;
		else
			return inSequenceEmpty[index].position;
	}

	public void ChangeYellowScore(float changeNum, ChangeScoreType type = ChangeScoreType.Add)
	{
		switch (type) {
			case ChangeScoreType.Add:
				curYellowScore += changeNum;
				break;

			case ChangeScoreType.Time:
				curYellowScore *= changeNum;
				break;

			case ChangeScoreType.Set:
				curYellowScore = changeNum;
				break;
		}
		scoreUI.UpdateYellowScore(curYellowScore, "PerformUnit");
		UpdateTotalScore();
	}

	public void ChangeRedScore(float changeNum, ChangeScoreType type = ChangeScoreType.Add)
	{
		switch (type) {
			case ChangeScoreType.Add:
				curRedScore += changeNum;
				break;
			case ChangeScoreType.Time:
				curRedScore *= changeNum;
				break;

			case ChangeScoreType.Set:
				curRedScore = changeNum;
				break;
		}
		scoreUI.UpdateRedScore(curRedScore, "PerformUnit");
		UpdateTotalScore(); // 更新总分
	}

	public void ChangeBlueScore(float changeNum, ChangeScoreType type = ChangeScoreType.Add)
	{
		switch (type) {
			case ChangeScoreType.Add:
				curBlueScore += changeNum;
				break;
			case ChangeScoreType.Time:
				curBlueScore *= changeNum;
				break;

			case ChangeScoreType.Set:
				curBlueScore = changeNum;
				break;
		}
		scoreUI.UpdateBlueScore(curBlueScore, "PerformUnit");
		UpdateTotalScore(); // 更新总分
	}

	void SetLastScore(float toNum)
	{
		curLastScore = toNum;
		scoreUI.UpdateLastScore(toNum, "PerformUnit");
	}

	float CalculateTotalScore()
	{
		return ((curYellowScore * curLastScore) + curRedScore) * curBlueScore;
	}

	public void UpdateTotalScore()
	{
		scoreUI.UpdateTotalScore(CalculateTotalScore(), "PerformUnit");
	}

	public enum ChangeScoreType { Add, Time, Set }

	public void InvokeOnExcitementEvent(AnimalInfoPack animalInfo)
	{
		OnExcitement?.Invoke(this, new OnExcitementEventArgs {
			animalInfo = animalInfo
		});
	}

	public float[] GetCurrentScoreArray(){
		return new float[] { curRedScore, curYellowScore, curBlueScore };
	}
}


public enum showState
{
	empty,
	showStart,
	turnStart,
	turnEnd,
	gameEnd
}
