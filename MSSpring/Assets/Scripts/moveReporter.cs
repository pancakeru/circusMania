using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveReporter : MonoBehaviour
{
    public GameObject reportReceiver;
    public void Report()
    {
        if (reportReceiver.GetComponent<IReportReceiver>() != null)
            reportReceiver.GetComponent<IReportReceiver>().reportFinish();
    }
}

public interface IReportReceiver
{
    void reportFinish();
}
