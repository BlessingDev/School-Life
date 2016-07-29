﻿using UnityEngine;
using System.Collections;
using System;

public class ScheduleTakeARest : Schedule
{

	// Use this for initialization
	new void Start ()
    {
        base.Start();
        type = ScheduleType.TakeARest;
	}

    public override void TypeInit()
    {
        Start();
    }

    public override void Effect(Schedule obj)
    {
        GameManager.Instance.SetParameter("Stress", GameManager.Instance.GetParameter("Stress") - 1);
        Debug.Log("Take a rest Effected!");
    }
}
