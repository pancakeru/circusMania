using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.Mathematics;

//using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class winLoseScript : MonoBehaviour
{
    int index = 0;
    float delay = 0.25f;

    public Image travelBar;
    //public Slider travelSlider;

    public string locations; //locations completed
    public string coins; //coins earned
    public GameObject endButton;
    private UiMover ebScript;
    public GameObject fourPanel;
    private UiMover fourScript;

    public GameObject bPerf;
    public GameObject bPass;

    private Vector2 endPos;
    private Vector2 startPos;

    [SerializeField] private GameObject locationIcon;
    [SerializeField] private GameObject animalPick;
    [SerializeField] private Transform canvasTransform; // Set this to your Canvas transform in Inspector
    public String[] Locations;
    public GameObject titleText;
    public GameObject ap_bg;

    private GameObject[] locationPics;
    private GameObject[] animalPicks;
    public GameObject pinObj;
    public GameObject bPerfText;
    public GameObject bPassText;

    public GameObject bigText;
    public GameObject resultsText;
    private TypewriterEffect rScript;
    private TypewriterEffect bScript;
    public bool startingAnim;
    private int stateIndex = 0;

    public int testN;

    public List<animalProperty> test;

    private GameObject newPin;
   // public GameObject startScreen;

    private enum displaySeq {
        header,
        map,
        animalPicks,
        topScorer,
        stats,
        playAgain

    }
    private List<displaySeq> dIndexes = new List<displaySeq>();
    private displaySeq currentState;

    void Awake()
    {
        bPass.SetActive(false);
        bPerf.SetActive(false);
        fourPanel.SetActive(false);
        endButton.SetActive(false);
        ap_bg.SetActive(false);

        //SetData(new SummaryRequireInfo(8, new int[]{1,1,1,1,1},10,1000,10,200, test.ToArray(), test[1], test[2]));
    }


    void Start()
    {
        bScript = bigText.GetComponent<TypewriterEffect>();
        rScript = resultsText.GetComponent<TypewriterEffect>();
        ebScript = endButton.GetComponent<UiMover>();
        fourScript = fourPanel.GetComponent<UiMover>();
        currentState = displaySeq.header;
        startPos = endButton.GetComponent<RectTransform>().anchoredPosition;
        endPos = new Vector2(0, -195);

        dIndexes.Add(displaySeq.header);
    dIndexes.Add(displaySeq.map);
    dIndexes.Add(displaySeq.animalPicks);
    dIndexes.Add(displaySeq.topScorer);
    dIndexes.Add(displaySeq.stats);
    dIndexes.Add(displaySeq.playAgain);
     //   gameObject.SetActive(false);
        BeginSeq();

        locationPics = new GameObject[8];
    animalPicks = new GameObject[5];

    for (int i = 0; i < 8; i++)
    {
        GameObject temp = Instantiate(locationIcon, canvasTransform);
        temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-317 + (i * 90), 75);
        temp.GetComponent<locationIconScript>().location = Locations[i];
        locationPics[i] = temp;
            /*
            if (i <= 3)
            {
                temp.GetComponent<locationIconScript>().currentState = locationIconScript.Status.passed;
            }
            else if (i == 4)
            {
                temp.GetComponent<locationIconScript>().currentState = locationIconScript.Status.failed;
            }
            else
            {
                temp.GetComponent<locationIconScript>().currentState = locationIconScript.Status.none;
            }*/

    }

        newPin = Instantiate(pinObj, canvasTransform);
        newPin.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-312, 100);

        fourScript.MoveTo(new Vector2(31.20644f, -187.2645f));
    ebScript.MoveTo(new Vector2(326, -195));
    }

private IEnumerator SpawnAnimalPicksWithDelay()
{
   // Debug.Log("spawing");

    for (int i = 0; i < selfInfo.topPicks.Length; i++)
    {
        GameObject temp = Instantiate(animalPick, canvasTransform);
        temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-316 + (i * 67), -93);
        animalPicks[i] = temp;
            temp.GetComponent<RapidTMPCounter>().SetFinalValueAndStart(selfInfo.topPicks[i]);
            temp.GetComponent<RapidTMPCounter>().reference.sprite = selfInfo.topProperties[i].animalCoreImg;

        yield return new WaitForSeconds(0.5f); // Delay between spawns (adjust as needed)
    }

   ChangeState();
}

private IEnumerator TopThings() {
    bPerf.SetActive(true);
        bPerfText.GetComponent<RapidTMPCounter>().reference.sprite = selfInfo.topScorer.animalCoreImg;
    bPerfText.GetComponent<RapidTMPCounter>().SetFinalValueAndStart(selfInfo.TopScore);

    yield return new WaitForSeconds(0.8f);

    bPass.SetActive(true);
    bPassText.GetComponent<RapidTMPCounter>().SetFinalValueAndStart(selfInfo.TopPass);
        bPassText.GetComponent<RapidTMPCounter>().reference.sprite = selfInfo.topPasser.animalCoreImg;

    ChangeState();
}


    void Update()
    {
        if (startingAnim) {
            
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            SetBasedOnLevelIndex(testN);
        }

    }

    public void ChangeState() {
        stateIndex++;

         if (stateIndex < dIndexes.Count)
    {
        currentState = dIndexes[stateIndex];
        BeginSeq();
       // Debug.Log(currentState);
    }
    }


    public void BeginSeq() {
       // ebScript.GetComponent<RectTransform>().anchoredPosition = startPos;

        switch (currentState) {
            case displaySeq.header:
                titleText.GetComponent<UiMover>().MoveTo(new Vector2(0, 163));
               // currentState = displaySeq.map;
            break;

            case displaySeq.map:
                //MAP display
                SetBasedOnLevelIndex(selfInfo.levelIndex);
               ChangeState();
               // currentState = displaySeq.animalPicks;
            break;

            case displaySeq.animalPicks:
                ap_bg.SetActive(true);
                StartCoroutine(SpawnAnimalPicksWithDelay());
            break;

            case displaySeq.topScorer:
               StartCoroutine(TopThings());
            break;

            case displaySeq.stats:
               fourPanel.SetActive(true);
               bigText.GetComponent<RapidTMPCounter>().SetFinalValueAndStart(selfInfo.coinCount);
               resultsText.GetComponent<RapidTMPCounter>().SetFinalValueAndStart(selfInfo.maxCombo);
            break;

            case displaySeq.playAgain:
               endButton.SetActive(true);
            break;

        }


    }

    public void StartPictureSequence()
    {
       // StartCoroutine(PlayPictureSequence());
    }

/*
    private IEnumerator PlayPictureSequence() {
    for (int i = 0; i < pictures.Length; i++)
    {
        pictures[i].GetComponent<pictureIconScript>().Appear();
        yield return new WaitForSeconds(delay);
    }

    currentState = displaySeq.message;
    BeginSeq();
    }
    */

    public void ResetGame() 
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit(); 
#endif
        return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    SummaryRequireInfo selfInfo;
    public void SetData(SummaryRequireInfo newInfo)
    {
        selfInfo = newInfo;
    }

    void SetBasedOnLevelIndex(int n)
    {
        int realIndex = Mathf.Clamp(n, 0, 8);
        for (int i = 0; i < 8; i++)
        {

            if (i <realIndex)
            {
                locationPics[i].GetComponent<locationIconScript>().currentState = i == 7? locationIconScript.Status.current : locationIconScript.Status.passed;
                
            }
            else if (i == realIndex)
            {
                locationPics[i].GetComponent<locationIconScript>().currentState = locationIconScript.Status.current;
            }
            else
            {
                locationPics[i].GetComponent<locationIconScript>().currentState = locationIconScript.Status.none;
            }

        }

        newPin.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-311 + (Mathf.Clamp(realIndex,0,7) * 90), 100);
        travelBar.fillAmount =Mathf.Clamp01( realIndex / 8);
    }

    
}

public class SummaryRequireInfo
{
    public int coinCount;
    public int maxCombo;
    public int TopScore;
    public int TopPass;
    public int[] topPicks;
    public animalProperty[] topProperties;
    public int levelIndex;
    public animalProperty topPasser;
    public animalProperty topScorer;

    public SummaryRequireInfo( int levelIndex, int[] topPicks, int topPass, int topScore, int maxCombo, int coinCount, animalProperty[] topProperties, animalProperty topPasser, animalProperty topScorer)
    {
        this.levelIndex = levelIndex;
        this.topPicks = topPicks;
        this.TopPass = topPass;
        this.TopScore = topScore;
        this.maxCombo = maxCombo;
        this.coinCount = coinCount;
        this.topProperties = topProperties;
        this.topPasser = topPasser;
        this.topScorer = topScorer;
    }

}
