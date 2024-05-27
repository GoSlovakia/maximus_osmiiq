using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using UnityEngine.UI;

public class NotificationTabManager : MonoBehaviour
{

    public CancellationTokenSource cancel = new CancellationTokenSource();
    public static NotificationTabManager instance;

    public List<GameObject> Filters = new List<GameObject>();
    private string notificationsJSON;
    public NotificationsAvatar NotifAvatar;
    public NotificationsColors NotifColors;
    private string _notificationscolor;

    public string NotificationsAvatarJSON
    {
        get => notificationsJSON; set
        {
            notificationsJSON = value;
            NotifAvatar = JsonUtility.FromJson<NotificationsAvatar>(notificationsJSON);
        }
    }

    public string NotificationsColorsJSON
    {
        get => _notificationscolor;
        set
        {
            _notificationscolor = value;
            NotifColors = JsonUtility.FromJson<NotificationsColors>(_notificationscolor);
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
        Task A = GetAvatarNotifs();
        Task B = GetColorNotifs();
        await Task.WhenAll(A, B);
    }

    public void AvatarCheckNotif(AvatarChangeBtn este)
    {
        if (NotifAvatar.All.Where(x => x.part == este.Acc.id).Count() > 0)
        {
            Filters.Where(x => x.transform.name.ToUpper() == este.Acc.avatarset.GetCategory().ToString()).Single().GetComponent<NotificationComponent>().showNotification = true;
            este.highlighted = true;
        }
    }

    public void ColorCheckNotif(ColorContainer este)
    {
        if (NotifColors.All.Where(x => x.colid == este.Colorset.code).Count() > 0)
        {
            Debug.Log("TEST " + Filters.Where(x => x.transform.name == "Color").Count());
            Filters.Where(x => x.transform.name == "Color").Single().GetComponent<NotificationComponent>().showNotification = true;
            este.highlighted = true;
        }
    }

    public async void RemoveColorNotif(ColorContainer este)
    {
        Debug.Log("REMOVING AAAAAAAAAAAAA");
        await RemoveNotificationColor(este.Colorset.code);
        este.highlighted = false;

        Filters.Where(x => x.transform.name == "Color").Single().GetComponent<NotificationComponent>().showNotification = GenerateColorPalettes.instance.Colors.Where(x => x.highlighted).Count() > 0;

    }

    public async void RemoveNotif(AvatarChangeBtn este)
    {
        await RemoveNotificationAvatar(este.Acc.id);
        este.highlighted = false;


        Filters.Where(x => x.transform.name.ToUpper() == este.Acc.avatarset.GetCategory().ToString()).Single().GetComponent<NotificationComponent>().showNotification = AssetCreatingFromFolder.AllAccBtns.Where(x => x.highlighted && x.Acc.avatarset.GetCategory() == este.Acc.avatarset.GetCategory()).Count() > 0;

    }

    private async Task RemoveNotificationAvatar(string part)
    {
        string Job = "Removing Notification from avatar customization";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "removeUserAvatarNotif.php?user=" + UserLogin.instance.LogInInfo.user.id + "&partid=" + part);

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

    public async Task GetAvatarNotifs()
    {
        //Debug.Log("Getting XtermPallet Async");
        string Job = "Getting notifications for the avatar scene";
        LoadingManager.instance.EnqueueLoad(Job);

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserAvatarNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id);
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
            NotificationsAvatarJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    private async Task RemoveNotificationColor(string colid)
    {
        string Job = "Removing color notification on server";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get("https://game.maximus2020.sk/maximus/removeUserColorNotif.php?user=" + UserLogin.instance.LogInInfo.user.id + "&colid=" + colid);

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

    public async Task GetColorNotifs()
    {
        //Debug.Log("Getting XtermPallet Async");
        string Job = "Getting notifications for the avatar scene";
        LoadingManager.instance.EnqueueLoad(Job);

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserColorNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id);
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
            NotificationsColorsJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
        }
        LoadingManager.instance.DequeueLoad(Job);
    }
}
