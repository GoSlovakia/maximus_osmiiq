using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimsScrollView : MonoBehaviour
{
  public Animator set, cards;

  public void ShowSet()
  {
    set.SetBool("ShowSets", true);
    set.SetBool("HideSets", false);
    cards.SetBool("ShowCards", false);
    cards.SetBool("HideCards", true);
  }

  public void ShowCards()
  {
    set.SetBool("ShowSets", false);
    set.SetBool("HideSets", true);
    cards.SetBool("ShowCards", true);
    cards.SetBool("HideCards", false);
  }
}
