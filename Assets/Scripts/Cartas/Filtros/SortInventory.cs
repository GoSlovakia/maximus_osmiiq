using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortInventory : MonoBehaviour
{
    public static GameObject ParentViewPort;
    public void FilterBy(CardField filtro,bool ascending)
    {
        List<Transform> children = new List<Transform>();
        for (int i = ParentViewPort.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = ParentViewPort.transform.GetChild(i);
            children.Add(child);
            child.parent = null;
        }

        switch (filtro)
        {
            case CardField.ID:
                
                break;
            case CardField.TITLE:
                break;
            case CardField.TYPE:
                break;
            case CardField.RARITY:
                break;
            case CardField.SETID:
                break;
        }
    }
}

public enum CardField
{
    ID,
    TITLE,
    TYPE,
    RARITY,
    SETID
}
