using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCategories : MonoBehaviour
{
  public List<SpriteChanger> spritesChangers = new List<SpriteChanger>();

  //public void DeselectCategories(SpriteChanger clicked)
  //{
  //  foreach (SpriteChanger spriteChanger in spritesChangers)
  //  {
  //    if(spriteChanger != clicked)
  //    {
  //      spriteChanger.image.sprite = spriteChanger.normal;
  //      spriteChanger.isPressed = false;

  //      ColorBlock colorBlock = spriteChanger.button.colors;
  //      //colorBlock.normalColor = spriteChanger.normalColor;
  //      //colorBlock.selectedColor = spriteChanger.normalColor;
  //      spriteChanger.button.colors = colorBlock;
  //    }
  //  }
  //}
}
