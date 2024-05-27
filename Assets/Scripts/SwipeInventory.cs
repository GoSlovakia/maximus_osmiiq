using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwipeInventory : MonoBehaviour
{
  private float startPos;
  public GameObject hideCompleted,sortbycollection, sortInventory, hideUnique;
  public SetStashManager sm;
  public Image colecao,todas;
  public CreateSets cs;
  public CreateCards cc;
  public AnimsScrollView asv;
  private bool isLeft = true, stopSwipe = false;
  public GraphicRaycaster raycaster;
  public PointerEventData pointerEventData;
  public GameObject glowSet, glowInv;
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
      pointerEventData = new(eventSystem) { position = Mouse.current.position.ReadValue() };
      raycaster.Raycast(pointerEventData, results);
      foreach (var item in results)
      {
        if (item.gameObject.name == "ViewPortSets" || item.gameObject.name == "ViewPortInventario")
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
          sortbycollection.SetActive(false);
          hideCompleted.SetActive(false);
          sortInventory.SetActive(true);
          hideUnique.SetActive(true);
          cc.RefreshCards();
          sm.ChangeColorPink(todas);
          sm.ChangeColorWhite(colecao);
          asv.ShowCards();
          glowInv.SetActive(true);
          glowSet.SetActive(false);
          
        }
        else if (!isLeft && startPos < Camera.main.ScreenToWorldPoint(mousePos).x)
        {
          isLeft = true;
          sortbycollection.SetActive(true);
          hideCompleted.SetActive(true);
          sortInventory.SetActive(false);
          hideUnique.SetActive(false);
          cs.RefreshCopies();
          sm.ChangeColorPink(colecao);
          sm.ChangeColorWhite(todas);
          asv.ShowSet();
          glowInv.SetActive(false);
          glowSet.SetActive(true);
        }
      }
      stopSwipe = false;
    }
  }
}
