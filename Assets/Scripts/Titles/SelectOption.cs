using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SelectOption : MonoBehaviour
{
    public TMP_Dropdown dropdownAdj;
    public TMP_Dropdown dropdownName;
    public CreateDropdownTitleOptions titleOptions;
    public SetTitle titleSet;
    public ShowUser ShowUser;
    private string adjID;
    private string nounID;
    public bool NoTitlesSet = false;
    public bool setonserver = false;

    public string AdjID
    {
        get => adjID; set
        {
            adjID = value;
            if (setonserver)
                SetOption();
        }
    }
    public string NameID
    {
        get => nounID; set
        {
            nounID = value;
            if (setonserver) SetOption();
        }
    }

    private void Awake()
    {
        titleSet = new SetTitle();
        if (UserLogin.instance.userTitles.All != null)
        {
            AdjID = UserLogin.instance.userTitles.All[0].AdjID;
            NameID = UserLogin.instance.userTitles.All[0].NameID;
            setonserver = true;
        }
        else
        {
            Debug.Log("No titles");
            NoTitlesSet = true;
        }
    }

    public void SelectedAdj(int opt)
    {
        //Debug.Log(GetTitles.UserTitlesArray.All.Where(x => x.NameEN==dropdownName.options[opt].text).Single().NameEN);
        AdjID = GetTitles.UserTitlesArray.All.Where(x => x.NameEN == dropdownAdj.options[opt].text).Single().TitleID;

    }

    public void SelectedName(int opt)
    {
        //Debug.Log(GetTitles.UserTitlesArray.All.Where(x => x.NameEN==dropdownName.options[opt].text).Single().NameEN);
        NameID = GetTitles.UserTitlesArray.All.Where(x => x.NameEN == dropdownName.options[opt].text).Single().TitleID;
        SetOption();
    }

    public async void SetOption()
    {
        Debug.Log("Adj e Noun" + AdjID + " " + NameID);
        titleSet.SetRequest(AdjID, NameID);
        await titleSet.SetTitleOnServer();
        Debug.Log("Title Changed");
        UserLogin.instance.userTitles.All[0].AdjID = AdjID;
        UserLogin.instance.userTitles.All[0].NameID = NameID;
        ShowUser.UpdateTitle();
        titleOptions.UpdateSelectedOption();
    }
}
