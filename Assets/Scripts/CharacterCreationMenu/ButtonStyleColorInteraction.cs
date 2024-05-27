using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonStyleColorInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
  public Button primary, secondary;
  public Image checkMark;
  public Image outline;
  public ButtonStyleSelect buttonStyle;
  public Color darkBlue;

  void Start()
  {
    buttonStyle = GetComponentInParent<ButtonStyleSelect>();
    buttonStyle.interactions.Add(gameObject.GetComponent<ButtonStyleColorInteraction>());


    primary = GameObject.Find("Primary").GetComponent<Button>();
    secondary = GameObject.Find("Secondary").GetComponent<Button>();

    outline.enabled = true;
    darkBlue = outline.color;
    checkMark.enabled = false;
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (outline != null)
    {
      outline.color = Color.white;
    }
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (outline != null)
    {
      if (checkMark.enabled)
        outline.color = Color.white;
      else
        outline.color = darkBlue;
    }
  }

  public void OnPointerClick(PointerEventData eventData)
  {
    checkMark.enabled = true;
    outline.color = Color.white;


    buttonStyle.Deselect(GetComponent<ButtonStyleColorInteraction>(), darkBlue);

    if (!primary.interactable)
      primary.transform.GetChild(0).GetComponent<Image>().color = GetComponent<ColorContainer>().Cor;
    else if (!secondary.interactable)
      secondary.transform.GetChild(0).GetComponent<Image>().color = GetComponent<ColorContainer>().Cor;
  }
}
