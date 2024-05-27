using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Networking;
using System.Threading;
using System.Linq;

public class AchievementTrackerComponent : MonoBehaviour
{
    public static AchievementTrackerComponent instance;

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    public AllAchievements AllAchievements;
    public AllUserStats AllUserStats;

    public CancellationTokenSource cancel = new CancellationTokenSource();
    private string achievementsJSON;
    public string AchievementsJSON
    {
        get => achievementsJSON;
        set
        {
            achievementsJSON = value;
            AllAchievements = JsonUtility.FromJson<AllAchievements>(achievementsJSON);
        }
    }

    private string allUserStatsJSON;
    public string AllUserStatsJSON
    {
        get => allUserStatsJSON;
        set
        {
            allUserStatsJSON = value;
            // Debug.Log(allUserStatsJSON);
            if (AllUserStats != null)
                AllUserStats = JsonUtility.FromJson<AllUserStats>(allUserStatsJSON);
            else
                JsonUtility.FromJsonOverwrite(allUserStatsJSON, AllUserStats);
        }
    }

    public int GetVariable(VariableType variable)
    {

        if (AllUserStats.All.Where(x => x.StatName == variable.GetVarName()).Count() != 0)
        {
            return AllUserStats.All.Where(x => x.StatName == variable.GetVarName()).Single().Value;
        }
        else
        {
            //Debug.Log("Stat " + variable.GetType() + " not found! " + AllUserStats.All.Where(x => x.StatName == variable.GetVarName()).Count() + " " + AllUserStats.All.Count());
            return 0;
        }

    }

    public int GetVariable(VariableType variable, int? set)
    {
        if (set == null)
        {
            return GetVariable(variable);
        }
        //Debug.Log("Set type" + set);
        if (AllUserStats.All.Where(x => x.StatName == variable.GetVarName() && x.setType == set).Count() != 0)
        {
            return AllUserStats.All.Where(x => x.StatName == variable.GetVarName() && x.setType == set).Single().Value;
        }
        else
        {
            // Debug.Log("Stat " + variable.GetType() + " not found! " + AllUserStats.All.Where(x => x.StatName == variable.GetVarName() && x.setType == set).Count() + " " + AllUserStats.All.Count());
            return 0;
        }

    }

    public async Task SetVariable(VariableType variable, int value)
    {
        // Debug.Log("Setting Var");
        if (AllUserStats.All.Where(x => x.StatName == variable.GetVarName()).Count() != 0)
        {
            AllUserStats.All.Where(x => x.StatName == variable.GetVarName()).Single().Value = value;
        }

        await SetUserStats(variable, value);
        await GetUserStats();
        foreach (var este in AllAchievements.All.Where(x => x.VarType == variable))
        {
            if (CheckAchievement(este))
            {
                await SetAchNotif(este.ID);
            }
        }
    }

    public async Task AddToVariable(VariableType variable, int value)
    {
        // Debug.Log("Setting Var");
        if (AllUserStats.All.Where(x => x.StatName == variable.GetVarName()).Count() != 0)
        {

            await SetUserStats(variable, value + AllUserStats.All.Where(x => x.StatName == variable.GetVarName()).Single().Value);
            AllUserStats.All.Where(x => x.StatName == variable.GetVarName()).Single().Value = value + AllUserStats.All.Where(x => x.StatName == variable.GetVarName()).Single().Value;
        }
        else
        {
            await SetUserStats(variable, value);
            await GetUserStats();
        }


        foreach (var este in AllAchievements.All.Where(x => x.VarType == variable))
        {
            if (CheckAchievement(este))
            {
                await SetAchNotif(este.ID);
            }
        }
    }

    public async Task AddToVariable(VariableType variable, int value, int setType)
    {
        // Debug.Log("Setting Var");
        if (AllUserStats.All.Where(x => x.StatName == variable.GetVarName() && x.setType == setType).Count() != 0)
        {
            // Debug.Log(variable.GetVarName() + " " + setType);

            await SetUserStats(variable, value + AllUserStats.All.Where(x => x.StatName == variable.GetVarName() && x.setType == setType).Single().Value, setType);
            AllUserStats.All.Where(x => x.StatName == variable.GetVarName()).Single().Value = value + AllUserStats.All.Where(x => x.StatName == variable.GetVarName() && x.setType == setType).Single().Value;
        }
        else
        {
            //Debug.Log("Setting Var" + variable.GetVarName() + " " + setType);
            await SetUserStats(variable, value, setType);
            await GetUserStats();

        }

        foreach (var este in AllAchievements.All.Where(x => x.VarType == variable))
        {
            if (CheckAchievement(este))
            {
                await SetAchNotif(este.ID);
            }
        }
    }

    public async Task SetVariable(VariableType variable, int value, int setType)
    {
        if (AllUserStats.All.Where(x => x.StatName == variable.GetVarName() && x.setType == setType).Count() != 0)
        {
            AllUserStats.All.Where(x => x.StatName == variable.GetVarName() && x.setType == setType).Single().Value = value;
        }

        await SetUserStats(variable, value, setType);

        foreach (var este in AllAchievements.All.Where(x => x.VarType == variable))
        {
            if (CheckAchievement(este))
            {
                await SetAchNotif(este.ID);
            }
        }
    }

    public async Task SetAchNotif(string achid)
    {
        //Debug.Log("Getting XtermPallet Async");
        string Job = "Setting a notification for an achievement";
        LoadingManager.instance.EnqueueLoad(Job);

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserAchNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id + "&achid=" + achid);
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

    public async Task GetUserStats()
    {
        string Job = "Fetching User Stats";
        LoadingManager.instance.EnqueueLoad(Job);
        //Debug.Log("Getting XtermPallet Async");
        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserStats.php?user=" + UserLogin.instance.LogInInfo.user.id);
        var req = www.SendWebRequest();

        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                LoadingManager.instance.DequeueLoad(Job);
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            LoadingManager.instance.DequeueLoad(Job);
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
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            AllUserStatsJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
            // Debug.Log(www.downloadHandler.text);
            //OnLoadEnded(GetHTMLColors());
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public async Task SetUserStats(VariableType variable, int value, int? setType)
    {
        string Job = "Updating User Stats";
        LoadingManager.instance.EnqueueLoad(Job);
        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserStats.php?user=" + UserLogin.instance.LogInInfo.user.id + "&statname=" + variable.GetVarName() + "&value=" + value);
        if (setType != null)
        {
            www.url = LoadSVGs.IP + "setUserStats.php?user=" + UserLogin.instance.LogInInfo.user.id + "&statname=" + variable.GetVarName() + "&value=" + value + "&set=" + setType;
        }
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
            // Debug.Log("Success");
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public async Task SetUserStats(VariableType variable, int value)
    {
        string Job = "Updating User Stats";
        LoadingManager.instance.EnqueueLoad(Job);
        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserStats.php?user=" + UserLogin.instance.LogInInfo.user.id + "&statname=" + variable.GetVarName() + "&value=" + value);

        var req = www.SendWebRequest();

        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                LoadingManager.instance.DequeueLoad(Job);
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            LoadingManager.instance.DequeueLoad(Job);
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
            // Debug.Log("Success");
        }
        LoadingManager.instance.DequeueLoad(Job);
    }


    public async Task GetAllAchievements()
    {
        string Job = "Fetching the User's Achievements";
        LoadingManager.instance.EnqueueLoad(Job);
        //Debug.Log("Getting XtermPallet Async");
        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getAchievements.php");
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
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            AchievementsJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
            // Debug.Log(www.downloadHandler.text);
            //OnLoadEnded(GetHTMLColors());
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public bool CheckAchievement(Achievement ach)
    {
       // Debug.Log("Comparison Type" + ach.ComparisonType);
        return AchievementComparison(ach);
    }

    public bool WasAvatarEverChanged(ComparisonEnum comp, int value)
    {
        switch (comp)
        {

            case ComparisonEnum.MORE_OR_EQUAL:
            case ComparisonEnum.EQUAL:
                return GetVariable(VariableType.SavedAvatar) == value;

            default:
            case ComparisonEnum.NULL:
                Debug.Log("VarType not defined!");
                return false;
        }
    }

    public bool AchievementComparison(Achievement ach)
    {
        switch (ach.ComparisonType)
        {
            case ComparisonEnum.EQUAL:
                return GetVariable(ach.VarType, ach.Value2) == ach.Value;
            case ComparisonEnum.MORE_OR_EQUAL:
                return GetVariable(ach.VarType, ach.Value2) >= ach.Value;
            case ComparisonEnum.LESS_OR_EQUAL:
                return GetVariable(ach.VarType, ach.Value2) <= ach.Value;

            default:
            case ComparisonEnum.NULL:
                Debug.Log("VarType not defined!");
                return false;
        }
    }
}
