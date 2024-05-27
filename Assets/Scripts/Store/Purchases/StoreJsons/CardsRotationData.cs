[System.Serializable]
public class CardsRotationData
{
    public CardRotation[] cardsRotations;
}

[System.Serializable]
public class CardRotation
{
    public string id;
    public string title;
    public int type;
    public string rarity;
    public int priceQUI;
}

