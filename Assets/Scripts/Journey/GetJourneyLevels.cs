using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class GetJourneyLevels : MonoBehaviour
{
  public static XPProgressionData journeyLevels;
  public static AvatarAccUnlocksContainer journeyAvatarParts;
  SetupJourney temp;

  async void Awake()
  {
    await GetLevels();
    await GetJourneyAvatarUnlocks();


    temp = GetComponent<SetupJourney>();
    temp.CurrentJourneyLevel();
    temp.SetupRewards();
  }

  public void JourneySlider()
  {
    bool allLocked = true;
    foreach (BannerInfo banner in temp.banners)
    {
      if (banner.button.interactable && allLocked)
      {
        allLocked = false;
        temp.scrollRectUtilities.target = banner.GetComponent<RectTransform>();
      }
    }
    if (allLocked && UserLevelComponent.UserLevels.All.Length > 0)
      temp.scrollRectUtilities.target = temp.banners[UserLevelComponent.UserLevels.All[0].UserLevel - 1].GetComponent<RectTransform>();
    else if(allLocked)
      temp.scrollRectUtilities.target = temp.banners[0].GetComponent<RectTransform>();

    temp.scrollRectUtilities.JumpToItem(temp.scrollRectUtilities.target);
  }

  public static CancellationTokenSource cancel = new();

  public async Task GetJourneyAvatarUnlocks()
  {
    string Job = "Fetching Journey Unlocks";
    LoadingManager.instance.EnqueueLoad(Job);
    LoadingManager.instance.DequeueLoad(Job);
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getLvAvatarSets.php");
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
      Debug.Log("Get all avatar accessories failed " + www.error);
    }
    else
    {
      journeyAvatarParts = JsonUtility.FromJson<AvatarAccUnlocksContainer>("{ 	\"All\": 	" + www.downloadHandler.text + "}");
    }
    LoadingManager.instance.DequeueLoad(Job);
  }

  public static async Task GetLevels()
  {
    string Job = "Fetching Journey Levels";
    LoadingManager.instance.EnqueueLoad(Job);
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getXPProgression.php");

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
      Debug.LogError("Purchase failed " + www.error);
    }
    else
    {
      journeyLevels = JsonUtility.FromJson<XPProgressionData>("{\"Levels\":" + www.downloadHandler.text + "}");
    }
    LoadingManager.instance.DequeueLoad(Job);
  }
}
