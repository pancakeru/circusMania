using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winLoseScript : MonoBehaviour
{
    public GameObject[] pictures;
    int index = 0;
    float delay = 0.25f;

    public string locations; //locations completed
    public string coins; //coins earned
    public GameObject endButton;
    private UiMover ebScript;
    private Vector2 endPos;
    private Vector2 startPos;

    public bool startingAnim = false;

    public GameObject bigText;
    public GameObject resultsText;
    private TypewriterEffect rScript;
    private TypewriterEffect bScript;

   // public GameObject startScreen;

    private enum displaySeq {
        header,
        pics,
        message,
        playAgain

    }

    private displaySeq currentState;

    void Start()
    {
        bScript = bigText.GetComponent<TypewriterEffect>();
        rScript = resultsText.GetComponent<TypewriterEffect>();
        ebScript = endButton.GetComponent<UiMover>();
        currentState = displaySeq.header;
        startPos = endButton.GetComponent<RectTransform>().anchoredPosition;
        endPos = new Vector2(0, -195);
        gameObject.SetActive(false);
       // BeginSeq();
    }

    void Update()
    {
        if (startingAnim) {
            if(bScript.isDoneTyping && currentState == displaySeq.header) {
                    currentState = displaySeq.pics;
                    BeginSeq();
                }
            
             if (rScript.isDoneTyping && currentState == displaySeq.message) {
                    currentState = displaySeq.playAgain;
                    BeginSeq();
                }
        }

    }

    public void BeginSeq() {
        startingAnim = true;
        ebScript.GetComponent<RectTransform>().anchoredPosition = startPos;

        switch (currentState) {
            case displaySeq.header:
                bScript.StartTyping();
              //  Debug.Log(currentState);
            break;

            case displaySeq.pics:
                StartPictureSequence();
            break;

            case displaySeq.message:
                rScript.fullText = "You have completed shows in " + $"<color=#D41818>{locations}</color>" + " Earning a total of " + $"<color=#D41818>{coins}</color> coins."  + "\n\nWe look forward to seeing you recreate the legend in the future!";
                rScript.StartTyping();
            break;

            case displaySeq.playAgain:
                ebScript.MoveTo(endPos);
            break;

        }


    }

    public void StartPictureSequence()
    {
        StartCoroutine(PlayPictureSequence());
    }

    private IEnumerator PlayPictureSequence() {
    for (int i = 0; i < pictures.Length; i++)
    {
        pictures[i].GetComponent<pictureIconScript>().Appear();
        yield return new WaitForSeconds(delay);
    }

    currentState = displaySeq.message;
    BeginSeq();
    }
    

    public void ResetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        /*
        startingAnim = false;
        currentState = displaySeq.header;
        bScript.isDoneTyping = false;
        rScript.isDoneTyping = false;
        bScript.fullText = "";
        rScript.fullText = "";
        startScreen.SetActive(true);
        AudioManagerScript.Instance.PlayUISound(AudioManagerScript.Instance.UI[0]);
        gameObject.SetActive(false);
        */
    }

}
