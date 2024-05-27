[System.Serializable]
public class BoostersData
{
    public Booster[] boosters;
}

[System.Serializable]
public class Booster
{
    public int BoosterID;
    public string name;
    public int BoosterPriceQUI;
    public int PriceQI;
    public int CommonMin;
    public int CommonMax;
    public int RareMin;
    public int RareMax;
    public int EpicMin;
    public int EpicMax;
    public int LegendaryMin;
    public int LegendaryMax;
    public int TotalCards;
    public int XPReward;
}
