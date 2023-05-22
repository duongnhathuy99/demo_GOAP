using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerTimer : MonoBehaviour {

    public float maxEatTime = 1f;
    public float EatTime;
    public float hungerPerFrame;

    public bool isHungry;

    void Start ()
    {
        isHungry = false;
        EatTime = maxEatTime;
	}
	
	void Update ()
    {
        if (EatTime > 0)
        {
            isHungry = false;
            EatTime -= hungerPerFrame * Time.deltaTime;
        }
        if(EatTime<=0)
        {
            isHungry = true;
        }
	}
}
