using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffUseableWood : GOAPAction
{

    private bool droppedOffWood = false;       
    private UseableWoodComponent targetWoodPile;    

    private float startTime = 0;     
    public float makingDuration = 3; 
    public DropOffUseableWood()
    {
        addPrecondition("hasWorkableWood", true);
        addEffect("hasWorkableWood", false);       
        addEffect("collectWorkableWood", true);   
    }
    public override void reset()
    {
        startTime = 0;
        droppedOffWood = false;    
        targetWoodPile = null;   
    }
    public override bool isDone()
    {
        return droppedOffWood;
    }
    public override bool requiresInRange()
    {
        return true;
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        UseableWoodComponent[] woodPiles = (UseableWoodComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(UseableWoodComponent));
        UseableWoodComponent closest = null;
        float closestDist = 0;

        foreach (UseableWoodComponent woodPile in woodPiles)
        {
            if (closest == null)
            {
                closest = woodPile;
                closestDist = (woodPile.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (woodPile.gameObject.transform.position - agent.transform.position).magnitude;

                if (dist < closestDist)
                {
                    closest = woodPile;
                    closestDist = dist;
                }
            }

        }

        if (closest == null)
        {
            return false;
        }

        targetWoodPile = closest;
        target = targetWoodPile.gameObject;
        
        return closest != null;

    }
    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > makingDuration)
        {
            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            targetWoodPile.numUsableWood += backpack.numFirewood;
            droppedOffWood = true;
            backpack.numFirewood = 0;
        }
        return true;
    }
}
