using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine.UI;

public class GetCurrency : MonoBehaviour
{
  public static GetCurrency instance;
  public TextMeshProUGUI Quiids, Qiis;
  void Start()
  {

    instance = this;
    UpdateUserBalance();
  }

  public async void UpdateUserBalance()
  {
    if (Qiis != null)
    {
      if (PlayerPrefs.GetString("UserQuiids") != string.Empty)
        Quiids.text = PlayerPrefs.GetString("UserQuiids");

      if (PlayerPrefs.GetString("UserQiis") != string.Empty)
        Qiis.text = PlayerPrefs.GetString("UserQiis");

      await GetUserBalance(Quiids, Qiis);
    }
    else
    {
      if (PlayerPrefs.GetString("UserQuiids") != string.Empty)
        Quiids.text = PlayerPrefs.GetString("UserQuiids");

      await GetUserBalance(Quiids);
    }
  }

  public static CancellationTokenSource cancel = new CancellationTokenSource();

  public static async Task GetUserBalance(TextMeshProUGUI Quiids)
  {
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getUserBalance.php?userID=" + UserLogin.instance.LogInInfo.user.id);

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
      PlayerPrefs.SetString("UserQuiids", JsonUtility.FromJson<CurrencyData>(www.downloadHandler.text).QUI);
      Quiids.text = PlayerPrefs.GetString("UserQuiids");

      //Quiids.text = JsonUtility.FromJson<CurrencyData>(www.downloadHandler.text).QUI;
    }
  }

  public static async Task GetUserBalance(TextMeshProUGUI Quiids, TextMeshProUGUI Qiis)
  {
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getUserBalance.php?userID=" + UserLogin.instance.LogInInfo.user.id);

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
      if (www.downloadHandler.text != "null")
      {
        PlayerPrefs.SetString("UserQuiids", JsonUtility.FromJson<CurrencyData>(www.downloadHandler.text).QUI);
        Quiids.text = PlayerPrefs.GetString("UserQuiids");
        PlayerPrefs.SetString("UserQiis", JsonUtility.FromJson<CurrencyData>(www.downloadHandler.text).QI);
        Qiis.text = PlayerPrefs.GetString("UserQiis");
        /*Quiids.text = JsonUtility.FromJson<CurrencyData>(www.downloadHandler.text).QUI;
        Qiis.text = JsonUtility.FromJson<CurrencyData>(www.downloadHandler.text).QI;*/
      }
      else
      {
        Quiids.text = "0";
        Qiis.text = "0";
      }
    }
  }

  public static async Task<CurrencyData> GetUserBalance()
  {
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getUserBalance.php?userID=" + UserLogin.instance.LogInInfo.user.id);

    var req = www.SendWebRequest();
    while (!req.isDone)
    {
      await Task.Yield();
      if (cancel.Token.IsCancellationRequested)
      {
        Debug.Log("Canceled");
        return null;
      }
    }

    if (cancel.Token.IsCancellationRequested)
    {
      Debug.Log("Canceled");
      return null;
    }

    if (www.result != UnityWebRequest.Result.Success)
    {
      Debug.LogError("Purchase failed " + www.error);
    }
    else
    {
      if (www.downloadHandler.text != "null")
        return JsonUtility.FromJson<CurrencyData>(www.downloadHandler.text);
      else
      {
        return new CurrencyData();
      }
    }
    return null;
  }
}
