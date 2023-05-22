using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientCare : GOAPAction
{
    private bool hasPatientCare = false;
    private BloodTransfusionBag targetBloodTrans;
    private HospitalBed hopitalBed;
    private float startTime = 0;
    public float treatmentDuration = 5;

    bool takingCare = false;

    public PatientCare()
    {
        //addPrecondition("hasPatient", true);
        addEffect("hasPatient", false);
    }
    public override void reset()
    {
        takingCare = false;
        startTime = 0;
        hasPatientCare = false;

        targetBloodTrans = null;
    }
    public override bool isDone()
    {
        return hasPatientCare;
    }

    public override bool requiresInRange()
    {
        return true;
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        BloodTransfusionBag[] BloodTransfusionBags = (BloodTransfusionBag[])UnityEngine.GameObject.FindObjectsOfType(typeof(BloodTransfusionBag));
        hopitalBed = (HospitalBed)UnityEngine.GameObject.FindObjectOfType(typeof(HospitalBed));
        BloodTransfusionBag closest = null;
        float closestDist = 0;

        foreach (BloodTransfusionBag BloodTransfusionBag in BloodTransfusionBags)
        {
            if (BloodTransfusionBag.isFree && !hopitalBed.isFree)
            {
                if (closest == null)
                {
                    closest = BloodTransfusionBag;
                    closestDist = (BloodTransfusionBag.gameObject.transform.position - agent.transform.position).magnitude;
                }
                else
                {
                    float dist = (BloodTransfusionBag.gameObject.transform.position - agent.transform.position).magnitude;

                    if (dist < closestDist)
                    {
                        closest = BloodTransfusionBag;
                        closestDist = dist;
                    }
                }
            }
        }

        if (closest == null)
        {
            return false;
        }

        targetBloodTrans = closest;
        target = targetBloodTrans.gameObject;

        return closest != null;

    }
    public override bool perform(GameObject agent)
    {
        if ((!hopitalBed.isFree && targetBloodTrans.isFree) || takingCare == true)
        {
            takingCare = true;
            targetBloodTrans.isFree = false;
            if (startTime == 0)
            {
                startTime = Time.time;
            }

            if (Time.time - startTime > treatmentDuration)
            {
                hasPatientCare = true;
                /*
                                SleepTimer sleepTimer = (SleepTimer)agent.GetComponent(typeof(SleepTimer));

                                sleepTimer.sleepTime = sleepTimer.maxSleepTime;*/
                hopitalBed.hasBeingTreated = true;
                targetBloodTrans.isFree = true;
            }
            return true;
        }

        else
        {
            return false;
        }
    }
}
