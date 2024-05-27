using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneyPanelControl : MonoBehaviour
{
  private Animator anim;

  private void Start()
  {
    anim = GetComponent<Animator>();
  }
  public void SlideJorney()
  {
    anim.SetBool("journey",true);
    anim.SetBool("achievements", false);
  }

  public void SlideAchievements()
  {
    anim.SetBool("journey", false);
    anim.SetBool("achievements", true);
  }
}
