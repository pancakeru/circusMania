using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ShowStressManager : MonoBehaviour
{
    [SerializeField] PerformUnit performUnit;
    [SerializeField] GameObject stressTextPrefab;

    public int[] stressPoints = { 0, 0, 0, 0, 0, 0 };
    public int[] ballPassToStress = { 0, 0, 0, 0, 0, 0 };
    Vector2[] animalPos = new Vector2[6];

    int collectiveStress;

    void Start()
    {
        Transform[] singleStages = performUnit.GetSingleStages();
        for (int i = 0; i < animalPos.Length; i++)
            animalPos[i] = singleStages[i].position + new Vector3(0, 1, 0);
    }

    public void Initialize()
    {
        for (int i = 0; i < stressPoints.Length; i++)
        {
            stressPoints[i] = 0;
            ballPassToStress[i] = 0;
        }
        performUnit.totalManager.scoreUIDisplay.UpdateStressText(stressPoints[0]);
    }

    public void UpdateStress(int index)
    {
        ballPassToStress[index] += 1;
        collectiveStress = 0;
        for (int i = 0; i < ballPassToStress.Length; i++)
        {
            collectiveStress += ballPassToStress[i];
        }

        if (collectiveStress >= 12)
        {
            for (int i = 0; i < ballPassToStress.Length; i++)
            {
                ballPassToStress[i] = 0;
                stressPoints[i] += 1;
            }

            GameObject newText = Instantiate(stressTextPrefab, new Vector2(0, 2), Quaternion.identity);
            //newText.GetComponent<TextMeshPro>().text = $"*Stress*\nRest +{stressPoints[index]}";
            newText.GetComponent<ScoreTextEffect>().MoveText(new Vector2(0, 2), new Vector2(0, 3));

            performUnit.totalManager.scoreUIDisplay.UpdateStressText(stressPoints[0]);
        }
    }

    public void UpdateStressIndividual(int index)
    {
        ballPassToStress[index] += 1;
        if (ballPassToStress[index] == 10)
        {
            ballPassToStress[index] = 0;
            stressPoints[index] += 1;

            GameObject newText = Instantiate(stressTextPrefab, animalPos[index], Quaternion.identity);
            //newText.GetComponent<TextMeshPro>().text = $"*Stress*\nRest +{stressPoints[index]}";
            newText.GetComponent<ScoreTextEffect>().MoveText(animalPos[index], animalPos[index] + new Vector2(0, 1));
        }
    }
}
