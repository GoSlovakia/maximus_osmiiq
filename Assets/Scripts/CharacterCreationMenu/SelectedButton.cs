using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedButton : MonoBehaviour
{
    public List<Image> outlines = new List<Image>();

    private void Awake()
    {
        outlines = new List<Image>();
    }
    public void ChangeOutline(Image outline, bool isPressed, Color normal, ColorBlock buttonColors)
    {
        foreach (Image outLine in outlines)
        {
            if (outLine != outline && outLine.GetComponentInParent<ButtonOutline>() != null)
            {
                outLine.gameObject.GetComponentInParent<ButtonOutline>().isPressed = false;

                ColorBlock colorBlock = buttonColors;
                colorBlock.normalColor = normal;
                colorBlock.selectedColor = normal;
                outLine.gameObject.GetComponentInParent<ButtonOutline>().button.colors = colorBlock;

                outLine.enabled = false;
            }
        }
    }
}
