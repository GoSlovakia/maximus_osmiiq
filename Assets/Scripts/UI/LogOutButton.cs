using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogOutButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void LogOut()
    {
        UserLogin.instance.Logout();
    }
}
