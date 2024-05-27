using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.Rendering.DebugUI;

public class SetTitle
{
    private UnityWebRequest www;
    public CancellationTokenSource cancel = new CancellationTokenSource();


    public void SetRequest(string adjID,string nameID)
    {
        www = UnityWebRequest.Get(LoadSVGs.IP + "setUserTitles.php?user="+ UserLogin.instance.LogInInfo.user.id+ "&adjID="+adjID+"&nameID="+nameID);
    }

    public async Task SetTitleOnServer()
    {
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
                Debug.LogError("Couldnt set titles " + www.error);
            }
            else
            {
                Debug.Log("Changed Title");

            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
