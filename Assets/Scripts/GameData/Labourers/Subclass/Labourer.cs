using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Labourer : MonoBehaviour, IGOAP {
    
    public BackpackComponent backpack;
    public SleepTimer sleepTimer;
    public HungerTimer hungerTimer;
    public int health = 100;
    public float moveSpeed = 1;

    Quaternion toolRotation = Quaternion.Euler(0,0,-45);

    void Start ()
    {
		if(backpack==null)
        {
            backpack = gameObject.AddComponent<BackpackComponent>() as BackpackComponent;
        }
        if (backpack.toolType != "null")
        {
            if (backpack.tool == null)
            {
                GameObject prefab = Resources.Load<GameObject>(backpack.toolType);
                GameObject tool = Instantiate(prefab, transform.position, transform.rotation * toolRotation) as GameObject;
                backpack.tool = tool;
                tool.transform.parent = transform;
            }
        }
	}

    public HashSet<KeyValuePair<string,object>>getWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();

        worldData.Add(new KeyValuePair<string, object>("hasUncookedFood", (backpack.numUncookedFood > 0)));
        
        worldData.Add(new KeyValuePair<string, object>("hasCookedFood", (backpack.numCookedFood > 0)));
        
        worldData.Add(new KeyValuePair<string, object>("hasOre", (backpack.numOre > 0)));
        
        worldData.Add(new KeyValuePair<string, object>("hasLogs", (backpack.numLogs > 0)));
        
        worldData.Add(new KeyValuePair<string, object>("hasWorkableWood", (backpack.numFirewood > 0)));
        
        worldData.Add(new KeyValuePair<string, object>("hasTool", (backpack.tool!=null)));

        worldData.Add(new KeyValuePair<string, object>("isTired", (sleepTimer.isTired)));

        worldData.Add(new KeyValuePair<string, object>("isHungry", (hungerTimer.isHungry)));

        worldData.Add(new KeyValuePair<string, object>("isSick", (health <= 0)));

        worldData.Add(new KeyValuePair<string, object>("hasTools", (backpack.numOfTools > 0)));
        return worldData;
    }
    public abstract HashSet<KeyValuePair<string, object>> createGoalState();

    public void planFailed(HashSet<KeyValuePair<string,object>> failedGoal)
    {
    }

    public void planFound(HashSet<KeyValuePair<string,object>> goal, Queue<GOAPAction> actions)
    {
        Debug.Log(name+"<color=green> Plan found</color> " + GOAPAgent.prettyPrint(actions));
    }

    public void actionsFinished()
    {
        health -= 10;
        Debug.Log(name + "<color=blue> Actions completed</color>");
    }

    public void planAborted(GOAPAction aborter)
    {
        Debug.Log(name + "<color=red> Plan Aborted</color> " + GOAPAgent.prettyPrint(aborter));
    }

    public bool moveAgent(GOAPAction nextAction)
    {
        float step = moveSpeed * Time.deltaTime;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, nextAction.target.transform.position,step);

        if(gameObject.transform.position.Equals(nextAction.target.transform.position))
        {
            nextAction.setInRange(true);
            return true;
        }
        else
        {
            return false;
        }
    }
}
