using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class SetUserParts : MonoBehaviour
{
    public static SetUserParts SetUserPartsSingleton;
    private static int _processes = 0;
    private static int processes
    {
        get => _processes;
        set
        {
            _processes = value;

            if (_processes > 0)
            {
                // Debug.LogWarning("Saving! Processes remaning " + _processes);
            }
            else
            {
                //  Debug.Log("Its now save to quit");
                _processes = 0;
            }
            allowquit = processes == 0;

        }
    }
    private static bool allowquit = true;
    public CancellationTokenSource cancel;


    private void Awake()
    {
        if (SetUserPartsSingleton == null)
        {
            SetUserPartsSingleton = this;
        }
        else
        {
            Destroy(this);
        }

        Application.wantsToQuit += WantsToQuit;
    }
    public void Start()
    {
        cancel = new CancellationTokenSource();
        //SetUserPartsOnServer(3, "123");

    }

    public static bool WantsToQuit()
    {
        return allowquit;
    }

    public async Task ResetParts()
    {
        string Job = "Reseting Avatar";
        bool done = false;
        UnityMainThread.wkr.AddJob(async () =>
        {
            LoadingManager.instance.EnqueueLoad(Job);
            UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "resetUserAvatarParts.php" + "?user=" + UserLogin.instance.LogInInfo.user.id);
            var req = www.SendWebRequest();
            processes++;
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

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Couldnt Chnge Part " + www.error);
            }
            else
            {
                //Debug.Log("Change Successful");
            }
            processes--;
            done = true;
            LoadingManager.instance.DequeueLoad(Job);
        });
        while (!done)
        {
            await Task.Yield();
        }
    }

    public async Task SetUserPartsOnServer(string partCode)
    {
        try
        {
            string Job = "Setting Avatar on the Server";
            LoadingManager.instance.EnqueueLoad(Job);

            if (UserLogin.instance.UserAvatar.All.Where(x => x.part.Contains(partCode.Substring(0, 6)) && x.part != partCode).Count() > 0)
            {
                Task A = RemoveUserPartsOnServer(UserLogin.instance.UserAvatar.All.Where(x => x.part.Contains(partCode.Substring(0, 6))).First().part);
            }


            bool done = false;
            UnityMainThread.wkr.AddJob(async () =>
            {

                UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserAvatarPart.php" + "?user=" + UserLogin.instance.LogInInfo.user.id + "&part=" + partCode);
                var req = www.SendWebRequest();
                processes++;
                while (!req.isDone)
                {

                    await Task.Yield();
                    if (cancel.Token.IsCancellationRequested)
                    {
                        LoadingManager.instance.DequeueLoad(Job);
                        processes--;
                        Debug.Log("Canceled");
                        return;
                    }
                }

                if (cancel.Token.IsCancellationRequested)
                {
                    Debug.Log("Canceled");
                    processes--;
                    LoadingManager.instance.DequeueLoad(Job);
                    return;
                }

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Couldnt Chnge Part " + www.error);
                }
                else
                {
                    Debug.Log("Change Successful " + partCode);
                    UserLogin.instance.UserAvatar.All.Concat(new PartFromJSON[] { new PartFromJSON(partCode) });
                    //UserLogin.GetAvatarFromServer();
                }

                processes--;
                done = true;
            });
            while (!done)
            {
                await Task.Yield();
            }
            LoadingManager.instance.DequeueLoad(Job);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public async Task RemoveUserPartsOnServer(string partCode)
    {
        bool done = false;
        UnityMainThread.wkr.AddJob(async () =>
        {
            string Job = "Removing Avatar Part from server";
            LoadingManager.instance.EnqueueLoad(Job);
            UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "removeUserAvatarPart.php" + "?user=" + UserLogin.instance.LogInInfo.user.id + "&part=" + partCode);
            var req = www.SendWebRequest();
            processes++;
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
                Debug.Log("Couldnt Remove Part " + www.error);
            }
            else
            {
                //Debug.Log("Accessory " + partCode + " removed Successful");
            }
            processes--;
            done = true;
            LoadingManager.instance.DequeueLoad(Job);
        });
        if (!done)
        {
            await Task.Yield();
        }
    }


}
