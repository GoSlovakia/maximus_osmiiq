using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStyleSelection : MonoBehaviour
{
  public ButtonStyleSelect buttonStyle;

  private void Start()
  {
    buttonStyle = GameObject.Find("ColGroup(Clone)").GetComponent<ButtonStyleSelect>();
  }

  public void ResetPrimarySelect()
  {
    buttonStyle.ResetPrimarySelect();
  }

  public void ResetSecondarySelect()
  {
    buttonStyle.ResetSecundarySelect();
  }
}
