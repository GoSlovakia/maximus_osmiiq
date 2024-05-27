using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.Util;

public class CreateSets : MonoBehaviour
{
  public static CreateSets instance;

  public GameObject CardPrefab;
  public CardsSetComponent SetPrefab;
  public GameObject SetViewport;

  [SerializeField]
  private GameObject RewardsReceivedPanel;
  [SerializeField]
  private CardInspectorComponents InspectCardPanel;
  [SerializeField]
  private RewardPanel SetRewards;

  public List<CardsSetComponent> Sets = new List<CardsSetComponent>();


  private void Awake()
  {
    if (instance != null && instance != this)
      Destroy(gameObject);
    else
      instance = this;
  }

  public void GenerateSets(Card[] All)
  {

    foreach (Card c in All)
    {
      if (Sets.Where(x => x.Set.id == c.SetID).Count() == 0)
      {
        var novo = Instantiate(SetPrefab);
        // Debug.Log("Card set " + c.SetID);
        if (CardManager.CardSets.All.Where(x => x.id == c.SetID).Count() == 0)
        {
        }
        else { novo.Set = CardManager.CardSets.All.Where(x => x.id == c.SetID).Single(); }

        // Debug.Log(novo.GetComponent<CardsSetComponent>().Set.id + " CardID");
        novo.transform.SetParent(SetViewport.transform);
        Sets.Add(novo);
        novo.Unlockbtn.onClick.AddListener(delegate { novo.RedeemItem(); });
        foreach (var item in UserSetsComponent.AllUserSets.All)
        {
          if (novo.set.id == item.SetID)
          {
            novo.Redeemed = true;
          }
        }


        //novo.Unlockbtn.onClick.AddListener(delegate { RefreshRewardPanel(novo); });
        //Debug.Log(novo.Unlockbtn.name);
      }
      var novacard = Instantiate(CardPrefab, Sets.Where(x => x.Set.id == c.SetID).Single().transform.GetChild(0));
      if (novacard == null)
        Debug.LogError("Card is null");

      Card setcard = new Card(c);

      if (CardManager.UserCards.Where(x => x.id == c.id).Count() != 0)
      {
        //Debug.Log(CardManager.UserCards.Where(x => x.id == c.id).Count());
        Debug.Log(CardManager.UserCards.Where(x => x.id == setcard.id).Count() + " Cards found" + setcard.id);
        setcard.copies = CardManager.UserCards.Where(x => x.id == setcard.id).Single().copies;
        //Debug.Log(setcard.copies + " " + CardManager.UserCards.Where(x => x.id == c.id).Single().copies + " " + CardManager.UserCards[1].copies);
      }
      else
      {
        setcard.copies = 0;
      }


      //Podes meter aqui mais cenas para configurar a carta quando e criada
      CardFromSetContainer cardFromSet = novacard.GetComponent<CardFromSetContainer>();
      //Debug.Log("setcard null?" + (setcard == null));
      cardFromSet.Card = setcard;

      cardFromSet.btn.onClick.AddListener(delegate { InspectCardPanel.InspectCard(cardFromSet.Card); });


    }


    foreach (CardsSetComponent este in Sets)
    {
      ToggleOptions.SetCardsSetComponents(este);
      este.gameObject.transform.GetChild(0).SetAsLastSibling();
      este.UpdateSetDisplay();
    }
  }

  private void RefreshRewardPanel(CardsSetComponent cardsSet)
  {
    SetRewards.redeem.onClick.AddListener(delegate { cardsSet.RedeemItem(); });
    SetRewards.gameObject.SetActive(true);
  }

  public void RefreshCopies()
  {

    foreach (CardsSetComponent set in Sets)
    {
      for (int i = 0; i < set.transform.GetChild(set.transform.childCount - 1).childCount; i++)
      {
        foreach (var item in CreateCards.CreatedCards)
        {
          if (item.Card.id == set.transform.GetChild(set.transform.childCount - 1).GetChild(i).GetComponent<CardFromSetContainer>().Card.id)
          {
            set.transform.GetChild(set.transform.childCount - 1).GetChild(i).GetComponent<CardFromSetContainer>().Card.copies = item.Card.copies;
          }
        }
      }
    }
  }
}
