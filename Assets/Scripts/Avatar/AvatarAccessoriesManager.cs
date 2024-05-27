using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AvatarAccessoriesManager
{
    private static string avatarAccsJSON;
    private static string avatarAccsUnlocksJSON;
    public static AvatarAccessoryContainer AvatarAccs;
    public static AvatarAccUnlocksContainer AvatarAccsUnlocks;
    public static Dictionary<string, string> Downloaded = new Dictionary<string, string>();

    public static string AvatarAccsJSON
    {
        get => avatarAccsJSON; set
        {
            avatarAccsJSON = value;
            AvatarAccs = JsonUtility.FromJson<AvatarAccessoryContainer>(AvatarAccsJSON);

            //Debug.Log(AvatarAccs.All.Count() + " Accessories loaded");
            foreach (var este in AvatarAccs.All)
            {
                
                este.avatarset = AvatarGenerator.AvatarComponents.Parts.Where(x => este.id.Substring(3, 3).Contains(x.spritegroup.ToString())).First().spritegroup;
                //Debug.Log(este.avatarset + " " + este.id.Substring(3, 3));
            }
        }
    }
    public static bool IsPartUnlocked(string ID)
    {
        string setid = "Starter";
        if (AvatarAccsUnlocks.All.Where(x => x.PartCode == ID).Count() > 0)
        {
            setid = AvatarAccsUnlocks.All.Where(x => x.PartCode == ID).Single().ID;

        }
        return UserSetsComponent.AllUserSets.All.Where(x => x.SetID == setid).Count() > 0 || setid == "Starter";

    }

    public static string AvatarAccsUnlocksJSON
    {
        get => avatarAccsUnlocksJSON;
        set
        {

            avatarAccsUnlocksJSON = value;

            AvatarAccsUnlocks = JsonUtility.FromJson<AvatarAccUnlocksContainer>(avatarAccsUnlocksJSON);
        }
    }
}
