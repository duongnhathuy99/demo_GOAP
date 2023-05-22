using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherBranches : GOAPAction
{
    private bool pickedup = false;           
    private BranchesComponent targetBranches;  

    private float startTime = 0;            
    public float gatheringDuration = 2; 

    public GatherBranches()
    {
        addPrecondition("hasLogs", false);  
        addEffect("hasLogs", true);        
    }
    public override void reset()
    {
        pickedup = false;     
        targetBranches = null;
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
        BranchesComponent[] branches = FindObjectsOfType(typeof(BranchesComponent)) as BranchesComponent[];
        BranchesComponent closest = null;
        float closestDist = 0;

        foreach (BranchesComponent branch in branches)
        {
            if (closest == null)
            {
                closest = branch;
                closestDist = (branch.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (branch.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    closest = branch;
                    closestDist = dist;
                }
            }
        }
        targetBranches = closest;
        target = targetBranches.gameObject;

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
            backpack.numLogs += 3;
            pickedup = true;

        }
        return true;
    }
}
