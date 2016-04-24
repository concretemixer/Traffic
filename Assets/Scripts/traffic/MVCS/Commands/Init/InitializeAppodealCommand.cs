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
        #if UNITY_IOS
        String appKey = "18e77ef8f5b7a2c7ca4a48464eb27de51c7364b9917032fe";
        #elif UNITY_ANDROID
        String appKey = "3e28bb6cebdf9e7bba63c8bb376df09ec65dd8eea3b8ec12";
        #endif
     //   Appodeal.setTesting(true);
    //    Appodeal.setLogging(true);
        Appodeal.disableLocationPermissionCheck();
        Appodeal.initialize(appKey, Appodeal.SKIPPABLE_VIDEO);
    }
}