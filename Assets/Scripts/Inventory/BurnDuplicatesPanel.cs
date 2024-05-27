using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BurnDuplicatesPanel : MonoBehaviour
{
  [SerializeField]
  private CardInspectorComponents cardDetails;

  public Image RarityImage, SetNamePanel,discardPanel;
  public TextMeshProUGUI cardName, maxDuplicates, rarity, burnDuplicates, quiidsReceived,qiis,SetNameText;
  public RawImage cardImage, cardImageBack;

  private int duplicatesSelected, quiidsValue;
  private void OnEnable()
  {
    duplicatesSelected = 0;
    quiidsValue = 0;
    quiidsReceived.text = quiidsValue.ToString();
    burnDuplicates.text = duplicatesSelected.ToString();
    cardName.text = cardDetails.cardName.text;
    maxDuplicates.text = cardDetails.duplicatesText.text;
    rarity.text = cardDetails.rarityText.text;
    cardImage.texture = cardDetails.cardImage.texture;
    cardImageBack.texture = cardDetails.cardImage.texture;
    RarityImage.sprite = cardDetails.rarityImage.sprite;
    SetNamePanel.color = cardDetails.setNamePanel.color;
    SetNameText.text = cardDetails.setNameText.text;
    discardPanel.color = cardDetails.displayPanel.color;
  }

  public async void IncreaseDuplicates()
  {
    duplicatesSelected += 1;
    int maxDuplicates = int.Parse(this.maxDuplicates.text);
    if (duplicatesSelected > maxDuplicates)
    {
      duplicatesSelected -= 1;
    }
    else
    {
      quiidsValue += await GetQuiidPrice(cardDetails.currenctCard.rarity);
      quiidsReceived.text = quiidsValue.ToString();
    }
    burnDuplicates.text = duplicatesSelected.ToString();
  }

  public async void DecreaseDuplicates()
  {
    duplicatesSelected -= 1;
    if (duplicatesSelected < 0)
    {
      duplicatesSelected = 0;
    }
    else
    {
      quiidsValue -= await GetQuiidPrice(cardDetails.currenctCard.rarity);
      quiidsReceived.text = quiidsValue.ToString();
    }
    burnDuplicates.text = duplicatesSelected.ToString();
  }

  public async void DiscardCards()
  {
    CurrencyData temp = await GetCurrency.GetUserBalance();
    int duplicates = int.Parse(cardDetails.duplicatesText.text) - duplicatesSelected;
    int moneyReceived = int.Parse(temp.QUI) + quiidsValue;
    if (moneyReceived > int.Parse(temp.QUI))
    {
      await SetDiscardCards(cardDetails.cardID, duplicates, moneyReceived);

      cardDetails.duplicatesText.text = duplicates.ToString();
      cardDetails.currenctCard.copies = duplicates;
      foreach (var card in CardManager.AllCards.All)
      {
        if (card.id == cardDetails.currenctCard.id)
        {
          card.copies = cardDetails.currenctCard.copies;
        }
      }

      if (cardDetails.currenctCard != null)
        gameObject.SetActive(false);

      Debug.Log("sss " + CreateCards.CreatedCards.Count);
      foreach (var item in CreateCards.CreatedCards)
      {
        if (item.Card.id == cardDetails.cardID)
        {
          item.UpdateCopies();
        }
      }
    }
    qiis.text = moneyReceived.ToString();
  }


  public static CancellationTokenSource cancel = new CancellationTokenSource();
  public static async Task<int> GetQuiidPrice(string rarity)
  {
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getQuiidPriceByRarity.php?rarity=" + rarity);

    var req = www.SendWebRequest();
    while (!req.isDone)
    {
      await Task.Yield();
      if (cancel.Token.IsCancellationRequested)
      {
        Debug.Log("Canceled");
        return 0;
      }
    }

    if (cancel.Token.IsCancellationRequested)
    {
      Debug.Log("Canceled");
      return 0;
    }

    if (www.result != UnityWebRequest.Result.Success)
    {
      Debug.LogError("Purchase failed " + www.error);
    }
    else
    {
      return int.Parse(www.downloadHandler.text);
    }
    return 0;
  }

  public static async Task SetDiscardCards(string cardID, int cardAmount, int amountQUI)
  {
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "setDiscardCards.php?userID=" + UserLogin.instance.LogInInfo.user.id + "&cardID=" + cardID + "&cardsAmount=" + cardAmount + "&amountQUI=" + amountQUI);

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
      Debug.Log("Discarded");
    }
  }
}
