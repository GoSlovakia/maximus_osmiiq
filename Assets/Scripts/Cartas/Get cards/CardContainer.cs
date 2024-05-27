
[System.Serializable]
public class CardContainer
{
    public Card[] All;
}
[System.Serializable]
public class Card
{
    public string id;
    public string title;
    public string singleworddescription;
    public string longdescription;
    public string quote;
    public string type;
    public string rarity;
    public string Artistname;
    public int CardYear;
    public string SetID
    {
        get=> id.Substring(0, id.Length - 1);
    }
    public int copies = 1;

    public Card(Card c)
    {
        this.id = c.id;
        this.title = c.title;
        this.singleworddescription = c.singleworddescription;
        this.longdescription = c.longdescription;
        this.quote = c.quote;
        this.type = c.type;
        this.rarity = c.rarity;
        this.copies = c.copies;
        this.CardYear = c.CardYear;
        this.Artistname = c.Artistname;
    }
}

[System.Serializable]
public class UserCardsContainer
{
    public UserCard[] All;
}

[System.Serializable]
public class UserCard
{
    public string CardID;
    public int CardAmount;
}
