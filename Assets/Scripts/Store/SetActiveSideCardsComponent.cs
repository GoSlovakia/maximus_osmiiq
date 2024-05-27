using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveSideCardsComponent : MonoBehaviour
{

    public void SetActiveCards(int amount)
    {
        int cur = amount;
        foreach (Transform este in transform)
        {
            este.gameObject.SetActive(cur > 0);
            cur--;
        }
    }
}
