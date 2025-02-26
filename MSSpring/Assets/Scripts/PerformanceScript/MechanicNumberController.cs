using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum MechanicNumberType
{
    None, 
    Power, 
    WarmUp, 
    Excited, 
}

public class MechanicNumberController : MonoBehaviour
{
    public MechanicNumberType myMechanic;
    public Image backImage;
    public TextMeshProUGUI text;

    public AnimalInfoPack myAnimalInfo;

    MechanicNumber myMechanicNumber;

    void Awake() //Reset
    {
        switch (myMechanic)
        {
            case MechanicNumberType.Power:
                myMechanicNumber = new MechanicPower();
                break;

            case MechanicNumberType.WarmUp:
                myMechanicNumber = new MechanicWarmUp();
                break;

            case MechanicNumberType.Excited:
                myMechanicNumber = new MechanicExcited();
                break;
        }
    }

    void Start() //Do sth at the beginning
    {
        switch (myMechanic)
        {
            case MechanicNumberType.Power:

                myMechanicNumber.Active();

                break;

            case MechanicNumberType.WarmUp:

                myMechanicNumber.Active();

                break;

            case MechanicNumberType.Excited:

                myMechanicNumber.Deactive();

                break;
        }
    }

    public void Active()
    {
        if (myMechanicNumber != null) 
            myMechanicNumber.Active();

    }

    public void Change(int changeValue)
    {
        if (myMechanicNumber != null)
            myMechanicNumber.Change(changeValue);
    }

    public void Deactive()
    {
        if (myMechanicNumber != null)
            myMechanicNumber.Deactive();
    }
}

public abstract class MechanicNumber
{
    public abstract void Active();
    public abstract void Change(int changeValue);
    public abstract void Deactive();

}

public class MechanicPower : MechanicNumber
{
    public override void Active()
    {

    }
    public override void Change(int changeValue)
    {

    }
    public override void Deactive()
    {

    }
}

public class MechanicWarmUp : MechanicNumber
{
    public override void Active()
    {

    }
    public override void Change(int changeValue)
    {

    }
    public override void Deactive()
    {

    }
}

public class MechanicExcited : MechanicNumber
{
    public override void Active()
    {

    }
    public override void Change(int changeValue)
    {

    }
    public override void Deactive()
    {

    }
}

//Dropdown menu in hierachy
[CustomEditor(typeof(MechanicNumberController))]
public class MechanicNumberEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MechanicNumberController mechanicNumberController = (MechanicNumberController)target;

        mechanicNumberController.myMechanic = (MechanicNumberType)EditorGUILayout.EnumPopup("myMechanic", mechanicNumberController.myMechanic);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
