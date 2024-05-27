using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpriteColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{

  [SerializeField]
  private Image image;

  [SerializeField]
  private Color normal;

  [SerializeField]
  private Color pressed;

  private Button button;

  private void Start()
  {
    if (gameObject.GetComponent<Button>())
    {
      button = gameObject.GetComponent<Button>();
    }
  }

  public void OnPointerDown(PointerEventData eventData)
  {
      image.color = normal;
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (button != null && !button.interactable)
      image.color = normal;
    else
      image.color = pressed;
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    image.color = normal;
  }
}
