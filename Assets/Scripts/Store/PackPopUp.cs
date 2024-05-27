using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PackPopUp : MonoBehaviour
{
    //public TypeOfCurrency currency;
    private BoostersInfo boosterInfo;


    [SerializeField]
    private Image Bg;

    //[SerializeField]
    //private Button QUI, QI;

    [SerializeField]
    private Image PackIcon;

    [SerializeField]
    private ViewCards viewCardsPanel;



    [SerializeField]
    private TextMeshProUGUI quiis, quiids, boosterName, commonMin, commonMax, rareMin, rareMax, epicMin, epicMax, LegMin, LegMax, priceQUI, priceQI, totalCards, guaranted;

    public void UpdatePopUp(BoostersInfo boosters)
    {
        boosterInfo = boosters;
        boosterName.text = boosters.Booster.name;
        commonMin.text = boosters.Booster.CommonMin.ToString();
        commonMax.text = boosters.Booster.CommonMax.ToString();
        rareMin.text = boosters.Booster.RareMin.ToString();
        rareMax.text = boosters.Booster.RareMax.ToString();
        epicMin.text = boosters.Booster.EpicMin.ToString();
        epicMax.text = boosters.Booster.EpicMax.ToString();
        LegMin.text = boosters.Booster.LegendaryMin.ToString();
        LegMax.text = boosters.Booster.LegendaryMax.ToString();
        priceQUI.text = boosters.Booster.BoosterPriceQUI.ToString();
        priceQI.text = boosters.Booster.PriceQI.ToString();
        Bg.color = boosters.BgColor;
        PackIcon.sprite = boosters.PackIMG;
        guaranted.text = boosters.guaranteedText.text;

        //if (boosters.Booster.PriceQI == 0)
        //{
        //    QI.gameObject.SetActive(false);
        //    QI.interactable = true;
        //    QUI.interactable = false;
        //    //PayWithQUI();
        //}
        //else
        //{
        //    QI.gameObject.SetActive(true);
        //}
    }

    //public void PayWithQUI()
    //{
    //    currency = TypeOfCurrency.QUI;
    //}
    //public void PayWithQI()
    //{
    //    if (booster.Booster.PriceQI > 0)
    //        currency = TypeOfCurrency.QI;
    //    else
    //        currency = TypeOfCurrency.QUI;
    //}

    public async void BuyPack()
    {
        await Purchase();
    }

    public CancellationTokenSource cancel = new CancellationTokenSource();

    private async Task Purchase()
    {
        string Job = "Purchasing Pack";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "buyBooster.php?userID=" + UserLogin.instance.LogInInfo.user.id + "&boosterID=" + boosterInfo.Booster.BoosterID);

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                LoadingManager.instance.DequeueLoad(Job);
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            LoadingManager.instance.DequeueLoad(Job);
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Purchase failed " + www.error);
            LoadingManager.instance.DequeueLoad(Job);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.downloadHandler.text == "Not enough currency!")
            {

                LoadingManager.instance.DequeueLoad(Job);
                return;
            }
            BoosterCardsData booster = JsonUtility.FromJson<BoosterCardsData>("{\"boosterCards\":" + www.downloadHandler.text + "}");


            await AchievementTrackerComponent.instance.AddToVariable(VariableType.BoostersBought, 1);
            for (int i = 0; i < booster.boosterCards.Length; i++)
            {
                await AddCard(booster.boosterCards[i].id);
                switch (booster.boosterCards[i].rarity)
                {
                    case "C":
                        await AchievementTrackerComponent.instance.AddToVariable(VariableType.CommonCards, 1);
                        break;
                    case "R":
                        await AchievementTrackerComponent.instance.AddToVariable(VariableType.RareCards, 1);
                        break;
                    case "E":
                        await AchievementTrackerComponent.instance.AddToVariable(VariableType.EpicCards, 1);
                        break;
                    case "L":
                        await AchievementTrackerComponent.instance.AddToVariable(VariableType.LegendaryCards, 1);
                        break;
                    default:
                        break;
                }

                if (CardManager.UserCards.Where(x => x.id == booster.boosterCards[i].id).Count() == 0)
                {
                    await AddNotificationCard(booster.boosterCards[i].id);
                }

            }
            await LoadCards.GetUserCards();
            if (CardManager.AllCardsFromSeasonRemaining(0) == 0 && AchievementTrackerComponent.instance.GetVariable(VariableType.AllCardsFromSeason, 0) == 0)
            {
                await AchievementTrackerComponent.instance.SetVariable(VariableType.AllCardsFromSeason, 1, 0);
                Debug.Log("Cards from season 0 remaining " + CardManager.AllCardsFromSeasonRemaining(0));
            }
            else
            {
                Debug.Log("Cards from season 0 remaining " + CardManager.AllCardsFromSeasonRemaining(0));
            }

            await AchievementTrackerComponent.instance.SetVariable(VariableType.UniqueCards, CardManager.UniqueCards);
            await AchievementTrackerComponent.instance.SetVariable(VariableType.DuplicateCards, CardManager.DuplicateCards);

            await GetCurrency.GetUserBalance(quiids, quiis);
            await UserLevelComponent.AddXP(boosterInfo.xp);
            viewCardsPanel.gameObject.SetActive(true);
            viewCardsPanel.boosterCards = booster;
            viewCardsPanel.SideCards.SetActiveCards(booster.boosterCards.Length - 1);
            //viewCardsPanel.UpdateCard(await GetJPEG.GetTexture(booster.boosterCards[0].id));
            viewCardsPanel.UpdateCard(AssetBundleCacher.Instance.cards[booster.boosterCards[0].id].texture);
            gameObject.SetActive(false);

            foreach (var este in FindObjectsOfType<CardSellInfo>())
            {
                este.Owned();
            }
            LoadingManager.instance.DequeueLoad(Job);
        }

    }

    private async Task AddNotificationCard(string cardID)
    {
        string Job = "Adding a notification for the card inventory";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "setUserCardNotifs.php?user=" + UserLogin.instance.LogInInfo.user.id + "&cardid=" + cardID);

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
            Debug.LogError("Purchase failed " + www.error);
        }
        else
        {
            Debug.Log("Success");
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    private async Task AddCard(string cardID)
    {
        string Job = "Adding Card to the User Inventory";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "setUserCards.php?user=" + UserLogin.instance.LogInInfo.user.id + "&cardID=" + cardID + "&amount=1");

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
            Debug.LogError("Purchase failed " + www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);

        }
        LoadingManager.instance.DequeueLoad(Job);
    }
}
