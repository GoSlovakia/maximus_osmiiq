using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupJourney : MonoBehaviour
{

  private readonly int bannersSpacing = 115;
  [SerializeField]
  private RectTransform scrollParent, sliderParent, lvlIcon;

  [SerializeField]
  private BannerInfo rewardBanner;

  [SerializeField]
  private Sprite specialLvl, borderSpecialLvl;
  private Levels currentLevel;

  [SerializeField]
  private JourneyRewardsPanel rewardsPanel;

  public ScrollRectUtilities scrollRectUtilities;

  public List<string> ClaimedSets = new();
  public static List<RectTransform> lvlsIcons = new();

  public List<BannerInfo> banners = new();
  public List<Slider> sliders = new();
  public List<RectTransform> lvlsIco = new();

  private void Start()
  {
    lvlsIcons.Clear();
  }

  public void CurrentJourneyLevel()
  {
    if (UserLevelComponent.UserLevels.All.Length > 0)
    {
      for (int i = 0; i < GetJourneyLevels.journeyLevels.Levels.Length; i++)
      {

        if (UserLevelComponent.UserLevels.All[0].XP == GetJourneyLevels.journeyLevels.Levels[i].Total)
        {
          currentLevel = GetJourneyLevels.journeyLevels.Levels[i];
          return;
        }
        else if (UserLevelComponent.UserLevels.All[0].XP < GetJourneyLevels.journeyLevels.Levels[i].Total)
        {
          currentLevel = GetJourneyLevels.journeyLevels.Levels[i];
          return;
        }
        else if (UserLevelComponent.UserLevels.All[0].XP > GetJourneyLevels.journeyLevels.Levels[19].Total)
        {
          currentLevel = GetJourneyLevels.journeyLevels.Levels[19];
        }
      }
    }
  }


  public void SetupRewards()
  {
    GetLevelsClaimed();
    SetLvlIcons();
    XPProgressionData temp = GetJourneyLevels.journeyLevels;
    int bannerSpacingCount = 0;
    for (int i = 0; i < temp.Levels.Length; i++)
    {
      BannerInfo currentBanner = Instantiate(rewardBanner, scrollParent);
      bannerSpacingCount += bannersSpacing;
      SetupBanner(temp.Levels[i], currentBanner);
      banners.Add(currentBanner);
    }
    scrollParent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bannerSpacingCount);
    SetupSliders(temp);
  }

  private void SetLvlIcons()
  {
    for (int i = 0; i < GetJourneyLevels.journeyLevels.Levels.Length; i++)
    {
      RectTransform temp = Instantiate(lvlIcon, scrollParent);
      temp.name = "icon" + i;
      lvlsIcons.Add(temp);
      temp.SetAsLastSibling();
      temp.anchoredPosition = new Vector3(1 * (111 + (111 * i + (i * 0.2f))), -8-50, 0);
      temp.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
    }
  }

  private void SetLvlIconsBorders(RectTransform rect, int lvl, bool interactable)
  {
    if (lvl % 5 == 0)
    {
      Image temp = rect.GetComponent<Image>();
      temp.sprite = specialLvl;
      temp.color = new Color(29.0f / 255.0f, 9.0f / 255.0f, 37.0f / 255.0f);
      rect.GetChild(1).GetComponent<Image>().sprite = borderSpecialLvl;
      rect.GetChild(1).GetComponent<Image>().color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
      rect.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
      rect.GetChild(1).gameObject.SetActive(true);
    }
    if (interactable)
    {
      rect.GetComponent<Image>().color = new Color(228.0f / 255.0f, 78.0f / 255.0f, 141.0f / 255.0f);
      rect.GetChild(1).gameObject.SetActive(true);
      rect.GetChild(1).GetComponent<Image>().color = Color.white;
      rect.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
    }
    else if (UserLevelComponent.UserLevels.All.Length != 0 && UserLevelComponent.UserLevels.All[0].UserLevel - 1 < lvl)
    {
      if (lvl % 5 == 0)
      {
        rect.GetComponent<Image>().color = new Color(29.0f / 255.0f, 9.0f / 255.0f, 37.0f / 255.0f);
        rect.GetChild(1).GetComponent<Image>().color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
      }
      else
      {
        rect.GetComponent<Image>().color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
      }
      rect.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
    }
    else
    {
      if (lvl % 5 == 0)
      {
        rect.GetComponent<Image>().color = new Color(29.0f / 255.0f, 9.0f / 255.0f, 37.0f / 255.0f);
        rect.GetChild(1).GetComponent<Image>().color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
      }
      else
      {
        rect.GetComponent<Image>().color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
      }
      rect.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
    }
  }

  private void SetupSliders(XPProgressionData xPProgression)
  {
    if (UserLevelComponent.UserLevels.All != null)
    {
      //primeiro slider
      RectTransform temp = Instantiate(sliderParent, scrollParent);
      temp.anchoredPosition = new Vector3(temp.anchoredPosition.x, -8 - 45);
      Slider sliderComponent = temp.GetComponent<Slider>();
      temp.offsetMax = new Vector2(-2186, temp.offsetMax.y);

      float oldPosR = temp.offsetMax.x + 112;

      if (UserLevelComponent.UserLevels.All.Length != 0)
      {
        if (UserLevelComponent.UserLevels.All[0].UserLevel > 1)
        {
          sliderComponent = temp.GetComponent<Slider>();
          sliderComponent.value = 1;
        }
        else
        {
          sliderComponent = temp.GetComponent<Slider>();
          sliderComponent.maxValue = xPProgression.Levels[0].Total;
          sliderComponent.value = UserLevelComponent.UserLevels.All[0].XP;
        }
      }
      else
      {
        sliderComponent = temp.GetComponent<Slider>();
        sliderComponent.maxValue = xPProgression.Levels[0].Total;
        sliderComponent.value = 0;
      }
      sliders.Add(sliderComponent);

      //segundo slider
      temp = Instantiate(sliderParent, scrollParent);
      temp.anchoredPosition = new Vector3(temp.anchoredPosition.x, -8 - 45);
      temp.offsetMin = new Vector2(130, temp.offsetMin.y);
      temp.offsetMax = new Vector2(oldPosR, temp.offsetMax.y);

      if (UserLevelComponent.UserLevels.All.Length != 0)
      {
        if (UserLevelComponent.UserLevels.All[0].UserLevel >= 2)
        {
          sliderComponent = temp.GetComponent<Slider>();
          sliderComponent.value = 1;
        }
        else
        {
          sliderComponent = temp.GetComponent<Slider>();
          sliderComponent.minValue = xPProgression.Levels[0].Total;
          sliderComponent.maxValue = xPProgression.Levels[1].Total;
          sliderComponent.value = UserLevelComponent.UserLevels.All[0].XP;
        }
      }
      else
      {
        sliderComponent = temp.GetComponent<Slider>();
        sliderComponent.minValue = xPProgression.Levels[0].Total;
        sliderComponent.maxValue = xPProgression.Levels[1].Total;
        sliderComponent.value = 0;
      }
      sliders.Add(sliderComponent);

      for (int i = 2; i < xPProgression.Levels.Length; i++)
      {
        oldPosR = temp.offsetMax.x + 112;
        float oldPosL = temp.offsetMin.x + 112;

        temp = Instantiate(sliderParent, scrollParent);
        temp.anchoredPosition = new Vector3(temp.anchoredPosition.x, -8 - 45);
        temp.offsetMin = new Vector2(oldPosL, temp.offsetMin.y);
        temp.offsetMax = new Vector2(oldPosR, temp.offsetMax.y);

        if (UserLevelComponent.UserLevels.All.Length != 0)
        {
          if (UserLevelComponent.UserLevels.All[0].UserLevel >= i + 1)
          {
            sliderComponent = temp.GetComponent<Slider>();
            sliderComponent.value = 1;
          }
          else
          {
            sliderComponent = temp.GetComponent<Slider>();
            sliderComponent.minValue = xPProgression.Levels[i - 1].Total;
            sliderComponent.maxValue = xPProgression.Levels[i].Total;
            if (UserLevelComponent.UserLevels.All[0].UserLevel == i)
              sliderComponent.value = UserLevelComponent.UserLevels.All[0].XP;
            else
              sliderComponent.value = 0;
          }
        }
        else
        {
          sliderComponent = temp.GetComponent<Slider>();
          sliderComponent.minValue = xPProgression.Levels[i - 1].Total;
          sliderComponent.maxValue = xPProgression.Levels[i].Total;
          sliderComponent.value = 0;
        }
        sliders.Add(sliderComponent);
      }
    }

    foreach (var item in lvlsIcons)
    {
      item.transform.SetAsLastSibling();
    }
  }

  private void SetupBanner(Levels levelData, BannerInfo level)
  {
    level.rewardsPanel = rewardsPanel;
    if (levelData.QIReward > 0)
      level.QI.text = levelData.QIReward.ToString();
    else
      level.qii.gameObject.SetActive(false);

    level.QUI.text = levelData.QUIReward.ToString();


    if (levelData.Level <= 9)
      level.setID = "LV-B00" + levelData.Level;
    else
      level.setID = "LV-B0" + levelData.Level;

    level.level = levelData.Level;
    //level.rewardImage.texture = await GetJPEG.GetJorneyThumbTexture(level.setID);
    level.rewardImage.texture = AssetBundleCacher.Instance.journeythumbsSprites[level.level - 1].texture;
    foreach (Coulourset colorSet in ColourManager.colourSets.couloursets)
    {
      if (colorSet.SetCode == level.setID)
      {
        level.colorIcon.gameObject.SetActive(true);
        level.AmountOfColors.text = "x1";
      }
    }

    int numberOfParts = 0;
    foreach (AvatarUnlocks avatarParts in GetJourneyLevels.journeyAvatarParts.All)
    {
      if (levelData.Level <= 9)
      {
        if (avatarParts.ID == levelData.SetReward)
          numberOfParts++;
      }
      else
        if (avatarParts.ID == levelData.SetReward)
        numberOfParts++;
    }
    level.AmountOfAvatarParts.text = "x" + numberOfParts.ToString();

    if (currentLevel != null)
    {
      if (levelData.Level < currentLevel.Level)
      {
        level.GetComponent<Button>().interactable = true;
        foreach (string claimedSet in ClaimedSets)
        {
          if (claimedSet == levelData.SetReward)
          {
            level.GetComponent<Button>().interactable = false;
            level.rewardPanel.color = new Color(level.rewardPanel.color.r, level.rewardPanel.color.g, level.rewardPanel.color.b, 0.8f);
            return;
          }
        }
      }
    }
    level.ChangeFontColor();
    SetLvlIconsBorders(lvlsIcons[levelData.Level - 1], levelData.Level, level.GetComponent<Button>().interactable);
  }

  private void GetLevelsClaimed()
  {
    ClaimedSets.Clear();
    foreach (UserSets set in UserSetsComponent.AllUserSets.All)
    {
      if (set.SetID.Contains("LV-B0"))
        ClaimedSets.Add(set.SetID);
    }
  }

  public void RefreshJourney()
  {
    GetLevelsClaimed();
    lvlsIco = lvlsIcons;
    if (UserLevelComponent.UserLevels.All.Length != 0)
    {
      for (int i = 0; i < banners.Count; i++)
      {
        if (!ClaimedSets.Contains(banners[i].setID) && UserLevelComponent.UserLevels.All[0].UserLevel >= banners[i].level)
        {
          banners[i].GetComponent<Button>().interactable = true;
          banners[i].ChangeFontColor();
        }
      }

      for (int i = 0; i < lvlsIco.Count; i++)
      {
        if (banners[i].level % 5 == 0)
        {
          Image temp = lvlsIco[i].GetComponent<Image>();
          temp.sprite = specialLvl;
          temp.color = Color.white;
          lvlsIco[i].GetChild(1).GetComponent<Image>().sprite = borderSpecialLvl;
          lvlsIco[i].GetChild(1).GetComponent<Image>().color = Color.white;
          lvlsIco[i].GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
          lvlsIco[i].GetChild(1).gameObject.SetActive(true);
        }
        if (banners[i].GetComponent<Button>().interactable)
        {
          lvlsIco[i].GetComponent<Image>().color = new Color(228.0f / 255.0f, 78.0f / 255.0f, 141.0f / 255.0f);
          lvlsIco[i].GetChild(1).gameObject.SetActive(true);
          lvlsIco[i].GetChild(1).GetComponent<Image>().color = Color.white;
          lvlsIco[i].GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        else if (UserLevelComponent.UserLevels.All.Length != 0 && UserLevelComponent.UserLevels.All[0].UserLevel < banners[i].level)
        {
          if (banners[i].level % 5 == 0)
          {
            lvlsIco[i].GetComponent<Image>().color = new Color(29.0f / 255.0f, 9.0f / 255.0f, 37.0f / 255.0f);
            lvlsIco[i].GetChild(1).GetComponent<Image>().color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
          }
          else
          {
            lvlsIco[i].GetComponent<Image>().color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
          }
          lvlsIco[i].GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
        }
      }

      for (int i = 0; i < sliders.Count; i++)
      {
        if (UserLevelComponent.UserLevels.All[0].UserLevel - 1 >= i)
        {
          sliders[i].minValue = 0;
          sliders[i].maxValue = 1;
          sliders[i].value = 1;
        }
        else
        {
          if (i == 0)
          {
            sliders[i].minValue = 0;
            sliders[i].maxValue = GetJourneyLevels.journeyLevels.Levels[i].Total;
            sliders[i].value = UserLevelComponent.UserLevels.All[0].XP;
          }
          else
          {
            sliders[i].minValue = GetJourneyLevels.journeyLevels.Levels[i - 1].Total;
            sliders[i].maxValue = GetJourneyLevels.journeyLevels.Levels[i].Total;
            sliders[i].value = UserLevelComponent.UserLevels.All[0].XP;
          }
        }
      }
    }
  }
}
