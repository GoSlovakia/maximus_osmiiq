using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class Enums
{
  public static string GetCategoryDescription(this Categories value)
  {
    FieldInfo fi = value.GetType().GetField(value.ToString());

    DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

    if (attributes != null && attributes.Any())
    {
      return attributes.First().Description;
    }

    return value.ToString();
  }
  public static string GetCardCategory(this CardType value)
  {
    FieldInfo fi = value.GetType().GetField(value.ToString());

    DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

    if (attributes != null && attributes.Any())
    {
      return attributes.First().Description;
    }

    return "No Description";
  }

  public static string GetDescription(this AccType este)
  {
    FieldInfo fi = este.GetType().GetField(este.ToString());
    TagAttribute attr = fi.GetCustomAttribute(typeof(TagAttribute)) as TagAttribute;

    if (attr != null)
    {
      return attr.Description;
    }

    return "No Description";
  }

  public static string GetFilenameRequired(this AccType este)
  {
    FieldInfo fi = este.GetType().GetField(este.ToString());
    TagAttribute attr = fi.GetCustomAttribute(typeof(TagAttribute)) as TagAttribute;

    if (attr != null)
    {
      return attr.FileName;
    }

    return "No FileName found";
  }

  public static bool GetRemovable(this AccType este)
  {
    FieldInfo fi = este.GetType().GetField(este.ToString());
    TagAttribute attr = fi.GetCustomAttribute(typeof(TagAttribute)) as TagAttribute;

    if (attr != null)
    {
      return attr.Removable;
    }

    return true;
  }


  public static Categories GetCategory(this AccType este)
  {
    FieldInfo fi = este.GetType().GetField(este.ToString());
    TagAttribute attr = fi.GetCustomAttribute(typeof(TagAttribute)) as TagAttribute;

    if (attr != null)
    {
      return attr.Category;
    }

    return Categories.NOT_SET;
  }
  public static string GetVarName(this VariableType este)
  {
    FieldInfo fi = este.GetType().GetField(este.ToString());
    TagStatAttribute attr = fi.GetCustomAttribute(typeof(TagStatAttribute)) as TagStatAttribute;

    if (attr != null)
    {
      return attr.StatName;
    }

    return "No FileName found";
  }

  public static string GetComparison(this ComparisonEnum este)
  {
    FieldInfo fi = este.GetType().GetField(este.ToString());
    ComparisonTagAttribute attr = fi.GetCustomAttribute(typeof(ComparisonTagAttribute)) as ComparisonTagAttribute;

    if (attr != null)
    {
      return attr.Comparison;
    }

    return "No FileName found";
  }
}


public enum AccType
{
  [Tag(Categories.HEAD, "Crown", "HTC", true)]
  HTC,
  [Tag(Categories.HEAD, "Head Accessory", "HTF", true)]
  HTF,
  [Tag(Categories.HEAD, "Hair", "HTH", true)]
  HTH,
  [Tag(Categories.HEAD, "Eyebrows", "HEE", true)]
  HEE,
  [Tag(Categories.HEAD, "Glasses", "HEF", true)]
  HEF,
  [Tag(Categories.HEAD, "Eyes", "HEY", false)]
  HEY,
  [Tag(Categories.HEAD, "Head Shape", "FHT", false)]
  FHT,
  [Tag(Categories.HEAD, "Nose", "HFN", true)]
  HFN,
  [Tag(Categories.HEAD, "Feature", "HFF", true)]
  HFF,
  [Tag(Categories.HEAD, "Sides", "HFS", true)]
  HFS,
  [Tag(Categories.JAW, "Mouth Feature", "HJF", true)]
  HJF,
  [Tag(Categories.JAW, "Mouth", "HJM", true)]
  HJM,
  [Tag(Categories.JAW, "Chin", "HJC", true)]
  HJC,
  [Tag(Categories.JAW, "Jaw", "FHJ", false)]
  FHJ,
  [Tag(Categories.HANDS, "Left Hand", "FHL", true)]
  FHL,
  [Tag(Categories.HANDS, "Right Hand", "FHR", true)]
  FHR,
  [Tag(Categories.BODY, "Body Back", "BDB", true)]
  BDB,
  [Tag(Categories.BODY, "Tail", "BDT", true)]
  BDT,
  [Tag(Categories.BODY, "Body Feature", "BDF", true)]
  BDF,
  [Tag(Categories.BODY, "Body Cover", "BDC", true)]
  BDC,
  [Tag(Categories.BODY, "Body Shape", "FBD", false)]
  FBD

}

public enum Categories
{
  [Description("Head")]
  HEAD,
  [Description("Hands")]
  HANDS,
  [Description("Jaw")]
  JAW,
  [Description("Body")]
  BODY,
  [Description("Not Set")]
  NOT_SET
}

public enum Rarity
{
  [Description("Common")]
  C,
  [Description("Rare")]
  R,
  [Description("Epic")]
  E,
  [Description("Legendary")]
  L
}

public enum Sets
{
  [Description("Language and literature")]
  LaL,
  [Description("History and geography")]
  HaG,
  [Description("Folklore, religion and myths")]
  FRM,
  [Description("Physics, chemistry and biology")]
  PCB,
  [Description("Culture and arts")]
  CaA,
  [Description("Maths and computation")]
  MaC,
  [Description("Philosphy and politics")]
  PaP
}

public enum CardType
{
  [Description("Persona")]
  PERSONA,
  [Description("Locale")]
  LOCALE,
  [Description("Artifact")]
  ARTIFACT,
  [Description("Essence")]
  ESSENCE,
  [Description("Critter")]
  CRITTER,
  [Description("Happening")]
  HAPPENING
}

public enum TypeOfCurrency
{
  [Description("Quiids")]
  QUI,
  [Description("Qiis")]
  QI,
}

public enum DirectoryKey
{
  [Description("Acess avatar svg folder")]
  SVG_FOLDER,
  [Description("Main url for web request")]
  SERVICE_URL,
  [Description("Acess folder with bundles for chaching")]
  BUNDLES_FOLDER,
  [Description("Url for login and password")]
  URL,
  [Description("Url for maximus website")]
  MAXIMUS_URL,
  [Description("Acess to folder with the localization for the cards")]
  CARDS_LOCALE,
  [Description("Acess to folder with the localization for the missions")]
  MISSIONS_LOCALE,
}