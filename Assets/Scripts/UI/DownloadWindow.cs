using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DownloadWindow : MonoBehaviour
{
    public TextMeshProUGUI InfoBox;

    public void ConfirmDownload()
    {
        AssetBundleCacher.Instance.StartDownloadingNewCache();
    }

    public void DownloadDeclined()
    {
        Application.Quit();
    }
}
