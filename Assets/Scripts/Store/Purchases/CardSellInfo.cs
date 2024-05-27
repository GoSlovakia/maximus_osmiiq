using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class CardSellInfo : MonoBehaviour
{
  [HideInInspector]
  public CardRotation card;

  [SerializeField] private Image BG;
  [SerializeField] RawImage DomainImage;

  [SerializeField]
  private LocalizeStringEvent rarityLocale;
  [SerializeField] private LocalizedString[] raritiesLocale;

  [SerializeField]
  private TextMeshProUGUI title, price, rarity, time;

  [SerializeField] private RawImage TypeImage;

  [SerializeField]
  private GameObject OwnedOverlay;
  public RawImage image;


  [SerializeField]
  private Color CommonBG;
  [SerializeField]
  private Color RareBG;
  [SerializeField]
  private Color EpicBG;
  [SerializeField]
  private Color LegendaryBG;

  public void UpdateCards()
  {
    for (int i = 0; i < CardManager.AllCards.All.Length; i++)
    {
      if (CardManager.AllCards.All[i].id == card.id)
        card.title = CardManager.AllCards.All[i].title;
    }
    title.text = card.title;
    price.text = card.priceQUI.ToString();
    //image.texture = await GetJPEG.GetThumbTexture(card.id);
    if (card.id != "PC0012")
      image.texture = AssetBundleCacher.Instance.cards[card.id + "_thumb"].texture;
    else
      image.texture = AssetBundleCacher.Instance.cards[card.id + "_thum"].texture;

    switch (card.rarity)
    {
      case "C":
        //rarity.text = "Common Card";
        rarityLocale.StringReference = raritiesLocale[0];
        rarity.color = CommonBG;
        BG.color = CommonBG;
        break;
      case "R":
        //rarity.text = "Rare Card";
        rarityLocale.StringReference = raritiesLocale[1];
        rarity.color = RareBG;
        BG.color = RareBG;
        break;
      case "E":
        //rarity.text = "Epic Card";
        rarityLocale.StringReference = raritiesLocale[2];
        rarity.color = EpicBG;
        BG.color = EpicBG;
        break;
      case "L":
        //rarity.text = "Legendary Card";
        rarityLocale.StringReference = raritiesLocale[3];
        rarity.color = LegendaryBG;
        BG.color = LegendaryBG;
        break;
    }

    //Debug.Log("card type " + card.type.ToString() + " ");
    //TypeImage.texture = await GetJPEG.GetCardType(card.type.ToString());
    TypeImage.texture = AssetBundleCacher.Instance.cardstypes[card.type.ToString()].texture;
    TypeImage.GetComponent<LayoutElement>().minWidth = TypeImage.GetComponent<RectTransform>().rect.height;
    //Debug.Log("Domain " + CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == card.id).Single().SetID).Single().id.Substring(0, 2));
    //DomainImage.texture = await GetJPEG.GetDomainTexture(CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == card.id).Single().SetID).Single().id.Substring(0, 2));
    DomainImage.texture = AssetBundleCacher.Instance.cardsdomains[CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == card.id).Single().SetID).Single().id.Substring(0, 2)].texture;

    DomainImage.GetComponent<LayoutElement>().minWidth = DomainImage.GetComponent<RectTransform>().rect.height;


    Owned();
  }

  public void Owned()
  {

    OwnedOverlay.SetActive(CardManager.UserCards.Where(x => x.id == card.id).Count() > 0);
    GetComponent<Button>().interactable = CardManager.UserCards.Where(x => x.id == card.id).Count() == 0;
    ServerTimeCoundown.instance.ticking.Add(time);
  }
}