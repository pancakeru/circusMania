using TMPro;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public enum MechanicNumberType
{
    None, 
    Power, 
    WarmUp, 
    Excited, 
}

public class MechanicNumberController : MonoBehaviour
{
    [HideInInspector] public AbstractSpecialAnimal myAnimalBrain;
    public List<Sprite> backSprites = new List<Sprite>();

    public TextMeshPro text;
    public SpriteRenderer spriteRenderer;
    Vector3 initialLocalScl;

    public void Begin() 
    {
        text = transform.GetChild(0).GetComponent<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialLocalScl = transform.localScale;

        //Initiate
        switch (myAnimalBrain.animalInfo.mechanicNumberType)
        {
            case MechanicNumberType.Power:
                spriteRenderer.sprite = backSprites[0];
                text.text = myAnimalBrain.animalInfo.power.ToString();
                break;

            case MechanicNumberType.WarmUp:
                spriteRenderer.sprite = backSprites[1];
                text.text = myAnimalBrain.animalInfo.warmUp.ToString();
                break;

            case MechanicNumberType.Excited:
                spriteRenderer.sprite = backSprites[2];
                text.text = myAnimalBrain.animalInfo.excited.ToString();
                break;

            default:
                spriteRenderer.sprite = backSprites[3];
                text.text = "";
                break;
        }

        //Do sth at the beginning
        switch (myAnimalBrain.animalInfo.mechanicNumberType)
        {
            case MechanicNumberType.Power:

                StartEffectImpact(false);

                break;

            case MechanicNumberType.WarmUp:

                StartEffectImpact(false);

                break;

            case MechanicNumberType.Excited:

                StartEffectDeath();

                break;
        }
    }

    void EffectReset()
    {
        transform.localScale = initialLocalScl;
        spriteRenderer.color = Color.white;
        text.color = Color.black;
    }
    public void StartEffectImpact(bool isDead)
    {
        EffectReset();
        StartCoroutine(EffectImpact(0.5f, isDead));
    }
    public void StartEffectDeath()
    {
        StartCoroutine(EffectDeath(0.5f));
    }
    IEnumerator EffectImpact(float duration, bool isDead)
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = initialScale * 2f;
        float halfDuration = duration / 2f;  
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;

        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            transform.localScale = Vector3.Lerp(targetScale, initialScale, t);
            yield return null;
        }

        transform.localScale = initialScale;

        if (isDead) StartEffectDeath();
    }
    IEnumerator EffectDeath(float duration)
    {
        Color initialColorSprite = spriteRenderer.color;
        Color initialColorText = text.color;
        Color targetColor;  ColorUtility.TryParseHtmlString("#3D3D3D", out targetColor);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            spriteRenderer.color = Color.Lerp(initialColorSprite, targetColor, t);
            text.color = Color.Lerp(initialColorText, targetColor, t);

            yield return null;
        }

        spriteRenderer.color = targetColor;
        text.color = targetColor;
    }

    public void UpdateText()
    {
        switch (myAnimalBrain.animalInfo.mechanicNumberType)
        {
            case MechanicNumberType.Power:
                text.text = myAnimalBrain.animalInfo.power.ToString();
                break;

            case MechanicNumberType.WarmUp:
                text.text = myAnimalBrain.animalInfo.warmUp.ToString();
                break;

            case MechanicNumberType.Excited:
                text.text = myAnimalBrain.animalInfo.excited.ToString();
                break;

            default:
                text.text = "";
                break;
        }
    }

    public void CallSound() {
        GameObject audioObj = GameObject.FindWithTag("audio manager");
        audioObj.GetComponent<AudioManagerScript>().PlayUISound(audioObj.GetComponent<AudioManagerScript>().UI[2]);
    }
}

//Dropdown menu in hierachy
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MechanicNumberType))]
public class MechanicNumberTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.enumValueIndex = (int)(MechanicNumberType)EditorGUI.EnumPopup(position, label, (MechanicNumberType)property.enumValueIndex);
    }
}
#endif