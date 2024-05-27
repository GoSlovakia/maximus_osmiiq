using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogInfailComponent : MonoBehaviour
{
    public static LogInfailComponent instance;
    public TextMeshProUGUI Description;
    // Start is called before the first frame update
    void Start()
    {
       
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;

        gameObject.SetActive(false);
    }

    public void ShowError(string fail)
    {
        Description.text = fail;
        gameObject.SetActive(true);
    }
}
