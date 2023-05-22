using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropoffCookedFood : GOAPAction
{

    private bool droppedOffCookedFood = false;
    //private FridgeComponent targetFridge;
    private DinnerTableComponent targetDinnerTable;
    public DropoffCookedFood()
    {
        addPrecondition("hasCookedFood", true);
        addEffect("hasCookedFood", false);
        addEffect("collectCookedFood", true);
    }

    public override void reset()
    {
        droppedOffCookedFood = false;
        targetDinnerTable = null;
    }

    public override bool isDone()
    {
        return droppedOffCookedFood;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        DinnerTableComponent[] fridges = (DinnerTableComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(DinnerTableComponent));
        DinnerTableComponent closest = null;
        float closestDist = 0;

        foreach (DinnerTableComponent fridge in fridges)
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
        if (closest == null)
        {
            return false;
        }

        targetDinnerTable = closest;
        target = targetDinnerTable.gameObject;
        return closest != null;
    }
    public override bool perform(GameObject agent)
    {
        BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
        targetDinnerTable.NumCookedFood += backpack.numCookedFood;
        droppedOffCookedFood = true;
        backpack.numCookedFood = 0;

        return true;
    }
}
