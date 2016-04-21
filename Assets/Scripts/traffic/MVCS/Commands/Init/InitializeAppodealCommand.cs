using UnityEngine;
using System.Collections;
using Commons.Utils;
using strange.extensions.command.impl;
using AppodealAds.Unity.Api;    
using System;

/// <summary>
/// Команда инициализации рекламмы, елси рекламма уже инициализирована просто возвращаем управление.
/// </summary>
public class InitializeAppodealCommand : Command
{
    [Inject]
    public UnityEventProvider Provider { private get; set; }

    float elapsedTime = 0.0f;

    public override void Execute()
    {
        String appKey = "3e28bb6cebdf9e7bba63c8bb376df09ec65dd8eea3b8ec12";
     //   Appodeal.setTesting(true);
    //    Appodeal.setLogging(true);
        Appodeal.initialize(appKey, Appodeal.NON_SKIPPABLE_VIDEO);
    }
}