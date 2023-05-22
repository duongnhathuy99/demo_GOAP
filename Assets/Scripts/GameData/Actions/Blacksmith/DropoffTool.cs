using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropoffTool : GOAPAction
{

    private bool droppedOffTool = false;          
    private ToolChestComponent targetToolChest;   

    public DropoffTool()
    {
        addPrecondition("hasTools", true);  
        addEffect("collectTools", true);    
        addEffect("hasTools", false);
    }

    public override void reset()
    {
        droppedOffTool = false; 
        targetToolChest = null; 
    }

    public override bool isDone()
    {
        return droppedOffTool;
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

        foreach (ToolChestComponent toolChest in toolChests)
        {
            if (closest == null)
            {
                closest = toolChest;
                closestDist = (toolChest.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (toolChest.gameObject.transform.position - agent.transform.position).magnitude;

                if (dist < closestDist)
                {
                    closest = toolChest;
                    closestDist = dist;
                }
            }

        }

        if (closest == null)
        {
            return false;
        }

        targetToolChest = closest;
        target = targetToolChest.gameObject;

        return closest != null;

    }

    public override bool perform(GameObject agent)
    {
        BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
        targetToolChest.NumTools += backpack.numOfTools;
        droppedOffTool = true;
        backpack.numOfTools = 0;

        return true;
    }
}
