using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

public class NotificationsMainMenu : MonoBehaviour
{
    public static NotificationsMainMenu instance;
    public CancellationTokenSource cancel = new CancellationTokenSource();
    private string _notificationsGeneralJSON;
    public NotificationsGeneral UserNotifs;
    public bool InboxHasItems;

    public GameObject NotifAch, NotifJourney, NotifParts, NotifCols, NotifCards,NotifInbox,NotifInboxDrawer;
    public string NotificationsGeneralJSON
    {
        get => _notificationsGeneralJSON; set
        {
            _notificationsGeneralJSON = value;
            UserNotifs= JsonUtility.FromJson<NotificationsGeneral>(value);
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
        Task A= GetCardNotifs();
        Task B= GetUserInbox();

        await Task.WhenAll(A, B);

        if (UserNotifs.All.Count()>0)
        {
            NotifAch.SetActive(UserNotifs.All[0].achievements > 0);
            NotifJourney.SetActive(UserNotifs.All[0].journey > 0);
            NotifParts.SetActive(UserNotifs.All[0].avatarparts > 0);
            NotifCols.SetActive(UserNotifs.All[0].colors > 0);
            NotifCards.SetActive(UserNotifs.All[0].cards > 0);
            NotifInbox.SetActive(InboxHasItems);
            NotifInboxDrawer.SetActive(InboxHasItems);
        }
    }

    public async Task GetCardNotifs()
    {
        //Debug.Log("Getting XtermPallet Async");
        string Job = "Getting General Notifications";
        LoadingManager.instance.EnqueueLoad(Job);

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserGeneralNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id);
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
            NotificationsGeneralJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public async Task GetUserInbox()
    {
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserInbox.php?user=" + UserLogin.instance.LogInInfo.user.id);
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
            Debug.LogError("Get inbox failed " + www.error);
        }
        //Debug.LogError(www.downloadHandler.text);
        else
        {
            InboxList res = JsonUtility.FromJson<InboxList>("{ 	\"All\": 	" + www.downloadHandler.text + "}");
            InboxHasItems = res.All.Where(x=>x.ClaimedVal==false).Count()>0;

        }
    }
}
