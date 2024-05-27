using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanel : MonoBehaviour
{
    public Button redeem;
    public GameObject rewardsGrid;
    public GameObject rewardPrefab;

    private void Start()
    {
        //apenas uma demonstraçao para mostrar os icons das rewards
        //apagar e alterar quando se souber o que é preciso

        for (int i = 0; i < 6; i++)
        {
            Instantiate(rewardPrefab).transform.SetParent(rewardsGrid.transform);
        }
    }
}
