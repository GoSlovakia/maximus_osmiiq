using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class ViewCards : MonoBehaviour
{
  [SerializeField]
  private Image setNamePanel;
  [SerializeField]
  private TextMeshProUGUI duplicate, cardType, domain, set, Rarity, cardName, singleWord;

  public SetActiveSideCardsComponent SideCards;

  [SerializeField]
  private RawImage cardImage;

  [SerializeField]
  private Image RarityImage;

  [SerializeField]
  private RawImage TypeImage;

  [SerializeField]
  private RawImage DomainImage;

  public BoosterCardsData boosterCards;

  [SerializeField]
  private Sprite CommonRarity;
  [SerializeField]
  private Sprite RareRarity;
  [SerializeField]
  private Sprite EpicRarity;
  [SerializeField]
  private Sprite LegendaryRarity;

  [SerializeField]
  private Image BG;

  [SerializeField]
  private Color CommonBG;
  [SerializeField]
  private Color RareBG;
  [SerializeField]
  private Color EpicBG;
  [SerializeField]
  private Color LegendaryBG;

  [SerializeField]
  private PackSumary sumary;

  [SerializeField]
  private LocalizeStringEvent duplicateLocale;
  [SerializeField]
  private LocalizedString[] duplicatesLocale;

  [SerializeField]
  private LocalizeStringEvent rarityLocale;
  [SerializeField]
  private LocalizedString[] raritesLocale;

  [SerializeField]
  private LocalizeStringEvent titleLocale;
  [SerializeField]
  private LocalizedString[] titlesLocale;

  [SerializeField]
  private LocalizeStringEvent domainLocale;
  [SerializeField]
  private LocalizedString[] domainsLocale;

  [SerializeField]
  private LocalizeStringEvent typeLocale;
  [SerializeField]
  private LocalizedString[] typesLocale;

  public List<Card> boosterCardList = new List<Card>();

  int i = 0;
  public void UpdateCard(Texture texture)
  {
    cardImage.texture = texture;

    foreach (Card card in CardManager.UserCards)
    {
      if (card.id == boosterCards.boosterCards[i].id)
      {
        boosterCardList.Add(card);
        if (card.copies > 1)
        {
          //duplicate.text = "duplicate";
          duplicateLocale.StringReference = duplicatesLocale[0];
          duplicate.color = Color.white;
        }
        else
        {
          duplicate.color = new Color(255.0f / 255.0f, 204.0f / 255.0f, 49.0f / 255.0f);
          duplicateLocale.StringReference = duplicatesLocale[1];
          //duplicate.text = "new";
        }

      }
    }
    /*if (CardManager.UserCards.Where(x => x.id == boosterCards.boosterCards[i].id).Count() == 0)
    {

      duplicate.text = "new";
    }
    else
    {
      duplicate.text = "duplicate";
    }*/

    switch (boosterCards.boosterCards[i].rarity)
    {
      case "C":
        //Rarity.text = "Common";
        rarityLocale.StringReference = raritesLocale[0];
        RarityImage.sprite = CommonRarity;
        BG.color = CommonBG;
        setNamePanel.color = CommonBG;
        singleWord.color = CommonBG;
        break;
      case "R":
        //Rarity.text = "Rare";
        rarityLocale.StringReference = raritesLocale[1];
        RarityImage.sprite = RareRarity;
        BG.color = RareBG;
        setNamePanel.color = RareBG;
        singleWord.color = RareBG;
        break;
      case "E":
        //Rarity.text = "Epic";
        rarityLocale.StringReference = raritesLocale[2];
        RarityImage.sprite = EpicRarity;
        BG.color = EpicBG;
        setNamePanel.color = EpicBG;
        singleWord.color = EpicBG;
        break;
      case "L":
        //Rarity.text = "Legendary";
        rarityLocale.StringReference = raritesLocale[3];
        RarityImage.sprite = LegendaryRarity;
        BG.color = LegendaryBG;
        setNamePanel.color = LegendaryBG;
        singleWord.color = LegendaryBG;
        break;
    }

    for (int i = 0; i < CardManager.UserCards.Count; i++)
    {
      if (boosterCards.boosterCards[this.i].id == CardManager.UserCards[i].id)
      {
        cardName.text = CardManager.UserCards[i].title;
        singleWord.text = CardManager.UserCards[i].singleworddescription;

        foreach (CardSet set in CardManager.CardSets.All)
        {
          if (boosterCards.boosterCards[this.i].id.Remove(boosterCards.boosterCards[this.i].id.Length - 1) == set.id)
          {
            switch (set.id)
            {
              case "CA001":
                titleLocale.StringReference = titlesLocale[0];
                break;
              case "CA002":
                titleLocale.StringReference = titlesLocale[1];
                break;
              case "FR001":
                titleLocale.StringReference = titlesLocale[2];
                break;
              case "FR002":
                titleLocale.StringReference = titlesLocale[3];
                break;
              case "HG001":
                titleLocale.StringReference = titlesLocale[4];
                break;
              case "HG002":
                titleLocale.StringReference = titlesLocale[5];
                break;
              case "HG003":
                titleLocale.StringReference = titlesLocale[6];
                break;
              case "HG004":
                titleLocale.StringReference = titlesLocale[7];
                break;
              case "HG005":
                titleLocale.StringReference = titlesLocale[8];
                break;
              case "LL001":
                titleLocale.StringReference = titlesLocale[9];
                break;
              case "LL002":
                titleLocale.StringReference = titlesLocale[10];
                break;
              case "LL003":
                titleLocale.StringReference = titlesLocale[11];
                break;
              case "LL004":
                titleLocale.StringReference = titlesLocale[12];
                break;
              case "LL005":
                titleLocale.StringReference = titlesLocale[13];
                break;
              case "MC001":
                titleLocale.StringReference = titlesLocale[14];
                break;
              case "PC001":
                titleLocale.StringReference = titlesLocale[15];
                break;
              case "PC002":
                titleLocale.StringReference = titlesLocale[16];
                break;
              case "PP002":
                titleLocale.StringReference = titlesLocale[17];
                break;
            }
            UpdateDomain(domainLocale, domainsLocale, set.domain);
          }
          UpdateType(typeLocale, typesLocale, int.Parse(CardManager.UserCards[i].type));
        }
      }
    }

    //cardType.text = CardManager.CardTypes.All[boosterCards.boosterCards[i].type - 1].type;
    //set.text = CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == boosterCards.boosterCards[i].id).Single().SetID).Single().name;
    //cardName.text = boosterCards.boosterCards[i].title;
    //domain.text = CardManager.CardDomains.All.Where(x => x.id == CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == boosterCards.boosterCards[i].id).Single().SetID).Single().domain).Single().domain;
    //singleWord.text = boosterCards.boosterCards[i].singleworddescription;

    //TypeImage.texture = await GetJPEG.GetCardType(boosterCards.boosterCards[i].type.ToString());
    TypeImage.texture = AssetBundleCacher.Instance.cardstypes[boosterCards.boosterCards[i].type.ToString()].texture;
    TypeImage.GetComponent<LayoutElement>().minWidth = TypeImage.GetComponent<RectTransform>().rect.height;
    //Debug.Log("Domain " + CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == boosterCards.boosterCards[i].id).Single().SetID).Single().id.Substring(0, 2));
    //DomainImage.texture = await GetJPEG.GetDomainTexture(CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == boosterCards.boosterCards[i].id).Single().SetID).Single().id.Substring(0, 2));
    DomainImage.texture = AssetBundleCacher.Instance.cardsdomains[CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == boosterCards.boosterCards[i].id).Single().SetID).Single().id.Substring(0, 2)].texture;
    DomainImage.GetComponent<LayoutElement>().minWidth = DomainImage.GetComponent<RectTransform>().rect.height;
  }

  private void UpdateDomain(LocalizeStringEvent domainLocale, LocalizedString[] domainsLocale, int domain)
  {
    switch (domain)
    {
      case 1:
        domainLocale.StringReference = domainsLocale[0];
        break;
      case 2:
        domainLocale.StringReference = domainsLocale[1];
        break;
      case 3:
        domainLocale.StringReference = domainsLocale[2];
        break;
      case 4:
        domainLocale.StringReference = domainsLocale[3];
        break;
      case 5:
        domainLocale.StringReference = domainsLocale[4];
        break;
      case 6:
        domainLocale.StringReference = domainsLocale[5];
        break;
      case 7:
        domainLocale.StringReference = domainsLocale[6];
        break;
    }
  }

  private void UpdateType(LocalizeStringEvent typeLocale, LocalizedString[] typesLocale, int type)
  {
    switch (type)
    {
      case 1:
        typeLocale.StringReference = typesLocale[0];
        break;
      case 2:
        typeLocale.StringReference = typesLocale[1];
        break;
      case 3:
        typeLocale.StringReference = typesLocale[2];
        break;
      case 4:
        typeLocale.StringReference = typesLocale[3];
        break;
      case 5:
        typeLocale.StringReference = typesLocale[4];
        break;
      case 6:
        typeLocale.StringReference = typesLocale[5];
        break;
    }
  }

  // Update is called once per frame
  public void NextCard()
  {

    if (i < boosterCards.boosterCards.Length - 1)
    {
      i++;
      //UpdateCard(await GetJPEG.GetTexture(boosterCards.boosterCards[i].id));
      UpdateCard(AssetBundleCacher.Instance.cards[boosterCards.boosterCards[i].id].texture);
      SideCards.SetActiveCards(boosterCards.boosterCards.Length - i - 1);
    }
    else
    {
      i = 0;
      gameObject.SetActive(false);
      sumary.gameObject.SetActive(true);
    }

  }


}
