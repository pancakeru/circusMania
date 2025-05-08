using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class singleLevelSummary : MonoBehaviour
{

    public Slider red;
    public Slider yellow;
    public Slider blue;
    // Start is called before the first frame update

    public float transitionDuration = 0.5f; // 动画持续时间（秒）

    // 平滑设置三个 slider 的值
    public void SetSliderValues(float r, float y, float b)
    {
        StartCoroutine(SmoothSetSlider(red, r));
        StartCoroutine(SmoothSetSlider(yellow, y));
        StartCoroutine(SmoothSetSlider(blue, b));
    }

    private IEnumerator SmoothSetSlider(Slider slider, float targetValue)
    {
        float startValue = slider.value;
        float timer = 0f;

        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / transitionDuration);
            slider.value = Mathf.Lerp(startValue, targetValue, t);
            yield return null;
        }

        slider.value = targetValue; // 保证最后到达精确值
    }


}
