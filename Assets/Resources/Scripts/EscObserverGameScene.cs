﻿using UnityEngine;
using System.Collections;
using System;

public class EscObserverGameScene : EscObserver
{

    public override void EscAction()
    {
        Application.Quit();
    }
}
