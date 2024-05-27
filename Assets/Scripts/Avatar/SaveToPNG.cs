using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SaveToPNG : MonoBehaviour
{
  public RenderTexture rt;
  //public static CancellationTokenSource cancel = new CancellationTokenSource();

  //public async void SaveToPng()
  //{
  //    await SavePNG();
  //}

  public async IAsyncEnumerator<WaitForEndOfFrame> SavePNG()
  {
    RenderTexture.active = rt;

    yield return new WaitForEndOfFrame();
    Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
    tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
    tex.Apply();
    string Job = "Saving an image of the avatar to the server ";
    LoadingManager.instance.EnqueueLoad(Job);
    //RenderTexture.active = null;

    //string info = System.DateTime.Now.Year.ToString("0000") + System.DateTime.Now.Month.ToString("00") +
    //    System.DateTime.Now.Day.ToString("00") +
    //    "-" + System.DateTime.Now.Hour.ToString("00") +
    //    System.DateTime.Now.Minute.ToString("00") +
    //    System.DateTime.Now.Second.ToString("00") +
    //    "_" + System.Environment.UserName;
    string info = Path.GetRandomFileName();

    tex.name = info;
    byte[] bytes;

    //bytes = ImageConversion.EncodeArrayToPNG(tex.GetRawTextureData(), tex.graphicsFormat, 4096, 4096);
    bytes = tex.EncodeToPNG();

    if (tex.GetPixel(tex.width / 2, tex.height / 2) == new Color(0, 0, 0, 0))
    {
      Debug.LogError("texture empty");
      await Task.Delay(1000);
      IAsyncEnumerator<WaitForEndOfFrame> e = SavePNG();
      try
      {
        while (await e.MoveNextAsync()) ;
      }
      finally { if (e != null) await e.DisposeAsync(); }
    }
    else
    {


      string path = Application.persistentDataPath + "/" + info + ".png";
      Task A = UserLogin.instance.UpdateUserInfo(bytes, true);
      Debug.Log("PNG SENT");
      await Task.WhenAll(A);
      System.IO.File.WriteAllBytes(path, bytes);
      UserLogin.instance.AvatarImage = tex;
      //Debug.Log("Saved to " + path);
    }
    LoadingManager.instance.DequeueLoad(Job);
  }



  //public async Task UploadImageToServer()
  //{
  //    WWWForm form = new WWWForm();
  //    RenderTexture.active = rt;
  //    Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
  //    tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);


  //    form.AddBinaryData("userImage" + UserLogin.instance.LogInInfo.user.id, tex.EncodeToPNG(), "image/png");

  //    UnityWebRequest www = UnityWebRequest.Post(LoadSVGs.IP + "uploadFile.php", form);
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
  //        Debug.Log(www.error);
  //    }
  //    else
  //    {
  //        Debug.Log("Image Uploaded");
  //        await UserLogin.userinfo.UpdateUserImage();
  //    }
  //}


}
