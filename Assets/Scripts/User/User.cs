[System.Serializable]
public class LogIn
{
    public string token;
    public User user;
}

[System.Serializable]
public class User
{
    public string token;
    public int id;
    public string first_name;
    public string avatar_file_name;
}

[System.Serializable]
public class UserLevels
{
    public UL[] All;
}

[System.Serializable]
public class UL
{
    public int XP;
    public int UserLevel;
}

[System.Serializable]
public class UserAvatar
{
    public volatile PartFromJSON[] All;
}
[System.Serializable]
public class AllLevelCaps
{
    public LevelCap[] All;
}
[System.Serializable]
public class LevelCap
{
    public int Level;
    public int Total;
    public int XPProgress;
    public int QUIReward;
    public int QIReward;
    public string SetReward;

}


[System.Serializable]
public class PartFromJSON
{
    public string part;

    public PartFromJSON(string part)
    {
        this.part = part;
    }
}

[System.Serializable]
public class UserData
{
    public string email;
    public string password;

    public UserData(string u, string p)
    {
        email = u;
        password = p;
    }
}

[System.Serializable]
public class AllUserStats
{
    public UserStat[] All;
}

[System.Serializable]
public class UserStat
{
    public string StatName;
    public int Value;
    public int setType;
}
