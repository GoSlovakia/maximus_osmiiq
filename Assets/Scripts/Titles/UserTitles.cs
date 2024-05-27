using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TitlesArray
{
    public Title[] All;
}
[System.Serializable]
public class Title
{
    public string TitleID;
    public string Set;
    public string NameEN;
    public string Type;
    public string ObtainMethod;

    public Title(Title title)
    {
        TitleID = title.TitleID;
        Set = title.Set;
        NameEN = title.NameEN;
        Type = title.Type;
        ObtainMethod = title.ObtainMethod;
    }
}
[System.Serializable]
public class UserTitleArray
{
    public UserTitle[] All;
}
[System.Serializable]
public class UserTitle
{
    public string AdjID;
    public string NameID;
}
