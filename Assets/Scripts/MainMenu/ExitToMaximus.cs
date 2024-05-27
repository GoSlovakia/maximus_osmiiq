using System.Threading.Tasks;
using UnityEngine;

public class ExitToMaximus : MonoBehaviour
{
  public Texture2D AvatarResetTex;
  public void ExitGame()
  {
    //Application.Quit();
    Application.OpenURL(GetDirectories.Instance.directories[DirectoryKey.MAXIMUS_URL.ToString()]);
  }

  public async void Logout()
  {
    UserLogin.instance.Logout();

    while (UserLogin.instance.loggingOut)
    {
      await Task.Yield();
    }
  }

  public async void ResetUser()
  {

    await UserLogin.instance.UpdateUserInfo(AvatarResetTex.EncodeToPNG(), false);
    await UserLogin.instance.ResetUser();
  }
}
