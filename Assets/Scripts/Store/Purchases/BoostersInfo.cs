using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoostersInfo : MonoBehaviour
{
    private Booster booster;
    public int xp;

    public Color BgColor;
    public Sprite PackIMG;

    [SerializeField]
    private TextMeshProUGUI numberOfCards, boosterName, guaranteedCard, priceQI, priceQUI;
 
    public TextMeshProUGUI guaranteedText;
    public Booster Booster
    {
        get => booster; set
        {
            booster = value;

            numberOfCards.text = "x" + Booster.TotalCards.ToString();
            boosterName.text = Booster.name;
            priceQI.text = "x " + Booster.PriceQI.ToString();
            priceQUI.text = Booster.BoosterPriceQUI.ToString();

            if (Booster.LegendaryMin > 0)
            {
                guaranteedCard.text = Booster.LegendaryMin.ToString() + " Legendary guaranteed";
                //ColorUtility.TryParseHtmlString("FF6666", out BgColor);
            }
            else if (Booster.EpicMin > 0)
            {
                guaranteedCard.text = Booster.EpicMin.ToString() + " Epic guaranteed";
                // ColorUtility.TryParseHtmlString("FE8C4B", out BgColor);
            }
            else if (Booster.RareMin > 0)
            {
                guaranteedCard.text = Booster.RareMin.ToString() + " Rare guaranteed";
                // ColorUtility.TryParseHtmlString("49AEED", out BgColor);
            }
            xp = Booster.XPReward;
            guaranteedText = guaranteedCard;
        }
    }
}
