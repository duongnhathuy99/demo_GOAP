using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GOAPPlanner
{
    public Queue<GOAPAction> plan(GameObject agent, HashSet<GOAPAction>availableActions, HashSet<KeyValuePair<string,object>>worldState, HashSet<KeyValuePair<string,object>>goal)
    {
        foreach(GOAPAction action in availableActions)
        {
            action.doReset();
        }
        HashSet<GOAPAction> usableActions = new HashSet<GOAPAction>();

        foreach(GOAPAction action in availableActions)
        {
            if(action.checkProceduralPrecondition(agent))
            {
                usableActions.Add(action);
            }
        }
        List<Node> leaves = new List<Node>();

        Node start = new Node(null, 0, worldState, null);
        bool success = buildGraph(start, leaves, usableActions, goal);

        if(!success)
        {
            Debug.Log(agent.name + " NO PLAN");
            return null;
        }

        Node cheapest = null;

        foreach(Node leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if (leaf.runningCost < cheapest.runningCost)
                {
                    cheapest = leaf;
                }
            }
        }

        List<GOAPAction> result = new List<GOAPAction>();
        Node n = cheapest;

        while(n!=null)
        {
            if(n.action!=null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        Queue<GOAPAction> queue = new Queue<GOAPAction>();

        foreach(GOAPAction a in result)
        {
            queue.Enqueue(a);
        }

        return queue;
    }

    private bool buildGraph(Node parent, List<Node> leaves, HashSet<GOAPAction> usableActions, HashSet<KeyValuePair<string,object>>goal)
    {
        bool foundOne = false;

        foreach(GOAPAction action in usableActions)
        {
            if(inState(action.Preconditions,parent.state))
            {
                HashSet<KeyValuePair<string, object>> currentState = populateState(parent.state, action.Effects);

                Node node = new Node(parent, parent.runningCost + action.cost, currentState, action);

                if(inState(goal,currentState))
                {
                   
                    leaves.Add(node);
                    foundOne = true;
                }
                else
                {
                    HashSet<GOAPAction> subset = actionSubset(usableActions, action);
                    bool found = buildGraph(node, leaves, subset, goal);
                    
                    if(found)
                    {
                        foundOne = true;
                    }
                }
            }
        }
        return foundOne;
    }

    private HashSet<GOAPAction>actionSubset(HashSet<GOAPAction>actions,GOAPAction removeMe)
    {
        HashSet<GOAPAction> subset = new HashSet<GOAPAction>();

        foreach(GOAPAction action in actions)
        {
            if(!action.Equals(removeMe))
            {
                subset.Add(action);
            }
        }
        return subset;
    }

    private bool inState(HashSet<KeyValuePair<string,object>> test, HashSet<KeyValuePair<string,object>>state)
    {
        bool allMatch = true;

        foreach(KeyValuePair<string,object> t in test)
        {
            bool match = false;

            foreach(KeyValuePair<string,object>s in state)
            {
                if(s.Equals(t))
                {
                    match = true;
                    break;
                }
            }
            if(!match)
            {
                allMatch = false;
            }
        }
        return allMatch;
    }

    private HashSet<KeyValuePair<string,object>> populateState(HashSet<KeyValuePair<string,object>> currentState, HashSet<KeyValuePair<string,object>> stateChange)
    {
        HashSet<KeyValuePair<string, object>> state = new HashSet<KeyValuePair<string, object>>();


        foreach (KeyValuePair<string,object> s in currentState)
        {
            state.Add(new KeyValuePair<string, object>(s.Key, s.Value));
        }


        foreach(KeyValuePair<string,object>change in stateChange)
        {
            bool exists = false;

            foreach (KeyValuePair<string,object> s in state)
            {
                if(s.Equals(change))
                {
                    exists = true;
                    break;
                }
            }

            if(exists)
            {
                state.RemoveWhere((KeyValuePair<string, object> kvp) => { return kvp.Key.Equals(change.Key); });
                KeyValuePair<string, object> updated = new KeyValuePair<string, object>(change.Key, change.Value);
                state.Add(updated);
            }

             else
            {
                state.Add(new KeyValuePair<string, object>(change.Key, change.Value));
            }
        }
        return state;
    }



    private class Node
    {
        public Node parent;
        public float runningCost;
        public HashSet<KeyValuePair<string, object>> state;
        public GOAPAction action;

        public Node(Node parent, float runningCost, HashSet<KeyValuePair<string,object>> state, GOAPAction action)
        {
            this.parent = parent;
            this.runningCost = runningCost;
            this.state = state;
            this.action = action;
        }

    }

}