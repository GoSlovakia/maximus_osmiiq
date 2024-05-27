using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetStashManager : MonoBehaviour
{
  public void ChangeColorPink(Image img)
  {
      img.color = new Color(228.0f/255.0f, 78.0f/255.0f,140.0f/255.0f);
  }

  public void ChangeColorWhite(Image img)
  {
    img.color = Color.white;
  }
}
