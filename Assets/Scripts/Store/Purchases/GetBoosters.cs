using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;

public class GetBoosters : MonoBehaviour
{
    [SerializeField]
    private BoostersInfo[] boostersInfo;

    BoostersData boosters;

    async void Start()
    {
        await GetBooster();
    }

    public CancellationTokenSource cancel = new CancellationTokenSource();

    private async Task GetBooster()
    {
        UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getBoosters.php");

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
            boosters = JsonUtility.FromJson<BoostersData>("{\"boosters\":" + www.downloadHandler.text + "}");

            for (int i = 0; i < boostersInfo.Length; i++)
            {
                boostersInfo[i].Booster = boosters.boosters[i];
            }
        }
    }
}
