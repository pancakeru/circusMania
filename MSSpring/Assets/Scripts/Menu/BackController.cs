using UnityEngine;

public class BackController : MonoBehaviour
{
    public void OnClick()
    {
        if (transform.parent.GetComponent<MenuController>() == null)
        {
            if (transform.parent.GetComponent<TroupeController>() != null) transform.parent.GetComponent<TroupeController>().Disable();
            if (transform.parent.GetComponent<MrShopManager>() != null) transform.parent.GetComponent<MrShopManager>().Disable();
        }
        else
        {
            GlobalManager.instance.SaveGlobalSaveData();
            transform.parent.GetComponent<MenuController>().startScreen.Enable();
        }
    }
}
