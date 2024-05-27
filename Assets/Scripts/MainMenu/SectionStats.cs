using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SectionStats : MonoBehaviour
{

    //6b5UlFcf/~ZAM\YA.O&8
    [Header("Section Type")]
    public SectionType SectionType;

    [Header("The counter on the left")]
    public TextMeshProUGUI UserAmountLeft;
    public TextMeshProUGUI TotalAmountLeft;

    [Header("The counter on the right")]
    public TextMeshProUGUI UserAmountRight;
    public TextMeshProUGUI TotalAmountRight;

    [Header("The percentage above, if type avatar this field isnt required")]
    public TextMeshProUGUI PercentageTotal;

    [Header("Radial Sliders, if type avatar this field isnt required")]
    public RadialSlider Inner;
    public RadialSlider Outer;


    public async void UpdateCollection()
    {
        if (AchievementTrackerComponent.instance.GetVariable(VariableType.AllSetsCompleted) == 0 && UserSetsComponent.AllUserSets.All.Where(x => !x.SetID.StartsWith("A") && x.SetID.Length != 4 && !x.SetID.StartsWith("LV-")).Count() != 0)
            await AchievementTrackerComponent.instance.SetVariable(VariableType.AllSetsCompleted, UserSetsComponent.AllUserSets.All.Where(x => !x.SetID.StartsWith("A") && x.SetID.Length != 4 && !x.SetID.StartsWith("LV-")).Count());

        if (CardManager.UniqueCards < AchievementTrackerComponent.instance.GetVariable(VariableType.UniqueCards))
        {
            await AchievementTrackerComponent.instance.SetVariable(VariableType.UniqueCards, CardManager.UniqueCards);
        }

        // Debug.Log("Unique Cards" + CardManager.UniqueCards);

        UserAmountLeft.text = AchievementTrackerComponent.instance.GetVariable(VariableType.AllSetsCompleted).ToString();
        TotalAmountLeft.text = CardManager.CardSets.All.Count().ToString();

        //Debug.Log(AchievementTrackerComponent.instance.GetVariable(VariableType.AllSetsCompleted) + "/" + CardManager.CardSets.All.Count());
        Inner.Value = (float)AchievementTrackerComponent.instance.GetVariable(VariableType.AllSetsCompleted) / CardManager.CardSets.All.Count();

        UserAmountRight.text = CardManager.UserCards.Count() + "";
        TotalAmountRight.text = CardManager.AllCards.All.Count() + "";

        Outer.Value = (float)AchievementTrackerComponent.instance.GetVariable(VariableType.UniqueCards) / CardManager.AllCards.All.Count();

        PercentageTotal.text = ((float)AchievementTrackerComponent.instance.GetVariable(VariableType.UniqueCards) / CardManager.AllCards.All.Count() * 100).ToString("F0") + "%";
    }

    public void UpdateJourney()
    {
        // Debug.Log("User Sets Null? " + UserSetsComponent.AllUserSets.All.Where(x => x.SetID.StartsWith("A0")).Count() + " " + gameObject.name);//
        UserAmountLeft.text = UserSetsComponent.AllUserSets.All.Where(x => x.SetID.StartsWith("A") && x.SetID.Length == 4).Count() + "";
        TotalAmountLeft.text = AchievementTrackerComponent.instance.AllAchievements.All.Count().ToString();

        Outer.Value = (float)UserSetsComponent.AllUserSets.All.Where(x => x.SetID.StartsWith("A") && x.SetID.Length == 4).Count() / AchievementTrackerComponent.instance.AllAchievements.All.Count();

        UserAmountRight.text = UserSetsComponent.AllUserSets.All.Where(x => x.SetID.StartsWith("LV-")).Count() + "";
        TotalAmountRight.text = UserLevelComponent.LevelCaps.All.Count().ToString();

        Inner.Value = (float)UserSetsComponent.AllUserSets.All.Where(x => x.SetID.StartsWith("LV-")).Count() / UserLevelComponent.LevelCaps.All.Count();

        PercentageTotal.text = ((float)UserSetsComponent.AllUserSets.All.Where(x => x.SetID.StartsWith("LV-")).Count() / UserLevelComponent.LevelCaps.All.Count() * 100).ToString("F0") + "%";
    }

    public void UpdateAvatar()
    {

        UserAmountLeft.text = AvatarAccessoriesManager.AvatarAccs.All.Where(x => AvatarAccessoriesManager.IsPartUnlocked(x.id) && AssetBundleCacher.Instance.avatarpartsthumbs.ContainsKey(x.id + "_thumb")).Count().ToString();
        // Debug.Log("Avatar parts Unlocked" + AvatarAccessoriesManager.AvatarAccsUnlocks.All.Count());
        TotalAmountLeft.text = AvatarAccessoriesManager.AvatarAccs.All.Count().ToString();


        UserAmountRight.text = (ColourManager.colourSets.couloursets.Where(x => ColourManager.CheckUnlocked(x) && x.SetCode != "Starter").Count() / 3).ToString();
        TotalAmountRight.text = (ColourManager.colourSets.couloursets.Where(x => !x.code.StartsWith("C0") && !x.code.StartsWith("C1") && x.SetCode != "Starter").Count() / 3).ToString();


    }

    private void Start()
    {
        Load();
    }

    public async void Load()
    {
        Task A = LoadSVGs.GetAllAvatarUnlocks();
        await Task.WhenAll(A);

        switch (SectionType)
        {
            case SectionType.JOURNEY:
                UpdateJourney();
                break;
            case SectionType.AVATAR:
                UpdateAvatar();
                break;
            case SectionType.COLLECTION:
                UpdateCollection();
                break;
        }
    }

}



public enum SectionType
{
    JOURNEY,
    AVATAR,
    COLLECTION
}
