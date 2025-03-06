using TMPro;
using UnityEditor;
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

    MechanicNumber mechanicNumber;
    MechanicNumberControllerPack myPack;

    public void Begin() 
    {
        text = transform.GetChild(0).GetComponent<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialLocalScl = transform.localScale;

        //Initiate
        switch (myAnimalBrain.animalInfo.mechanicNumberType)
        {
            case MechanicNumberType.Power:
                mechanicNumber = new MechanicPower();
                spriteRenderer.sprite = backSprites[0];
                text.text = myAnimalBrain.animalInfo.power.ToString();
                break;

            case MechanicNumberType.WarmUp:
                mechanicNumber = new MechanicWarmUp();
                spriteRenderer.sprite = backSprites[1];
                text.text = myAnimalBrain.animalInfo.warmUp.ToString();
                break;

            case MechanicNumberType.Excited:
                mechanicNumber = new MechanicExcited();
                spriteRenderer.sprite = backSprites[2];
                text.text = myAnimalBrain.animalInfo.excited.ToString();
                break;

            default:
                spriteRenderer.sprite = backSprites[3];
                text.text = "";
                break;
        }

        myPack = new MechanicNumberControllerPack
        {
            myAnimalBrain = myAnimalBrain,
            myUIController = this,
        };

        //Do sth at the beginning
        switch (myAnimalBrain.animalInfo.mechanicNumberType)
        {
            case MechanicNumberType.Power:

                mechanicNumber.Active(myPack);

                break;

            case MechanicNumberType.WarmUp:

                mechanicNumber.Active(myPack);

                break;

            case MechanicNumberType.Excited:

                mechanicNumber.Deactive(myPack);

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


    public void Active()
    {
        if (mechanicNumber != null) 
            mechanicNumber.Active(myPack);
        //Debug.Log("ACTIVE");
    }

    public void Change()
    {
        if (mechanicNumber != null)
        {
            mechanicNumber.Change(myPack);
            Debug.Log("CHANGE");
        }
            
    }

    public void Deactive()
    {
        if (mechanicNumber != null)
            mechanicNumber.Deactive(myPack);
        //Debug.Log("DEACTIVE");
    }
}

public class MechanicNumberControllerPack
{
    public AbstractSpecialAnimal myAnimalBrain;
    public MechanicNumberController myUIController;
}

public abstract class MechanicNumber
{
    public abstract void Active(MechanicNumberControllerPack myPack);
    public abstract void Change(MechanicNumberControllerPack myPack);
    public abstract void Deactive(MechanicNumberControllerPack myPack);

}

public class MechanicPower : MechanicNumber
{
    public override void Active(MechanicNumberControllerPack myPack)
    {
        myPack.myUIController.text.text = myPack.myAnimalBrain.animalInfo.power.ToString();
        myPack.myUIController.StartEffectImpact(false);
    }
    public override void Change(MechanicNumberControllerPack myPack)
    {
        myPack.myUIController.text.text = myPack.myAnimalBrain.animalInfo.power.ToString();
    }
    public override void Deactive(MechanicNumberControllerPack myPack)
    {
        myPack.myUIController.text.text = myPack.myAnimalBrain.animalInfo.power.ToString();
        myPack.myUIController.StartEffectDeath();
    }
}

public class MechanicWarmUp : MechanicNumber
{
    public override void Active(MechanicNumberControllerPack myPack)
    {
        myPack.myUIController.text.text = myPack.myAnimalBrain.animalInfo.warmUp.ToString();
        myPack.myUIController.StartEffectImpact(false);
    }
    public override void Change(MechanicNumberControllerPack myPack)
    {
        myPack.myUIController.text.text = myPack.myAnimalBrain.animalInfo.warmUp.ToString();
    }
    public override void Deactive(MechanicNumberControllerPack myPack)
    {
        myPack.myUIController.text.text = myPack.myAnimalBrain.animalInfo.warmUp.ToString();
        myPack.myUIController.StartEffectImpact(true);
    }
}

public class MechanicExcited : MechanicNumber
{
    public override void Active(MechanicNumberControllerPack myPack)
    {
        myPack.myUIController.text.text = myPack.myAnimalBrain.animalInfo.excited.ToString();
        myPack.myUIController.StartEffectImpact(false);
    }
    public override void Change(MechanicNumberControllerPack myPack)
    {
        myPack.myUIController.text.text = myPack.myAnimalBrain.animalInfo.excited.ToString();
    }
    public override void Deactive(MechanicNumberControllerPack myPack)
    {
        myPack.myUIController.text.text = myPack.myAnimalBrain.animalInfo.excited.ToString();
        myPack.myUIController.StartEffectDeath();
    }
}

//Dropdown menu in hierachy
[CustomPropertyDrawer(typeof(MechanicNumberType))]
public class MechanicNumberTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.enumValueIndex = (int)(MechanicNumberType)EditorGUI.EnumPopup(position, label, (MechanicNumberType)property.enumValueIndex);
    }
}