
[System.Serializable]
public class UserColorsArray
{
    public UserColors[] All;
}

[System.Serializable]
public class UserColors
{
    public string PrimaryColor;
    public string SecondaryColor;

    public UserColors(string primaryColor, string secondaryColor)
    {
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
    }
}
