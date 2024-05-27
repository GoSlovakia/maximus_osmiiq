using cakeslice;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Toggle = UnityEngine.UI.Toggle;

public class changeinfo : MonoBehaviour
{
    public TMP_InputField UsernameTB;
    public TMP_InputField PasswordTB;
    public Toggle UsernameTg;
    public Toggle PasswordTg;

    private void Awake()
    {
        //Debug.Log("Stored info " + PlayerPrefs.GetString("User") + " " + PlayerPrefs.GetString("Password") + " " + (PlayerPrefs.GetString("User") != "") + " " + (PlayerPrefs.GetString("Password") != ""));

        if (PlayerPrefs.GetString("User") != "")
        {
            username = PlayerPrefs.GetString("User");
            UsernameTg.isOn = true;
        }
        if (PlayerPrefs.GetString("Password") != "")
        {
            password = PlayerPrefs.GetString("Password");
            PasswordTg.isOn = true;
        }



        //Debug.Log("Stored info " + PlayerPrefs.GetString("User") + " " + PlayerPrefs.GetString("Password") + " " + (PlayerPrefs.GetString("User") != "") + " " + (PlayerPrefs.GetString("Password") != ""));
        UsernameTB.text = UsernameTg.isOn ? PlayerPrefs.GetString("User") : null;
        PasswordTB.text = PasswordTg.isOn ? PlayerPrefs.GetString("Password") : null;
        AutoLogIn();
    }

    public string username
    {
        get { return UserLogin.instance.username; }
        set
        {
            UserLogin.instance.username = value;
        }

    }
    public string password
    {
        get { return UserLogin.instance.password; }
        set
        {
            // Debug.Log("Setting apssword to " + value);
            UserLogin.instance.password = value;
        }
    }

    public bool RememberPassword
    {
        get { return UserLogin.instance.rememberPassword; }
        set
        {

            UserLogin.instance.rememberPassword = value;
            UserLogin.instance.rememberUsername = value;
            if (value)
            {

                PlayerPrefs.SetString("User", username);
                if (password != "")
                    PlayerPrefs.SetString("Password", password);
            }
            else
            {
                PlayerPrefs.DeleteKey("User");
                PlayerPrefs.DeleteKey("Password");
            }
        }

    }

    public bool RememberUsername
    {
        get { return UserLogin.instance.rememberUsername; }
        set
        {
            UserLogin.instance.rememberUsername = value;
            PlayerPrefs.SetString("User", value ? username : null);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }

    public void LogIn()
    {
        UserLogin.instance.LogIn();
    }

    public void AutoLogIn()
    {
        if (!PlayerPrefs.HasKey("User") && !PlayerPrefs.HasKey("Password"))
        {
            // Debug.Log("No keys found!");
            return;
        }
        else
        {
            if (PlayerPrefs.GetString("User") == "" || PlayerPrefs.GetString("Password") == "")
            {
                // Debug.Log("inconsistent data " + PlayerPrefs.GetString("User") + " " + PlayerPrefs.GetString("Password"));
                PlayerPrefs.DeleteKey("User");
                PlayerPrefs.DeleteKey("Password");
                PlayerPrefs.Save();
                return;
            }

            // Debug.Log("Username " + PlayerPrefs.GetString("User") + " Password" + PlayerPrefs.GetString("Password"));

            UserLogin.instance.rememberUsername = true;
            UserLogin.instance.rememberPassword = true;
            //username = PlayerPrefs.GetString("User");
            //password = PlayerPrefs.GetString("Password");
        }

        if (AssetBundleCacher.Instance.NoCache)
            UserLogin.instance.LogIn();
    }
}
