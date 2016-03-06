using UnityEngine;
using System.Collections;
using Commons.Utils;
using strange.extensions.command.impl;
using UnityEngine.Advertisements;

public class InitializeUnityAdsCommand : Command
{
    [Inject]
    public UnityEventProvider Provider { private get; set; }

    public override void Execute()
    {
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
        var appId = GetAppId();
        Advertisement.Initialize(appId, true);

        Provider.StartCoroutine(WaitForAdReady());
    }

    private IEnumerator WaitForAdReady()
    {
        while (!Advertisement.isInitialized || !Advertisement.IsReady())
            yield return new WaitForSeconds(0.5f);

        Debug.Log("Ads ready!");
        Release();
    }

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