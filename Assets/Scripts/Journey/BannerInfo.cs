using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

public class BannerInfo : MonoBehaviour
{
    public static CancellationTokenSource cancel=new CancellationTokenSource();
    public Button button;
    public TextMeshProUGUI QUI;
    public TextMeshProUGUI QI;
    public TextMeshProUGUI AmountOfAvatarParts;
    public TextMeshProUGUI AmountOfColors;
    public Image glow, partIcon, colorIcon, qii, qiid, rewardPanel;
    public GameObject notif;
    public RawImage rewardImage;
    public string setID;
    public int level;

    public JourneyRewardsPanel rewardsPanel;
    public async void ChangeFontColor()
    {
        button = GetComponent<Button>();
        if (button.interactable)
        {
            QI.color = Color.white;
            QUI.color = Color.white;
            AmountOfAvatarParts.color = Color.white;
            AmountOfColors.color = Color.white;
            qii.color = Color.white;
            qiid.color = Color.white;
            partIcon.color = Color.white;
            colorIcon.color = Color.white;

            while (NotificationsAchJourney.instance.loaded == false)
            {
                await Task.Yield();
            }

            if (NotificationsAchJourney.instance.NotifJourney.All.Where(x => x.setid == setID).Count() > 0)
            {
                glow.gameObject.SetActive(true);
                notif.SetActive(true);
                await NotificationsAchJourney.instance.RemoveNotificationJourney(setID);
            }
        }
        else
        {
            QI.color = new Color(129, 64, 152);
            QUI.color = new Color(129, 64, 152);
            AmountOfAvatarParts.color = new Color(129, 64, 152);
            AmountOfColors.color = new Color(129, 64, 152);
            qii.color = new Color(129, 64, 152);
            qiid.color = new Color(129, 64, 152);
            partIcon.color = new Color(129, 64, 152);
            colorIcon.color = new Color(129, 64, 152);
            glow.gameObject.SetActive(false);
        }
    }

    public async void ReedemSet()
    {
        ConfigureRewardsPanel();
        button.interactable = false;
        ChangeFontColor();
        await UserSetsComponent.UnlockSetForUser(setID);
        await UserSetsComponent.GiveLevelReward(level.ToString());
        SetupJourney.lvlsIcons[level - 1].GetComponent<Image>().color = Color.white;
        SetupJourney.lvlsIcons[level - 1].GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
        SetupJourney.lvlsIcons[level - 1].GetChild(1).gameObject.SetActive(false);
        await UserSetsComponent.GetUserSets();
        GetCurrency.instance.UpdateUserBalance();
    }

    private async void ConfigureRewardsPanel()
    {
        List<AvatarUnlocks> avatarUnlocks = new List<AvatarUnlocks>();

        foreach (AvatarUnlocks avatar in GetJourneyLevels.journeyAvatarParts.All)
        {
            if (avatar.ID == setID)
                avatarUnlocks.Add(avatar);
        }

        foreach (AvatarUnlocks avatar in avatarUnlocks)
        {
            string type = string.Empty, name = string.Empty;
            foreach (var item in AvatarAccessoriesManager.AvatarAccs.All)
            {
                if (item.id == avatar.PartCode)
                {
                    name = item.name;
                    type = item.avatarset.GetDescription();
                    await SetAvatarNotif(item.id);
                    break;
                }
            }
            rewardsPanel.SetupAvatarBanners(avatar.PartCode, type, name);
        }
        bool hasColor = false;
        foreach (Coulourset color in ColourManager.colourSets.couloursets)
        {
            if (color.SetCode == setID)
            {
                hasColor = true;
                await SetColorNotif(color.code);
                rewardsPanel.SetupColordBanners("color", color.ColorName, new Color((color.red + 0.0f) / 255.0f, (color.green + 0.0f) / 255.0f, (color.blue + 0.0f) / 255.0f));
                break;
            }
        }

        if (!hasColor)
            rewardsPanel.panelLayout.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 115 * (avatarUnlocks.Count + 2));
        else
            rewardsPanel.panelLayout.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 115 * (avatarUnlocks.Count + 3));

        rewardsPanel.SetupQuiidsBanner(QUI);
        if (int.Parse(QI.text) != 0)
            rewardsPanel.SetupQiisBanner(QI);

        rewardsPanel.gameObject.SetActive(true);
    }

    public async Task SetAvatarNotif(string partid)
    {
        //Debug.Log("Getting XtermPallet Async");
        string Job = "Setting a notification on the avatar scene";
        LoadingManager.instance.EnqueueLoad(Job);

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserAvatarNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id + "&partid=" + partid);
        var req = www.SendWebRequest();

        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }
        //Debug.Log("Getting XtermPallet Async Finished");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);

        }
        else
        {
            Debug.Log("Success");

        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public async Task SetColorNotif(string colid)
    {
        //Debug.Log("Getting XtermPallet Async");
        string Job = "Setting a notification on the avatar scene";
        LoadingManager.instance.EnqueueLoad(Job);

        using UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserColorNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id + "&colid=" + colid);
        var req = www.SendWebRequest();

        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }
        //Debug.Log("Getting XtermPallet Async Finished");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);

        }
        else
        {
            Debug.Log("Success");

        }
        LoadingManager.instance.DequeueLoad(Job);
    }
}
