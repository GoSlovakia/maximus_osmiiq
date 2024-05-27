using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;
using System.Xml.Serialization;
using System.Linq;

public class InboxGenerator : MonoBehaviour
{
    public InboxItemContainer prefab;
    public static InboxGenerator instance;
    public InboxList InboxList;
    private string inboxJSON;
    public CancellationTokenSource cancel = new CancellationTokenSource();

    public string InboxJSON
    {
        get => inboxJSON; set
        {
            inboxJSON = value;
            Debug.Log(inboxJSON);
            if (InboxList != null) { JsonUtility.FromJsonOverwrite(InboxJSON, InboxList); } else { InboxList = JsonUtility.FromJson<InboxList>(inboxJSON); }

        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }
    public void Start()
    {
        GetInbox();
    }

    public async void GetInbox()
    {
        await GetUserInbox();
        GenerateItems();
    }

    public void GenerateItems()
    {
        Debug.Log("Inbox size " + InboxList.All.Count());
        foreach (var este in InboxList.All)
        {
            var novo = Instantiate(prefab, transform);
            novo.Item = este;
        }
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
            InboxJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";


        }
    }

}
