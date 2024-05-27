using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JourneyButtons : MonoBehaviour
{
  public Button achiev, journey;
  private ColorBlock cb, cbWhite;
  void Start()
  {
    cb = new ColorBlock();
    cb.normalColor = new Color(228, 78, 141);
    cb.highlightedColor = new Color(228, 78, 141);
    cb.pressedColor = new Color(228, 78, 141);
    cb.selectedColor = new Color(228, 78, 141);
    cb.colorMultiplier = 1;

    cbWhite = new ColorBlock();
    cbWhite.normalColor = Color.white;
    cbWhite.highlightedColor = Color.white;
    cbWhite.pressedColor = Color.white;
    cbWhite.selectedColor = Color.white;
    cbWhite.colorMultiplier = 1;

    SwitchPanelAchievements();
  }


  public void SwitchPanelJourney()
  {
    achiev.colors = cb;
    journey.colors = cbWhite;
  }

  public void SwitchPanelAchievements()
  {
    achiev.colors = cbWhite;
    journey.colors = cb;
  }
}
