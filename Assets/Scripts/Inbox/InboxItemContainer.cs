using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InboxItemContainer : MonoBehaviour
{
    private InboxItem item;

    public InboxItem Item
    {
        get => item; set
        {
            item = value;

            Title.text = "Reward for assigment " + item.Title;
            if (item.XPAmount != 0)
            {
                XP.text = item.XPAmount.ToString();
            }
            else
            {
                XP.transform.parent.gameObject.SetActive(false);
            }

            if (item.QUIAmount != 0)
            {
                QUI.text = item.QUIAmount.ToString();
            }
            else
            {
                QUI.transform.parent.gameObject.SetActive(false);
            }

            if (Item.QIAmount != 0)
            {
                QI.text = item.QIAmount.ToString();
            }
            else
            {
                QI.transform.parent.gameObject.SetActive(false);
            }

            Date.text = item.Date;

            if (item.ClaimedVal)
            {
                AlreadyRedeemed();
                btn.interactable = false;
            }

        }
    }
    [SerializeField]
    private TextMeshProUGUI Title;

    [Header("Rewards")]
    [SerializeField]
    private TextMeshProUGUI XP;
    [SerializeField]
    private TextMeshProUGUI QUI;
    [SerializeField]
    private TextMeshProUGUI QI;
    [SerializeField]
    private TextMeshProUGUI Date;

    [Header("Icons")]
    [SerializeField]
    private Image XPIcon;
    [SerializeField]
    private Image QUIIcon;
    [SerializeField]
    private Image QIIcon;

    [SerializeField]
    private Sprite QIIconNoGlow;

    [Header("Claimed Texts")]
    [SerializeField]
    private GameObject ClaimedTxt;
    [SerializeField]
    private GameObject ReadyToBeClaimedTxt;

    public CancellationTokenSource cancel = new CancellationTokenSource();


    [Header("Misc")]
    [SerializeField]
    private Button btn;


    public async void Redeem()
    {
        await RedeemItem();
        btn.interactable = false;
        InboxGenerator.instance.InboxList.All.Where(x => x.Title == item.Title && x.Date == item.Date).SingleOrDefault().Claimed = 1;
        // InboxGenerator.instance.GetInbox();
        AlreadyRedeemed();
        ShowRewards.instance.ShowRewardsPanel(item.XPAmount, item.QIAmount, item.QUIAmount);
    }

    private void AlreadyRedeemed()
    {
        ReadyToBeClaimedTxt.SetActive(false);
        ClaimedTxt.SetActive(true);
        var col = new Color();
        ColorUtility.TryParseHtmlString("#814098", out col);
        Title.color = col;
        XP.color = col;
        QUI.color = col;
        QI.color = col;
        Date.color = col;
        XPIcon.color = col;
        QUIIcon.color = col;
        QIIcon.color = col;
        QIIcon.sprite = QIIconNoGlow;

    }

    public async Task RedeemItem()
    {
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "RedeemUserInboxItem.php?user=" + UserLogin.instance.LogInInfo.user.id + "&Title=" + Item.Title);

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

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error " + www.result);
            return;
        }
        else
        {
            Debug.Log("Redeemed");
            GetCurrency.instance.UpdateUserBalance();
            await UserLevelComponent.GetUserLevel();
            UserLevelDisplay.instance.UpdateDisplay();
        }
    }

}
