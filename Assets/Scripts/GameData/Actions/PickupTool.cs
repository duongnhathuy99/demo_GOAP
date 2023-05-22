using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTool : GOAPAction {

    private bool hasTool = false;                         
    private ToolChestComponent targetToolChest;          
    Quaternion toolRotation = Quaternion.Euler(0, 0, -45);  
    public PickupTool()
    {
        addPrecondition("hasTool", false);  
        addEffect("hasTool", true);      
    }
    public override void reset()
    {
        hasTool = false;   
        targetToolChest = null;   
    }
    public override bool isDone()
    {
        return hasTool;
    }
    public override bool requiresInRange()
    {
        return true; 
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        ToolChestComponent[] toolChests = (ToolChestComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(ToolChestComponent));
        ToolChestComponent closest = null;
        float closestDist = 0;

        foreach(ToolChestComponent chest in toolChests)
        {
            if(chest.NumTools>0)
            {
                if(closest==null)
                {
                    closest = chest;
                    closestDist = (chest.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (chest.gameObject.transform.position - agent.transform.position).magnitude;

                    if(dist<closestDist)
                    {
                        closest = chest;
                        closestDist = dist;
                    }
                }
            }
        }

        if (closest==null)
        {
            return false;
        }

        targetToolChest = closest;
        target = targetToolChest.gameObject;

        return closest != null;

    }
    public override bool perform(GameObject agent)
    {
        if (targetToolChest.NumTools > 0)
        {
            targetToolChest.NumTools -= 1;
            hasTool = true;

            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            GameObject prefab = Resources.Load<GameObject>(backpack.toolType);
            GameObject tool = Instantiate(prefab, transform.position, transform.rotation*toolRotation) as GameObject;
            backpack.tool = tool;
            tool.transform.parent = transform;

            return true;
        }
        else
        {
            return false;
        }
    }
}
