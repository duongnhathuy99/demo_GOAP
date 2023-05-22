using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntChicken : GOAPAction
{
    private bool hunted = false;        
    private ChickenComponent targetChicken;     

    private float startTime = 0;       
    public float huntingDuration = 3;   

    public HuntChicken()
    {
        addPrecondition("hasUncookedFood", false);  
        addEffect("hasUncookedFood", true);        
    }

    public override void reset()
    {
        hunted = false;       
        targetChicken = null;   
        startTime = 0;         
    }

    public override bool isDone()
    {
        return hunted;
    }

    public override bool requiresInRange()
    {
        return true;    
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        ChickenComponent[] chickens = FindObjectsOfType(typeof(ChickenComponent)) as ChickenComponent[];   
        ChickenComponent closest = null;                                                                  
        float closestDist = 0;                                                                             

        foreach (ChickenComponent chicken in chickens)      
        {
            if (closest == null)
            {
              
                closest = chicken;
                closestDist = (chicken.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (chicken.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    closest = chicken;
                    closestDist = dist;
                }
            }
        }
        targetChicken = closest;   
        target = targetChicken.gameObject;

        return closest != null;     
    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > huntingDuration)
        {
            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            backpack.numUncookedFood += 3;     
            hunted = true;
        }
        return true;
    }
}
