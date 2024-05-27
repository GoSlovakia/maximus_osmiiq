using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardReceivedPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject RewardReceiveTemplate;

    [SerializeField]
    private GameObject RewardGrid;

    private void OnEnable()
    {
        for (int i = 0; i < RewardGrid.transform.childCount; i++)
        {
            GameObject temp = Instantiate(RewardReceiveTemplate);
            temp.transform.SetParent(transform);
            temp.transform.GetChild(0).GetComponent<Image>().sprite = RewardGrid.transform.GetChild(i).GetComponent<Image>().sprite;
            temp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = RewardGrid.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        }
    }
}
