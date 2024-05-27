using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class CardInspectorComponents : MonoBehaviour
{
  [SerializeField]
  private GameObject cardsCollection;
  public string cardID;
  public TextMeshProUGUI illustratorName, copyrightYear, cardName, cardName1, shortDescription, shortDescription1, description, quote, cardTypeText, domainText, setNameText, setNameText1, rarityText, duplicatesText, artist, year;
  public LocalizeStringEvent rarityLocale, typeLocale, domainLocale;
  public LocalizedString[] raritiesLocale, typesLocale, domainsLocale;
  public Image displayPanel, setNamePanel, setNamePanel1, rarityImage;
  public RawImage cardImage, cardTypeImage, domainImage;
  public Card currenctCard;
  [SerializeField]
  private Sprite[] rarities;
  [SerializeField]
  private LocalizeStringEvent setNameLocale, setNameLocale1;
  [SerializeField]
  private LocalizedString[] setNamesLocale;

  public void InspectCard(CardButton cardbtn)
  {
    UpdateCard(cardbtn.Card);
  }

  public void InspectCard(Card card)
  {
    UpdateCard(card);
  }

  public void NextCard()
  {
    for (int i = 0; i < cardsCollection.transform.childCount; i++)
    {
      Card card = cardsCollection.transform.GetChild(i).GetComponent<CardButton>().Card;
      if (card.id == cardID)
      {
        if (i == cardsCollection.transform.childCount - 1)
          card = cardsCollection.transform.GetChild(0).GetComponent<CardButton>().Card;
        else
          card = cardsCollection.transform.GetChild(i + 1).GetComponent<CardButton>().Card;

        if (cardsCollection.transform.GetChild(i).GetComponent<CardButton>().highlighted)
        {
          cardsCollection.transform.GetChild(i).GetComponent<CardButton>().highlighted = false;
          NotificationsCardManager.instance.RemoveCardNotif(cardsCollection.transform.GetChild(i).GetComponent<CardButton>());
        }
        UpdateCard(card);
        return;
      }
    }
  }

  private void OnDisable()
  {
    for (int i = 0; i < cardsCollection.transform.childCount; i++)
    {
      Card card = cardsCollection.transform.GetChild(i).GetComponent<CardButton>().Card;
      if (card.id == cardID && cardsCollection.transform.GetChild(i).GetComponent<CardButton>().highlighted)
      {
        NotificationsCardManager.instance.RemoveCardNotif(cardsCollection.transform.GetChild(i).GetComponent<CardButton>());
        return;
      }
    }
  }

  public void PreviousCard()
  {
    for (int i = 0; i < cardsCollection.transform.childCount; i++)
    {
      Card card = cardsCollection.transform.GetChild(i).GetComponent<CardButton>().Card;
      if (card.id == cardID)
      {
        if (i == 0)
          card = cardsCollection.transform.GetChild(cardsCollection.transform.childCount - 1).GetComponent<CardButton>().Card;
        else
          card = cardsCollection.transform.GetChild(i - 1).GetComponent<CardButton>().Card;

        if (cardsCollection.transform.GetChild(i).GetComponent<CardButton>().highlighted)
        {
          cardsCollection.transform.GetChild(i).GetComponent<CardButton>().highlighted = false;
          NotificationsCardManager.instance.RemoveCardNotif(cardsCollection.transform.GetChild(i).GetComponent<CardButton>());
        }
        UpdateCard(card);
        return;
      }
    }
  }

  private void UpdateCard(Card card)
  {

    cardID = card.id;
    //cardImage.texture = await GetJPEG.GetTexture(card.id);
    cardImage.texture = AssetBundleCacher.Instance.cards[card.id].texture;
    cardName.text = card.title;
    cardName1.text = card.title;
    shortDescription.text = card.singleworddescription;
    shortDescription1.text = card.singleworddescription;
    description.text = card.longdescription;
    quote.text = card.quote;
    duplicatesText.text = card.copies.ToString();
    currenctCard = card;
    artist.text = "Illustrated  by <" + card.Artistname + ">";
    year.text = "Copyright, <" + card.CardYear + ">";
    gameObject.SetActive(true);
    //cardTypeImage.texture = await GetJPEG.GetCardType(card.type);
    cardTypeImage.texture = AssetBundleCacher.Instance.cardstypes[card.type].texture;
    //domainImage.texture = await GetJPEG.GetDomainTexture(card.SetID[0] + "" + card.SetID[1]);
    domainImage.texture = AssetBundleCacher.Instance.cardsdomains[card.SetID[0] + "" + card.SetID[1]].texture;
    //cardTypeText.text = GetCardType(card);
    UpdateType(typeLocale, typesLocale, int.Parse(card.type));
    foreach (CardSet set in CardManager.CardSets.All)
    {
      if (set.id == card.SetID)
      {
        switch (set.id)
        {
          case "CA001":
            setNameLocale.StringReference = setNamesLocale[0];
            setNameLocale1.StringReference = setNamesLocale[0];
            break;
          case "CA002":
            setNameLocale.StringReference = setNamesLocale[1];
            setNameLocale1.StringReference = setNamesLocale[1];
            break;
          case "FR001":
            setNameLocale.StringReference = setNamesLocale[2];
            setNameLocale1.StringReference = setNamesLocale[2];
            break;
          case "FR002":
            setNameLocale.StringReference = setNamesLocale[3];
            setNameLocale1.StringReference = setNamesLocale[3];
            break;
          case "HG001":
            setNameLocale.StringReference = setNamesLocale[4];
            setNameLocale1.StringReference = setNamesLocale[4];
            break;
          case "HG002":
            setNameLocale.StringReference = setNamesLocale[5];
            setNameLocale1.StringReference = setNamesLocale[5];
            break;
          case "HG003":
            setNameLocale.StringReference = setNamesLocale[6];
            setNameLocale1.StringReference = setNamesLocale[6];
            break;
          case "HG004":
            setNameLocale.StringReference = setNamesLocale[7];
            setNameLocale1.StringReference = setNamesLocale[7];
            break;
          case "HG005":
            setNameLocale.StringReference = setNamesLocale[8];
            setNameLocale1.StringReference = setNamesLocale[8];
            break;
          case "LL001":
            setNameLocale.StringReference = setNamesLocale[9];
            setNameLocale1.StringReference = setNamesLocale[9];
            break;
          case "LL002":
            setNameLocale.StringReference = setNamesLocale[10];
            setNameLocale1.StringReference = setNamesLocale[10];
            break;
          case "LL003":
            setNameLocale.StringReference = setNamesLocale[11];
            setNameLocale1.StringReference = setNamesLocale[11];
            break;
          case "LL004":
            setNameLocale.StringReference = setNamesLocale[12];
            setNameLocale1.StringReference = setNamesLocale[12];
            break;
          case "LL005":
            setNameLocale.StringReference = setNamesLocale[13];
            setNameLocale1.StringReference = setNamesLocale[13];
            break;
          case "MC001":
            setNameLocale.StringReference = setNamesLocale[14];
            setNameLocale1.StringReference = setNamesLocale[14];
            break;
          case "PC001":
            setNameLocale.StringReference = setNamesLocale[15];
            setNameLocale1.StringReference = setNamesLocale[15];
            break;
          case "PC002":
            setNameLocale.StringReference = setNamesLocale[16];
            setNameLocale1.StringReference = setNamesLocale[16];
            break;
          case "PP002":
            setNameLocale.StringReference = setNamesLocale[17];
            setNameLocale1.StringReference = setNamesLocale[17];
            break;
        }
        //setNameText.text = set.name;
        //setNameText1.text = set.name;
        //domainText.text = GetDomain(set.domain);
        UpdateDomain(domainLocale, domainsLocale, set.domain);
        Debug.Log(set.id + " " + card.SetID + " " + set.domain);
      }
    }
    UpdateRarity(card);
  }

  public static void UpdateDomain(LocalizeStringEvent domainLocale, LocalizedString[] domainsLocale, int domain)
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

  public static void UpdateType(LocalizeStringEvent typeLocale, LocalizedString[] typesLocale, int type)
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

  private void UpdateRarity(Card card)
  {
    switch (card.rarity)
    {
      case "C":
        Color color = new Color(163.0f / 255.0f, 160.0f / 255.0f, 156.0f / 255.0f);
        setNamePanel.color = color;
        setNamePanel1.color = color;
        displayPanel.color = color;
        shortDescription.color = color;
        shortDescription1.color = color;
        rarityImage.sprite = rarities[0];
        //rarityText.text = "Common";
        rarityLocale.StringReference = raritiesLocale[0];
        break;
      case "R":
        Color color2 = new Color(73.0f / 255.0f, 174.0f / 255.0f, 237.0f / 255.0f);
        setNamePanel.color = color2;
        setNamePanel1.color = color2;
        displayPanel.color = color2;
        shortDescription.color = color2;
        shortDescription1.color = color2;
        rarityImage.sprite = rarities[1];
        //rarityText.text = "Rare";
        rarityLocale.StringReference = raritiesLocale[1];
        break;
      case "E":
        Color color3 = new Color(254.0f / 255.0f, 140.0f / 255.0f, 75.0f / 255.0f);
        setNamePanel.color = color3;
        setNamePanel1.color = color3;
        displayPanel.color = color3;
        shortDescription.color = color3;
        shortDescription1.color = color3;
        rarityImage.sprite = rarities[2];
        //rarityText.text = "Epic";
        rarityLocale.StringReference = raritiesLocale[2];
        break;
      case "L":
        Color color4 = new Color(255, 102.0f / 255.0f, 102.0f / 255.0f);
        setNamePanel.color = color4;
        setNamePanel1.color = color4;
        displayPanel.color = color4;
        shortDescription.color = color4;
        shortDescription1.color = color4;
        rarityImage.sprite = rarities[3];
        //rarityText.text = "Legendary";
        rarityLocale.StringReference = raritiesLocale[3];
        break;
    }
  }

  private string GetCardType(Card card)
  {
    foreach (CTypes type in CardManager.CardTypes.All)
    {
      if (type.id == card.type)
        return type.type;
    }
    return null;
  }

  private string GetDomain(int domainID)
  {
    foreach (Domain domain in CardManager.CardDomains.All)
    {
      if (domain.id == domainID)
        return domain.domain;
    }
    return null;
  }
}
