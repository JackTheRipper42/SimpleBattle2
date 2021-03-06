﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Unguided : FlightGuidance
{
    protected override void Start()
    {
        base.Start();
        GetComponent<Rigidbody>().velocity = transform.forward * Speed;
    }
}
