using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class LoadAvatarPNG : MonoBehaviour
{

  public static CancellationTokenSource cancel = new CancellationTokenSource();
  // Start is called before the first frame update
  void Start()
  {
    if (UserLogin.instance.AvatarImage != null)
      GetComponent<RawImage>().texture = UserLogin.instance.AvatarImage;
  }

  //public async void UpdateImage()
  //{
  //    if (UserLogin.userinfo.AvatarImage != null)
  //    {
  //       // await UserLogin.userinfo.UpdateUserImage();

  //    }
  //}


}
