using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorCategory : MonoBehaviour
{

    public static ColorCategory Instance;
    public TextMeshProUGUI TopText;
    public TextMeshProUGUI Counter;

    public TextMeshProUGUI CurrentColor;

    public Image MainBG;
    public Image SecBG;

    public void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

    }


    private void OnDestroy()
    {
        Instance = null;
    }
}
