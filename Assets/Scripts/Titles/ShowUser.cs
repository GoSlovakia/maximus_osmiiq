using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShowUser : MonoBehaviour
{
   // public TextMeshProUGUI title;
    public TextMeshProUGUI username;

    // Start is called before the first frame update
    private void Start()
    {
        UpdateTitle();
    }
    public void UpdateTitle()
    {
        username.text = UserLogin.instance.LogInInfo.user.first_name;
        //title.text = GetTitles.UserTitlesArray.All.Where(x => x.TitleID == UserLogin.userinfo.userTitles.All[0].AdjID).Single().NameEN
        //    + " " + GetTitles.UserTitlesArray.All.Where(x => x.TitleID == UserLogin.userinfo.userTitles.All[0].NameID).Single().NameEN;
    }
}
