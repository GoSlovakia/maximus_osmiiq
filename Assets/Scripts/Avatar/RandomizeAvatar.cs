using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RandomizeAvatar : MonoBehaviour
{
    [SerializeField]
    private GameObject _undoRandomBtn;

    public static GameObject UndoRandomBtn;

    [SerializeField]
    private GameObject RandomBtn;

    private void Awake()
    {
        if (_undoRandomBtn != null)
            UndoRandomBtn = _undoRandomBtn;
    }

    public void Randomize(int interactions)
    {
        Task.Run(() => RandomizeTask(interactions));
    }

    // Start is called before the first frame update
    public async Task RandomizeTask(int intereactions)
    {
        try
        {
            await AvatarReader.SaveAvatar(AvatarReader.PreviousState);

            UnityMainThread.wkr.AddJob(() =>
            {
                if (Random.Range(0f, 1f) < 0.6f)
                {
                    int res = Random.Range(0, ColourManager.colourSets.couloursets.Length);
                    if (ColourManager.CheckUnlocked(ColourManager.colourSets.couloursets[res]))
                        Load_SVG_From_File.PrimaryCode = ColourManager.colourSets.couloursets[res].code;
                    if (Load_SVG_From_File.PrimaryCode.Length == 3)
                        Load_SVG_From_File.PrimaryCode = Load_SVG_From_File.PrimaryCodeDefault;


                }
                if (Random.Range(0f, 1f) < 0.3f)
                {
                    int res = Random.Range(0, ColourManager.colourSets.couloursets.Length);
                    if (ColourManager.CheckUnlocked(ColourManager.colourSets.couloursets[res]))
                        Load_SVG_From_File.SecondaryCode = ColourManager.colourSets.couloursets[res].code;
                    if (Load_SVG_From_File.SecondaryCode.Length == 3)
                        Load_SVG_From_File.SecondaryCode = Load_SVG_From_File.SecondaryCodeDefault;

                }
                UndoRandomBtn.SetActive(true);
                //RandomBtn.SetActive(false);
                Debug.Log("Undo button Active? " + UndoRandomBtn.activeSelf);
            });

            UnityMainThread.wkr.AddJob(async () =>
            {
                for (int i = 0; i < intereactions; i++)
                {
                    AvatarAcc res = AvatarAccessoriesManager.AvatarAccs.All[Random.Range(0, AvatarAccessoriesManager.AvatarAccs.All.Count())];
                    if (AvatarAccessoriesManager.IsPartUnlocked(res.id))
                    {
                        await AvatarReader.ChangePart(res, false);
                        // await ButtonOutline.SelectThisAccessoryFromFresh(res);
                    }


                }
                //await AvatarReader.SaveAvatarToServer();
                AvatarReader.ReloadAvatar();
                UndoRandomBtn.SetActive(true);

                foreach (var este in FindObjectsOfType<RemovePiece>())
                {
                    este.toggle.isOn = true;
                    este.Selected = true;
                }
                foreach (var este in AvatarReader.Parts)
                {
                    if (este.Acc != null)
                    {
                        Debug.Log("Acc " + este.Acc.id);
                        AvatarChangeBtn res = AssetCreatingFromFolder.AllAccBtns.Where(x => x.Acc.id == este.Acc.id).First();
                        res.toggle.isOn = true;
                        res.Selected = true;
                    }

                }
            });

            ExportAvatar.instance.Saved = false;

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    public void ReturnToLastAvatarBeforeRandomize()
    {
        AvatarReader.current = AvatarReader.PreviousState;

        AvatarReader.LoadAvatar();
        UndoRandomBtn.SetActive(false);
        //RandomBtn.SetActive(true);
    }

}
