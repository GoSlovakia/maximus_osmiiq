using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardIconType", menuName = "ScriptableObjects/IconForCardType", order = 1)]
public class CardTypeIconSO : ScriptableObject
{
    public CardType Type;
    public Sprite Icon;

}
