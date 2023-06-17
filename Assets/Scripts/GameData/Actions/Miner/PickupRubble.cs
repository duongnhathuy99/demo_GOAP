using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRubble : GOAPAction
{
    private bool pickedup = false;       
    private RubbleComponent targetRubble; 

    private float startTime = 0;           
    public float gatheringDuration = 2;  
    public PickupRubble()
    {
        addPrecondition("hasOre", false);  
        addEffect("hasOre", true);        
    }
    public override void reset()
    {
        pickedup = false;   
        targetRubble= null;  
        startTime = 0;  
    }
    public override bool isDone()
    {
        return pickedup;
    }
    public override bool requiresInRange()
    {
        return true;   
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        RubbleComponent[] rubbles = FindObjectsOfType(typeof(RubbleComponent)) as RubbleComponent[];
        RubbleComponent closest = null;
        float closestDist = 0;

        foreach (RubbleComponent rubble in rubbles)
        {
            if (closest == null)
            {
                closest = rubble;
                closestDist = (rubble.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (rubble.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    closest = rubble;
                    closestDist = dist;
                }
            }
        }
        targetRubble = closest;
        target = targetRubble.gameObject;

        return closest != null;
    }
    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > gatheringDuration)
        {
            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            backpack.numOre += 3;
            pickedup = true;

        }
        return true;
    }
}
