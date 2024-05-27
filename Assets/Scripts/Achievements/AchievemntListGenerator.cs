using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AchievemntListGenerator : MonoBehaviour
{
    public AchievementDisplayComponent AchievementPrefab;
    public Transform Parent;
    public JourneyRewardsPanel JourneyRewardsPanel;
    public void Start()
    {
        GenerateAchievements();
    }
    // Start is called before the first frame update
    public async void GenerateAchievements()
    {
        while (NotificationsAchJourney.instance.loaded == false)
        {
            await Task.Yield();
        }
        foreach (var este in AchievementTrackerComponent.instance.AllAchievements.All)
        {
            AchievementDisplayComponent novo = Instantiate(AchievementPrefab, Parent);
            novo.Ach = este;
            novo.RedeeemBtn.onClick.AddListener(delegate { JourneyRewardsPanel.SetupXPBanner(novo.XP); });
            novo.RedeeemBtn.onClick.AddListener(delegate { JourneyRewardsPanel.SetupQuiidsBanner(novo.QUI); });
            if (novo.QI.gameObject.activeSelf)
                novo.RedeeemBtn.onClick.AddListener(delegate { JourneyRewardsPanel.SetupQiisBanner(novo.QI); });
            novo.RedeeemBtn.onClick.AddListener(delegate { JourneyRewardsPanel.gameObject.SetActive(true); });
        }
    }
}
