using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserLevelDisplay : MonoBehaviour
{
  public static UserLevelDisplay instance;
  public TextMeshProUGUI LevelTMP;
  public Slider Slider;
  public TextMeshProUGUI xpNumbers;

  private void Awake()
  {

    instance = this;


  }

  public void Start()
  {
    UpdateDisplay();
  }

  public void UpdateDisplay()
  {
    if (UserLevelComponent.UserLevels != null)
    {
      if (UserLevelComponent.UserLevels.All.Count() != 0)
      {
        LevelTMP.text = "Lv. " + UserLevelComponent.UserLevels.All[0].UserLevel.ToString();


        if (UserLevelComponent.UserLevels.All[0].UserLevel + 1 <= UserLevelComponent.LevelCaps.All.Length)
        {
          if (UserLevelComponent.UserLevels.All[0].UserLevel == 0)
            Slider.minValue = 0;
          else
            Slider.minValue = UserLevelComponent.LevelCaps.All[UserLevelComponent.UserLevels.All[0].UserLevel - 1].Total;

          Slider.maxValue = UserLevelComponent.LevelCaps.All[UserLevelComponent.UserLevels.All[0].UserLevel].Total;
        }
        else
          Slider.maxValue = 1;

        float currentXP = UserLevelComponent.UserLevels.All[0].XP;
        //float currentXP = UserLevelComponent.LevelCaps.All[UserLevelComponent.UserLevels.All[0].UserLevel - 1].Total + UserLevelComponent.UserLevels.All[0].XP;
        /*for (int i = 0; i < UserLevelComponent.UserLevels.All[0].UserLevel - 1; i++)
        {
            temp -= UserLevelComponent.LevelCaps.All[i].XPProgress;
        }*/
        Slider.value = currentXP;
        Debug.Log("CURRENT XP " + currentXP);
        if (xpNumbers != null)
          if (UserLevelComponent.UserLevels.All[0].UserLevel != 0)
            xpNumbers.text = currentXP.ToString() + "/" + UserLevelComponent.LevelCaps.All[UserLevelComponent.UserLevels.All[0].UserLevel].Total.ToString();
          else
            xpNumbers.text = currentXP.ToString() + "/" + UserLevelComponent.LevelCaps.All[UserLevelComponent.UserLevels.All[0].UserLevel].Total.ToString();
      }
      else
      {
        LevelTMP.text = "Lv. 0";
        Slider.maxValue = UserLevelComponent.LevelCaps.All[0].XPProgress;

        float temp = 0;
        Slider.value = temp;
        if (xpNumbers != null)
          xpNumbers.text = temp.ToString() + "/" + UserLevelComponent.LevelCaps.All[0].XPProgress.ToString();
      }
      //Slider.value = (float)UserLevelComponent.UserLevels.All[0].XP / (float)UserLevelComponent.LevelCaps.All[UserLevelComponent.UserLevels.All[0].UserLevel - 1].XPProgress;
      // Debug.Log("Percentage " + ((float)UserLevelComponent.UserLevels.All[0].XP / (float)UserLevelComponent.LevelCaps.All[UserLevelComponent.UserLevels.All[0].UserLevel - 1].XPProgress) + " " + UserLevelComponent.LevelCaps.All[UserLevelComponent.UserLevels.All[0].UserLevel - 1].XPProgress + " " + (float)UserLevelComponent.UserLevels.All[0].XP);


    }
  }
}
