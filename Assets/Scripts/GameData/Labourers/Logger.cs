using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : Labourer
{
    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {

        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();

        if (GetComponent<HungerTimer>().isHungry && GetComponent<GoEat>().target != null)
        {
            goal.Add(new KeyValuePair<string, object>("hasAte", true));
        }

        if (health <= 0 && GetComponent<GoHospital>().target != null)
        {
            goal.Add(new KeyValuePair<string, object>("isSick", false));
        }
        if (GetComponent<SleepTimer>().isTired && GetComponent<GoSleep>().target != null)
        {
            goal.Add(new KeyValuePair<string, object>("hasSlept", true));
        }

        else
        {
            goal.Add(new KeyValuePair<string, object>("collectLogs", true));
        }


        return goal;
    }
}
