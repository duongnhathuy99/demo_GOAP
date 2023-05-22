using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinnerTableComponent : MonoBehaviour
{
    public int NumCookedFood;
    public Transform transforms;
    private void Start()
    {
        transforms = transform.GetChild(0).GetComponent<Transform>();
    }
    private void Update()
    {
        if (NumCookedFood <= 0)
            transforms.gameObject.SetActive(false);
        else
            transforms.gameObject.SetActive(true);
    }
}
