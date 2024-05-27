using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnabletoLogin : MonoBehaviour
{
    public void UnableLogin()
    {
        Application.OpenURL(GetDirectories.Instance.directories[DirectoryKey.URL.ToString()] + "password");
    }
}
