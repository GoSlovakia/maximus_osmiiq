using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;
using TMPro;
using System.Linq;

public class GetCardsAndSets : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI cards, duplicates, sets;

  private static TextMeshProUGUI Cards, Duplicates, Sets;

  private static InventoryCardsData inventoryCards;
  private void Start()
  {
    Cards = cards;
    Duplicates = duplicates;
    Sets = sets;
    RefreshNumbers();
  }

  public static async void RefreshNumbers()
  {
    await GetInventoryCards();

    Cards.text = inventoryCards.card.Length.ToString() + "/" + await GetTotalCardsCount();
    int duplicateCount = 0;
    foreach (Cards card in inventoryCards.card)
    {
      if (card.CardAmount > 1)
        duplicateCount += card.CardAmount - 1;
    }
    Duplicates.text = duplicateCount.ToString();
    Sets.text = AchievementTrackerComponent.instance.GetVariable(VariableType.AllSetsCompleted).ToString() + "/" + CardManager.CardSets.All.Count().ToString();//await GetSets() + "/" + await GetTotalSetsCount();
  }

  public static CancellationTokenSource cancel = new CancellationTokenSource();

  public static async Task GetInventoryCards()
  {
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getUserTotalCards.php?userID=" + UserLogin.instance.LogInInfo.user.id);

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
      inventoryCards = JsonUtility.FromJson<InventoryCardsData>("{\"card\":" + www.downloadHandler.text + "}");
    }
  }

  public static async Task<string> GetTotalCardsCount()
  {
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getTotalCardsCount.php");

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
      return www.downloadHandler.text;
    }
    return null;
  }


  public static async Task<string> GetSets()
  {
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getUserTotalSetsCount.php?userID=" + UserLogin.instance.LogInInfo.user.id);

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
      return null;
    }
    else
    {
      SetsData serverSets = JsonUtility.FromJson<SetsData>("{\"set\":" + www.downloadHandler.text + "}");
      List<Setss> temp = new List<Setss>();
      for (int i = 0; i < serverSets.set.Length; i++)
      {
        if (temp.Count == 0)
          temp.Add(serverSets.set[i]);
        else
        {
          for (int j = 0; j < temp.Count; j++)
          {
            if (temp[j].SetID == serverSets.set[i].SetID)
              break;
            else if (j == temp.Count - 1)
              temp.Add(serverSets.set[i]);
          }
        }
      }
      return temp.Count.ToString();
    }

  }

  public static async Task<string> GetTotalSetsCount()
  {
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getTotalSetsCount.php");

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
      return www.downloadHandler.text;
    }
    return null;
  }
}
