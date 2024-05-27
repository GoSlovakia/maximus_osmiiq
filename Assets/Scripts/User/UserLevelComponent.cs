using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

public static class UserLevelComponent
{
    private static string userlevelJSON;
    public static string UserLevelJSON
    {
        get { return userlevelJSON; }
        set
        {
            userlevelJSON = value;

            if (UserLevels != null)
            {
                int last = UserLevels.All.Count() == 0 ? 0 : UserLevels.All[0].UserLevel;
                JsonUtility.FromJsonOverwrite(userlevelJSON, UserLevels);
                if (UserLevels.All.Count() > 0 && last != UserLevels.All[0].UserLevel)
                {
                    for (int i = UserLevels.All[0].UserLevel; i > last; i--)
                    {
                        Debug.Log(i + " " + last);
                        SetJourney("LV-B" + i.ToString("D3"));
                    }
                    Debug.Log("for done " + UserLevels.All[0].UserLevel + " " + last);
                }
                else
                {
                    Debug.Log("No need " + UserLevels.All.Count());
                }
            }
            else
            {
                UserLevels = JsonUtility.FromJson<UserLevels>(userlevelJSON);
            }
        }
    }

    public static string LevelcapsJson
    {
        get => levelcapsJson;
        set
        {
            levelcapsJson = value;
            LevelCaps = JsonUtility.FromJson<AllLevelCaps>(levelcapsJson);
        }
    }

    private static string levelcapsJson;

    public static AllLevelCaps LevelCaps;

    public static UserLevels UserLevels;
    public static CancellationTokenSource cancel;

    public static async Task GetUserLevel()
    {
        //Debug.Log("Getting XtermPallet Async");
        string Job = "Getting the User Level ";
        LoadingManager.instance.EnqueueLoad(Job);

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserLevel.php?user=" + UserLogin.instance.LogInInfo.user.id);
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
        //Debug.Log("Getting XtermPallet Async Finished");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error + " " + LoadSVGs.IP + "getUserLevel.php?user=" + UserLogin.instance.LogInInfo.user.id);

        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            UserLevelJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
            //OnLoadEnded(GetHTMLColors());
            if (UserLevelDisplay.instance != null) UserLevelDisplay.instance.UpdateDisplay();

        }
        LoadingManager.instance.DequeueLoad(Job);
    }
    private static async void SetJourney(string setid)
    {
        await SetJourneyNotif(setid);
    }

    public static async Task SetJourneyNotif(string setid)
    {
        //Debug.Log("Getting XtermPallet Async");
        string Job = "Setting a notification on the journey tab";
        LoadingManager.instance.EnqueueLoad(Job);

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserJourneyNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id + "&setid=" + setid);
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
        //Debug.Log("Getting XtermPallet Async Finished");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);

        }
        else
        {
            Debug.Log("Success");

        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task GetLevelCaps()
    {
        // Debug.Log("Getting Level Caps");
        string Job = "Getting the levels values";
        LoadingManager.instance.EnqueueLoad(Job);
        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getLevelCaps.php");
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
        Debug.Log("OK");
        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }
        //Debug.Log("Getting XtermPallet Async Finished");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            return;
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);
            Debug.Log("Converting JSON");
            LevelcapsJson = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
            //OnLoadEnded(GetHTMLColors());
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task<bool> AddXP(int amount)
    {
        //Debug.Log("Getting XtermPallet Async");

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "addXP.php?user=" + UserLogin.instance.LogInInfo.user.id + "&amount=" + amount);
        var req = www.SendWebRequest();

        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return false;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return false;
        }
        //Debug.Log("Getting XtermPallet Async Finished");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            return false;
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);
            // Defaults do level up
            if (www.downloadHandler.text.Contains("Level Up!"))
            {
                // await UnlockSetForUser(LevelCaps.All[UserLevels.All[0].UserLevel - 1].SetReward);
                await LevelUpAddCurrency(LevelCaps.All[UserLevels.All[0].UserLevel - 1].QUIReward, LevelCaps.All[UserLevels.All[0].UserLevel - 1].QIReward);
                //Cenas para o nivel


            }
            await GetUserLevel();
            return www.downloadHandler.text.Contains("Level Up!");




        }
    }

    public static async Task UnlockSetForUser(string SetID)
    {
        Debug.Log("Item Redeemed");

        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserSets.php?user=" + UserLogin.instance.LogInInfo.user.id + "&setID=" + SetID);
        CreateCards.UpdateInventory();

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


    }
    public static async Task LevelUpAddCurrency(int QUI, int QI)
    {
        Debug.Log("Item Redeemed");

        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "LevelUpAddCurrency.php?userID=" + UserLogin.instance.LogInInfo.user.id + "&QUI=" + QUI + "&QI=" + QI);
        CreateCards.UpdateInventory();

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


    }
}
