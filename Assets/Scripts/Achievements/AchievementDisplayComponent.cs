using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AchievementDisplayComponent : MonoBehaviour
{
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI XP;
    public TextMeshProUGUI QUI;
    public TextMeshProUGUI QI;
    public TextMeshProUGUI claimText;
    public GameObject Notif;
    public Image QiLogo;
    public Slider progress;
    public TextMeshProUGUI ProgressDesc;
    private Achievement ach;
    public Button RedeeemBtn;
    public Image bgGlow;
    public GameObject Claimed;

    public Achievement Ach
    {
        get => ach; set
        {
            ach = value;
            Title.text = ach.Name;
            Description.text = ach.DescriptionEN;

            if (ach.XP != 0)
            {
                XP.text = ach.XP.ToString();
            }
            else
            {
                XP.gameObject.SetActive(false);
            }

            if (ach.QUI != 0)
            {
                QUI.text = ach.QUI.ToString();
            }
            else
            {
                QUI.gameObject.SetActive(false);
            }

            if (ach.QI != 0)
            {
                QI.text = ach.QI.ToString();
            }
            else
            {
                QI.gameObject.SetActive(false);
                QiLogo.gameObject.SetActive(false);
            }

            //Meter o progresso
            progress.maxValue = ach.Value;
            progress.value = AchievementTrackerComponent.instance.GetVariable(ach.VarType, ach.Value2);
            //progress.value = AchievementTrackerComponent.instance.GetVariable(ach.VarType, ach.Value2) / ach.Value;
            ProgressDesc.text = AchievementTrackerComponent.instance.GetVariable(ach.VarType, ach.Value2) + " / " + ach.Value;

            //Ver se ta completo

            if (UserSetsComponent.AllUserSets.All.Where(x => x.SetID == ach.ID).Count() != 0)
            {
                SetToComplete();
            }
            else
            if (AchievementTrackerComponent.instance.CheckAchievement(ach))
            {
                SetToRedeemable();
            }

        }
    }


    public void SetToComplete()
    {
        RedeeemBtn.interactable = false;
        ColorBlock cb = new ColorBlock();
        cb.colorMultiplier = 1;
        cb.disabledColor = new Color(29.0f / 255.0f, 9.0f / 255.0f, 37.0f / 255.0f);
        RedeeemBtn.colors = cb;
        RedeeemBtn.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        bgGlow.gameObject.SetActive(false);
        claimText.gameObject.SetActive(false);
        Claimed.SetActive(true);
    }

    public async void SetToRedeemable()
    {
        RedeeemBtn.interactable = true;
        if (NotificationsAchJourney.instance.NotifAch.All.Where(x => x.achid == ach.ID).Count() > 0)
        {
            Debug.Log("Notif");
            RedeeemBtn.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            bgGlow.gameObject.SetActive(true);
            await NotificationsAchJourney.instance.RemoveNotificationAch(ach.ID);
            Notif.SetActive(true);
        }
        claimText.gameObject.SetActive(true);
    }

    public void Redeem()
    {
        UnityMainThread.wkr.AddJob(async () =>
        {

            await UserSetsComponent.UnlockSetForUser(ach.ID);
            await UserSetsComponent.GiveAchievementReward(ach.ID);
            await UserSetsComponent.GetUserSets();
            await UserLevelComponent.GetUserLevel();
            claimText.text = "Claimed";
            claimText.color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);

            UserLevelDisplay.instance.UpdateDisplay();
            GetCurrency.instance.UpdateUserBalance();
            SetToComplete();
        });


    }


}
