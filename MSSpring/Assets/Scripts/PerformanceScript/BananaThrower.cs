using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaThrower : MonoBehaviour
{
    public GameObject bananaPrefab;
    public int maxBanana = 20;
    private int curBanana = 20;

    private bool showStart;

    // Start is called before the first frame update
    void Start()
    {
        curBanana = maxBanana;
    }

    public void ShowStart(bool ifStart)
    {
        showStart = ifStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (!showStart)
        {
            Debug.Log("没开始呢");
            return;
        }
            
        if (Input.GetMouseButtonDown(0))
        {
            if (curBanana <= 0)
                return;
            // 获取鼠标点击位置
            Vector3 mousePosition = Input.mousePosition;

            // 将鼠标屏幕坐标转换为世界坐标
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));

            // 调用 throwBanana
            throwBanana(new Vector3(worldPosition.x, worldPosition.y, 0));


        }
    }

    void throwBanana(Vector3 pos)
    {
        BananaScript banana = Instantiate(bananaPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity).GetComponent<BananaScript>();
        banana.ThrowObject(pos);
        curBanana -= 1;

        //TODO:改变ui显示的数量
        //animalManager.Instance.curLeft.changeLeft(curBanana);
    }

    public void reportUiReach()
    {
        //这个是之前ui移动的，这里用不到
        //animalManager.Instance.reportReachShow_banana();
    }

    public void changeCount(int n)
    {
        curBanana = n;
        //TODO:改变ui显示的数量
        //animalManager.Instance.curLeft.changeLeft(curBanana);
    }

    public void changeCount()
    {
        curBanana = maxBanana;
        //这个目前好像不需要，因为有继承
        //animalManager.Instance.curLeft.changeLeft(curBanana);
    }

    public void addBanana(int n)
    {
        curBanana += n;
        //TODO:改变ui显示的数量
        //animalManager.Instance.curLeft.changeLeft(curBanana);
    }
}
