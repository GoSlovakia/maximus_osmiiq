using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AssetCreatingFromFolder : MonoBehaviour
{
    public AvatarChangeBtn AccessoryBtnprefab;

    public static List<AvatarChangeBtn> AllAccBtns = new List<AvatarChangeBtn>();
    public static bool LoadingBtns = false;

    public static volatile string filepath;

    public static CancellationTokenSource cancel = new CancellationTokenSource();


    private int _remaning = 0;

    private int remaning
    {
        get => _remaning;
        set
        {
            _remaning = value;
            Debug.Log(_remaning + " remaning");
        }
    }
    void Start()
    {
        filepath = Application.persistentDataPath + "/parts/";
        AllAccBtns = new List<AvatarChangeBtn>();

        if (!Directory.Exists(filepath))
        {

            Debug.LogWarning("Creating parts directory!");
            Directory.CreateDirectory(filepath);
        }
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, LoadSceneMode arg1)
    {
        if (!cancel.IsCancellationRequested)
            cancel.Cancel();
        else
            cancel = new CancellationTokenSource();
    }

    // Start is called before the first frame update
    //void GenerateFromAddresables()


    //    AccType[] all = (AccType[])System.Enum.GetValues(typeof(AccType));
    //    Debug.Log(all.Length);

    //    Addressables.LoadAssetsAsync<Texture2D>("Parts", null).Completed += objects =>
    //    {


    //        foreach (Texture2D tex in objects.Result)
    //        {

    //            //Debug.Log($"Addressable Loaded: {tex.name}");
    //            GameObject novobtn = Instantiate(AccessoryBtnprefab, transform);
    //            novobtn.transform.GetChild(0).GetComponent<RawImage>().texture = tex;
    //            novobtn.GetComponent<AvatarChangeBtn>().Type = all.Where(x => tex.name.Contains(x.GetFilenameRequired())).First();
    //            novobtn.GetComponent<AvatarChangeBtn>().Acc = new Accessory(novobtn.GetComponent<AvatarChangeBtn>().Type, tex);
    //            // Debug.Log(novobtn.GetComponent<AvatarChangeBtn>().Acc.Type.GetDescription());
    //            //novobtn.GetComponent<AvatarChangeBtn>().Acc.Type = novobtn.GetComponent<AvatarChangeBtn>().Type;
    //            //novobtn.GetComponent<AvatarChangeBtn>().Acc.img = tex;
    //        }
    //        FindObjectOfType<BrowseAssets>().SortAcc();
    //    };

    //    //foreach (FileInfo este in info)
    //    //{
    //    //    string pathname = este.FullName;
    //    //    //pathname = pathname.Substring(0, pathname.Length - 4);
    //    //    Texture2D novo = Resources.Load(pathname) as Texture2D;
    //    //    GameObject novobtn = Instantiate(AccessoryBtnprefab, transform);
    //    //    novobtn.transform.GetChild(0).GetComponent<RawImage>().texture = novo;
    //    //    Debug.Log(pathname);
    //    //}
    //}

    //public void GenerateFromList(List<Sprite> lista)
    //{
    //    List<Accessory> novos = new List<Accessory>();
    //    AccType[] all = (AccType[])System.Enum.GetValues(typeof(AccType));
    //    foreach (Sprite tex in lista)
    //    {

    //        //Debug.Log($"Addressable Loaded: {tex.name}");
    //        GameObject novobtn = Instantiate(AccessoryBtnprefab, transform);
    //        string Code = tex.name;
    //        Code = Code.Replace(all.Where(x => tex.name.Contains(x.GetFilenameRequired())).First().GetFilenameRequired(), "");
    //        Code = Code.Replace("PC-", "");
    //        Code = Code.Replace("F", "B");
    //        //novobtn.transform.GetChild(0).GetComponent<SVGImage>().sprite = tex;

    //        novobtn.GetComponent<AvatarChangeBtn>().Type = all.Where(x => tex.name.Contains(x.GetFilenameRequired() + "B")).First();
    //        novos.Add(new Accessory(novobtn.GetComponent<AvatarChangeBtn>().Type, Code));
    //        novobtn.GetComponent<AvatarChangeBtn>().Acc = novos.Last();

    //        novobtn.transform.GetChild(0).GetComponent<SVGImage>().sprite = Load_SVG_From_File.GetThumbnail(novos.Last(), novos.Last().Code);
    //        //Debug.Log(Code);
    //        AllAccBtns.Add(novobtn.GetComponent<AvatarChangeBtn>());

    //    }
    //    //FindObjectOfType<BrowseAssets>().SortAcc();

    //    All = novos;
    //    //Debug.Log("Accessory Amount " + All.Count);
    //}

    public async Task GenerateFromListServer(AvatarAccessoryContainer lista)
    {
        string Job = "Generating Accessories";
        LoadingBtns = true;
        //Debug.Log("Running GenerateFromListServer");
        //Debug.Log("Generating List");
        foreach (var este in lista.All.Where(x => AssetBundleCacher.Instance.avatarpartsthumbs.ContainsKey(x.id+"_thumb")))
        {

            LoadingManager.instance.EnqueueLoad(Job);
            //Debug.Log("Creating " + este.id);
            try
            {

                await Task.Run(() =>
            {
                UnityMainThread.wkr.AddJob(() =>
                {

                    if (cancel.IsCancellationRequested)
                    {
                        LoadingManager.instance.DequeueLoad(Job);
                        return;
                    }
                    AvatarChangeBtn acbnovo = Instantiate(AccessoryBtnprefab, transform);



                    //Debug.Log($"Addressable Loaded: {tex.name}");

                    //novobtn.transform.GetChild(0).GetComponent<SVGImage>().sprite = tex;

                    acbnovo.Type = este.avatarset;
                    //Debug.Log(este.id);
                    acbnovo.Acc = este;



                    //Debug.Log(Code);
                    AllAccBtns.Add(acbnovo);
                    // acbnovo = novobtn.GetComponent<AvatarChangeBtn>();
                    acbnovo.toggle.group = GenerateCategories.AllCategories.Where(x => x.Type == este.avatarset).FirstOrDefault().GetComponent<ToggleGroup>();
                    acbnovo.transform.SetParent(GenerateCategories.AllCategories.Where(x => x.Type == este.avatarset).FirstOrDefault().transform);
                    acbnovo.Container = GenerateCategories.AllCategories.Where(x => x.Type == este.avatarset).FirstOrDefault();

                    if (UserLogin.instance.UserAvatar.All.Where(x => x.part == acbnovo.Acc.id).Count() > 0)
                    {
                        acbnovo.allowchanges = false;
                        acbnovo.GetComponent<Toggle>().isOn = true;
                        acbnovo.Selected = true;
                        acbnovo.allowchanges = true;
                    }


                    LoadingManager.instance.DequeueLoad(Job);
                });
            }
            );
            }
            catch (Exception e)
            {
                Debug.LogError(e);

            }
        }
        LoadingBtns = false;
        //  Debug.Log("List Generated, All size " + novos.Count);
        //foreach(var este in FindObjectsOfType<AccessoryContainer>())
        //{
        //    este.UpdateCounter();
        //}
        //FindObjectOfType<BrowseAssets>().SortAcc();

        //GenerateThumbnailsServer();
        //Debug.Log("Accessory Amount " + All.Count);
    }

    //public IEnumerator GenerateThumbnailsServer(int index)
    //{
    //    //Debug.Log("Getting thumbnail " + index + "/" + AllAccBtns.Count);
    //    if (AllAccBtns[index].gameObject.activeSelf)
    //    {
    //        Task A = AllAccBtns[index].GetThumbnail();
    //        yield return Task.WhenAll(A);
    //    }
    //    else
    //    {
    //        yield return null;
    //    }

    //    if (AllAccBtns.Count - 1 > index)
    //    {
    //        StartCoroutine(GenerateThumbnailsServer(index + 1));
    //    }
    //}

    public static async Task SavePartToSVG(string content, string code)
    {
        if (code == "")
        {
            return;
        }
        string path = filepath + code + ".svg";
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        //Debug.Log(path);
        await Task.Run(() =>
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, content);
        }
        );
    }


    public static AccType GetType(string filename)
    {
        AccType[] all = (AccType[])System.Enum.GetValues(typeof(AccType));
        //Debug.Log(filename.Substring(0, filename.Length - 3));
        return all.Where(x => filename.Substring(0, filename.Length - 3).Contains(x.ToString())).First();
    }



}
