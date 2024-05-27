using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideForFreeQuids : MonoBehaviour
{
  public ScrollRectUtilities scrollRectUtilities;
  public Button button;
  void Start()
  {
    if (button.interactable)
      scrollRectUtilities.SlideToItem(scrollRectUtilities.target);
  }
}
