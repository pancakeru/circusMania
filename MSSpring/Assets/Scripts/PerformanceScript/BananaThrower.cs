using UnityEngine;
using TMPro;

public class BananaThrower : MonoBehaviour
{
    public GameObject bananaPrefab;
    public int maxBanana = 20;
    private int curBanana = 20;

    private bool showStart;
    private ShowManager selfManager;


    [SerializeField] private TextMeshProUGUI InDecisionText;
    [SerializeField] private TextMeshProUGUI InShowText;

    // Start is called before the first frame update
    void Start()
    {
        /*
        curBanana = maxBanana;
        if(InDecisionText!= null)InDecisionText.text = curBanana.ToString();
        if (InShowText != null) InShowText.text = curBanana.ToString();
        */
    }

    public void ShowStart(bool ifStart, ShowManager manager)
    {
        showStart = ifStart;
        if (ifStart)
        {
            selfManager = manager;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!showStart)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && selfManager.GetIfBananaEnabled())
        {
            // 获取鼠标点击位置
            Vector3 mousePosition = Input.mousePosition;
            if (curBanana <= 0 || mousePosition.y > 540)
                return;

            // 将鼠标屏幕坐标转换为世界坐标
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));

            // 调用 throwBanana
            throwBanana(new Vector3(worldPosition.x, worldPosition.y, 0));


        }
    }

    void throwBanana(Vector3 pos)
    {
        if (isInPause)
            return;
        BananaScript banana = Instantiate(bananaPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity).GetComponent<BananaScript>();
        banana.ThrowObject(pos);
        curBanana -= 1;

        if (InDecisionText != null) InDecisionText.text = curBanana.ToString();
        if (InShowText != null) InShowText.text = curBanana.ToString();
        //animalManager.Instance.curLeft.changeLeft(curBanana);

        GameObject audioObj = GameObject.FindWithTag("audio manager");
        audioObj.GetComponent<AudioManagerScript>().PlayBattleSound(audioObj.GetComponent<AudioManagerScript>().Battle[3]);
    }

    public void reportUiReach()
    {
        //这个是之前ui移动的，这里用不到
        //animalManager.Instance.reportReachShow_banana();
    }

    public void changeCount(int n)
    {

        curBanana = n;
        if (InDecisionText != null) InDecisionText.text = curBanana.ToString();
        if (InShowText != null) InShowText.text = curBanana.ToString();

    }

    public void changeCount()
    {

        curBanana = maxBanana;
        if (InDecisionText != null) InDecisionText.text = curBanana.ToString();
        if (InShowText != null) InShowText.text = curBanana.ToString();

    }

    public void addBanana(int n)
    {
        //Debug.Log("添加了"+n);
        curBanana += n;
        if (InDecisionText != null) InDecisionText.text = curBanana.ToString();
        if (InShowText != null) InShowText.text = curBanana.ToString();

    }

    public int takeBanana(int expectedNum)
    {
        if (!ShowManager.instance.GetIfBananaEnabled())
            return 0;
        int toTake = curBanana >= expectedNum ? expectedNum : curBanana;
        if (curBanana - expectedNum < 0)
        {
            toTake = 0;
        }
        curBanana -= toTake;
        if (InDecisionText != null) InDecisionText.text = curBanana.ToString();
        if (InShowText != null) InShowText.text = curBanana.ToString();
        return toTake;

    }

    private bool isInPause = false;
    public void SwitchThrowEnableWhenPause(bool ifEnable) => isInPause = !ifEnable;
}
