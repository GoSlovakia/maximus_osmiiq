[System.Serializable]
public class AvatarComponent
{
    public Part[] Parts;
}

[System.Serializable]
public class Part
{
    public string code;
    public string name;
    public AccType spritegroup;
    public bool allowpattern;
    public bool contributetooutline;
    public int orderinlayer;
    public string description;
}
