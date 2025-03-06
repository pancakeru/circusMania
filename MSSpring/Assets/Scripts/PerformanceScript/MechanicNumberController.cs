using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

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

    TextMeshPro text;
    SpriteRenderer spriteRenderer;

    MechanicNumber mechanicNumber;
    MechanicNumberControllerPack myPack;

    public void Begin() 
    {
        text = transform.GetChild(0).GetComponent<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
            spriteRenderer = spriteRenderer,
            text = text,
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
    public TextMeshPro text;
    public SpriteRenderer spriteRenderer;
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
        myPack.text.text = myPack.myAnimalBrain.animalInfo.power.ToString();
    }
    public override void Change(MechanicNumberControllerPack myPack)
    {
        myPack.text.text = myPack.myAnimalBrain.animalInfo.power.ToString();
        Debug.Log(myPack.myAnimalBrain.animalInfo.power.ToString());
    }
    public override void Deactive(MechanicNumberControllerPack myPack)
    {
        myPack.text.text = myPack.myAnimalBrain.animalInfo.power.ToString();
    }
}

public class MechanicWarmUp : MechanicNumber
{
    public override void Active(MechanicNumberControllerPack myPack)
    {
        myPack.text.text = myPack.myAnimalBrain.animalInfo.warmUp.ToString();
    }
    public override void Change(MechanicNumberControllerPack myPack)
    {
        myPack.text.text = myPack.myAnimalBrain.animalInfo.warmUp.ToString();
    }
    public override void Deactive(MechanicNumberControllerPack myPack)
    {
        myPack.text.text = myPack.myAnimalBrain.animalInfo.warmUp.ToString();
    }
}

public class MechanicExcited : MechanicNumber
{
    public override void Active(MechanicNumberControllerPack myPack)
    {
        myPack.text.text = myPack.myAnimalBrain.animalInfo.excited.ToString();
    }
    public override void Change(MechanicNumberControllerPack myPack)
    {
        myPack.text.text = myPack.myAnimalBrain.animalInfo.excited.ToString();
    }
    public override void Deactive(MechanicNumberControllerPack myPack)
    {
        myPack.text.text = myPack.myAnimalBrain.animalInfo.excited.ToString();
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