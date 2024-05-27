using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class CreateDropdownTitleOptions : MonoBehaviour
{
    public TMP_Dropdown AdjDropdown;
    public TMP_Dropdown NounDropdown;
    public SelectOption selectoptn;

    void Start()
    {
        GenerateOptions();
    }

    public void UpdateSelectedOption()
    {
        AdjDropdown.SetValueWithoutNotify(AdjDropdown.options.IndexOf(AdjDropdown.options.Where(x => x.text == GetTitles.UserTitlesArray.All.Where(x => x.TitleID == UserLogin.instance.userTitles.All[0].AdjID).Single().NameEN).Single()));
        NounDropdown.SetValueWithoutNotify(NounDropdown.options.IndexOf(NounDropdown.options.Where(x => x.text == GetTitles.UserTitlesArray.All.Where(x => x.TitleID == UserLogin.instance.userTitles.All[0].NameID).Single().NameEN).Single()));
    }

    public void SetDefaults()
    {
        if (selectoptn.NameID == null)
        {
            selectoptn.NameID = GetTitles.UserTitlesArray.All.Where(x => x.Type == "Name").First().TitleID;
        }
        if (selectoptn.AdjID == null)
        {
            selectoptn.AdjID = GetTitles.UserTitlesArray.All.Where(x => x.Type == "Adjective").First().TitleID;
        }

        Debug.Log("Set default titles? " + selectoptn.NoTitlesSet);
        if (selectoptn.NoTitlesSet)
        {

            selectoptn.SetOption();
        }
    }

    public void GenerateOptions()
    {
        List<TMP_Dropdown.OptionData> dropdownAdj = new List<TMP_Dropdown.OptionData>();
        List<TMP_Dropdown.OptionData> dropdownNoun = new List<TMP_Dropdown.OptionData>();


        foreach (var title in GetTitles.UserTitlesArray.All)
        {
            switch (title.Type)
            {
                case "Adjective":
                    dropdownAdj.Add(new TMP_Dropdown.OptionData(title.NameEN));
                    break;
                case "Name":
                    dropdownNoun.Add(new TMP_Dropdown.OptionData(title.NameEN));
                    break;
                default:
                    break;
            }
        }
        AdjDropdown.AddOptions(dropdownAdj);
        NounDropdown.AddOptions(dropdownNoun);

        UpdateSelectedOption();
        SetDefaults();
    }
}
