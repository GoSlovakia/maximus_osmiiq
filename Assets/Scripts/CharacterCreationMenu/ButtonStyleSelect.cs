using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStyleSelect : MonoBehaviour
{
  public List<ButtonStyleColorInteraction> interactions = new List<ButtonStyleColorInteraction>();
  public void Deselect(ButtonStyleColorInteraction buttonStyle, Color darkBlue)
  {
    foreach (ButtonStyleColorInteraction buttonStyleColor in interactions)
    {
      if(buttonStyleColor != buttonStyle)
      {
        buttonStyleColor.checkMark.enabled = false;
        buttonStyleColor.outline.color = darkBlue;
      }
    }
  }

  public void ResetPrimarySelect()
  {
    foreach (ButtonStyleColorInteraction buttonStyleColor in interactions)
    {
      buttonStyleColor.checkMark.enabled = false;
      buttonStyleColor.outline.color = interactions[0].darkBlue;

      if(buttonStyleColor.GetComponent<Button>().colors.normalColor == interactions[0].primary.transform.GetChild(0).GetComponent<Image>().color)
      {
        buttonStyleColor.checkMark.enabled = true;
        buttonStyleColor.outline.color = Color.white;
      }
      
    }
  }

  public void ResetSecundarySelect()
  {
    foreach (ButtonStyleColorInteraction buttonStyleColor in interactions)
    {
      buttonStyleColor.checkMark.enabled = false;
      buttonStyleColor.outline.color = interactions[0].darkBlue;

      if (buttonStyleColor.GetComponent<Button>().colors.normalColor == interactions[0].secondary.transform.GetChild(0).GetComponent<Image>().color)
      {
        buttonStyleColor.checkMark.enabled = true;
        buttonStyleColor.outline.color = Color.white;
      }

    }
  }
}
