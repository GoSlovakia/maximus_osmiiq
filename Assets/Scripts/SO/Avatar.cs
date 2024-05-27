using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Avatar", menuName = "ScriptableObjects/Avatar", order = 2)]
public class Avatar : ScriptableObject
{
    public string[] AllAccFile;
    public string Primary;
    public string secondary;
    public Texture2D Pattern;

}
