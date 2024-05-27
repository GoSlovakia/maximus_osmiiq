using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public static class CardManager
{
    private static string allCardsJSON;
    public static CardContainer AllCards;

    private static string userCardsJSON;
    public static UserCardsContainer UserCardCodes;
    public static List<Card> UserCards = new List<Card>();

    private static string cardSetsJSON;
    public static CardSetContainer CardSets;

    private static string cardDomainJSON;
    public static CardDomains CardDomains;

    private static string cardSubsetsJSON;
    public static CardSubsets CardSubsets;

    private static string cardTypesJSON;
    public static CardTypes CardTypes;

    public static string UserCardsJSON
    {
        get => userCardsJSON; set
        {
            userCardsJSON = value;

            if (UserCardCodes == null)
                UserCardCodes = JsonUtility.FromJson<UserCardsContainer>(userCardsJSON);
            else
                JsonUtility.FromJsonOverwrite(userCardsJSON, UserCardCodes);

            if (UserCards != null)
                UserCards.Clear();

            if (UserCardCodes != null)
            {
                UserCards.Clear();

                foreach (var card in UserCardCodes.All)
                {
                    Card res = AllCards.All.Where(x => x.id == card.CardID).SingleOrDefault();
                    res.copies = card.CardAmount;
                    if (res != null)
                    {
                        if (UserCards.Where(x => x.id == res.id).Count() != 0)
                        {
                            UserCards.Where(x => x.id == res.id).Single().copies = res.copies;
                        }
                        else
                        {
                            UserCards.Add(res);
                        }
                        //Debug.Log(res.copies + " this card copies");
                    }
                    else
                    {
                        Debug.LogError("This card code doesnt exist! " + card.CardID);
                    }

                }
            }
            Debug.Log("User has " + UserCards.Count()+"cards.");
        }
    }

    public static int UniqueCards
    {

        get { return UserCardCodes.All.Where(x => UserSetsComponent.AllUserSets.All.Where(y => y.SetID == x.CardID.Substring(0, x.CardID.Length - 1)).Count() == 0).Count() + (AchievementTrackerComponent.instance.GetVariable(VariableType.AllSetsCompleted) * 5); }
    }

    public static int DuplicateCards
    {
        get
        {
            int res = 0;
            foreach (var este in UserCardCodes.All)
            {
                res += este.CardAmount - 1;
            }

            return res;
        }
    }

    public static int AllCardsFromSeasonRemaining(int season)
    {
        Debug.Log(AllCardFromSeason(season) + " All cards from season ");
        //Debug.Log(AllCards.All.Where(x => UserCardCodes.All.Where(y => y.CardID == x.id).Count() > 0 && x.id[2] == (char)season).Count());
        return AllCards.All.Where(x =>
        UserCardCodes.All.Where(y => y.CardID == x.id).Count() == 0
        && UserSetsComponent.AllUserSets.All.Where(y => y.SetID == x.SetID).Count() == 0
        && x.id.Substring(2).StartsWith(season.ToString("00"))
        ).Count();
    }

    public static int AllCardFromSeason(int Season)
    {
        return AllCards.All.Where(x => x.id.Substring(2).StartsWith(Season.ToString("00"))).Count();
    }

    public static string AllCardsJSON
    {
        get => allCardsJSON; set
        {
            allCardsJSON = value;
            if (AllCards == null)
                AllCards = JsonUtility.FromJson<CardContainer>(allCardsJSON);
            else
                JsonUtility.FromJsonOverwrite(allCardsJSON, AllCards);
            //Debug.Log("Card json converted " + AllCards.All.Count());
        }
    }

    public static string CardTypesJSON
    {
        get => cardTypesJSON; set
        {
            cardTypesJSON = value;
            if (CardTypes == null)
                CardTypes = JsonUtility.FromJson<CardTypes>(CardTypesJSON);
            else
                JsonUtility.FromJsonOverwrite(CardTypesJSON, CardTypes);
        }
    }

    public static string CardSetsJSON
    {
        get => cardSetsJSON; set
        {
            cardSetsJSON = value;
            if (CardSets == null)
                CardSets = JsonUtility.FromJson<CardSetContainer>(CardSetsJSON);
            else
                JsonUtility.FromJsonOverwrite(CardSetsJSON, CardSets);
        }
    }

    public static string CardDomainJSON
    {
        get => cardDomainJSON; set
        {
            cardDomainJSON = value;
            if (CardDomains == null)
                CardDomains = JsonUtility.FromJson<CardDomains>(CardDomainJSON);
            else
                JsonUtility.FromJsonOverwrite(CardDomainJSON, CardDomains);
        }
    }

    public static string CardSubsetsJSON
    {
        get => cardSubsetsJSON; set
        {
            cardSubsetsJSON = value;
            if (CardSubsets == null)
                CardSubsets = JsonUtility.FromJson<CardSubsets>(CardSubsetsJSON);
            else
                JsonUtility.FromJsonOverwrite(cardSubsetsJSON, CardSubsets);
        }
    }

    public static void GenerateCardsFromServer()
    {
        CreateCards.CreateInventory(UserCards.ToArray());
        CreateSets.instance.GenerateSets(AllCards.All);
    }
}
