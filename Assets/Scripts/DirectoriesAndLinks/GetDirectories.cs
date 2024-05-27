using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GetDirectories : MonoBehaviour
{
    public string bundleUrl;
    public static GetDirectories Instance;
    public Dictionary<string, string> directories = new();
    public Directories directory;
    private async void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        await SetDirectories();
    }

    public static CancellationTokenSource cancel = new();

    public async Task SetDirectories()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://game.maximus2020.sk/maximus/getSettings.php");

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");

        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Download failed " + www.error);
        }
        else
        {
            directory = JsonUtility.FromJson<Directories>("{\"settings\":" + www.downloadHandler.text + "}");

            foreach (var item in directory.settings)
            {
                directories.Add(item.skey, item.svalue);
            }


            bundleUrl = directories[DirectoryKey.SERVICE_URL.ToString()] + directories[DirectoryKey.BUNDLES_FOLDER.ToString()];
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    bundleUrl += "/Android/";
                    break;
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    bundleUrl += "/Windows/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    bundleUrl += "/IOS/";
                    break;
            }


            StartCoroutine(AssetBundleCacher.Instance.DownloadAndCacheAssetBundle(bundleUrl, Application.persistentDataPath));
        }
    }
}
