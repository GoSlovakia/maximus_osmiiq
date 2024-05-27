using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JourneyRewardsPanel : MonoBehaviour
{
  public CurrencyBaner quiisBanner, qiisBanner,xpBanner;
  public JourneyRewardBanner journeyBanner;
  public RectTransform panelLayout;
  public Sprite part, colour;

  private List<GameObject> spawnedBanners = new List<GameObject>();
  public void SetupAvatarBanners(string code, string type, string name)
  {
    JourneyRewardBanner temp = Instantiate(journeyBanner, panelLayout);
    spawnedBanners.Add(temp.gameObject);
    temp.avatarType.text = type;
    temp.avatarPart.text = name;

    temp.icon.sprite = part;
    //temp.mainImage.texture = await GetJPEG.GetAvatarThumb(code);
    temp.mainImage.texture = AssetBundleCacher.Instance.avatarpartsthumbs[code + "_thumb"].texture;
    temp.mainImage.GetComponent<AspectRatioFitter>().aspectRatio = (float)temp.mainImage.texture.width / (float)temp.mainImage.texture.height;
    Debug.Log(temp.avatarType.text);
    if (temp.avatarType.text == "Left Hand" || temp.avatarType.text == "Right Hand")
      temp.mainImage.rectTransform.localScale *= 0.5f;
  }

  public void SetupColordBanners(string type, string name, Color colours)
  {
    JourneyRewardBanner temp = Instantiate(journeyBanner, panelLayout);
    spawnedBanners.Add(temp.gameObject);
    temp.avatarType.text = type;
    temp.avatarPart.text = name;


    temp.icon.sprite = colour;
    temp.mainImage.gameObject.SetActive(false);
    temp.colorImage.gameObject.SetActive(true);
    temp.colorImage.color = colours;
  }

  public void SetupQuiidsBanner(TextMeshProUGUI currency)
  {
    CurrencyBaner temp = Instantiate(quiisBanner, panelLayout);
    spawnedBanners.Add(temp.gameObject);

    temp.quiids.text = currency.text;
  }

  public void SetupQiisBanner(TextMeshProUGUI currency)
  {
    CurrencyBaner temp = Instantiate(qiisBanner, panelLayout);
    spawnedBanners.Add(temp.gameObject);

    temp.quiids.text = currency.text;
  }

  public void SetupXPBanner(TextMeshProUGUI xp)
  {
    CurrencyBaner temp = Instantiate(xpBanner,panelLayout);
    spawnedBanners.Add(temp.gameObject);

    temp.quiids.text = xp.text;
  }

  public void CleanReward()
  {
    foreach (var item in spawnedBanners)
    {
      Destroy(item);
    }
    spawnedBanners.Clear();
  }
}
