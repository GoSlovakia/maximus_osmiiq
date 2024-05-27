using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AvatarChangeBtn : MonoBehaviour
{
    [SerializeField]
    private Color HiglightColor;
    [SerializeField]
    public AccType Type;
    private AvatarAcc acc;
    //public ButtonOutline ButtonOutline;
    public GameObject LockIcon;
    public AccessoryContainer Container;

    public static bool triggerchanges = true;

    public static CancellationTokenSource cancel = new CancellationTokenSource();

    private bool _highlighted;

    public bool highlighted
    {
        get => _highlighted;
        set
        {
            _highlighted = value;
            if (_highlighted)
            {
                GetComponent<NotificationComponent>().showNotification = true;
                ColorBlock cb = toggle.colors;
                cb.normalColor = HiglightColor;
                toggle.colors = cb;
            }
            else
            {
                GetComponent<NotificationComponent>().showNotification = false;
                ColorBlock cb = toggle.colors;
                cb.normalColor = Color.white;
                toggle.colors = cb;
            }
        }
    }

    [SerializeField]
    public bool Unlocked = true;
    public bool Show = false;
    public bool FetchFromServer = true;
    //public SVGImage SVGImage;
    [SerializeField]
    private Image Thumb;
    [SerializeField]
    private Image LoadingImg;
    [SerializeField]
    private Image BG;
    [SerializeField]
    private Image Border;

    public bool allowchanges = false;

    [SerializeField]
    private Material LockedShader;
    //public Button btn;
    public Toggle toggle;
    private bool _loading;

    [SerializeField]
    private Color RegularBG;
    [SerializeField]
    private Color LockedBG;
    [SerializeField]
    private Color LockedBorder;

    public bool loading
    {
        get => _loading;
        set
        {
            _loading = value;
            LoadingImg.gameObject.SetActive(value);
            Thumb.gameObject.SetActive(!value);
            //LoadingImg.transform.rotation = Quaternion.Euler(0, 0, 0);
            //if (loading)
            //{
            //    StartCoroutine(RotateIcon());
            //}
        }
    }

    public AvatarAcc Acc
    {
        get => acc; set
        {
            acc = value;
            // Debug.Log("Avatar Part" + value.id);

            GetThumbnail();
            CheckUnlocked();
        }
    }

    private void Awake()
    {
        // btn = GetComponent<Button>();
        gameObject.SetActive(!Show);
    }
    private void Start()
    {
        if (gameObject.activeSelf)
            StartCoroutine(Cooldown());
    }
    private void OnEnable()
    {
        //StartCoroutine(Cooldown());
        if (Thumb == null)
        {
            GetThumbnail();
        }
        allowchanges = true;
        gameObject.SetActive(!Show);
    }

    private void OnDisable()
    {
        allowchanges = false;
    }
    public IEnumerator Cooldown()
    {
        allowchanges = false;
        yield return new WaitForSeconds(0.2f);
        allowchanges = true;
    }
    //public IEnumerator RotateIcon()
    //{
    //    LoadingImg.transform.Rotate(0, 0, -50f * Time.deltaTime);
    //    yield return null;
    //    if (loading)
    //        StartCoroutine(RotateIcon());
    //    else
    //    {
    //        LoadingImg.color = new Color(0, 0, 0, 0);
    //    }
    //}


    public void CheckUnlocked()
    {

        if (!AvatarAccessoriesManager.IsPartUnlocked(Acc.id))
        {
            // btn.interactable = false;
            toggle.enabled = false;
            LockIcon.SetActive(true);
            BG.color = LockedBG;
            Border.color = LockedBorder;
            Thumb.material = LockedShader;
            Unlocked = false;
            if (!Unlocked)
            {
                Show = ShowLockedParts.Instance.show;
            }
            gameObject.SetActive(Show);
        }
        else
        {
            BG.color = RegularBG;
            NotificationTabManager.instance.AvatarCheckNotif(this);

        }
    }


    public async void ChangeAvatar()
    {
        try
        {
            if (!allowchanges || !Unlocked || !Selected || !triggerchanges)
            {
                allowchanges = true;
                //Debug.Log("Not yet");
                return;
            }
            else
            {

                while (Acc == null)
                {
                    await Task.Yield();
                }

                if (highlighted)
                {
                    NotificationTabManager.instance.RemoveNotif(this);
                }
                if (await Comparehash(Acc.id))
                {
                    Debug.Log("Same hash");
                    await AvatarReader.ChangePart(Acc, false);
                }
                else
                {
                    Debug.Log("File Modified, getting from server");
                    await LoadSVGs.GetPartSprite(Acc.id, true, true);

                    //if (thumbnail != null)
                    //{
                    //    //Sprite sprite = Sprite.Create(thumbnail, new Rect(0, 0, thumbnail.width, thumbnail.height), Vector2.one / 2f);
                    //    Thumb.sprite = thumbnail;
                    //}
                    await AvatarReader.ChangePart(Acc, false);
                }

                if (AchievementTrackerComponent.instance.GetVariable(VariableType.SavedAvatar) == 0)
                {
                    await AchievementTrackerComponent.instance.SetVariable(VariableType.SavedAvatar, 1);
                }
                else
                {
                    Debug.Log("No need " + AchievementTrackerComponent.instance.GetVariable(VariableType.SavedAvatar));
                }
                ExportAvatar.instance.Saved = false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    private bool _selected;
    [SerializeField]
    private Color BgSelected;
    [SerializeField]
    private Color BgDefault;

    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            toggle.interactable = !value;
            toggle.image.color = toggle.isOn ? BgSelected : BgDefault;
            if (value)
            {
                Container.PartName.text = Acc.name;
            }
        }
    }

    public void GetThumbnail()
    {
        loading = true;
        if (File.Exists(AssetCreatingFromFolder.filepath + Acc.id + ".png"))
        {


            //StreamReader inp_stm = new StreamReader(AssetCreatingFromFolder.filepath + Acc.Code + ".svg");
            //string svg = inp_stm.ReadLine();
            //UnityMainThread.wkr.AddJob(() =>
            //{
            //    LoadingManager.instance.EnqueueLoad(Job);
            //    if (SVGImage != null)
            //    {
            //        SVGImage.sprite = CombineTextures.Load_SVG_as_Sprite(ColourManager.ReplaceColors(svg, Load_SVG_From_File.PrimaryCodeDefault, Load_SVG_From_File.SecondaryCodeDefault));
            //        loading = false;
            //    }
            //    LoadingManager.instance.DequeueLoad(Job);
            //});

            // btn.interactable = true;


        }
        else
        {
            //UnityMainThread.wkr.AddJob(() =>
            //{
            //Thumb.texture = await GetJPEG.GetAvatarThumb(acc.id);
            //Debug.Log("Teste " + AssetBundleCacher.Instance.avatarpartsthumbsSprites.Where(x => x.name.Contains(acc.id)).Count());
            Sprite res;
            AssetBundleCacher.Instance.avatarpartsthumbs.TryGetValue(acc.id + "_thumb", out res);
            if (res == null)
            {
                AssetCreatingFromFolder.AllAccBtns.Remove(this);
                AccessoryContainer.Invoke();
                gameObject.SetActive(false);
                return;
            }
            if (Thumb == null)
            {
                return;
            }
            Thumb.sprite = res;
            //Thumb.GetComponent<AspectRatioFitter>().aspectRatio = (float)Thumb.texture.width / (float)Thumb.texture.height;
            Thumb.color = Color.white;
            //LoadingImg.color = new Color(0, 0, 0, 0);
            // allowchanges = true;
            loading = false;

            //});
            //Debug.Log("File not found " + AssetCreatingFromFolder.filepath + Acc.Code + ".svg");

            //UnityMainThread.wkr.AddJob(async () =>
            //{
            //    LoadingManager.instance.EnqueueLoad(Job);
            //    if (cancel.IsCancellationRequested)
            //    {
            //        LoadingManager.instance.DequeueLoad(Job);
            //        return;
            //    }
            //    Sprite thumbnail = await LoadSVGs.GetPartSprite(Acc.Code, true, false);

            //    if (thumbnail != null)
            //    {

            //        //Sprite sprite = Sprite.Create(thumbnail, new Rect(0, 0, thumbnail.width, thumbnail.height), Vector2.one / 2f);
            //        SVGImage.sprite = thumbnail;
            //    }
            //    else
            //    {
            //        //Debug.LogError("No thumbnail found! " + Acc.Code);
            //        gameObject.SetActive(false);
            //        AssetCreatingFromFolder.AllAccBtns.Remove(this);
            //        AssetCreatingFromFolder.All.Remove(Acc);
            //    }
            //    loading = false;
            //    LoadingManager.instance.DequeueLoad(Job);
            //});

        }

        //yield return null;
    }

    public async Task<bool> Comparehash(string Code)
    {
        //Debug.Log("COMPARING HASH");

        MD5 md5 = MD5.Create();
        string hash = "";
        if (!File.Exists(AssetCreatingFromFolder.filepath + Code + ".svg"))
        {
            //Debug.Log("File Not found + " + AssetCreatingFromFolder.filepath + Code + ".svg");
            return false;
        }
        else
        {
            //Debug.Log("File found " + Code);
            StreamReader inp_stm = new StreamReader(AssetCreatingFromFolder.filepath + Code + ".svg");
            string svg = inp_stm.ReadLine();
            md5.ComputeHash(File.ReadAllBytes(AssetCreatingFromFolder.filepath + Code + ".svg"));
            for (int i = 0; i < md5.Hash.Length; i++)
            {
                hash += md5.Hash[i].ToString("x2");
            }
        }
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "GetSVGHash.php?file=" + Code + ".svg");
        var req = www.SendWebRequest();

        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return false;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return false;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Comparison Failed " + www.error);
            return false;
        }
        else
        {
            //Debug.Log(hash == www.downloadHandler.text);
            return hash == www.downloadHandler.text;
        }
    }


}



