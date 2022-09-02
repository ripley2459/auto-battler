using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class NotNullVsReferenceEquals : MonoBehaviour
{
    GameObject testObj;

    // Use this for initialization
    void Start()
    {
        Stopwatch sw = new Stopwatch();

        sw.Reset();
        sw.Start();

        if (testObj != null)
        {
        }

        sw.Stop();
        UnityEngine.Debug.Log("!=null performance: " + sw.Elapsed.ToString());

        sw.Reset();
        sw.Start();

        if (!System.Object.ReferenceEquals(testObj, null))
        {
        }

        sw.Stop();
        UnityEngine.Debug.Log("ReferenceEqausl null performance: " + sw.Elapsed.ToString());

        sw.Reset();
        sw.Start();

        for (int i = 0; i < 10000; i++)
        {
            if (testObj != null)
            {
            }
        }

        sw.Stop();
        UnityEngine.Debug.Log("!=null performance X10000: " + sw.Elapsed.ToString());

        sw.Reset();
        sw.Start();

        for (int i = 0; i < 10000; i++)
        {
            if (!System.Object.ReferenceEquals(testObj, null))
            {
            }
        }

        sw.Stop();
        UnityEngine.Debug.Log("ReferenceEqausl null performance X10000: " + sw.Elapsed.ToString());
    }
}