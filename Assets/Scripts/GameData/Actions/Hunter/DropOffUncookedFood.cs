using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffUncookedFood : GOAPAction {

    private bool droppedOffUncookedFood = false;
    private FridgeComponent targetFridge;

    public DropOffUncookedFood()
    {
        addPrecondition("hasUncookedFood", true);
        addEffect("hasUncookedFood", false);
        addEffect("collectUncookedFood", true);
    }

    public override void reset()
    {
        droppedOffUncookedFood = false;
        targetFridge = null;
    }

    public override bool isDone()
    {
        return droppedOffUncookedFood;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {

        FridgeComponent[] fridges = (FridgeComponent[]) UnityEngine.GameObject.FindObjectsOfType(typeof(FridgeComponent));
        FridgeComponent closest = null;
        float closestDist = 0;

        foreach(FridgeComponent fridge in fridges)
        {
            if(closest==null)
            {
                closest = fridge;
                closestDist = (fridge.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (fridge.gameObject.transform.position - agent.transform.position).magnitude;
                if(dist<closestDist)
                {
                    closest = fridge;
                    closestDist = dist;
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
        BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
        targetFridge.NumUncookedFood += backpack.numUncookedFood;
        droppedOffUncookedFood = true;
        backpack.numUncookedFood = 0;

        return true;
    }
}
