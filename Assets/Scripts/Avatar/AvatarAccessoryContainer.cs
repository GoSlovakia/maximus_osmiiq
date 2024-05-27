using JetBrains.Annotations;

[System.Serializable]
public class AvatarAccessoryContainer
{
    public AvatarAcc[] All;
}
[System.Serializable]
public class AvatarAcc
{
    public string id;
    public AccType avatarset;
    public string season;
    public int number;
    public string name;
    public string description;

    public AvatarAcc(string id, AccType avatarset, string season, int number, string name, string description)
    {
        this.id = id;
        this.avatarset = avatarset;
        this.season = season;
        this.number = number;
        this.name = name;
        this.description = description;
    }
    public AvatarAcc(AvatarAcc other)
    {
        id = other.id;
        avatarset = other.avatarset;
        season = other.season;
        number = other.number;
        name = other.name;
        description = other.description;

    }
}
[System.Serializable]
public class AvatarAccUnlocksContainer
{
    public AvatarUnlocks[] All;
}
[System.Serializable]
public class AvatarUnlocks
{
    public string ID;
    public string PartCode;
}
