using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject button;
    public Image image;
    public List<Sprite> MenuSprites = new List<Sprite>();
    public List<Sprite> troupeSprites = new List<Sprite>();
    public List<Sprite> showSprites = new List<Sprite>();
    public Sprite blank;

    List<List<Sprite>> sprites = new List<List<Sprite>>();
    bool isActive;
    int pageNum = -1;

    [Header("ID: 1.menu, 2.troupe, 3.show")]
    public int id;

    void Start()
    {
        if (id == 0) Debug.LogError("Assign ID");
        sprites.Add(MenuSprites);
        sprites.Add(troupeSprites);
        sprites.Add(showSprites);

        image.raycastTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && Input.GetKeyUp(KeyCode.Mouse0))
        {
            ShowTutorial();
        }
    }

    public void Activate()
    {
        isActive = true;
        pageNum = -1;
        image.raycastTarget = true;
        CanvasMain.DisableUIInteraction();
    }

    void ShowTutorial()
    {
        pageNum++;
        if (pageNum >= sprites[id - 1].Count)
        {
            pageNum = -1;
            isActive = false;
            image.sprite = blank;
            image.raycastTarget = false;
            CanvasMain.EnableUIInteraction();
            return;
        }
        image.sprite = sprites[id - 1][pageNum];
    }
}
