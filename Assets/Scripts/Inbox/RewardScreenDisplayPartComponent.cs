using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class RewardScreenDisplayPartComponent : MonoBehaviour
{
    public CurrencyType currencyType;
    public TextMeshProUGUI AmountText;
    public TextMeshProUGUI CurrencyText;




}

public static class CurrencyTypeExtensionHelper
{
    public static string ToDescription(this Enum value)
    {
        DescriptionAttribute[] da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
        return da.Length > 0 ? da[0].Description : value.ToString();
    }
}

public enum CurrencyType
{
    [Description("Qiis")]
    QIIS,
    [Description("Quiids")]
    QUIIDS,
    [Description("XP")]
    XP
}