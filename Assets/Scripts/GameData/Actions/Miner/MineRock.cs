using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineRock : GOAPAction
{
    private bool mined = false;     
    private RockComponent targetRock;  

    private float startTime = 0;       
    public float miningDuration = 1;  
    public MineRock()
    {
        addPrecondition("hasTool", true);
        addPrecondition("hasOre", false);  
        addEffect("hasOre", true);      
    }

    public override void reset()
    {
        mined = false;     
        targetRock = null; 
        startTime = 0;     
    }

    public override bool isDone()
    {
        return mined;
    }
    public override bool requiresInRange()
    {
        return true;    
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        RockComponent[] rocks = FindObjectsOfType(typeof(RockComponent)) as RockComponent[];
        RockComponent closest = null;
        float closestDist = 0;

        foreach (RockComponent rock in rocks)
        {
            if (closest == null)
            {
                closest = rock;
                closestDist = (rock.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (rock.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    closest = rock;
                    closestDist = dist;
                }
            }
        }
        targetRock = closest;
        target = targetRock.gameObject;

        return closest != null;
    }
    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > miningDuration)
        {

            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            backpack.numOre += 5;
            mined = true;

            ToolComponent tool = backpack.tool.GetComponent(typeof(ToolComponent)) as ToolComponent;

            tool.use(0.34f);
            if (tool.destroyed())
            {
                Destroy(backpack.tool);
                backpack.tool = null;
            }
        }
        return true;
    }
}
