using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoEat : GOAPAction
{

    private bool hasAte = false;
    //private FridgeComponent targetFridge;
    private DinnerTableComponent dinnerTable;
    private ChairComponent targetChair;

    private float startTime = 0;      
    public float eatingDuration = 5;

    bool eating = false;
    public GoEat()
    {
        addPrecondition("isHungry", true);  
        addEffect("isHungry", false);      
        addEffect("hasAte", true);
    }
    public override void reset()
    {
        eating = false;
        startTime = 0;    
        hasAte = false;
        targetChair = null;   
    }

    public override bool isDone()
    {
        return hasAte;
    }

    public override bool requiresInRange()
    {
        return true; 
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        ChairComponent[] chairs = (ChairComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(ChairComponent));
        dinnerTable = (DinnerTableComponent)UnityEngine.GameObject.FindObjectOfType(typeof(DinnerTableComponent));
        ChairComponent closest = null;
        float closestDist = 0;

        foreach (ChairComponent chair in chairs)
        {
            if (dinnerTable.NumCookedFood > 0 && chair.isFree)
            {
                if (closest == null)
                {
                    closest = chair;
                    closestDist = (chair.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (chair.gameObject.transform.position - agent.transform.position).magnitude;

                    if (dist < closestDist)
                    {
                        closest = chair;
                        closestDist = dist;
                    }
                }
            }
        }

        if (closest == null)
        {

            return false;
        }

        targetChair = closest;
        target = targetChair.gameObject;

        return closest != null;

    }

    public override bool perform(GameObject agent)
    {
        if ((dinnerTable.NumCookedFood > 0 && targetChair.isFree) || eating)
        {
            eating = true;
            targetChair.isFree = false;
            if (startTime == 0)
            {
                startTime = Time.time;
            }

            if (Time.time - startTime > eatingDuration)
            {
                dinnerTable.NumCookedFood -= 1;
                hasAte = true;

                HungerTimer hungerTimer = (HungerTimer)agent.GetComponent(typeof(HungerTimer));

                hungerTimer.EatTime = hungerTimer.maxEatTime;
                targetChair.isFree = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}
