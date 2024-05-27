using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreCard : MonoBehaviour
{
    private Card _card;
    public Card card
    {
        get { return _card; }
        set
        {
            _card = value;
            Rarity.text = card.rarity;
            CardName.text = card.title;
        }
    }

    public Sprite Type1;
    public Sprite Type2;
    public TextMeshProUGUI Rarity;
    public TextMeshProUGUI CardName;
    public TextMeshProUGUI Price;

}
