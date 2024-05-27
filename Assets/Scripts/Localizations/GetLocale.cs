using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GetLocale : MonoBehaviour
{
  public static AllAchievements allAchievements;
  public AllAchievements sallAchievements;
  async void Start()
  {
    DontDestroyOnLoad(gameObject);
    await GetCardsLanguage(PlayerPrefs.GetInt("CurrentLanguageID"));
    await GetMissionsLanguage(PlayerPrefs.GetInt("CurrentLanguageID"));
  }

  private void Update()
  {
    sallAchievements = allAchievements;
  }
  public static CancellationTokenSource cancel = new CancellationTokenSource();

  public static async Task GetCardsLanguage(int id)
  {
    string Job = "Getting language pack";
    LoadingManager.instance.EnqueueLoad(Job);
    UnityWebRequest www;
    switch (id)
    {
      case 0:
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.CARDS_LOCALE.ToString()] + "EnTranslation.json");
        break;
      case 1:
        //greek 
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.CARDS_LOCALE.ToString()] + "EnTranslation.json");
        break;
      case 2:
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.CARDS_LOCALE.ToString()] + "PtTranslation.json");
        break;
      case 3:
        //Slovak
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.CARDS_LOCALE.ToString()] + "EnTranslation.json");
        break;
      case 4:
        //Spanish
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.CARDS_LOCALE.ToString()] + "EnTranslation.json");
        break;

      default:
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.CARDS_LOCALE.ToString()] + "EnTranslation.json");
        Debug.LogError("No Language");
        break;
    }

    if (www.url == "")
    {
      //this is for debuging
      Debug.Log("Using Local");
      CardManager.GenerateCardsFromServer();
    }
    else
    {
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
        Debug.LogError("Get language pack " + www.error);
      }
      //Debug.LogError(www.downloadHandler.text);
      else
      {
        // Show results as text
        string CardsJson = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
        CardManager.AllCards = JsonUtility.FromJson<CardContainer>(CardsJson);
        for (int i = 0; i < CardManager.UserCards.Count; i++)
        {
          for (int j = 0; j < CardManager.AllCards.All.Length; j++)
          {
            if (CardManager.UserCards[i].id == CardManager.AllCards.All[j].id)
            {
              int temp = CardManager.UserCards[i].copies;
              CardManager.UserCards[i] = CardManager.AllCards.All[j];
              CardManager.UserCards[i].copies = temp;
            }
          }
        }
        //Avancar para o proximo

      }

    }
    LoadingManager.instance.DequeueLoad(Job);
  }

  public static async Task GetMissionsLanguage(int id)
  {
    string Job = "Getting language pack";
    LoadingManager.instance.EnqueueLoad(Job);
    UnityWebRequest www;

    switch (id)
    {
      case 0:
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.MISSIONS_LOCALE.ToString()] + "MissionsEnTranslation.json");
        break;
      case 1:
        //greek 
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.MISSIONS_LOCALE.ToString()] + "EnTranslation.json");
        break;
      case 2:
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.MISSIONS_LOCALE.ToString()] + "MissionsPtTranslation.json");
        break;
      case 3:
        //Slovak
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.MISSIONS_LOCALE.ToString()] + "EnTranslation.json");
        break;
      case 4:
        //Spanish
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.MISSIONS_LOCALE.ToString()] + "EnTranslation.json");
        break;

      default:
        www = UnityWebRequest.Get(LoadSVGs.IP + GetDirectories.Instance.directories[DirectoryKey.MISSIONS_LOCALE.ToString()] + "EnTranslation.json");
        Debug.LogError("No Language");
        break;
    }

    if (www.url == "")
    {
      //this is for debuging
      Debug.Log("Using Local");
    }
    else
    {
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
        Debug.LogError("Get language pack " + www.error);
      }
      //Debug.LogError(www.downloadHandler.text);
      else
      {
        // Show results as text
        string allAchievementsJson = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
        allAchievements = JsonUtility.FromJson<AllAchievements>(allAchievementsJson);

        for (int i = 0; i < allAchievements.All.Length; i++)
        {
          for (int j = 0; j < AchievementTrackerComponent.instance.AllAchievements.All.Length; j++)
          {
            if (allAchievements.All[i].ID == AchievementTrackerComponent.instance.AllAchievements.All[j].ID)
            {
              AchievementTrackerComponent.instance.AllAchievements.All[j].Name = allAchievements.All[i].Name;
              AchievementTrackerComponent.instance.AllAchievements.All[j].DescriptionEN = allAchievements.All[i].DescriptionEN;
            }
          }
        }
        //Avancar para o proximo

      }

    }
    LoadingManager.instance.DequeueLoad(Job);
  }
}
