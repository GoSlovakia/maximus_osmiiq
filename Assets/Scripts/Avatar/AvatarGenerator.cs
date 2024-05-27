using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

public static class AvatarGenerator
{
    private static string avatarComponentsJSON;
    public static AvatarComponent AvatarComponents;

    public static string AvatarComponentsJSON
    {
        get => avatarComponentsJSON; set
        {
            avatarComponentsJSON = value;

            AvatarComponents = JsonUtility.FromJson<AvatarComponent>(AvatarComponentsJSON);
            foreach (var este in AvatarComponents.Parts)
            {
                este.spritegroup = (AccType)System.Enum.Parse(typeof(AccType), este.code);
            }
            AvatarComponents.Parts.OrderBy(x => x.orderinlayer);

        }
    }
}
