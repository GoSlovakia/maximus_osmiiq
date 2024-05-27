using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOptions : MonoBehaviour
{

    private static List<CardsSetComponent> cardsSetComponents;
    private static List<CardButton> cardButtons;
    // Start is called before the first frame update
    void Start()
    {
        cardsSetComponents = new List<CardsSetComponent>();
        cardButtons = new List<CardButton>();
    }

    public void CheckOn(Toggle toggle)
    {
        Image temp = toggle.transform.GetChild(1).GetChild(0).GetComponent<Image>();
        if (toggle.isOn)
        {
            
            temp.color = Color.black;
        }
        else
        {
            temp.color = Color.white;
        }
    }

    public static void SetCardsSetComponents(CardsSetComponent cardSetComponent)
    {
        cardsSetComponents.Add(cardSetComponent);
    }

    public static void SetCardsButtons(CardButton cardButton)
    {
        cardButtons.Add(cardButton);
    }

    public void HideUniqueCards(Toggle toggle)
    {
        if (toggle.isOn)
        {
            foreach (CardButton card in cardButtons)
            {
                if (card.Card.copies == 0)
                    card.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (CardButton card in cardButtons)
            {
                card.gameObject.SetActive(true);
            }
        }

    }

    public void HideAllCompletedSets(Toggle toggle)
    {
        if (toggle.isOn)
        {
            foreach (CardsSetComponent card in cardsSetComponents)
            {
                if (card.Redeemed)
                    card.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (CardsSetComponent card in cardsSetComponents)
            {
                card.gameObject.SetActive(true);
            }
        }
    }
}
