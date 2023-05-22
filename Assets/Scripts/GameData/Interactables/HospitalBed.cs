using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HospitalBed : MonoBehaviour
{
    public bool isFree;
    public bool hasBeingTreated;
    private void Start()
    {
        isFree = true;
        hasBeingTreated = false;
    }
}
