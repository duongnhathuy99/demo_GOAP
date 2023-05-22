using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWorkableWood : GOAPAction
{

    private bool hasWorkableWood = false;         
    private UseableWoodComponent targetWood;   
    public PickupWorkableWood()
    {
        addPrecondition("hasWorkableWood", false);  
        addEffect("hasWorkableWood", true);       
    }
    public override void reset()
    {
        hasWorkableWood = false;  
        targetWood = null;         
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
        UseableWoodComponent[] woodPiles = (UseableWoodComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(UseableWoodComponent));
        UseableWoodComponent closest = null;
        float closestDist = 0;

        foreach (UseableWoodComponent woodPile in woodPiles)
            if (woodPile.numUsableWood > 0)
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

        targetWood = closest;
        target = targetWood.gameObject;

        return closest != null;

    }
    public override bool perform(GameObject agent)
    {

        if (targetWood.numUsableWood > 0)
        {
            int WoodTaken;

            if (targetWood.numUsableWood < 2)
            {
                WoodTaken = targetWood.numUsableWood;
            }
            else
            {
                WoodTaken = 2;
            }


            targetWood.numUsableWood -= WoodTaken;

            hasWorkableWood = true;

            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));

            backpack.numFirewood+=WoodTaken;

            return true;
        }
        else
        {
            return false;
        }
    }
}
