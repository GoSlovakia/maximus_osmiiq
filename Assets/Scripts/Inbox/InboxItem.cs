
using System;

[System.Serializable]
public class InboxList
{
    public InboxItem[] All;
}
[System.Serializable]
public class InboxItem
{
    public string Title;
    public int XPAmount;
    public int QIAmount;
    public int QUIAmount;
    public string Date;
    public int Claimed;
    public bool ClaimedVal { get => Claimed == 1; }
}
