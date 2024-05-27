using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowRewards : MonoBehaviour
{
    public static ShowRewards instance;
    [SerializeField]
    private RewardScreenDisplayPartComponent QiisDisplay;
    [SerializeField]
    private RewardScreenDisplayPartComponent QuiidsDisplay;
    [SerializeField]
    private RewardScreenDisplayPartComponent XPDisplay;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            Debug.Log("Reward screen set");
            gameObject.SetActive(false);

        }

    }


    public void ShowRewardsPanel(int xp, int qi, int qui)
    {
        gameObject.SetActive(true);
        QiisDisplay.AmountText.text = qi.ToString();
        QuiidsDisplay.AmountText.text = qui.ToString();
        XPDisplay.AmountText.text = xp.ToString();

        QiisDisplay.CurrencyText.text = qi <= 1 ? QiisDisplay.currencyType.ToDescription().Substring(0, QiisDisplay.currencyType.ToDescription().Length - 1) : QiisDisplay.currencyType.ToDescription();
        QuiidsDisplay.CurrencyText.text = qi <= 1 ? QuiidsDisplay.currencyType.ToDescription().Substring(0, QuiidsDisplay.currencyType.ToDescription().Length - 1) : QuiidsDisplay.currencyType.ToDescription();



        QuiidsDisplay.gameObject.SetActive(qui > 0);
        QiisDisplay.gameObject.SetActive(qi > 0);
        XPDisplay.gameObject.SetActive(xp > 0);
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
