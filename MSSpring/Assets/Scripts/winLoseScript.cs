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

        if (i == 0) {
            temp.GetComponent<locationIconScript>().currentState = locationIconScript.Status.current;
        } else {
            temp.GetComponent<locationIconScript>().currentState = locationIconScript.Status.none;
        }

    }

    Instantiate(pinObj, canvasTransform);
    pinObj.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-312, 100);

    fourScript.MoveTo(new Vector2(31.20644f, -187.2645f));
    ebScript.MoveTo(new Vector2(326, -195));
    }

private IEnumerator SpawnAnimalPicksWithDelay()
{
   // Debug.Log("spawing");

    for (int i = 0; i < 5; i++)
    {
        GameObject temp = Instantiate(animalPick, canvasTransform);
        temp.GetComponentInChildren<RectTransform>().anchoredPosition = new Vector2(-316 + (i * 67), -93);
        animalPicks[i] = temp;
        temp.GetComponent<RapidTMPCounter>().SetFinalValueAndStart(12);

        yield return new WaitForSeconds(0.5f); // Delay between spawns (adjust as needed)
    }

   ChangeState();
}

private IEnumerator TopThings() {
    bPerf.SetActive(true);
    bPerfText.GetComponent<RapidTMPCounter>().SetFinalValueAndStart(2300);

    yield return new WaitForSeconds(0.8f);

    bPass.SetActive(true);
    bPassText.GetComponent<RapidTMPCounter>().SetFinalValueAndStart(20);

    ChangeState();
}


    void Update()
    {
        if (startingAnim) {
            
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
               bigText.GetComponent<RapidTMPCounter>().SetFinalValueAndStart(200);
               resultsText.GetComponent<RapidTMPCounter>().SetFinalValueAndStart(30);
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

    public void ResetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
