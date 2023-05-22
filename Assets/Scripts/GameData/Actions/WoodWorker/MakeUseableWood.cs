using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeUseableWood : GOAPAction
{

    private bool hasWorkableWood = false;                         
    private LogPileComponent targetLogPile;       

    public MakeUseableWood()
    {
        addPrecondition("hasWorkableWood", false);  
        addEffect("hasWorkableWood", true);       
    }

    public override void reset()
    {
        hasWorkableWood = false;   
        targetLogPile = null;       
    }
    public override bool isDone()
    {
        return hasWorkableWood;
    }
    public override bool requiresInRange()
    {
        return true; 
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        LogPileComponent[] logs = (LogPileComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(LogPileComponent));
        LogPileComponent closest = null;
        float closestDist = 0;

        foreach (LogPileComponent log in logs)
        {
            if (log.numLogs > 0)
            {
                if (closest == null)
                {
                    closest = log;
                    closestDist = (log.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (log.gameObject.transform.position - agent.transform.position).magnitude;

                    if (dist < closestDist)
                    {
                        closest = log;
                        closestDist = dist;
                    }
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
        if (targetLogPile.numLogs > 0)
        {
            int LogsTaken;

            if (targetLogPile.numLogs < 3)
            {
                LogsTaken = targetLogPile.numLogs;
            }
            else
            {
                LogsTaken = 3;
            }
            targetLogPile.numLogs -= LogsTaken;


            hasWorkableWood = true;

            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));

            backpack.numFirewood += LogsTaken*3;

            return true;
        }
        else
        {
            return false;
        }
    }
}
