using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pictureIconScript : MonoBehaviour
{
    private Image myImg;
    private Transform myTransform;
    private Vector3 targetScale;
    private float shrinkDuration = 0.5f;

    void Start()
    {
        myImg = this.GetComponentInChildren<Image>();
        myTransform = this.GetComponent<Transform>();
        targetScale = new Vector3(1.8f, 1.8f, 1.8f);
    }

    public void Appear() {

        StopAllCoroutines();
        StartCoroutine(LerpToScale(targetScale, shrinkDuration));
    }

     private IEnumerator LerpToScale(Vector3 target, float duration)
    {
        Vector3 startScale = myTransform.localScale;
        float time = 0f;

        Color startColor = myImg.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            myTransform.localScale = Vector3.Lerp(startScale, target, t);
            myImg.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        myTransform.localScale = target;
    }
}
