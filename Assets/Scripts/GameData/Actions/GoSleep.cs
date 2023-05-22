using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoSleep : GOAPAction
{

    private bool hasSlept = false;                  
    private BedComponent targetBed;          

    private float startTime = 0;      
    public float sleepingDuration = 5;   

    bool Sleeping = false;

    public GoSleep()
    {
        addPrecondition("isTired", true); 
        addEffect("isTired", false);      
        addEffect("hasSlept", true);
    }
    public override void reset()
    {
        Sleeping = false;
        startTime = 0;     
        hasSlept = false;  
            
        targetBed = null;  
    }
    public override bool isDone()
    {
        
        return hasSlept;
    }

    public override bool requiresInRange()
    {
        return true; 
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        BedComponent[] beds = (BedComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(BedComponent));
        BedComponent closest = null;
        float closestDist = 0;

        foreach (BedComponent bed in beds)
        {
            if (bed.isFree)
            {
                if (closest == null)
                {
                    closest = bed;
                    closestDist = (bed.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (bed.gameObject.transform.position - agent.transform.position).magnitude;

                    if (dist < closestDist)
                    {
                        closest = bed;
                        closestDist = dist;
                    }
                }
            }
        }

        if (closest == null)
        {
            return false;
        }

        targetBed = closest;
        target = targetBed.gameObject;

        return closest != null;

    }
    public override bool perform(GameObject agent)
    { 
        if (targetBed.isFree||Sleeping==true)
        {
            Sleeping = true;
            targetBed.isFree = false;
            if (startTime == 0)
            {
                startTime = Time.time;
            }

            if (Time.time - startTime > sleepingDuration)
            {
                hasSlept = true;

                SleepTimer sleepTimer = (SleepTimer)agent.GetComponent(typeof(SleepTimer));

                sleepTimer.sleepTime = sleepTimer.maxSleepTime;

                targetBed.isFree = true;
            }
            return true;
        }

        else
        {
            return false;
        }
    }
}
