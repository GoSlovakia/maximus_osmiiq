using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SortCards : MonoBehaviour
{
  [SerializeField]
  LocalizedString domain, incomplete, cardDomain, cardRarity, duplicates;
  [SerializeField]
  private LocalizeStringEvent setsLocalization, inventoryLocalization;
  [SerializeField]
  private GameObject setsHolder;

  [SerializeField]
  private GameObject cardsHolder;

  public string[] cardsDomains;

  public void SortSets(TMP_Dropdown dropdown)
  {
    switch (dropdown.value)
    {
      case 0:
        setsLocalization.StringReference = domain;
        SortBySetDomain();
        break;


      case 1:
        setsLocalization.StringReference = incomplete;
        SortByIncomplete();
        break;
    }
  }

  private void SortBySetDomain()
  {
    Dictionary<string, Transform> sets = new();
    string[] setDomainsNames;

    setDomainsNames = new string[setsHolder.transform.childCount];
    for (int i = 0; i < setsHolder.transform.childCount; i++)
    {
      setDomainsNames[i] = setsHolder.transform.GetChild(i).GetComponent<CardsSetComponent>().SetNameTMP.text;
      sets.Add(setsHolder.transform.GetChild(i).GetComponent<CardsSetComponent>().SetNameTMP.text, setsHolder.transform.GetChild(i));
    }
    Array.Sort(setDomainsNames);
    foreach (string setName in setDomainsNames)
    {
      sets[setName].SetAsLastSibling();
    }
  }

  private void SortByIncomplete()
  {
    SortBySetDomain();

    int siblingCount = 0;
    for (int i = 0; i < setsHolder.transform.childCount; i++)
    {
      CardsSetComponent temp = setsHolder.transform.GetChild(i).GetComponent<CardsSetComponent>();

      if (!temp.Redeemed)
      {
        temp.transform.SetSiblingIndex(siblingCount);
        siblingCount++;
      }
    }
  }

  public void SortInventoryCards(TMP_Dropdown dropdown)
  {
    switch (dropdown.value)
    {
      case 0:
        inventoryLocalization.StringReference = cardDomain;
        SortByCardDomain();
        break;
      case 1:
        inventoryLocalization.StringReference = duplicates;
        SortByCardDuplicates();
        break;
      case 2:
        inventoryLocalization.StringReference = cardRarity;
        SortByCardRarity();
        break;
    }
  }

  private void SortByCardDomain()
  {
    SortByCardRarity();

    List<CardButton> cardButtons = new();
    cardsDomains = new string[cardsHolder.transform.childCount];
    for (int i = 0; i < cardsHolder.transform.childCount; i++)
    {
      cardButtons.Add(cardsHolder.transform.GetChild(i).GetComponent<CardButton>());
      foreach (CardSet set in CardManager.CardSets.All)
      {
        if (set.id == cardButtons[i].Card.SetID)
          cardsDomains[i] = cardButtons[i].Card.SetID;
      }
    }

    Array.Sort(cardsDomains);

    for (int i = 0; i < cardsDomains.Length; i++)
    {
      for (int j = 0; j < cardButtons.Count; j++)
      {
        if (cardsDomains[i] == cardButtons[j].Card.SetID)
        {
          cardButtons[j].transform.SetAsLastSibling();
          cardButtons.Remove(cardButtons[j]);
        }

      }
    }
  }

  private void SortByCardName()
  {
    string[] cardNames = new string[cardsHolder.transform.childCount];
    Dictionary<string, Transform> cardsTemp = new();

    for (int i = 0; i < cardsHolder.transform.childCount; i++)
    {
      CardButton temp = cardsHolder.transform.GetChild(i).GetComponent<CardButton>();
      cardsTemp.Add(temp.Card.title, temp.transform);
      cardNames[i] = temp.Card.title;
    }
    Array.Sort(cardNames);

    foreach (string name in cardNames)
    {
      cardsTemp[name].SetAsLastSibling();
    }
  }

  private void SortByCardDuplicates()
  {
    SortByCardName();
    int siblingCount = cardsHolder.transform.childCount - 1;
    int maxDuplicates = 0;
    CardButton[] cardButtons = new CardButton[cardsHolder.transform.childCount];

    for (int i = 0; i < cardsHolder.transform.childCount; i++)
    {
      cardButtons[i] = cardsHolder.transform.GetChild(i).GetComponent<CardButton>();

      if (cardButtons[i].Card.copies > maxDuplicates)
        maxDuplicates = cardButtons[i].Copies;
    }

    for (int i = 0; i < maxDuplicates; i++)
    {
      for (int j = 0; j < cardsHolder.transform.childCount; j++)
      {
        if (cardButtons[j].Card.copies == i)
        {
          cardButtons[j].transform.SetSiblingIndex(siblingCount);
          siblingCount--;
        }
      }
    }
  }

  private void SortByCardRarity()
  {
    SortByCardName();

    int siblingCount = 0;
    int rarityCount = Enum.GetNames(typeof(Rarity)).Length;
    CardButton[] cardButtons = new CardButton[cardsHolder.transform.childCount];

    for (int i = 0; i < rarityCount; i++)
    {
      string currentRarity = Enum.GetName(typeof(Rarity), i);
      for (int j = 0; j < cardsHolder.transform.childCount; j++)
      {
        cardButtons[j] = cardsHolder.transform.GetChild(j).GetComponent<CardButton>();
        if (cardButtons[j].Card.rarity.Trim() == currentRarity.Trim())
        {
          cardButtons[j].transform.SetSiblingIndex(siblingCount);
          siblingCount++;
        }
      }
    }

    for (int i = 0; i < cardButtons.Length; i++)
    {
      siblingCount--;
      cardButtons[i].transform.SetSiblingIndex(siblingCount);
    }
  }
}
