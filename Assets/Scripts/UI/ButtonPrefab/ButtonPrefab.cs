using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPrefab : MonoBehaviour
{
    [SerializeField] private Image outLine;
    [SerializeField] private bool outLineActive;

    [SerializeField] private Image icon;
    [SerializeField] private Sprite iconImage;
    [SerializeField] private RectTransform iconPos;

    [SerializeField] private Image fundo;

    [SerializeField] private Color corFundo;
    
    //Mudar texto no objeto texto
    //[SerializeField] private TextMeshProUGUI texto;

    void Start()
    {
        fundo.color = corFundo;
        icon.sprite = iconImage;
        //teste de mudança de anchors, muda no objeto icone
        //iconPos.anchorMin = new Vector2(0, 0.5f);
        //iconPos.anchorMax = new Vector2(0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!outLineActive)
        {
            outLine.enabled = false;
        }
        else
        {
            outLine.enabled = true;
        }
    }
}
