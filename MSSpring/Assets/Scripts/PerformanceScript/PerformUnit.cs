using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformUnit : MonoBehaviour
{

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

    [Header("For Test")]
    public PerformAnimalControl[] testAnimals;
    public bool ifTest;

    // Start is called before the first frame update
    void Start()
    {
        curState = showState.empty;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();

        if (ifTest)
        {
            ifTest = false;
            StartState(showState.showStart);
        }
    }

    void StartShow()
    {
        allAnimalsInShow = GetAllAnimalsInShow();
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

    public PerformAnimalControl[] GetAllAnimalsInShow()
    {
        //TODO:接入ShowManager获取到正确的表演
        return testAnimals;
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
