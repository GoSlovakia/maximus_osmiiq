using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;

public class NotificationsAchJourney : MonoBehaviour
{
    public static NotificationsAchJourney instance;
    private string notificationsAchJSON;
    private string notificationsJourneyJSON;
    public bool loaded = false;

    public CancellationTokenSource cancel = new CancellationTokenSource();
    public NotificationAchs NotifAch;
    public NotificationsJourney NotifJourney;

    public string NotificationsAchJSON
    {
        get => notificationsAchJSON; set
        {
            notificationsAchJSON = value;
            NotifAch = JsonUtility.FromJson<NotificationAchs>(notificationsAchJSON);
        }
    }

    public string NotificationsJourneyJSON
    {
        get => notificationsJourneyJSON; set
        {
            notificationsJourneyJSON = value;
            NotifJourney = JsonUtility.FromJson<NotificationsJourney>(notificationsJourneyJSON);
        }
    }

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        Setup();
    }

    public async void Setup()
    {
        Task A = GetAchNotifs();
        Task B = GetJourneyNotifs();
        await Task.WhenAll(A, B);
        loaded = true;
    }

    public async Task RemoveNotificationAch(string achid)
    {
        string Job = "Removing Notification from Achievement";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "removeUserAchNotif.php?user=" + UserLogin.instance.LogInInfo.user.id + "&achid=" + achid);

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
            Debug.LogError("Purchase failed " + www.error);
        }
        else
        {
            Debug.Log("Success");
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public async Task GetAchNotifs()
    {
        //Debug.Log("Getting XtermPallet Async");
        string Job = "Getting notifications for the achievement scene";
        LoadingManager.instance.EnqueueLoad(Job);

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserAchNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id);
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
            NotificationsAchJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public async Task RemoveNotificationJourney(string setid)
    {
        string Job = "Removing Notification from Journey";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP+"removeUserJourneyNotif.php?user=" + UserLogin.instance.LogInInfo.user.id + "&setid=" + setid);

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
            Debug.LogError("Purchase failed " + www.error);
        }
        else
        {
            Debug.Log("Success");
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public async Task GetJourneyNotifs()
    {
        //Debug.Log("Getting XtermPallet Async");
        string Job = "Getting notifications for the journey scene";
        LoadingManager.instance.EnqueueLoad(Job);

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserJourneyNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id);
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
            NotificationsJourneyJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
        }
        LoadingManager.instance.DequeueLoad(Job);
    }
}
