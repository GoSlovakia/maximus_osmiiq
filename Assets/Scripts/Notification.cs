[System.Serializable]
public class NotificationsAvatar
{
    public NotificationAvatar[] All;
}
[System.Serializable]
public class NotificationAvatar
{
    public string user;
    public string part;
}
[System.Serializable]
public class NotificationsColors
{
    public NotificationColor[] All;
}
[System.Serializable]
public class NotificationColor
{
    public string user;
    public string colid;
}
[System.Serializable]
public class NotificationAchs
{
    public NotificationAch[] All;
}
[System.Serializable]
public class NotificationAch
{
    public string user;
    public string achid;
}
[System.Serializable]
public class NotificationsJourney
{
    public NotificationJourney[] All;
}
[System.Serializable]
public class NotificationJourney
{
    public string user;
    public string setid;
}
[System.Serializable]
public class NotificationsCards
{
    public NotificationCard[] All;
}
[System.Serializable]
public class NotificationCard
{
    public string user;
    public string cardid;
}

[System.Serializable]
public class NotificationsGeneral
{
    public UserNotification[] All;
}
[System.Serializable]
public class UserNotification
{
    public int user;
    public int cards;
    public int avatarparts;
    public int colors;
    public int journey;
    public int achievements;

}
