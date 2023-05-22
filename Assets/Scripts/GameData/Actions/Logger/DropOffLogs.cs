using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffLogs : GOAPAction
{

    private bool droppedOffLogs = false;    
    private LogPileComponent targetLogPile;   

    public DropOffLogs()
    {
        addPrecondition("hasLogs", true);   
        addEffect("hasLogs", false);       
        addEffect("collectLogs", true);   
    }
    public override void reset()
    {
        droppedOffLogs = false;   
        targetLogPile = null;   
    }
    public override bool isDone()
    {
        return droppedOffLogs;
    }
    public override bool requiresInRange()
    {
        return true; 
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        LogPileComponent[] logPiles = (LogPileComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(LogPileComponent));
        LogPileComponent closest = null;
        float closestDist = 0;

        foreach (LogPileComponent logPile in logPiles)
        {
            if (closest == null)
            {
                closest = logPile;
                closestDist = (logPile.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (logPile.gameObject.transform.position - agent.transform.position).magnitude;

                if (dist < closestDist)
                {
                    closest = logPile;
                    closestDist = dist;
                }
            }
        }

        if (closest == null)
        {
            return false;
        }

        targetLogPile = closest;
        target = targetLogPile.gameObject;

        return closest != null;
    }

    public override bool perform(GameObject agent)
    {
        BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
        targetLogPile.numLogs += backpack.numLogs;
        droppedOffLogs = true;
        backpack.numLogs = 0;

        return true;
    }
}
