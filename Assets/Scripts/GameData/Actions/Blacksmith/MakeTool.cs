using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTool : GOAPAction
{

    private bool hasMadeTool = false;                 
    private AnvilComponent targetAnvil;              

    private float startTime = 0;           
    public float smithingDuration = 3;      

    public MakeTool()
    {
        addPrecondition("hasOre", true);      
        addPrecondition("hasWorkableWood", true);   
        addEffect("hasTools", true);               
    }
    public override void reset()
    {
        hasMadeTool = false;      
        targetAnvil = null;        
        startTime = 0;              
    }
    public override bool isDone()
    {
        return hasMadeTool;
    }
    public override bool requiresInRange()
    {
        return true;
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        AnvilComponent[] anvils = (AnvilComponent[])UnityEngine.GameObject.FindObjectsOfType(typeof(AnvilComponent));
        AnvilComponent closest = null;
        float closestDist = 0;

        foreach (AnvilComponent anvil in anvils)
        {
            if (closest == null)
            {
                closest = anvil;
                closestDist = (anvil.gameObject.transform.position - agent.transform.position).magnitude;
            }
            else
            {
                float dist = (anvil.gameObject.transform.position - agent.transform.position).magnitude;

                if (dist < closestDist)
                {
                    closest = anvil;
                    closestDist = dist;
                }
            }

        }

        if (closest == null)
        {
            return false;
        }

        targetAnvil = closest;
        target = targetAnvil.gameObject;

        return closest != null;

    }
    public override bool perform(GameObject agent)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > smithingDuration)
        {
            BackpackComponent backpack = (BackpackComponent)agent.GetComponent(typeof(BackpackComponent));

            int Test = backpack.numOre - backpack.numFirewood;

            if(Test==0)
            {
                backpack.numOfTools += backpack.numFirewood;
                backpack.numFirewood = 0;
                backpack.numOre = 0;
            }
            if(Test<0)
            {
                backpack.numOfTools += backpack.numOre;
                backpack.numFirewood -= backpack.numOre;
                backpack.numOre = 0;
            }
            if(Test>0)
            {
                backpack.numOfTools += backpack.numFirewood;
                backpack.numOre -= backpack.numFirewood;
                backpack.numFirewood = 0;
            }

                hasMadeTool = true;
        }
            return true;
     
    }
}
