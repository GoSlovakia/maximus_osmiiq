using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwipesJourney : MonoBehaviour
{
  private float startPos;
  public JourneyPanelControl jpc;
  public JourneyButtons jb;
  public SetupJourney sj;
  public GetJourneyLevels gjl;
  private bool isLeft = true, stopSwipe = false;
  public GraphicRaycaster raycaster;
  public PointerEventData pointerEventData;
  public GameObject glowJourney, glowMissions;
  public EventSystem eventSystem;

  public void ChangeBool()
  {
    isLeft = !isLeft;
  }

  public void Swipe(InputAction.CallbackContext callbackContext)
  {
    Vector3 mousePos = Mouse.current.position.ReadValue();
    if (callbackContext.started)
    {
      startPos = Camera.main.ScreenToWorldPoint(mousePos).x;


      List<RaycastResult> results = new();
      pointerEventData = new(eventSystem){position = Mouse.current.position.ReadValue()};
      raycaster.Raycast(pointerEventData, results);
      foreach (var item in results)
      {
        if (item.gameObject.name == "Viewport")
          stopSwipe = true;
      }
    }


    if (callbackContext.canceled)
    {
      if (!stopSwipe)
      {
        if (isLeft && startPos > Camera.main.ScreenToWorldPoint(mousePos).x)
        {
          isLeft = false;
          jpc.SlideJorney();
          jb.SwitchPanelJourney();
          sj.RefreshJourney();
          gjl.JourneySlider();
          glowJourney.SetActive(true);
          glowMissions.SetActive(false);
        }
        else if (!isLeft && startPos < Camera.main.ScreenToWorldPoint(mousePos).x)
        {
          isLeft = true;
          jpc.SlideAchievements();
          jb.SwitchPanelAchievements();
          glowJourney.SetActive(false);
          glowMissions.SetActive(true);
        }
      }
      stopSwipe = false;
    }
  }
}
