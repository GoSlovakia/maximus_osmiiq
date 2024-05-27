using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class MainMenuDisplays : MonoBehaviour
{
    public TMP_Text usernameTextbox;
    //public TMP_Text TitleTextbox;

    private void Start()
    {
        if (UserLogin.instance.LogInInfo != null)
            usernameTextbox.text = UserLogin.instance.LogInInfo.user.first_name;
        //TitleTextbox.text = GetTitles.UserTitlesArray.All.Where(x => x.TitleID == UserLogin.userinfo.userTitles.All[0].AdjID).Single().NameEN
        //    + " " + GetTitles.UserTitlesArray.All.Where(x => x.TitleID == UserLogin.userinfo.userTitles.All[0].NameID).Single().NameEN;
    }
}
