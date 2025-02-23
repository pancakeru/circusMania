using UnityEngine.SceneManagement;
using UnityEngine;

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

    [Header("For Test")]
    public PerformAnimalControl[] testAnimals;
    public bool ifTest;
    private bool ifInitWithTest = false;

    // Start is called before the first frame update
    void Start()
    {
        if (scoreUI == null)
            scoreUI = FindObjectOfType<ScoreUIDisplay>();
        curState = showState.empty;
        InitShow();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();

        if (ifTest)
        {
            ifTest = false;
            ifInitWithTest = true;
            StartState(showState.showStart);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
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
        ChangeYellowScore(0);
        ChangeBlueScore(1);
        ChangeRedScore(0);
        
    }

    void StartShow()
    {
        allAnimalsInShow = GetAllAnimalsInShow(ifInitWithTest);
        thrower.ShowStart(true);

        for (int i = 0; i < allAnimalsInShow.Length; i++)
        {
            if (allAnimalsInShow[i] != null)
            {
                allAnimalsInShow[i].ShowStart(this, i);
            }
        }

        PerformAnimalControl startAnimal = ReturnFirstAnimal();
        if (startAnimal == null)
            Debug.LogError("没有起始动物");
        curBall = Instantiate(ballPrefab).GetComponent<BallScript>();
        curBall.DoInitialDrop(startAnimal.AcceptPos.position, startAnimal, this);
    }

    public void StartState(showState newState)
    {
        EndState(curState);
        switch (newState)
        {
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
        switch (lastState)
        {
            default:
                break;
        }
    }

    private void UpdateState()
    {
        switch (curState)
        {
            case showState.showStart:
                if (ifBallMoveFinish)
                {
                    StartState(showState.turnStart);

                }
                break;

            case showState.turnStart:
                if (ifBallMoveFinish)
                {

                    StartState(gameFail ? showState.gameEnd : showState.turnEnd);

                }

                break;

            case showState.turnEnd:
                if (ifendTurnAnimationFinish)
                {
                    StartState(showState.turnStart);

                }

                break;

            default:
                break;
        }
    }

    PerformAnimalControl ReturnFirstAnimal()
    {
        return allAnimalsInShow[0];
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
        if (totalManager!= null)
        {
            foreach (PerformAnimalControl control in allAnimalsInShow)
            {
                if (control != null)
                    control.BackToInitial();
            }

            thrower.ShowStart(false);
            Invoke("BackToDecide", 1f);
        }
    }

    void BackToDecide()
    {
        totalManager.EndMoveToDecide();
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
        if (allAnimalsInShow[i] == null)
        {
            pos = Vector3.zero;
            return false;
        }
        pos = ifThrowPos ? allAnimalsInShow[i].ThrowPos.position : allAnimalsInShow[i].AcceptPos.position;
        return true;
    }

    public bool CheckAndGetAnimalBasedOnIndex(int n, out PerformAnimalControl animal)
    {

        if (allAnimalsInShow[n] == null)
        {
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
        foreach (PerformAnimalControl an in allAnimalsInShow)
        {
            if (an != null)
                an.DoTurnStart();
        }
    }

    public void TurnEnd()
    {
        Debug.Log("回合结束了");
        foreach (PerformAnimalControl an in allAnimalsInShow)
        {
            if (an != null)
                an.DoTurnEnd();
        }
    }

    public PerformAnimalControl[] GetAllAnimalsInShow(bool ifTest = true)
    {
        //TODO:接入ShowManager获取到正确的表演
        if(ifTest)
            return testAnimals;

        return allAnimalsInShow;
    }

    public void ChangeYellowScore(float changeNum,ChangeScoreType type = ChangeScoreType.Add)
    {
        switch (type)
        {
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
        scoreUI.UpdateYellowScore((int)curYellowScore);
        UpdateTotalScore();
    }

    public void ChangeRedScore(float changeNum, ChangeScoreType type = ChangeScoreType.Add)
    {
        switch (type)
        {
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
        scoreUI.UpdateRedScore((int)curRedScore);
        UpdateTotalScore(); // 更新总分
    }

    public void ChangeBlueScore(float changeNum, ChangeScoreType type = ChangeScoreType.Add)
    {
        switch (type)
        {
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
        scoreUI.UpdateBlueScore((int)curBlueScore);
        UpdateTotalScore(); // 更新总分
    }

    void SetLastScore(float toNum)
    {
        curLastScore = toNum;
        scoreUI.UpdateLastScore((int)toNum);
    }

    float CalculateTotalScore()
    {
        return ((curYellowScore * curLastScore) + curRedScore) * curBlueScore;
    }

    public void UpdateTotalScore()
    {
        scoreUI.UpdateTotalScore((int)CalculateTotalScore());
    }

    public enum ChangeScoreType { Add,Time, Set}
}


public enum showState
{
    empty,
    showStart,
    turnStart,
    turnEnd,
    gameEnd
}
