using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;

public static class UserSetsComponent
{
    private static string userSetsJSON;
    public static UserSetsContainer AllUserSets;

    public static CancellationTokenSource cancel = new CancellationTokenSource();

    public static string UserSetsJSON
    {
        get => userSetsJSON;
        set
        {
            userSetsJSON = value;
            if (AllUserSets == null)
                AllUserSets = JsonUtility.FromJson<UserSetsContainer>(userSetsJSON);
            else
                JsonUtility.FromJsonOverwrite(value, AllUserSets);
        }
    }

    public static async Task GetUserSets()
    {
        string Job = "Fetching user's sets";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserSets.php?user=" + UserLogin.instance.LogInInfo.user.id);
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Get user sets failed " + www.error);
        }
        //Debug.LogError(www.downloadHandler.text);
        else
        {
            // Show results as text

            UserSetsJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
            // Debug.Log("User Sets " + www.downloadHandler.text);
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task UnlockSetForUser(string SetID)
    {
        string Job = "Unlocking Set";
        LoadingManager.instance.EnqueueLoad(Job);
        Debug.Log("Item Redeemed");


        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserSets.php?user=" + UserLogin.instance.LogInInfo.user.id + "&setID=" + SetID);

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to redeem " + www.error);
        }
        //Debug.LogError(www.downloadHandler.text);
        else
        {
            Debug.Log("Success");
        }

        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task GiveLevelReward(string Level)
    {
        string Job = "Giving the level's rewards";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "RedeemLevelUpReward.php?user=" + UserLogin.instance.LogInInfo.user.id + "&lvl=" + Level);

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to redeem " + www.error);
        }
        //Debug.LogError(www.downloadHandler.text);
        else
        {
            Debug.Log("Success");
        }

        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task GiveAchievementReward(string achID)
    {
        string Job = "Redeeming achievements rewards";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "RedeemAchievementRewards.php?user=" + UserLogin.instance.LogInInfo.user.id + "&ach=" + achID);

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to redeem " + www.error);
        }
        //Debug.LogError(www.downloadHandler.text);
        else
        {
            Debug.Log("Success");
        }

        LoadingManager.instance.DequeueLoad(Job);
    }
}
