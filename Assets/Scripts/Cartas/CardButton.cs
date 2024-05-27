using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
  public Card Card;
  public Button btn;
  private int _copies;
  public int Copies
  {
    get => _copies;
    set
    {
      _copies = value;
      Counter.text = "x" + value.ToString();
      if (value <= 0)
      {
        Counter.transform.parent.gameObject.SetActive(false);
      }
      else
      {
        Counter.transform.parent.gameObject.SetActive(true);
      }
    }
  }
  public TextMeshProUGUI Counter;

  public Image RarityDisplay;
  public RawImage card;
  public RawImage TypeDisplay, DomainDisplay;
  private bool _highlighted;

  public bool highlighted
  {
    get => _highlighted;
    set
    {
      _highlighted = value;
      if (_highlighted)
      {
        GetComponent<NotificationComponent>().showNotification = true;
      }
      else
      {
        GetComponent<NotificationComponent>().showNotification = false;
      }
    }
  }

  public void SetCard(Card nova)
  {
    Card = nova;
    switch (nova.rarity)
    {
      case "C":
        RarityDisplay.color = new Color(163.0f / 255.0f, 160.0f / 255.0f, 156.0f / 255.0f);
        break;
      case "R":
        RarityDisplay.color = new Color(73.0f / 255.0f, 174.0f / 255.0f, 237.0f / 255.0f);
        break;
      case "E":
        RarityDisplay.color = new Color(254.0f / 255.0f, 140.0f / 255.0f, 75.0f / 255.0f);
        break;
      case "L":
        RarityDisplay.color = new Color(255.0f / 255.0f, 102.0f / 255.0f, 102.0f / 255.0f);
        break;
    }
    //TypeDisplay.texture = await GetJPEG.GetCardType(Card.type);
    Debug.Log("Card.type = " + Card.type);
    TypeDisplay.texture = AssetBundleCacher.Instance.cardstypes[Card.type].texture;
    //DomainDisplay.texture = await GetJPEG.GetDomainTexture(Card.SetID[0] + "" + Card.SetID[1]);
    Debug.Log("Card.SetID 1 = " + Card.SetID[0] + " Card.SetID 2 = " + Card.SetID[1]);
    DomainDisplay.texture = AssetBundleCacher.Instance.cardsdomains[Card.SetID[0] + "" + Card.SetID[1]].texture;
    //card.texture = await GetJPEG.GetThumbTexture(Card.id);
    Debug.Log("Card.id = " + Card.id);
    if (Card.id != "PC0012")
      card.texture = AssetBundleCacher.Instance.cards[Card.id + "_thumb"].texture;
    else
      card.texture = AssetBundleCacher.Instance.cards[Card.id + "_thum"].texture;

    NotificationsCardManager.instance.CheckHighlightCard(this);
  }

  public void UpdateCopies()
  {
    if (Card.copies <= 0)
    {
      Counter = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
      Counter.transform.parent.gameObject.SetActive(false);
    }
    else
    {
      Counter.transform.parent.gameObject.SetActive(true);
      Counter.text = Card.copies.ToString();
    }
  }
}
