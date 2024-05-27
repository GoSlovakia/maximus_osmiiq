using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CardsSetComponent : MonoBehaviour
{
  public static CancellationTokenSource cancel = new CancellationTokenSource();
  private List<CardFromSetContainer> All = new List<CardFromSetContainer>();
  public Button Unlockbtn;
  public TextMeshProUGUI SetNameTMP;
  public RawImage Icon;
  [SerializeField]
  private TextMeshProUGUI claimText;
  [SerializeField]
  private GameObject Claimed;
  [SerializeField]
  private Image avatarIcon, donutIcon, glowButton, glowBanner;
  [SerializeField]
  private TextMeshProUGUI avatarText, quii;

  [SerializeField]
  private Transform MinicardPanel;


  [SerializeField]
  private GameObject minicard;


  [SerializeField]
  LocalizeStringEvent setNameLocale;

  [SerializeField]
  LocalizedString[] setNamesLocale;

  private static int _processes;
  private static int processes
  {
    get => _processes;
    set
    {
      _processes = value;

      if (_processes > 0)
        Debug.LogWarning("Saving! Processes remaning " + _processes);
      else
      {
        Debug.Log("Its now save to quit");
        _processes = 0;
      }
      allowquit = processes == 0;

    }
  }

  private static bool allowquit = true;

  public CardSet set;

  private bool _redeemed = false;

  public bool Redeemed
  {
    get { return _redeemed; }
    set
    {
      _redeemed = value;
      if (value)
      {
        Unlockbtn.interactable = !value;
        /* foreach (var este in All)
         {
             ColorBlock rd = new ColorBlock
             {
                 disabledColor = Color.blue,
                 normalColor = Color.red,
                 selectedColor = Color.red,
                 pressedColor = Color.red

             };
             rd.colorMultiplier = 1;
             //Debug.Log("Changing Colors");
             este.GetComponent<Button>().colors = rd;
         }

         Unlockbtn.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Set Complete!";*/
      }

    }
  }

  public bool CanRedeem
  {
    //get => All.Where(x => x.Card.copies == 0).Count() == 0;
    get => All.Where(x => x.btn.interactable).Count() >= 5;
  }

  public CardSet Set
  {
    get => set; set
    {
      set = value;
      SetNameTMP.text = set.name;
    }
  }

  private void Awake()
  {
    Unlockbtn.interactable = CanRedeem;

    Application.wantsToQuit += Application_wantsToQuit;
  }

  private bool Application_wantsToQuit()
  {
    return allowquit;
  }

  public void UpdateSetDisplay()
  {
    All = transform.GetComponentsInChildren<CardFromSetContainer>().ToList();
    // Debug.Log(transform.GetComponentsInChildren<CardFromSetContainer>().ToList().Count + " List size");

    if (UserSetsComponent.AllUserSets.All.Where(x => x.SetID == Set.id).Count() > 0)
    {
      Redeemed = true;
      //Debug.Log("Set Already redeemed");
    }
    Unlockbtn.interactable = CanRedeem;
    ChangeReddeemButton();
    SetUpMiniCards();
    ClaimText();
    GetRewards();
    GetIcon();
    UpdateSetName();
  }

  public void UpdateSetName()
  {
    switch (set.id)
    {
      case "CA001":
        setNameLocale.StringReference = setNamesLocale[0];
        break;
      case "CA002":
        setNameLocale.StringReference = setNamesLocale[1];
        break;
      case "FR001":
        setNameLocale.StringReference = setNamesLocale[2];
        break;
      case "FR002":
        setNameLocale.StringReference = setNamesLocale[3];
        break;
      case "HG001":
        setNameLocale.StringReference = setNamesLocale[4];
        break;
      case "HG002":
        setNameLocale.StringReference = setNamesLocale[5];
        break;
      case "HG003":
        setNameLocale.StringReference = setNamesLocale[6];
        break;
      case "HG004":
        setNameLocale.StringReference = setNamesLocale[7];
        break;
      case "HG005":
        setNameLocale.StringReference = setNamesLocale[8];
        break;
      case "LL001":
        setNameLocale.StringReference = setNamesLocale[9];
        break;
      case "LL002":
        setNameLocale.StringReference = setNamesLocale[10];
        break;
      case "LL003":
        setNameLocale.StringReference = setNamesLocale[11];
        break;
      case "LL004":
        setNameLocale.StringReference = setNamesLocale[12];
        break;
      case "LL005":
        setNameLocale.StringReference = setNamesLocale[13];
        break;
      case "MC001":
        setNameLocale.StringReference = setNamesLocale[14];
        break;
      case "PC001":
        setNameLocale.StringReference = setNamesLocale[15];
        break;
      case "PC002":
        setNameLocale.StringReference = setNamesLocale[16];
        break;
      case "PP002":
        setNameLocale.StringReference = setNamesLocale[17];
        break;
    }
  }

  public void GetIcon()
  {
    //Icon.texture = await GetJPEG.GetDomainTexture(set.id[0] + "" + set.id[1]);
    Icon.texture = AssetBundleCacher.Instance.cardsdomains[set.id[0] + "" + set.id[1]].texture;
  }

  private async void GetRewards()
  {
    await GetAllAvatarUnlocks();
    int count = 0;
    foreach (AvatarUnlocks avatar in AvatarAccessoriesManager.AvatarAccsUnlocks.All)
    {
      if (Set.id == avatar.ID)
        count++;
    }
    avatarText.text = "x" + count.ToString();
  }

  public async Task GetAllAvatarUnlocks()
  {
    UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getAvatarPartsUnlocks.php");
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
      Debug.Log("Get all avatar accessories failed " + www.error);
    }
    else
    {
      // Show results as text
      //Debug.Log(www.downloadHandler.text);
      AvatarAccessoriesManager.AvatarAccsUnlocksJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
    }
  }

  private void ClaimText()
  {
    if (!Unlockbtn.interactable && !Redeemed)
    {
      claimText.gameObject.SetActive(false);
      glowBanner.gameObject.SetActive(false);
      glowButton.gameObject.SetActive(false);
    }
    else if (!Redeemed)
    {
      claimText.text = "Ready to claim";
      claimText.color = new Color(255, 204.0f / 255.0f, 49.0f / 255.0f);
      glowBanner.gameObject.SetActive(true);
      glowButton.gameObject.SetActive(true);
    }
    else if (Redeemed)
    {
      claimText.gameObject.SetActive(false);
      Claimed.SetActive(true);
      glowBanner.gameObject.SetActive(false);
      glowButton.gameObject.SetActive(false);
      avatarIcon.color = Color.black;
      donutIcon.color = Color.black;
      avatarText.color = Color.black;
      quii.color = Color.black;
      Unlockbtn.interactable = false;
    }
  }

  private void ChangeReddeemButton()
  {
    if (Unlockbtn.interactable)
    {
      avatarIcon.color = Color.black;
      avatarText.color = Color.black;
      donutIcon.color = Color.black;
      quii.color = Color.black;
    }
    else
    {
      avatarIcon.color = Color.white;
      avatarText.color = Color.white;
      donutIcon.color = Color.white;
      quii.color = Color.white;
    }
  }

  private void SetUpMiniCards()
  {

    foreach (CardFromSetContainer card in All)
    {
      GameObject temp = Instantiate(minicard, MinicardPanel);
      if (Unlockbtn.interactable)
      {
        temp.GetComponent<Image>().color = new Color(228.0f / 255.0f, 78.0f / 255.0f, 141.0f / 255.0f);
        temp.transform.GetChild(0).GetComponent<Image>().color = Color.white;
      }
      else
      {
        if (card.btn.interactable)
        {
          temp.GetComponent<Image>().color = new Color(228.0f / 255.0f, 78.0f / 255.0f, 141.0f / 255.0f);
          temp.transform.GetChild(0).GetComponent<Image>().color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
        }
        else
        {
          card.border.color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
          temp.GetComponent<Image>().color = new Color(29.0f / 255.0f, 9.0f / 255.0f, 37.0f / 255.0f);
          temp.transform.GetChild(0).GetComponent<Image>().color = new Color(129.0f / 255.0f, 64.0f / 255.0f, 152.0f / 255.0f);
        }
      }
    }
  }

  //Metodo para usar no botao
  public async void RedeemItem()
  {
    if (CanRedeem)
    {
      //fazer aqui a cena de desloquear o item
      await UnlockSetForUser();
      await AchievementTrackerComponent.instance.AddToVariable(VariableType.AllSetsCompleted, 1);
      await AchievementTrackerComponent.instance.AddToVariable(VariableType.SetsCompletedDomain, 1, Set.domain);

      if (cancel.Token.IsCancellationRequested)
      {
        Debug.Log("Canceled");
        return;
      }
      Redeemed = true;
      //panel.SetActive(false);           ////se voltar a ser necessário colocar estas 2 variaveis como parametro na funcao do tipo gameObject
      //receivedPanel.SetActive(true);
      GetCardsAndSets.RefreshNumbers();
      ClaimText();
    }
  }

  public async Task UnlockSetForUser()
  {
    // Debug.Log("Item Redeemed");
    processes++;

    UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserSets.php?user=" + UserLogin.instance.LogInInfo.user.id + "&setID=" + Set.id);
    foreach (var este in All)
    {
      //Card User = CardManager.UserCards.Where(x => x.id == este.Card.id).Single();
      //await RemoveCard(este.Card);
      //if (este.Card.copies - 1 > 0)
      //{
      //    await RestockCard(este.Card);
      //    este.Card.copies--;
      //    User.copies--;
      //}
      //else
      //{
      //    CardManager.UserCards.Remove(este.Card);
      //}


    }
    CreateCards.UpdateInventory();

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
      Debug.Log("Failed to redeem " + www.error);
    }
    //Debug.LogError(www.downloadHandler.text);
    else
    {
      //  Debug.Log("Success");
      Redeemed = true;
      await UserLevelComponent.AddXP(100);
      await UserSetsComponent.GetUserSets();
      processes--;
    }


  }

  public async Task RemoveCard(Card UC)
  {

    UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "removeUserCards.php?user=" + UserLogin.instance.LogInInfo.user.id + "&cardID=" + UC.id + "&amount=" + 1);
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
      Debug.Log("Failed to redeem " + www.error);
    }
    //Debug.LogError(www.downloadHandler.text);
    else
    {
      //Debug.Log("Success");

      Debug.Log(UC.id + " Cards of this id " + CardManager.UserCards.Where(x => x.id == UC.id).Count());
      Card User = CardManager.UserCards.Where(x => x.id == UC.id).Single();
      //Debug.Log(UC.copies + " total copies");
      if (UC.copies - 1 <= 0)
      {
        // Debug.Log("Cards before " + CardManager.UserCards.Count);
        // Debug.Log("Card Removed");
        CardManager.UserCards.Remove(User);

        //Debug.Log("Cards now " + CardManager.UserCards.Count);
      }
      else
      {
        User.copies--;
      }
    }

  }

  public async Task RestockCard(Card UC)
  {

    //Esta comentado, falta apenas o link para o servico

    UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "setUserCards.php?user=" + UserLogin.instance.LogInInfo.user.id + "&cardID=" + UC.id + "&amount=" + (UC.copies));
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
      Debug.Log("Failed to redeem " + www.error);
    }
    //Debug.LogError(www.downloadHandler.text);
    else
    {
      Debug.Log("Success");
    }

  }

}
