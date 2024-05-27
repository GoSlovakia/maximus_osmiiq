using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardFromSetContainer : MonoBehaviour
{
  public Button btn;
  public TextMeshProUGUI text;
  public Image rarity;
  public Image border;
  public RawImage cardType, cardImage;
  private Card _Card;
  public Card Card
  {
    get => _Card;
    set
    {
      _Card = value;
      //btn.interactable = value.copies > 0;
      CheckCard(value);
      text.text = Card.id;
      switch (Card.rarity)
      {
        case "C":
          rarity.color = new Color(163.0f / 255.0f, 160.0f / 255.0f, 156.0f / 255.0f);
          break;
        case "R":
          rarity.color = new Color(73.0f / 255.0f, 174.0f / 255.0f, 237.0f / 255.0f);
          break;
        case "E":
          rarity.color = new Color(254.0f / 255.0f, 140.0f / 255.0f, 75.0f / 255.0f);
          break;
        case "L":
          rarity.color = new Color(255.0f / 255.0f, 102.0f / 255.0f, 102.0f / 255.0f);
          break;
      }
      GetImage();
    }
  }

  private void CheckCard(Card value)
  {
    foreach (var item in CardManager.UserCards)
    {
      if (item.id == value.id)
        return;
    }
    btn.interactable = false;
  }

  private void GetImage()
  {
    cardImage.enabled = false;
    Invoke("dd", 0.2f); //se isto nao tiver aqui por alguma razao as card back dos sets ficam pretas......
    //cardImage.enabled = true;
    if (CardManager.UserCards.Where(x => x.id == Card.id).Count() > 0)
    {
      //cardImage.texture = await GetJPEG.GetThumbTexture(Card.id);
      if (Card.id != "PC0012")
        cardImage.texture = AssetBundleCacher.Instance.cards[Card.id + "_thumb"].texture;
      else
        cardImage.texture = AssetBundleCacher.Instance.cards[Card.id + "_thum"].texture;

      cardImage.color = Color.white;
      //cardType.texture = await GetJPEG.GetCardType(Card.type);
      cardType.texture = AssetBundleCacher.Instance.cardstypes[Card.type].texture;
    }
    else
    {
      cardType.gameObject.SetActive(false);
    }
  }
  private void dd()
  {
    cardImage.enabled = true;
    cardImage.enabled = false;
    cardImage.enabled = true;
  }
}
