using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.Networking;
using System.Threading;
using TMPro;

public class ServerTimeCoundown : MonoBehaviour
{
    public static ServerTimeCoundown instance;
    public DateTime time;

    public CancellationTokenSource cancel = new CancellationTokenSource();

    public List<TextMeshProUGUI> ticking = new List<TextMeshProUGUI>();

    public async void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        await GetTimeUntilReset();
        StartCoroutine(Tick());
    }


    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
    private void FixedUpdate()
    {
        time = time.AddSeconds(Time.deltaTime);

    }
    public IEnumerator Tick()
    {
        yield return new WaitForSeconds(1);
        string res = "";
        if (23 - time.Hour > 0)
        {
            res += (23 - time.Hour) + "h ";
        }

        if (59 - time.Minute > 0)
        {
            res += (59 - time.Minute) + "m";
        }
        if (res == "")
        {
            res = "< 1m";
        }
        foreach (TextMeshProUGUI este in ticking)
        {
            este.text = res;
        }

        if (!cancel.Token.IsCancellationRequested)
        {
            StartCoroutine(Tick());
        }
        else
        {
            ticking.Clear();
        }
    }

    public async Task GetTimeUntilReset()
    {
        string Job = "Grabbing server time";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getTime.php");

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
            Debug.LogError("Couldnt get server time " + www.error);
        }
        else
        {
            string[] res = www.downloadHandler.text.Split('/');
            time = new DateTime(int.Parse(res[2]), int.Parse(res[1]), int.Parse(res[0]), int.Parse(res[3]), int.Parse(res[4]), int.Parse(res[5]), 0);
        }
        LoadingManager.instance.DequeueLoad(Job);
    }
}
