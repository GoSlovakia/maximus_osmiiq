using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SpriteChanger : MonoBehaviour
{
    public Toggle self;

    [SerializeField]
    private Image BG;
    [SerializeField]
    private Color BgSelected;
    [SerializeField]
    private Color BgDefault;

    private bool _setInteractable;

    public bool SetInteractable
    {
        get
        {
            return _setInteractable;
        }
        set
        {
            _setInteractable = value;
            self.interactable = !value;
            BG.color = value ? BgSelected : BgDefault;
        }
    }

    //void Start()
    //{
    //  button = GetComponent<Button>();
    //  normalColor = button.colors.disabledColor;
    //  selected = button.colors.selectedColor;
    //  image.sprite = normal;
    //}

    //void Start()
    //{
    //  button = GetComponent<Button>();
    //  normalColor = button.colors.disabledColor;
    //  selected = button.colors.selectedColor;
    //  image.sprite = normal;
    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //  image.sprite = hover;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //  if (isPressed)
    //    image.sprite = pressed;
    //  else
    //    image.sprite = normal;
    //}

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //  if (isPressed)
    //  {
    //    isPressed = false;
    //    ColorBlock colorBlock = button.colors;
    //    colorBlock.normalColor = normalColor;
    //    colorBlock.selectedColor = normalColor;
    //    button.colors = colorBlock;
    //  }
    //  else
    //  {
    //    isPressed = true;
    //    ColorBlock colorBlock = button.colors;
    //    colorBlock.normalColor = selected;
    //    colorBlock.selectedColor = selected;
    //    button.colors = colorBlock;
    //  }


    //  image.sprite = pressed;
    //}
}
