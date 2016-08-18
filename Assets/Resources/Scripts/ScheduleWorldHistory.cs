﻿using UnityEngine;
using System.Collections;
using System;

public class ScheduleWorldHistory : Schedule
{
    public override void Init()
    {
        type = ScheduleType.WorldHistory;
        ratable = true;
        categories.Add(ParameterCategory.Stress);
        categories.Add(ParameterCategory.Social);
    }

    public override void Effect(Schedule obj)
    {
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 0.7f);
        SchedulingManager.Instance.AddParameterAndShowText("Social", 1f);
    }

    public override void Failed()
    {
        SchedulingManager.Instance.AddParameterAndShowText("Stress", 1);
    }
}
