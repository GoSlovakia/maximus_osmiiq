using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEngine.UI;
using System.ComponentModel;
using static System.Net.WebRequestMethods;

public class UserLogin : MonoBehaviour
{
  public static UserLogin instance { get; private set; }
  public UserAvatar UserAvatar
  {
    get => userAvatar; set
    {

      userAvatar = value;
    }
  }

  public UserTitleArray userTitles;

  private string _userTitlesJSON;
  public string UserTitlesJSON
  {
    get => _userTitlesJSON;
    set
    {
      _userTitlesJSON = value;
      Debug.Log(value);
      userTitles = JsonUtility.FromJson<UserTitleArray>(_userTitlesJSON);
      Debug.Log(userTitles.All[0].AdjID);
    }
  }

  public string username;
  public string password;
  public CancellationTokenSource cancel = new CancellationTokenSource();
  public LogIn LogInInfo;
  private UserAvatar userAvatar;
  public Texture AvatarImage;
  public bool rememberPassword;
  public bool rememberUsername;
  public bool DailyOfferRedeemed = false;

  //public string LoginIP = "https://maximus2020.sk/api";
  public string LoginIP = GetDirectories.Instance.directories[DirectoryKey.URL.ToString()] + "/api";


  public void Awake()
  {
    if (instance != null && instance != this)
      Destroy(gameObject);
    else
      instance = this;


    UserLevelComponent.cancel = cancel;
    DontDestroyOnLoad(gameObject);
  }

  public void ChangeUserField(string novo)
  {
    username = rememberUsername ? username : novo;
  }

  public void ChangePasswordField(string novo)
  {
    password = rememberPassword ? username : novo;
  }

  public void LogIn()
  {
    //UserLevelComponent.GetLevelCaps();
    Debug.Log("beginning process");
    LogInProcess();
  }
  private async void LogInProcess()
  {
    Debug.Log("beginning process 2");
    await Login();
  }

  private void SetUser(string handlertext)
  {
    if (LogInInfo == null)
    {
      LogInInfo = JsonUtility.FromJson<LogIn>(handlertext);
    }
    else
    {
      JsonUtility.FromJsonOverwrite(handlertext, LogInInfo);
      //Debug.Log("Overwritting");
    }
    //LogInInfo.token = LogInInfo.token.Substring(LogInInfo.token.IndexOf("|") + 1);
    //Debug.Log("Log In successful, user set to " + LogInInfo.user.first_name + " id:" + LogInInfo.user.id + " token " + LogInInfo.token);
  }

  private void SetUserAvatar(string handlertext)
  {
    Debug.Log("Avatar " + handlertext);
    if (UserAvatar == null)
      UserAvatar = JsonUtility.FromJson<UserAvatar>(handlertext);
    else
    {
      JsonUtility.FromJsonOverwrite(handlertext, UserAvatar);
    }
    //Debug.Log(UserAvatar.All[0].part);

    //USAR APENAS PARA REMOVER ACESSORIOS DUPLICADOS
    //Nao utilizar isto porque significa que existe um erro em qualquer lado, arranja o erro e nao confies neste metodo
    //CleanUpAvatar();
  }

  //private void CleanUpAvatar()
  //{
  //    UserAvatar CleanUp = UserAvatar;

  //    foreach (var este in UserAvatar.All)
  //    {

  //        if (CleanUp.All.Where(x => x.part.Contains(este.part.Remove(este.part.Length - 3))).Count() > 1)
  //        {
  //            if (CleanUp.All.Where(x => x != este).Distinct().ToArray().Length != CleanUp.All.Where(x => x != este).ToArray().Length)
  //            {
  //                SetUserParts.SetUserPartsSingleton.RemoveUserPartsOnServer(LogInInfo.user.id, CleanUp.All.Where(x => x.part.Contains(este.part.Remove(este.part.Length - 3))).Single().part);
  //            }
  //            Debug.Log("Duplicate Found " + este.part.Remove(este.part.Length - 3));

  //            CleanUp.All = CleanUp.All.Where(x => x != este).Distinct().ToArray();
  //            SetUserParts.SetUserPartsSingleton.RemoveUserPartsOnServer(LogInInfo.user.id, este.part);
  //        }
  //    }
  //    UserAvatar = CleanUp;
  //}

  private async Task Login()
  {
    string Job = "Logging in";
    LoadingManager.instance.EnqueueLoad(Job);
    WWWForm form = new WWWForm();
    Debug.Log("Using " + username + " " + password);
    form.AddField("email", username);
    form.AddField("password", password);

    using (UnityWebRequest www = UnityWebRequest.Post(LoginIP + "/login", form))
    {
      //www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
      www.SetRequestHeader("Accept", www.GetRequestHeader("Content-Type"));
      Debug.Log(www.GetRequestHeader("Content-Type"));

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
        Debug.Log("LogInFailed " + www.error + " " + www.url);
        switch (www.error)
        {
          case string a when a.Contains("401"):
            LogInfailComponent.instance.ShowError(LogFail.UNAUTHORIZED.ToDescription());
            break;
          case string a when a.Contains("408"):
            LogInfailComponent.instance.ShowError(LogFail.TIMEOUT.ToDescription());
            break;
          case string a when a.Contains("400"):
            LogInfailComponent.instance.ShowError(LogFail.BAD_REQUEST.ToDescription());
            break;
          case string a when a.Contains("500"):
            LogInfailComponent.instance.ShowError(LogFail.SERVER_ERROR.ToDescription());
            break;
          case string a when a.Contains("502"):
            LogInfailComponent.instance.ShowError(LogFail.BAD_GATEWAY.ToDescription());
            break;
          case string a when a.Contains("504"):
            LogInfailComponent.instance.ShowError(LogFail.GATEWAY_TIMEOUT.ToDescription());
            break;
          default:
            LogInfailComponent.instance.ShowError(LogFail.UNKNOWN.ToDescription() + www.error);
            break;
        }
        LoadingManager.instance.ResetQueue();
      }
      else
      {
        SetUser(www.downloadHandler.text);
        //GetAvatarFromServer();
        AvatarImage = await GetAvatarImage();
        if (AvatarImage != null)
          AvatarImage.name = LogInInfo.user.avatar_file_name;

        GetTitles.GenerateGetUserTitlesRequest();
        GetTitles.GenerateGetTitlesRequest();
        Task A = AchievementTrackerComponent.instance.GetAllAchievements();
        Task B = AchievementTrackerComponent.instance.GetUserStats();
        Task C = LoadCards.LoadAll(true);
        Task D = UserSetsComponent.GetUserSets();
        Task E = UserLevelComponent.GetLevelCaps();
        Task F = UserLevelComponent.GetUserLevel();

        Task G = GetTitles.GetUserTitles();


        Task H = LoadSVGs.GeneralInfo(true);
        Task I = GetTitles.GetAllTitles();
        Task J = GetDailyOfferRedeemed();

        await Task.WhenAll(A, B, C, D, E, F, G, H, I, J);



        username = rememberUsername ? username : null;
        password = rememberPassword ? password : null;
        //Debug.Log("Saving " + username + " " + password);
        PlayerPrefs.SetString("User", username);
        PlayerPrefs.SetString("Password", password);
        PlayerPrefs.Save();

        //Debug.Log("Username " + PlayerPrefs.GetString("User") + " Password" + PlayerPrefs.GetString("Password"));
        //UserInfo();
        LoadingManager.instance.DequeueLoad(Job);

        SceneManager.LoadScene("MainMenu");

      }
    }
  }

  public IEnumerator LogOut()
  {
    string Job = "Logging out";
    LoadingManager.instance.EnqueueLoad(Job);
    using (UnityWebRequest www = UnityWebRequest.Get(LoginIP + "/logout"))
    {
      Debug.Log(LogInInfo.token);
      //www.SetRequestHeader("Content-Type", "multipart/form-data");
      www.SetRequestHeader("Authorization", "Bearer " + LogInInfo.token);
      www.SetRequestHeader("Accept", "application/json");

      Debug.Log(www.GetRequestHeader("Accept") + " " + www.GetRequestHeader("Authorization") + " " + www.GetRequestHeader("Content-Type"));

      yield return www.SendWebRequest();

      if (www.result != UnityWebRequest.Result.Success)
      {
        Debug.Log(www.error);
        LogInInfo = null;
        UserAvatar = null;
        SceneManager.LoadScene("Login Scene");
      }
      else
      {

        Debug.Log("Log Out Success " + www.result + " " + www.downloadHandler.text);
        LogInInfo = null;
        UserAvatar = null;
        PlayerPrefs.DeleteKey("User");
        PlayerPrefs.DeleteKey("Password");
        PlayerPrefs.Save();

        SceneManager.LoadScene("Login Scene");
      }
    }
    LoadingManager.instance.DequeueLoad(Job);
    loggingOut = false;
  }

  public async Task UpdateUserImage()
  {
    try
    {
      bool success = false;
      UnityMainThread.wkr.AddJob(async () =>
      {
        string Job = "Updating the User's Image";
        LoadingManager.instance.EnqueueLoad(Job);
        await GetAvatarFileLocation();

        AvatarImage = await GetAvatarImage();

        AvatarImage.name = LogInInfo.user.avatar_file_name;
        Debug.Log("Image Loaded " + LogInInfo.user.avatar_file_name);
        LoadingManager.instance.DequeueLoad(Job);
        success = true;
      });


      while (!success)
      {
        await Task.Yield();
      }
    }
    catch (Exception e)
    {
      Debug.LogError(e);
    }
  }

  public async Task GetAvatarFileLocation()
  {
    try
    {
      string Job = "Downloading the link to the new avatar";
      LoadingManager.instance.EnqueueLoad(Job);
      bool success = false;
      UnityMainThread.wkr.AddJob(async () =>
      {

        Debug.Log("Getting new avatar location");

        UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.URL.ToString()] + "api/game/profile");
        //www.SetRequestHeader("Content-Type", "multipart/form-data");
        www.SetRequestHeader("Accept", "application/json");
        www.SetRequestHeader("Authorization", "Bearer " + LogInInfo.token);

        var req = www.SendWebRequest();

        while (!req.isDone)
        {
          await Task.Yield();
          if (cancel.Token.IsCancellationRequested)
          {
            LoadingManager.instance.DequeueLoad(Job);
            Debug.Log("Canceled");
            return;
          }
        }

        if (cancel.Token.IsCancellationRequested)
        {
          LoadingManager.instance.DequeueLoad(Job);
          Debug.Log("Canceled");
          return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
          Debug.Log("Couldnt Get Avatar PNG " + www.error + " " + www.url);
        }
        else
        {

          LogInInfo.user.avatar_file_name = JsonUtility.FromJson<User>(www.downloadHandler.text).avatar_file_name;
          //Debug.Log(JsonUtility.FromJson<User>(www.downloadHandler.text).avatar_file_name + " is the new avatar " + LogInInfo.user.avatar_file_name);
          // Debug.Log("User Profile Success " + www.downloadHandler.text + " " + www.downloadHandler.data.Length);
          //UserAvatarIMG = DownloadHandlerTexture.GetContent(www);

        }
        success = true;
        LoadingManager.instance.DequeueLoad(Job);
      });

      while (!success)
      {
        await Task.Yield();
      }
    }
    catch (Exception e)
    {
      Debug.LogError(e.ToString());
    }
  }

  public async Task UpdateUserInfo(byte[] data, bool refresh)
  {
    try
    {
      Debug.Log("OLAAAAAAAAAAAAA");
      string Job = "Updating avatar image on the server";
      LoadingManager.instance.EnqueueLoad(Job);
      bool success = false;
      UnityMainThread.wkr.AddJob(async () =>
      {

        List<IMultipartFormSection> formm = new List<IMultipartFormSection>
          {
                    new MultipartFormFileSection("photo", data, "avatar" + LogInInfo.user.id + ".png", "image/png")
          };
        //Debug.Log("avatar" + LogInInfo.user.id + " " + data.Length);
        //WWWForm form = new WWWForm();
        //form.AddBinaryData("photo", data,"avatar.png","image/png");

        //UnityWebRequest www = UnityWebRequest.Put(LoginIP + "/game/profile", form.data);
        UnityWebRequest www = UnityWebRequest.Post(LoginIP + "/game/profile", formm);

        //www.method = "POST";
        //www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("Accept", "multipart/form-data");
        www.SetRequestHeader("Authorization", "Bearer " + LogInInfo.token);

        var req = www.SendWebRequest();

        while (!req.isDone)
        {
          await Task.Yield();
          if (cancel.Token.IsCancellationRequested)
          {
            LoadingManager.instance.DequeueLoad(Job);

            Debug.Log("Canceled");
            return;
          }
        }

        if (cancel.Token.IsCancellationRequested)
        {
          LoadingManager.instance.DequeueLoad(Job);
          Debug.Log("Canceled " + www.url);
          return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
          Debug.Log("Failed " + www.error + " " + www.url);
        }
        else
        {
          Debug.Log("User Profile Update " + www.result + " " + "avatar" + LogInInfo.user.id + ".png");
        }

        success = true;
      });

      while (!success)
      {
        await Task.Yield();
      }

      if (refresh)
      {
        await instance.UpdateUserImage();
      }
      LoadingManager.instance.DequeueLoad(Job);
    }
    catch (Exception e)
    {
      Debug.LogError(e.ToString());
    }
  }

  //Old Login
  //public async void Login()
  //{
  //    UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "login.php?" + "username=" + username + "&password=" + password);
  //    var req = www.SendWebRequest();
  //    while (!req.isDone)
  //    {
  //        await Task.Yield();
  //        if (cancel.Token.IsCancellationRequested)
  //        {
  //            Debug.Log("Canceled");
  //            return;
  //        }
  //    }

  //    if (cancel.Token.IsCancellationRequested)
  //    {
  //        Debug.Log("Canceled");
  //        return;
  //    }

  //    if (www.result != UnityWebRequest.Result.Success)
  //    {
  //        Debug.Log("LogInFailed " + www.error);
  //    }
  //    else
  //    {
  //        if (www.downloadHandler.text != "Unauthorized")
  //        {
  //            SetUser(www.downloadHandler.text.Remove(0, 1).Remove(www.downloadHandler.text.Length - 2, 1));
  //            GetAvatarFromServer();
  //        }
  //        else
  //        {
  //            //Log in fail
  //            Loginfail.Invoke(LogFail.WRONG_DATA);
  //        }
  //    }

  //}

  public async Task GetAvatarFromServer()
  {

    bool done = false;
    UnityMainThread.wkr.AddJob(async () =>
    {
      string Job = "Getting Avatar from server";
      LoadingManager.instance.EnqueueLoad(Job);
      UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserAvatarParts.php?" + "user=" + LogInInfo.user.id);

      var req = www.SendWebRequest();
      while (!req.isDone)
      {
        await Task.Yield();
        if (cancel.Token.IsCancellationRequested)
        {
          Debug.Log("Canceled " + cancel.IsCancellationRequested);
          return;
        }
      }

      if (cancel.Token.IsCancellationRequested)
      {
        Debug.Log("Canceled " + cancel.IsCancellationRequested);
        return;
      }

      if (www.result != UnityWebRequest.Result.Success)
      {
        Debug.Log("Couldnt get Avatar " + www.error);
        AvatarReader.LoadDefaultAvatar();
      }
      else
      {
        if (www.downloadHandler.text == null)
        {
          Debug.Log("NO AVATAR FOUND");
          AvatarReader.LoadDefaultAvatar();
        }
        else
          SetUserAvatar("{ 	\"All\": 	" + www.downloadHandler.text + "}");
      }
      done = true;
      LoadingManager.instance.DequeueLoad(Job);
    });
    while (!done)
    {
      await Task.Yield();
    }
  }

  public async Task GetDailyOfferRedeemed()
  {
    string Job = "Checking if the user redeemed the daily reward";
    LoadingManager.instance.EnqueueLoad(Job);
    bool done = false;
    UnityMainThread.wkr.AddJob(async () =>
    {

      UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserDailyOffer.php?" + "user=" + LogInInfo.user.id);

      var req = www.SendWebRequest();
      while (!req.isDone)
      {
        await Task.Yield();
        if (cancel.Token.IsCancellationRequested)
        {
          Debug.Log("Canceled " + cancel.IsCancellationRequested);
          return;
        }
      }

      if (cancel.Token.IsCancellationRequested)
      {
        Debug.Log("Canceled " + cancel.IsCancellationRequested);
        return;
      }

      if (www.result != UnityWebRequest.Result.Success)
      {
        Debug.Log("Couldnt get Avatar " + www.error);
        AvatarReader.LoadDefaultAvatar();
      }
      else
      {
        //Debug.Log(www.downloadHandler.text+ " Daily offer");
        DailyOfferRedeemed = www.downloadHandler.text == "1";
      }
      done = true;
    });
    while (!done)
    {
      await Task.Yield();
    }

    LoadingManager.instance.DequeueLoad(Job);
  }

  public async Task<Texture> GetAvatarImage()
  {
    string Job = "Downloading the image of the avatar";
    LoadingManager.instance.EnqueueLoad(Job);

    UnityWebRequest www = UnityWebRequestTexture.GetTexture("https://maximus2020.sk/storage/" + LogInInfo.user.avatar_file_name.Replace("avatars\\/", ""));

    Debug.Log("Loading Avatar" + LogInInfo.user.avatar_file_name.Replace("avatars\\/", ""));
    var req = www.SendWebRequest();
    while (!req.isDone)
    {
      await Task.Yield();
      if (cancel.Token.IsCancellationRequested)
      {
        Debug.Log("Canceled");
        LoadingManager.instance.DequeueLoad(Job);
        return null;
      }
    }

    if (cancel.Token.IsCancellationRequested)
    {
      Debug.Log("Canceled");
      LoadingManager.instance.DequeueLoad(Job);
      return null;
    }

    if (www.result != UnityWebRequest.Result.Success)
    {
      Debug.LogWarning("Image Not Found " + www.error);
      LoadingManager.instance.DequeueLoad(Job);
      return null;
    }
    else
    {
      LoadingManager.instance.DequeueLoad(Job);
      // Debug.Log("Avatar image downloaded");

      return DownloadHandlerTexture.GetContent(www);
    }



  }


  ////Nao ha log in page, portanto vou meter o log in no start e mete se os dados no editor
  //public void Start()
  //{
  //    Login();
  //}

  public bool loggingOut = false;
  public void Logout()
  {
    loggingOut = true;
    instance.StartCoroutine(LogOut());

  }

  public async Task ResetUser()
  {
    string Job = "Resetting User";
    LoadingManager.instance.EnqueueLoad(Job);
    UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "resetUser.php?user=" + LogInInfo.user.id);

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
      Debug.LogError("Reset Failed " + www.error);
    }
    else
    {
      Debug.Log("Reset Success");
      DailyOfferRedeemed = false;
      Logout();

    }
    LoadingManager.instance.DequeueLoad(Job);

  }

  private void OnApplicationQuit()
  {
    if (cancel != null)
      cancel.Cancel();
  }

  public delegate void LogInfail(LogFail reason);

  public static LogInfail Loginfail;

}

public enum LogFail
{
  //401
  [Description("Username and Password combination incorrect. \nPlease verify the credentials again.")]
  UNAUTHORIZED,
  //408
  [Description("Connection Timed out \nPlease verify your internet connection, your signal might be too weak.")]
  TIMEOUT,
  //400
  [Description("Request had bad syntax or was impossible to fulfill")]
  BAD_REQUEST,
  //500
  [Description("Error in the code \nBlame the devs.")]
  SERVER_ERROR,
  //502
  [Description("Server is currently down, or as a config error.")]
  BAD_GATEWAY,
  //504
  [Description("Service did not respond within the time frame that the gateway was willing to wait")]
  GATEWAY_TIMEOUT,
  //OUTROS
  [Description("An Unknown error has occured \n")]
  UNKNOWN
}
