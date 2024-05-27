[System.Serializable]
public class ReferencePalette
{
    public Colour[] colours;
}

[System.Serializable]
public class Colour
{
    public string id;
    public string name;
    public int red;
    public int green;
    public int blue;
}
