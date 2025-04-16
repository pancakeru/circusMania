using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winLoseScript : MonoBehaviour
{
    public GameObject[] pictures;
    int index = 0;
    float delay = 0.5f;

    public string locations; //locations completed
    public string coins; //coins earned

    public GameObject resultsText;
    private TypewriterEffect rScript;

    void Start()
    {
        rScript = resultsText.GetComponent<TypewriterEffect>();
        rScript.fullText = "You have completed shows in " + $"<color=#D41818>{locations}</color>" + "\nEarning a total of " + $"<color=#D41818>{coins}</color> coins."  + "\n\nWe look forward to seeing you recreate the legend in the future!";
        rScript.StartTyping();
    }

    void Update()
    {
        delay -= Time.deltaTime;

        if (index < pictures.Length && delay <= 0) {
            pictures[index].GetComponent<pictureIconScript>().Appear();
            index += 1;
            delay = 0.5f;
        }
    }
}
