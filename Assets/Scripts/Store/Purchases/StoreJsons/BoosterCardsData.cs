[System.Serializable]
public class BoosterCardsData
{
    public BoosterCards[] boosterCards;
}

[System.Serializable]
public class BoosterCards
{
    public string id;
    public string title;
    public string singleworddescription;
    public string rarity;
    public string set;
    public int type;
}

