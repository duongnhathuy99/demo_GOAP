using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepTimer : MonoBehaviour {

    public float maxSleepTime = 1f;
    public float sleepTime;
    public float tirednessPerFrame;

    public bool isTired;

    void Start ()
    {
        isTired = false;
        sleepTime = maxSleepTime;
	}
	
	void Update ()
    {
        if (sleepTime > 0)
        {
            isTired = false;
            sleepTime -= tirednessPerFrame * Time.deltaTime;
        }
        if(sleepTime<=0)
        {
            isTired = true;
        }
	}
}
