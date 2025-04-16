using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class winLoseScript : MonoBehaviour
{
    public GameObject[] pictures;
    int index = 0;
    float delay = 0.5f;

    public string locations; //locations completed
    public string coins; //coins earned

    public bool startingAnim = false;

    public GameObject bigText;
    public GameObject resultsText;
    private TypewriterEffect rScript;
    private TypewriterEffect bScript;

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
        currentState = displaySeq.header;
        BeginSeq();
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

        switch (currentState) {
            case displaySeq.header:
                bScript.StartTyping();
              //  Debug.Log(currentState);
            break;

            case displaySeq.pics:
                StartPictureSequence();
            break;

            case displaySeq.message:
                rScript.fullText = "You have completed shows in " + $"<color=#D41818>{locations}</color>" + "\nEarning a total of " + $"<color=#D41818>{coins}</color> coins."  + "\n\nWe look forward to seeing you recreate the legend in the future!";
                rScript.StartTyping();
            break;

            case displaySeq.playAgain:

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
    
}
