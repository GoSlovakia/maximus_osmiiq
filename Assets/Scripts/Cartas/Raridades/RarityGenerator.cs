using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RarityGenerator
{
    public static string RaritiesJSON;
    public static RarityContainer Rarities;

    public static void GenerateRarities()
    {
        Rarities = JsonUtility.FromJson<RarityContainer>(RaritiesJSON);
    }
}
