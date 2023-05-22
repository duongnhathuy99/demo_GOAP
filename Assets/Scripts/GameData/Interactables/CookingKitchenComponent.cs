using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingKitchenComponent : MonoBehaviour
{
    public bool isCooking;

    public Transform transforms;
    private void Start()
    {
        isCooking = false;
        transforms = transform.GetChild(0).GetComponent<Transform>();
    }
    private void Update()
    {
        if (!isCooking)
            transforms.gameObject.SetActive(false);
        else
            transforms.gameObject.SetActive(true);
    }
}