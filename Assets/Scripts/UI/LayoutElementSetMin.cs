using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class LayoutElementSetMin : MonoBehaviour
{
    public RectTransform Guide;

    public void AdjustSize()
    {

       // Debug.Log("Adjusting Size " + Guide.rect.width + " " + transform.name);
        if (Guide.rect.width > 0)
            GetComponent<LayoutElement>().minWidth = Guide.rect.width;

    }

    private void OnValidate()
    {
        if (!Application.IsPlaying(this))
        {
            //Debug.Log("Editor " + Guide.rect.width);
            AdjustSize();
        }
        else
        {
            //Debug.Log("No " + Guide.rect.width + " " + transform.name);

        }

    }
}
