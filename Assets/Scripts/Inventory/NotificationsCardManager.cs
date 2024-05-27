using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

public class NotificationsCardManager : MonoBehaviour
{
    public static NotificationsCardManager instance;
    public CancellationTokenSource cancel = new CancellationTokenSource();

    private string _notificationCardsJSON;
    public NotificationsCards NotifCards;

    [SerializeField]
    private NotificationComponent MyStaashNotif;

    public string NotificationCardsJSON
    {
        get => _notificationCardsJSON; set
        {
            _notificationCardsJSON = value;
            NotifCards = JsonUtility.FromJson<NotificationsCards>(value);
            if (NotifCards.All.Count() > 0)
                MyStaashNotif.showNotification = true;
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
        await GetCardNotifs();

    }

    public async void CheckHighlightCard(CardButton card)
    {
        while (NotifCards == null)
        {
            await Task.Yield();
        }
        if (NotifCards.All.Where(x => x.cardid == card.Card.id).Count() > 0)
        {
            card.highlighted = true;
        }
    }

    public async void RemoveCardNotif(CardButton cardbtn)
    {
        await RemoveNotificationCard(cardbtn.Card.id);

        cardbtn.highlighted = false;
        Debug.Log("Before " + NotifCards.All.Count());
        NotifCards.All = NotifCards.All.Where(x => x.cardid != cardbtn.Card.id).ToArray();
        Debug.Log("After " + NotifCards.All.Count());
    }
    private async Task RemoveNotificationCard(string cardid)
    {
        string Job = "Removing Notification from Card";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "removeUserCardNotif.php?user=" + UserLogin.instance.LogInInfo.user.id + "&partid=" + cardid);

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

    public async Task GetCardNotifs()
    {
        //Debug.Log("Getting XtermPallet Async");
        string Job = "Getting notifications for the inventory scene";
        LoadingManager.instance.EnqueueLoad(Job);

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserCardNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id);
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
            NotificationCardsJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
        }
        LoadingManager.instance.DequeueLoad(Job);
    }
}
