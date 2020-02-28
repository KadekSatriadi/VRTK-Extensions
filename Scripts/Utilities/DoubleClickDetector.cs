using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleClickDetector 
{
    public Action OnDoubleClick = delegate { };
    private DateTime lastClicked = DateTime.MinValue;
    private float intervalMiliseconds;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="intervalMilS">recommended 300</param>
    public DoubleClickDetector(float intervalMilS)
    {
        intervalMiliseconds = intervalMilS;
        lastClicked = DateTime.MinValue;
    }

    public void Click()
    {
       
        DateTime current = DateTime.Now;
        double time = (current - lastClicked).TotalMilliseconds;

      //  Debug.Log("Time " + time);

        if (time <= intervalMiliseconds)
        {
           // Debug.Log("Double click");
            lastClicked = DateTime.MinValue;
            OnDoubleClick.Invoke();
        }
        else
        {
           // Debug.Log("Single click");
            lastClicked = DateTime.Now;
        }
        
    }

}
