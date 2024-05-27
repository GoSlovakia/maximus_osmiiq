using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization;

public class CardPopUpComponent : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI Quiis, Quiids;


  [SerializeField]
  private TextMeshProUGUI CardName;
  [SerializeField]
  private TextMeshProUGUI set;
  [SerializeField]
  private TextMeshProUGUI CardType;
  [SerializeField]
  private LocalizeStringEvent rarityLocale, typeLocale, domainLocale;
  [SerializeField]
  private LocalizedString[] raritiesLocale, typesLocale, domainsLocale;
  [SerializeField]
  private TextMeshProUGUI Rarity;
  [SerializeField]
  private TextMeshProUGUI Domain;
  [SerializeField]
  private TextMeshProUGUI OneWordDesc;

  [SerializeField]
  private Image rarityImage;
  [SerializeField]
  private Image BG;

  private int xp = 4;

  [SerializeField]
  private Color CommonBG;
  [SerializeField]
  private Color RareBG;
  [SerializeField]
  private Color EpicBG;
  [SerializeField]
  private Color LegendaryBG;

  [SerializeField]
  private RawImage TypeImage;
  [SerializeField]
  private RawImage DomainImage;

  [SerializeField]
  private TextMeshProUGUI PriceQUI;
  [SerializeField]
  private TextMeshProUGUI PriceQI;
  [SerializeField]
  private RawImage cardImage;

  [SerializeField]
  private Sprite CommonRarity;
  [SerializeField]
  private Sprite RareRarity;
  [SerializeField]
  private Sprite EpicRarity;
  [SerializeField]
  private Sprite LegendaryRarity;

  [SerializeField]
  SingleCardSummary singleCardSummary;

  [SerializeField]
  private LocalizeStringEvent titleLocale;
  [SerializeField]
  private LocalizedString[] titlesLocale;

  CardSellInfo card;

  public void UpdateSummary()
  {
    singleCardSummary.cardImage.texture = cardImage.texture;

    switch (card.card.rarity)
    {
      case "C":
        singleCardSummary.panel.color = CommonBG;
        break;
      case "R":
        singleCardSummary.panel.color = RareBG;
        break;
      case "E":
        singleCardSummary.panel.color = EpicBG;
        break;
      case "L":
        singleCardSummary.panel.color = LegendaryBG;
        break;
    }

  }


  public void UpdatePopUp(CardSellInfo card)
  {
    this.card = card;
    CardName.text = card.card.title;
    PriceQUI.text = card.card.priceQUI.ToString();
    CardType.text = card.card.type.ToString();
    cardImage.texture = card.image.texture;
    switch (card.card.rarity)
    {
      case "C":
        //Rarity.text = "Common";
        rarityLocale.StringReference = raritiesLocale[0];
        rarityImage.sprite = CommonRarity;
        BG.color = CommonBG;
        xp = 4;
        break;
      case "R":
        //Rarity.text = "Rare";
        rarityLocale.StringReference = raritiesLocale[1];
        rarityImage.sprite = RareRarity;
        BG.color = RareBG;
        xp = 9;
        break;
      case "E":
        //Rarity.text = "Epic";
        rarityLocale.StringReference = raritiesLocale[2];
        rarityImage.sprite = EpicRarity;
        BG.color = EpicBG;
        xp = 19;
        break;
      case "L":
        //Rarity.text = "Legendary";
        rarityLocale.StringReference = raritiesLocale[3];
        rarityImage.sprite = LegendaryRarity;
        BG.color = LegendaryBG;
        xp = 38;
        break;
    }
    foreach (CardSet set in CardManager.CardSets.All)
    {
      if (card.card.id.Remove(card.card.id.Length - 1) == set.id)
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
    }

    //set.text = CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == card.card.id).Single().SetID).Single().name;
    //CardType.text = CardManager.CardTypes.All[card.card.type - 1].type;
    OneWordDesc.text = CardManager.AllCards.All.Where(x => x.id == card.card.id).Single().singleworddescription;
    //Domain.text = CardManager.CardDomains.All.Where(x => x.id == CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == card.card.id).Single().SetID).Single().domain).Single().domain;
    foreach (Card currentCard in CardManager.AllCards.All)
    {
      if (currentCard.id == card.card.id)
      {
        CardInspectorComponents.UpdateType(typeLocale, typesLocale, int.Parse(currentCard.type));
        foreach (CardSet set in CardManager.CardSets.All)
        {
          if (set.id == currentCard.SetID)
          {
            CardInspectorComponents.UpdateDomain(domainLocale, domainsLocale, set.domain);
          }
        }
      }
    }
    Debug.Log("card type " + card.card.type.ToString() + " ");
    //TypeImage.texture = await GetJPEG.GetCardType(card.card.type.ToString());
    TypeImage.texture = AssetBundleCacher.Instance.cardstypes[card.card.type.ToString()].texture;
    TypeImage.GetComponent<LayoutElement>().minWidth = TypeImage.GetComponent<RectTransform>().rect.height;
    Debug.Log("Domain " + CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == card.card.id).Single().SetID).Single().id.Substring(0, 2));
    //DomainImage.texture = await GetJPEG.GetDomainTexture(CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == card.card.id).Single().SetID).Single().id.Substring(0, 2));
    DomainImage.texture = AssetBundleCacher.Instance.cardsdomains[CardManager.CardSets.All.Where(x => x.id == CardManager.AllCards.All.Where(x => x.id == card.card.id).Single().SetID).Single().id.Substring(0, 2)].texture;
    DomainImage.GetComponent<LayoutElement>().minWidth = DomainImage.GetComponent<RectTransform>().rect.height;
  }

  public async void BuyCard()
  {
    if (card != null)
    {
      await Purchase(TypeOfCurrency.QUI, card.card.id);
      await AddNotificationCard(card.card.id);
    }
    else
      Debug.Log("Card is null");
  }

  public CancellationTokenSource cancel = new CancellationTokenSource();

  private async Task Purchase(TypeOfCurrency currency, string cardId)
  {
    string Job = "Purchasing Card";
    LoadingManager.instance.EnqueueLoad(Job);
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "buyCard.php?userID=" + UserLogin.instance.LogInInfo.user.id + "&currencyType=" + currency + "&cardID=" + cardId);

    var req = www.SendWebRequest();
    while (!req.isDone)
    {
      await Task.Yield();
      if (cancel.Token.IsCancellationRequested)
      {
        Debug.Log("Canceled");
        LoadingManager.instance.DequeueLoad(Job);
        return;
      }
    }

    if (cancel.Token.IsCancellationRequested)
    {
      Debug.Log("Canceled");
      LoadingManager.instance.DequeueLoad(Job);
      return;
    }

    if (www.result != UnityWebRequest.Result.Success)
    {
      Debug.LogError("Purchase failed " + www.error);
    }
    else
    {
      Debug.Log(www.downloadHandler.text);
      if (www.downloadHandler.text == "1")
      {
        Debug.LogError("Not enough money");
        LoadingManager.instance.DequeueLoad(Job);
        return;
      }
      await AddCard(cardId);
      await GetCurrency.GetUserBalance(Quiids, Quiis);
      await UserLevelComponent.AddXP(xp);

      card.Owned();
      gameObject.SetActive(false);
    }
    LoadingManager.instance.DequeueLoad(Job);
  }

  private async Task AddCard(string cardID)
  {
    string Job = "Adding the card to the User's inventory";
    LoadingManager.instance.EnqueueLoad(Job);
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "setUserCards.php?user=" + UserLogin.instance.LogInInfo.user.id + "&cardID=" + cardID + "&amount=1");

    var req = www.SendWebRequest();
    while (!req.isDone)
    {
      await Task.Yield();
      if (cancel.Token.IsCancellationRequested)
      {
        Debug.Log("Canceled");
        return;
      }
    }

    if (cancel.Token.IsCancellationRequested)
    {
      Debug.Log("Canceled");
      return;
    }

    if (www.result != UnityWebRequest.Result.Success)
    {
      Debug.LogError("Purchase failed " + www.error);
    }
    else
    {
      Debug.Log(www.downloadHandler.text);
      await LoadCards.LoadAll(true);
    }
    LoadingManager.instance.DequeueLoad(Job);
  }

  private async Task AddNotificationCard(string cardID)
  {
    string Job = "Adding a notification for the card inventory";
    LoadingManager.instance.EnqueueLoad(Job);
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "setUserCardNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id + "&cardid=" + cardID);

    var req = www.SendWebRequest();
    while (!req.isDone)
    {
      await Task.Yield();
      if (cancel.Token.IsCancellationRequested)
      {
        Debug.Log("Canceled");
        return;
      }
    }

    if (cancel.Token.IsCancellationRequested)
    {
      Debug.Log("Canceled");
      return;
    }

    if (www.result != UnityWebRequest.Result.Success)
    {
      Debug.LogError("Purchase failed " + www.error);
    }
    else
    {
      Debug.Log("Success");
    }
    LoadingManager.instance.DequeueLoad(Job);
  }
}
