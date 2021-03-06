﻿using UnityEngine;
using System.Collections;
using System;

public class GameEventPoliticalViewPresentation : GameEvent {

    static bool executed = false;

    public override void Init()
    {
        eventName = "Political View Presentation";
    }

    public override bool ConditionCheck()
    {
        Date gameDate = GameManager.Instance.GameDate;

        if (!executed &&
            gameDate.Year == 2 &&
            gameDate.Month == 6 &&
            gameDate.Day == 20 &&
            SchedulingManager.Instance.GameTime >= 17f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void ExecuteEvent()
    {
        executed = true;
        ConversationManager.Instance.StartConversationEvent("Political View Presentation");
    }

    public override void EventEnded()
    {
        SchedulingManager.Instance.initTime = false;
        SchedulingManager.Instance.AddGameTime(1);
        GameManager.Instance.ScheduleExecute();
    }
}
