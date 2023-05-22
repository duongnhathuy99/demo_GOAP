using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookUncookedFood : GOAPAction
{

    private bool hasCookedFood = false;                            
    private CookingKitchenComponent targetCookingKitChen;                 

    private float startTime = 0;
    public float cookingDuration = 3;   

    public CookUncookedFood()
    {
        addPrecondition("hasUncookedFood", true);  
        addEffect("hasCookedFood", true);          
    }

    public override void reset()
    {
        hasCookedFood = false;     
        targetCookingKitChen = null;  
        startTime = 0;         
    }

    public override bool isDone()
    {
        return hasCookedFood;
    }

    public override bool requiresInRange()
    {
        return true; 
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        CookingKitchenComponent[] cookingKitchens = (CookingKitchenComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(CookingKitchenComponent));
        CookingKitchenComponent closest = null;
        float closestDist = 0;
        foreach (CookingKitchenComponent cookingKitchen in cookingKitchens)
        {
            if (closest == null)
            {
                closest = cookingKitchen;
                closestDist = (cookingKitchen.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (cookingKitchen.gameObject.transform.position - agent.transform.position).magnitude;

                if (dist < closestDist)
                {
                    closest = cookingKitchen;
                    closestDist = dist;
                }
            }
     
        }

        if (closest == null)
        {
            return false;
        }

        targetCookingKitChen = closest;
        target = targetCookingKitChen.gameObject;

        return closest != null;

    }

    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
            targetCookingKitChen.isCooking = true;
        }

        if (Time.time - startTime > cookingDuration)
        {
            int FoodCooked;
            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            targetCookingKitChen.isCooking = false;
            FoodCooked = backpack.numUncookedFood;
            backpack.numUncookedFood -= FoodCooked;
            backpack.numCookedFood += FoodCooked;

            hasCookedFood = true;

        }
        return true;
    }
}
