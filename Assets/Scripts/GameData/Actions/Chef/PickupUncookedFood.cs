using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupUncookedFood : GOAPAction
{

    private bool hasUncookedFood = false; 
    private FridgeComponent targetFridge;
    public PickupUncookedFood()
    {
        addPrecondition("hasUncookedFood", false);
        addEffect("hasUncookedFood", true);       
    }

    public override void reset()
    {
        hasUncookedFood = false;   
        targetFridge = null;    
    }

    public override bool isDone()
    {
       
        return hasUncookedFood;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        FridgeComponent[] fridges = (FridgeComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(FridgeComponent));

        FridgeComponent closest = null;
        float closestDist = 0;

        foreach (FridgeComponent fridge in fridges)
        {
            if (fridge.NumUncookedFood > 0)
            {
                if (closest == null)
                {
                    closest = fridge;
                    closestDist = (fridge.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (fridge.gameObject.transform.position - agent.transform.position).magnitude;

                    if (dist < closestDist)
                    {
                        closest = fridge;
                        closestDist = dist;
                    }
                }
            }
        }

        if (closest == null)
        {
            return false;
        }

        targetFridge = closest;
        target = targetFridge.gameObject;

        return closest != null;

    }
    public override bool perform(GameObject agent)
    {
        if (targetFridge.NumUncookedFood > 0)
        {
            int FoodTaken;

            if(targetFridge.NumUncookedFood <3)
            {
                FoodTaken = targetFridge.NumUncookedFood;
            }
            else
            {
                FoodTaken = 3;
            }
            targetFridge.NumUncookedFood -= FoodTaken;
            hasUncookedFood = true;

            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));

            backpack.numUncookedFood += FoodTaken;

            return true;
        }
        else
        {
            return false;
        }
    }
}
