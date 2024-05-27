
[System.Serializable]
public class CardSetContainer
{
    public CardSet[] All;
}
[System.Serializable]
public class CardSet
{
    public string id;
    public string name;
    public int domain;
    public string type;
    public string subset;
}

[System.Serializable]
public class UserSetsContainer
{
    public UserSets[] All;
}

[System.Serializable]
public class UserSets
{
    public string SetID;
}
