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
    [HideInInspector] public MechanicNumberType mechanicNumberType;
    [HideInInspector] public AnimalInfoPack myAnimalInfo;
    public List<Sprite> backSprites = new List<Sprite>();

    TextMeshPro text;
    SpriteRenderer spriteRenderer;

    MechanicNumber mechanicNumber;
    MechanicNumberControllerPack myPack;

    public void Begin() 
    {
        text = transform.GetChild(0).GetComponent<TextMeshPro>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        myPack = new MechanicNumberControllerPack
        {
            animalInfo = myAnimalInfo,
            text = text,
            spriteRenderer = spriteRenderer
        };

        switch (mechanicNumberType)
        {
            case MechanicNumberType.Power:
                mechanicNumber = new MechanicPower();
                spriteRenderer.sprite = backSprites[0];
                text.text = myAnimalInfo.power.ToString();
                break;

            case MechanicNumberType.WarmUp:
                myAnimalInfo.warmUp = myAnimalInfo.mechanicActiveNum;
                mechanicNumber = new MechanicWarmUp();
                spriteRenderer.sprite = backSprites[1];
                text.text = myAnimalInfo.warmUp.ToString();
                break;

            case MechanicNumberType.Excited:
                myAnimalInfo.excited = myAnimalInfo.mechanicActiveNum;
                mechanicNumber = new MechanicExcited();
                spriteRenderer.sprite = backSprites[2];
                text.text = myAnimalInfo.excited.ToString();
                break;

            default:
                spriteRenderer.sprite = backSprites[3];
                text.text = "";
                break;
        }
    }

    void Start() //Do sth at the beginning
    {
        switch (mechanicNumberType)
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

    }

    public void Change(int changeValue)
    {
        if (mechanicNumber != null)
            mechanicNumber.Change(myPack, changeValue);
    }

    public void Deactive()
    {
        if (mechanicNumber != null)
            mechanicNumber.Deactive(myPack);
    }
}

public class MechanicNumberControllerPack
{
    public AnimalInfoPack animalInfo;
    public TextMeshPro text;
    public SpriteRenderer spriteRenderer;
}

public abstract class MechanicNumber
{
    public abstract void Active(MechanicNumberControllerPack myPack);
    public abstract void Change(MechanicNumberControllerPack myPack, int changeValue);
    public abstract void Deactive(MechanicNumberControllerPack myPack);

}

public class MechanicPower : MechanicNumber
{
    public override void Active(MechanicNumberControllerPack myPack)
    {

    }
    public override void Change(MechanicNumberControllerPack myPack, int changeValue)
    {
        myPack.animalInfo.power += changeValue;
        myPack.text.text = myPack.animalInfo.power.ToString();
    }
    public override void Deactive(MechanicNumberControllerPack myPack)
    {

    }
}

public class MechanicWarmUp : MechanicNumber
{
    public override void Active(MechanicNumberControllerPack myPack)
    {
        myPack.animalInfo.warmUp = myPack.animalInfo.mechanicActiveNum;
    }
    public override void Change(MechanicNumberControllerPack myPack, int changeValue)
    {
        Debug.Log($"{myPack.animalInfo.warmUp} + {changeValue}");
        myPack.animalInfo.warmUp += changeValue;
        myPack.text.text = myPack.animalInfo.power.ToString();
        if (myPack.animalInfo.excited == 0)
        {
            Deactive(myPack);
        }
    }
    public override void Deactive(MechanicNumberControllerPack myPack)
    {

    }
}

public class MechanicExcited : MechanicNumber
{
    public override void Active(MechanicNumberControllerPack myPack)
    {
        myPack.animalInfo.excited = myPack.animalInfo.mechanicActiveNum;
        myPack.text.text = myPack.animalInfo.power.ToString();
    }
    public override void Change(MechanicNumberControllerPack myPack, int changeValue)
    {
        myPack.animalInfo.excited += changeValue;
        myPack.text.text = myPack.animalInfo.power.ToString();
        if (myPack.animalInfo.excited == 0) Deactive(myPack);
    }
    public override void Deactive(MechanicNumberControllerPack myPack)
    {

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