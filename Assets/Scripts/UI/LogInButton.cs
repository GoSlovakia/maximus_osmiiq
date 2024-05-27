using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class LogInButton : MonoBehaviour
{
    [SerializeField] private Button loginbutton;
    [SerializeField] private Sprite bttnimagev1;
    [SerializeField] private Sprite bttnimagev2;
    [SerializeField] private UnityEngine.UI.Image bttnimage;
    [SerializeField] private TMP_InputField UsernameTB;
    [SerializeField] private TMP_InputField PasswordTB;

    // Update is called once per frame
    void Update()
    {
        if (UsernameTB.text != "" && PasswordTB.text != "")
        {
            bttnimage.sprite = bttnimagev2;
            loginbutton.interactable = true;
        }
        else
        {
            bttnimage.sprite = bttnimagev1;
            loginbutton.interactable = false;
        }
    }
}
