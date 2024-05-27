using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;
using System;

public class GetTitles
{

    public static TitlesArray UserTitlesArray;
    private static string uTJSON;
    public static CancellationTokenSource cancel = new CancellationTokenSource();
    private static UnityWebRequest www;
    private static UnityWebRequest wwwuser;


    public static string UTJSON
    {
        get => uTJSON;
        set
        {
            uTJSON = value;
            UserTitlesArray = JsonUtility.FromJson<TitlesArray>(value);
        }
    }

    public static void GenerateGetTitlesRequest()
    {
        www = UnityWebRequest.Get(LoadSVGs.IP + "getTitles.php");
    }

    public static async Task GetAllTitles()
    {
        string Job = "Downloading All titles";
        LoadingManager.instance.EnqueueLoad(Job);
        try
        {
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
                Debug.LogError("Couldnt get all titles " + www.error);
            }
            else
            {
                UTJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";

            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static void GenerateGetUserTitlesRequest()
    {
        wwwuser = UnityWebRequest.Get(LoadSVGs.IP + "getUserTitles.php?user=" + UserLogin.instance.LogInInfo.user.id);
    }
    public static async Task GetUserTitles()
    {
        string Job = "Getting the Users titles ";
        LoadingManager.instance.EnqueueLoad(Job);
        try
        {

            var req = wwwuser.SendWebRequest();
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

            if (wwwuser.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Couldnt get all titles " + wwwuser.error);
            }
            else
            {
                if (wwwuser.downloadHandler.text != "[]")
                    UserLogin.instance.UserTitlesJSON = "{ 	\"All\": 	" + wwwuser.downloadHandler.text + "}";

            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        LoadingManager.instance.DequeueLoad(Job);
    }
}
