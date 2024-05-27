using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTiling : MonoBehaviour
{
  SpriteRenderer sr;
  float countHeight = 100;
  float countWidth = 100;
  void Start()
  {
    sr = gameObject.GetComponent<SpriteRenderer>();
    sr.drawMode = SpriteDrawMode.Tiled;

  }

  // Update is called once per frame
  void Update()
  {
    sr.size = new Vector2(IncrementWidth(), IncrementHeight());
  }

  float IncrementHeight()
  {
    if (countHeight > 30)
      return countHeight -= 0.85f * Time.deltaTime;
    else
      return countHeight = 100;
  }

  float IncrementWidth()
  {
    if (countWidth > 30)
      return countWidth += 0.5f * Time.deltaTime;
    else
      return countWidth = 100;
  }
}
