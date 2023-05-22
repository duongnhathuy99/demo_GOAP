using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHospital : GOAPAction
{

    private bool isHealed = false;
    private HospitalBed targetHospitalBed;
    private BloodTransfusionBag bloodTransfusionBag;

    //public float recoveringDuration = 5;

    bool recovering = false;
    public GoHospital()
    {
        addPrecondition("isSick", true);
        addEffect("isSick", false);
    }
    public override void reset()
    {
        recovering = false;
        isHealed = false;
        targetHospitalBed = null;
    }

    public override bool isDone()
    {
        return isHealed;
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {
        HospitalBed[] hospitalBeds = (HospitalBed[])UnityEngine.GameObject.FindObjectsOfType(typeof(HospitalBed));
        bloodTransfusionBag = (BloodTransfusionBag)UnityEngine.GameObject.FindObjectOfType(typeof(BloodTransfusionBag));
        HospitalBed closest = null;
        float closestDist = 0;

        foreach (HospitalBed hospitalBed in hospitalBeds)
        {
            if (hospitalBed.isFree)
            {
                if (closest == null)
                {
                    closest = hospitalBed;
                    closestDist = (hospitalBed.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (hospitalBed.gameObject.transform.position - agent.transform.position).magnitude;

                    if (dist < closestDist)
                    {
                        closest = hospitalBed;
                        closestDist = dist;
                    }
                }
            }
        }

        if (closest == null)
        {

            return false;
        }

        targetHospitalBed = closest;
        target = targetHospitalBed.gameObject;

        return closest != null;

    }

    public override bool perform(GameObject agent)
    {
        if (targetHospitalBed.isFree || recovering)
        {
            recovering = true;
            targetHospitalBed.isFree = false;
            if (targetHospitalBed.hasBeingTreated)
            {
                isHealed = true;
                targetHospitalBed.hasBeingTreated = false;
                Labourer hungerTimer = (Labourer)agent.GetComponent(typeof(Labourer));

                hungerTimer.health = 100;
                targetHospitalBed.isFree = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
}

