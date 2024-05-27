using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExportAvatar : MonoBehaviour
{
    public static ExportAvatar instance;
    [SerializeField]
    private Button btn;
    [SerializeField]
    private TextMeshProUGUI savedtxt;
    [SerializeField]
    private SaveToPNG SaveToPNG;

    private bool saved;

    public bool Saved
    {
        get => saved; set
        {
            saved = value;
            if (btn != null)
            {
                btn.interactable = !saved;
                savedtxt.gameObject.SetActive(saved);
            }
        }
    }

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        Saved = true;
    }
    // Start is called before the first frame update
    public async void Save()
    {
        string Job = "Saving Avatar to server";
        LoadingManager.instance.EnqueueLoad(Job);
        LoadSVGs.SetUserColor();
        IAsyncEnumerator<WaitForEndOfFrame> e = SaveToPNG.SavePNG();
        try
        {
            while (await e.MoveNextAsync());
        }
        finally { if (e != null) await e.DisposeAsync(); }
        Task A = AvatarReader.SaveAvatarToServer();
        await Task.WhenAll(A);


        Debug.Log("DONE");
        Saved = true;
        LoadingManager.instance.DequeueLoad(Job);

    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
