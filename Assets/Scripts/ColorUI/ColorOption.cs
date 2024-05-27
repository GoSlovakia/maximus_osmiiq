using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorOption
{
    public Color Bright;
    public Color Main;
    public Color Dark;
    public string Code;

    public ColorOption(string code, Color32 a, Color32 b, Color32 c)
    {
        Bright = a;
        Main = b;
        Dark = c;
        this.Code = code;
    }
}
