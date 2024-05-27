using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class CreateCards : MonoBehaviour
{
    [SerializeField]
    private CardInspectorComponents cardInspectorPanel;
    private static CardInspectorComponents CardInspectorPanel;

    [SerializeField]
    private GameObject CardPrefabSerialize;
    [SerializeField]
    private Transform AllCardsParentSerialize;
    private static GameObject CardPrefab;
    public static Transform AllCardsParent;
    [SerializeField]
    private List<CardTypeIconSO> CardIconsSerialize;

    public static List<CardButton> AllCards = new List<CardButton>();
    public static List<CardTypeIconSO> CardIcons;
    public static List<CardButton> CreatedCards = new List<CardButton>();
    //Apenas para debug, enquanto nao ha servidor
    private void Start()
    {
        Initialize();
    }

    public async void Initialize()
    {
        if (CardIcons != null)
            CardIcons.Clear();
        CardInspectorPanel = cardInspectorPanel;
        CardPrefab = CardPrefabSerialize;
        AllCardsParent = AllCardsParentSerialize;
        CardIcons = CardIconsSerialize;
        await LoadCards.LoadAll(false);

        CardManager.GenerateCardsFromServer();
        //AllCardsParentSerialize.parent.parent.gameObject.SetActive(false);
    }

    public static void CreateInventory(Card[] All)
    {
        AllCards.Clear();
        CreatedCards.Clear();
        Debug.Log("Creating Cards " + All.Count());
        foreach (Card card in All)
        {
            if (AllCards.Where(x => x.Card.title == card.title).Count() != 0)
            {
                //Debug.Log("Copy Found");
                AllCards.Where(x => x.Card.title == card.title).Single().Copies++;
            }
            else
            {
                //Debug.Log("Creating Card " + card.title);
                GameObject novo = Instantiate(CardPrefab, AllCardsParent);
                CardButton novoCB = novo.GetComponent<CardButton>();
                CreatedCards.Add(novoCB);
                novoCB.SetCard(card);
                novoCB.Copies = card.copies;
                if (novoCB.Card.type == "0")
                    novoCB.Card.type = "1";
                //Debug.Log(CardIcons[0].Type.GetCardCategory() + " " + novoCB.Card.type + " " + novoCB.Copies);

                //novoCB.CardTypeDisplay.sprite = CardIcons.Where(x => x.Type.GetCardCategory() == CardManager.CardTypes.All.Where(x => x.id == card.type).Single().type).Single().Icon;

                InspectCard(novoCB);
                ToggleOptions.SetCardsButtons(novoCB);
            }

        }
    }

    public static void UpdateInventory()
    {
        // Debug.Log("Updating Inventory");
        foreach (var este in AllCardsParent.GetComponentsInChildren<CardButton>())
        {

            if (CardManager.UserCards.Where(x => x.id == este.Card.id).Count() != 0)
            {
                Card res = CardManager.UserCards.Where(x => x.id == este.Card.id).Single();
                //Debug.Log("Card Still Exists " + CardManager.UserCards.Where(x => x.id == este.Card.id).Count());
                este.Card.copies = res.copies;
                este.Copies = res.copies;
                InspectCard(este);
            }
            else
            {
                este.gameObject.SetActive(false);
            }
        }
    }

    private static void InspectCard(CardButton btn)
    {
        btn.btn.onClick.AddListener(delegate
        {
            CardInspectorPanel.InspectCard(btn);
            if (btn.highlighted)
            {
                btn.highlighted = false;
                NotificationsCardManager.instance.RemoveCardNotif(btn);
            }
        });
    }

    public void RefreshCards()
    {
        for (int i = 0; i < AllCardsParentSerialize.childCount; i++)
        {
            AllCardsParentSerialize.GetChild(i).GetComponent<CardButton>().Counter.text = AllCardsParentSerialize.GetChild(i).GetComponent<CardButton>().Card.copies.ToString();
        }
    }
}
