using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BuyQuiids : MonoBehaviour
{
    [SerializeField]
    private Button btn;
    public int purchaseID;
    [SerializeField]
    TextMeshProUGUI Quiids;

    [SerializeField]
    TextMeshProUGUI Qiis;

    [SerializeField]
    TextMeshProUGUI value;

    [SerializeField]
    TextMeshProUGUI timer;

    [SerializeField]
    private GameObject Claimed;

    [SerializeField]
    private CurrencyBaner banner;

    [SerializeField]
    Slider QiisSlider;

    CurrencyData currency;
    private void Start()
    {
        //timer
        currency = new CurrencyData();
        if (purchaseID == 0)
        {
            btn.interactable = !UserLogin.instance.DailyOfferRedeemed;
            Claimed.SetActive(UserLogin.instance.DailyOfferRedeemed);
            ServerTimeCoundown.instance.ticking.Add(timer);
        }
    }

    public async void Buy()
    {
        if (purchaseID == 0)
        {

            if (!UserLogin.instance.DailyOfferRedeemed)
            {
                Debug.Log("Giving free quids");
                await ConvertQiis(purchaseID);
                await SetUserDaillyOffer();
            }
            else
            {
                Debug.Log("Offer already redeemed");
                btn.interactable = false;
            }
        }
        else
        {
            await ConvertQiis(purchaseID);
        }
    }

    public CancellationTokenSource cancel = new CancellationTokenSource();

    private async Task ConvertQiis(int id)
    {
        string Job = "Converting Qiis to Quiids";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "BuyCurrency.php?userID=" + UserLogin.instance.LogInInfo.user.id + "&ID=" + id);

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                LoadingManager.instance.DequeueLoad(Job);
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            LoadingManager.instance.DequeueLoad(Job);
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Purchase failed " + www.error);
        }
        else
        {
            if (www.downloadHandler.text != "1")
            {
                await UserLevelComponent.GetUserLevel();
                await GetUserBalance();
                banner.quiids.text = value.text;
                banner.transform.parent.gameObject.SetActive(true);
            }
        }
        LoadingManager.instance.DequeueLoad(Job);

    }

    private async Task GetUserBalance()
    {
        string Job = "Fetching the User's Balance";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getUserBalance.php?userID=" + UserLogin.instance.LogInInfo.user.id);

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                LoadingManager.instance.DequeueLoad(Job);
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            LoadingManager.instance.DequeueLoad(Job);
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Purchase failed " + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.downloadHandler.text != "null")
            {
                currency = JsonUtility.FromJson<CurrencyData>(www.downloadHandler.text);
                Quiids.text = currency.QUI;
                Qiis.text = currency.QI;
                QiisSlider.value = int.Parse(currency.QI);
            }
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    private async Task SetUserDaillyOffer()
    {
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserDailyOffer.php?user=" + UserLogin.instance.LogInInfo.user.id);

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
            Debug.Log("Done");
            UserLogin.instance.DailyOfferRedeemed = true;
            btn.interactable = false;
            Claimed.SetActive(true);
            ServerTimeCoundown.instance.ticking.Add(timer);
        }
    }
}
