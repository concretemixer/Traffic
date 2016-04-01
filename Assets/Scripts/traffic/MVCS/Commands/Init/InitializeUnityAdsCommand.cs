using UnityEngine;
using System.Collections;
using Commons.Utils;
using strange.extensions.command.impl;
using UnityEngine.Advertisements;
using System;

/// <summary>
/// Команда инициализации рекламмы, елси рекламма уже инициализирована просто возвращаем управление.
/// </summary>
public class InitializeUnityAdsCommand : Command
{
    const float MAX_WAITING_TIME = 5;

    [Inject]
    public UnityEventProvider Provider { private get; set; }

    float elapsedTime = 0.0f;

    public override void Execute()
    {
#if UNITY_STANDALONE
        return;
#endif

        Retain();
        CheckPlatfromSupport();
        InitializeUnityAds();
    }

    private void CheckPlatfromSupport()
    {
#if !UNITY_ADS
        throw new NotSupportedException("Platform is not supported for unity ads");
#endif
    }

    private void InitializeUnityAds()
    {
#if UNITY_ADS
        if (Advertisement.isInitialized && Advertisement.IsReady())
            Release();
        else
        {
            var appId = GetAppId();
            Advertisement.Initialize(appId, true);

            Provider.StartCoroutine(WaitForAdReady());
        }
#endif
    }

#if UNITY_ADS

    private IEnumerator WaitForAdReady()
    {
        while (!Advertisement.isInitialized || !Advertisement.IsReady())
        {
            if (elapsedTime > MAX_WAITING_TIME)
            {
                Release();
                yield return null;
            }
            else
            {
                Debug.Log("wait");
                elapsedTime += 0.5f;
                yield return new WaitForSeconds(0.5f);
            }
        }

        Debug.Log("Ads ready!");
        Release();
    }
#endif

    private string GetAppId()
    {
        var appId = "";
#if UNITY_IOS
        appId = "1045637";
#elif UNITY_ANDROID
        appId = "1045636";
#endif
        return appId;
    }
}