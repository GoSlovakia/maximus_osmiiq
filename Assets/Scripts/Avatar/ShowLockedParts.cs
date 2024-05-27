using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowLockedParts : MonoBehaviour
{
    public static ShowLockedParts Instance;
    public Toggle toggle;
    public bool show=true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void Showparts(bool show)
    {
        foreach (var este in AssetCreatingFromFolder.AllAccBtns)
        {
            este.Show = !(AvatarAccessoriesManager.IsPartUnlocked(este.Acc.id) || show);
            este.gameObject.SetActive(AvatarAccessoriesManager.IsPartUnlocked(este.Acc.id) || show);

        }
        foreach (var este in GenerateColorPalettes.instance.Colors)
        {
            este.HiddenFromView = !(este.Unlocked || show);
            este.gameObject.SetActive(este.Unlocked || show);
        }
        this.show = show;
        Debug.Log("Changes " + AssetCreatingFromFolder.AllAccBtns.Count);
        toggle.SetIsOnWithoutNotify(show);
    }

    public void Refresh()
    {
        foreach (var este in AssetCreatingFromFolder.AllAccBtns)
        {
            este.Show = !(este.Unlocked || toggle.isOn);
            este.gameObject.SetActive(este.Unlocked || toggle.isOn);

        }
        foreach (var este in GenerateColorPalettes.instance.Colors)
        {
            este.HiddenFromView = !(este.Unlocked || toggle.isOn);
            este.gameObject.SetActive(este.Unlocked || toggle.isOn);
        }
    }
}
