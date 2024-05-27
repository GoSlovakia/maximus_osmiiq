using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullSVGSOnAwake : MonoBehaviour
{
  private void Awake()
  {
    Load_SVG_From_File.SetSVGS("","", "ST01", "ST02");
 
  }
}
