using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutDownTree : GOAPAction
{
    private bool cutDown = false;        
    private TreeComponent targetTree;          

    private float startTime = 0;       
    public float cuttingDownDuration = 1;    
    public CutDownTree()
    {
        addPrecondition("hasTool", true); 
        addPrecondition("hasLogs", false); 
        addEffect("hasLogs", true);       
    }
    public override void reset()
    {
        cutDown = false;    
        targetTree = null; 
        startTime = 0;     
    }
    public override bool isDone()
    {
        return cutDown;
    }
    public override bool requiresInRange()
    {
        return true;  
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        TreeComponent[] trees = FindObjectsOfType(typeof(TreeComponent)) as TreeComponent[];
        TreeComponent closest = null;
        float closestDist = 0;

        foreach (TreeComponent tree in trees)
        {
            if (closest == null)
            {
                closest = tree;
                closestDist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (tree.gameObject.transform.position - agent.transform.position).magnitude;
                if (dist < closestDist)
                {
                    closest = tree;
                    closestDist = dist;
                }
            }
        }
        targetTree = closest;
        target = targetTree.gameObject;

        return closest != null;
    }
    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > cuttingDownDuration)
        {
            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));
            backpack.numLogs += 5;
            cutDown = true;

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
