[System.Serializable]
public class XPProgressionData
{
    public Levels[] Levels;
}


[System.Serializable]
public class Levels
{
    public int Level;
    public int Total;
    public int XPProgress;
    public int QUIReward;
    public int QIReward;
    public string SetReward;
}
