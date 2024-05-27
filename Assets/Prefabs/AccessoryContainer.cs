using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class AccessoryContainer : MonoBehaviour
{
    public delegate void UpdateCountersDelegate();

    public static event UpdateCountersDelegate UpdateCounters;

    public TextMeshProUGUI PartName;



    public AccType Type;

    private TextMeshProUGUI counter;
    public AvatarAcc current;

    public TextMeshProUGUI Counter
    {
        get => counter; set
        {
            counter = value;
            UpdateCounter();
        }
    }

    public static void Invoke()
    {
        UpdateCounters();
    }
    private void Awake()
    {
        UpdateCounters += UpdateCounter;
    }

    public void UpdateCounter()
    {
        //Debug.Log("COUNTER BEING UPDATED");
        Counter.text = "(" + AvatarAccessoriesManager.AvatarAccs.All.Where(x => AvatarAccessoriesManager.IsPartUnlocked(x.id) && x.avatarset == Type && AssetBundleCacher.Instance.avatarpartsthumbs.ContainsKey(x.id + "_thumb")).Count().ToString() + "/" + AvatarAccessoriesManager.AvatarAccs.All.Where(x => x.avatarset == Type && AssetBundleCacher.Instance.avatarpartsthumbs.ContainsKey(x.id + "_thumb")).Count().ToString() + ")";
    }
}
