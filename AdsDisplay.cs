using System;
using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class AdsDisplay : MonoBehaviour
{
    private void Awake()
    {
        GameMaster.Instance.Load();
    }
    public void ShowRewardedAd()
    {
        if (!GameMaster.Instance.disableAds && Advertisement.IsReady("rewardedVideo") && GameMaster.Instance.deathCounter == 5)
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
            print("ads played");
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                //
                // YOUR CODE TO REWARD THE GAMER
                // Give coins etc.
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }
    }
}
