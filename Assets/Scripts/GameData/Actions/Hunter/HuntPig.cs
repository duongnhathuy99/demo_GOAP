using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntPig : GOAPAction
{
    private bool hunted = false;       
    private PigComponent targetPig;    

    private float startTime = 0;      
    public float huntingDuraion = 2;   

   
    public HuntPig()
    {
        addPrecondition("hasTool", true);          
        addPrecondition("hasUncookedFood", false); 
        addEffect("hasUncookedFood", true);       
    }

    public override void reset()
    {
        hunted = false;     
        targetPig = null; 
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
        PigComponent[] pigs = FindObjectsOfType(typeof(PigComponent)) as PigComponent[];  
        PigComponent closest = null;                                                       
        float closestDist = 0;                                                            
        foreach(PigComponent pig in pigs)       
        {
            if (closest == null)
            {
                closest = pig;
                closestDist = (pig.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (pig.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist<closestDist)
                {
                    closest = pig;
                    closestDist = dist;
                }
            }
        }
        targetPig = closest;    
        target = targetPig.gameObject;

        return closest != null;    
    }

    public override bool perform (GameObject agent)
    {
        if(startTime==0)
        {
            startTime = Time.time;
        }

        if(Time.time-startTime>huntingDuraion)
        {
            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            backpack.numUncookedFood += 5;      
            hunted = true;
            ToolComponent tool = backpack.tool.GetComponent(typeof(ToolComponent)) as ToolComponent;
   
            tool.use(0.34f);
            if(tool.destroyed())
            {
                Destroy(backpack.tool);
                backpack.tool = null;
            }
        }
        return true;
    }
}
