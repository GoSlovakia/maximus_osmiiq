using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class TagAttribute : System.Attribute
{
    public Categories Category;
    public string Description;
    public string FileName;
    public bool Removable;

    public TagAttribute(Categories cat, string desc, string f, bool r)
    {
        Category = cat;
        Description = desc;
        FileName = f;
        Removable = r;
    }
}

[System.AttributeUsage(System.AttributeTargets.Field)]
public class TagStatAttribute : System.Attribute
{
    public string StatName;

    public TagStatAttribute(string statName)
    {
        StatName = statName;
    }
}

[System.AttributeUsage(System.AttributeTargets.Field)]
public class ComparisonTagAttribute : System.Attribute
{
    public string Comparison;

    public ComparisonTagAttribute(string comparison)
    {
        Comparison = comparison;
    }
}
