using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupIronOre : GOAPAction
{

    private bool hasIronOre = false;                
    private ToolChestComponent targetChest;         

    public PickupIronOre()
    {
        addPrecondition("hasOre", false);
        addEffect("hasOre", true);     
    }

    public override void reset()
    {
        hasIronOre = false;   
        targetChest = null;   
    }
  
    public override bool isDone()
    {
      
        return hasIronOre;
    }
    public override bool requiresInRange()
    {
        return true; 
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        ToolChestComponent[] chests = (ToolChestComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(ToolChestComponent));
        ToolChestComponent closest = null;
        float closestDist = 0;

        foreach (ToolChestComponent chest in chests)
            if (chest.NumIronOre > 0)
            {
                if (closest == null)
                {
                    closest = chest;
                    closestDist = (chest.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (chest.gameObject.transform.position - agent.transform.position).magnitude;

                    if (dist < closestDist)
                    {
                        closest = chest;
                        closestDist = dist;
                    }
                }
            }

        if (closest == null)
        {
            return false;
        }

        targetChest = closest;
        target = targetChest.gameObject;

        return closest != null;

    }

    public override bool perform(GameObject agent)
    {

        if (targetChest.NumIronOre>0)
        {
            int IronTaken;

            if (targetChest.NumIronOre<3)
            {
                IronTaken = targetChest.NumIronOre;
            }
            else
            {
                IronTaken = 3;
            }


            targetChest.NumIronOre -= IronTaken;


            hasIronOre = true;

            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));

            backpack.numOre += IronTaken;

            return true;
        }
        else
        {
            return false;
        }
    }
}
