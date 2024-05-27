using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEditor;
using System;

public class AvatarReader : MonoBehaviour
{
    public static List<AvatarPart> Parts = new List<AvatarPart>();

    [SerializeField]
    private Avatar _previousState;
    public static Avatar PreviousState;

    public List<RemovePiece> Removes = new List<RemovePiece>();


    [SerializeField]
    private Avatar _current;
    public static Avatar current;

    [SerializeField]
    private Avatar _defaultAvatar;
    public static Avatar defaultAvatar;

    public ColorSet currentColorSet = ColorSet.PRIMARY;

    public static AvatarReader avatarReader;

    public static List<string> acc;

    public Material prefabMat;

    private void Awake()
    {
        CombineTextures.material = GetComponentInChildren<SpriteRenderer>().material;

        current = _current;
        avatarReader = this;
        defaultAvatar = _defaultAvatar;
        PreviousState = _previousState;

        Parts = GetComponentsInChildren<AvatarPart>().ToList();
        Removes = FindObjectsOfType<RemovePiece>().ToList();


    }

    public static async Task ChangePart(AvatarAcc novo, bool setOnServer)
    {
        try
        {
            AvatarPart Change = Parts.Where(x => x.Part == novo.avatarset).First();
            Task A = Task.Run(() =>
            {
                if (Change.Acc == null)
                {
                    // Debug.Log("Accessory is null");
                    UnityMainThread.wkr.AddJob(async () =>
                    {
                        Change.SetPart(novo);

                        if (setOnServer)
                        {
                            Debug.Log("Setting on server " + novo.id);
                            await SetUserParts.SetUserPartsSingleton.SetUserPartsOnServer(novo.id);
                            // await LoadUserAvatarSavedOnServer();
                        }
                    });
                    //ButtonOutline.SelectThisAccessoryFromFresh(novo);
                    //Mudar no servidor
                }
                else
                if (novo.id == Change.Acc.id && Change.Part.GetRemovable())
                {
                    //UnityMainThread.wkr.AddJob(() =>
                    //{
                    //    if (setOnServer)
                    //    {
                    //        SetUserParts.SetUserPartsSingleton.RemoveUserPartsOnServer(UserLogin.instance.LogInInfo.user.id, novo.Code);
                    //        // await LoadUserAvatarSavedOnServer();
                    //    }

                    //    Change.Clear();
                    //});
                    ////ButtonOutline.SelectThisAccessoryFromFresh(novo);

                    ////Remover a parte no servidor
                    //Debug.Log("Removing Part" + novo.Code);

                }
                else
                {
                    //Debug.Log(Change.Acc.Code + " " + novo.Code);

                    UnityMainThread.wkr.AddJob(async () =>
                    {

                        if (setOnServer)
                            await SetUserParts.SetUserPartsSingleton.RemoveUserPartsOnServer(Change.Acc.id);
                        Change.SetPart(novo);

                        if (setOnServer)
                        {
                            Debug.Log("Setting on server " + novo.id);
                            await SetUserParts.SetUserPartsSingleton.SetUserPartsOnServer(novo.id);
                            // await LoadUserAvatarSavedOnServer();
                        }
                    });
                    //ButtonOutline.SelectThisAccessory(novo);

                    //Mudar no servidor
                }
            }
            );
            RandomizeAvatar.UndoRandomBtn.SetActive(false);
            await Task.WhenAll(A);
            // Debug.Log("Changed Part " + novo.Code + " " + Load_SVG_From_File.PrimaryCode);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }

    public static async Task RemovePiece(AccType tipo, bool setOnServer)
    {
        Task A = Task.Run(() =>
        {
            if (!tipo.GetRemovable())
            {
                return;
            }
            Debug.Log(Parts.Count() + " " + tipo);
            if (Parts.Where(x => x.Part == tipo).Count() == 0)
            {
                Debug.Log("Already Empty");
            }
            AvatarPart Change = Parts.Where(x => x.Part == tipo).First();
            AvatarAcc acc = Change.Acc;
            if (acc == null)
            {
                Debug.LogError("Accessory was already null " + tipo);
                return;
            }
            UnityMainThread.wkr.AddJob(async () =>
            {
                if (setOnServer)
                {
                    await SetUserParts.SetUserPartsSingleton.RemoveUserPartsOnServer(acc.id);
                    // await LoadUserAvatarSavedOnServer();
                }

                Change.Clear();
            });
            //ButtonOutline.SelectThisAccessoryFromFresh(novo);

            //Remover a parte no servidor
            Debug.Log("Removing piece of type " + tipo);
        }
            );
        await Task.WhenAll(A);
    }

    //public static void ChangePart(Texture2D acc, string code, AccType type)
    //{

    //    AvatarPart Change = Parts.Where(x => x.Part == type).First();

    //    if (Change.Acc == null)
    //    {
    //        // Debug.Log("Accessory is null");
    //        Change.SetPart(acc, code);
    //        //ButtonOutline.SelectThisAccessoryFromFresh(novo);
    //    }
    //    else
    //    if (code == Change.Acc.Code && Change.Part.GetRemovable())
    //    {
    //        Debug.Log("Removing");
    //        Change.Clear();
    //        //ButtonOutline.SelectThisAccessoryFromFresh(novo);
    //    }
    //    else
    //    {
    //        Change.SetPart(acc, code);
    //        //ButtonOutline.SelectThisAccessory(novo);
    //    }
    //    RandomizeAvatar.UndoRandomBtn.SetActive(false);
    //}

    //public void GenerateAvatar()
    //{
    //    acc = new List<string>();
    //    //CombineTextures.prefab = display.material;
    //    StartCoroutine(ReloadAvatar(AvatarGenerator.AvatarComponents.Parts, 0));

    //}

    //public IEnumerator ReloadAvatar(Part[] components, int index)
    //{
    //    //Debug.Log(index);
    //    if (components[index].code != components[index].spritegroup.GetFilenameRequired() && components[index].code != null)
    //    {
    //        // Debug.Log(LoadSVGs.IP + "GetSVG.php" + "?file=PC-" + components[index].code + ".svg");
    //        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "GetSVG.php" + "?file=PC-" + components[index].code + ".svg");
    //        yield return www.SendWebRequest();

    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            Debug.Log(www.error);
    //        }
    //        else
    //        {
    //            acc.Add(www.downloadHandler.text);
    //            Debug.Log(acc.Last()+ " OLAAAAAAAAAA");

    //            Accessory novo = new Accessory(components[index].spritegroup, components[index].code);
    //            //ChangePart(CombineTextures.Load_SVG_as_Texture(ColourManager.ReplaceColors(acc.Last(), Load_SVG_From_File.PrimaryCode, Load_SVG_From_File.SecondaryCode, white, black)), components[index].code, components[index].spritegroup);
    //            ChangePart(novo, true);
    //        }
    //    }
    //    else
    //    {
    //        yield return null;
    //    }
    //    // Debug.Log(LoadSVGs.IP + "GetSVG.php" + "?file=PC-" + components[index].code + ".svg");
    //    //Avancar para o proximo
    //    if (index < components.Length - 1)
    //    {
    //        StartCoroutine(ReloadAvatar(components, index + 1));

    //    }
    //    else
    //    {

    //    }
    //    //}
    //    //else
    //    //{
    //    //    display.material = CombineTextures.CombineAllTextures(acc.ToArray());
    //    //}
    //}




    public static void SaveAvatar()
    {
        List<string> novos = new List<string>();
        foreach (AvatarPart este in Parts)
        {

            if (este.Acc != null)
                novos.Add(este.Acc.avatarset.GetFilenameRequired() + este.Acc.id);
            else
            {
                Debug.Log(este.Part.GetDescription() + " has no accessory");
            }
        }
        current.Primary = Load_SVG_From_File.PrimaryCode;
        current.secondary = Load_SVG_From_File.SecondaryCode;
        current.AllAccFile = novos.ToArray();

    }

    public static async Task SaveAvatarToServer()
    {
        try
        {
            await SetUserParts.SetUserPartsSingleton.ResetParts();

            foreach (AvatarPart este in Parts)
            {
                if (este.Acc != null && este.Acc.id != "")
                    await SetUserParts.SetUserPartsSingleton.SetUserPartsOnServer(este.Acc.id);
            }
            //Depois pode se meter aqui para gravar cores tambem
            LoadSVGs.SetUserColor();

            Debug.Log("Save done");
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
        }

    }

    public static async Task SaveAvatar(Avatar Where)
    {
        try
        {
            await Task.Run(() =>
        {

            List<string> novos = new List<string>();
            foreach (AvatarPart este in Parts)
            {

                if (este.Acc != null)
                    novos.Add(este.Acc.id);
                else
                {
                    //Debug.Log(este.Part.GetDescription() + " has no accessory");
                }
            }
            Where.Primary = Load_SVG_From_File.PrimaryCode;
            Where.secondary = Load_SVG_From_File.SecondaryCode;
            Where.AllAccFile = novos.ToArray();

        });
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static void ReloadAvatar()
    {
        Debug.Log("RELOADING AVATAR");
        foreach (AvatarPart este in Parts)
        {

            if (este.Acc != null)
            {
                //Debug.Log(este.Acc.Code);
                este.SetPart(este.Acc);
            }
            else
            {
                //Debug.Log(este.Part + " was Null");
            }
        }
    }
    public static async void LoadAvatar()
    {
        await LoadAvatarTask();
    }
    private static async Task LoadAvatarTask()
    {
        Task A = Task.Run(() =>
         {
             foreach (AvatarPart este in Parts)
             {
                 UnityMainThread.wkr.AddJob(() =>
                 {
                     if (este.SRenderer != null)
                     {
                         este.SRenderer.sprite = null;
                         este.Acc = null;
                     }
                     if (este.Acc != null)
                     {
                         este.SRenderer.sprite = null;
                         este.Acc = null;
                     }
                 });
             }
         }
         );
        await Task.WhenAll(A);
        Load_SVG_From_File.PrimaryCode = current.Primary;
        Load_SVG_From_File.SecondaryCode = current.secondary;

        ButtonOutline.ClearAllSelections();
        Task B = Task.Run(async () =>
        {
            bool done = false;
            UnityMainThread.wkr.AddJob(async () =>
            {
                foreach (string este in current.AllAccFile)
                {
                    Debug.Log("este Codigo" + este);
                    await ChangePart(AvatarAccessoriesManager.AvatarAccs.All.Where(x => x.id == este).First(), false);

                }
                done = true;
            });

            while (!done)
            {
                await Task.Yield();
            }

            foreach (string este in current.AllAccFile)
            {
                if (AvatarAccessoriesManager.AvatarAccs.All.Where(x => x.id == este).Count() == 0)
                {
                    await Task.Yield();
                }
                AvatarAcc res = AvatarAccessoriesManager.AvatarAccs.All.Where(x => x.id == este).First();
                //await ButtonOutline.SelectThisAccessoryFromFresh(res);
            }
        }
        );
        await Task.WhenAll(B);
    }
    public static async void LoadDefaultAvatar()
    {
        Debug.Log("Loading Default Avatar");
        await LoadDefaultAvatarTask();
    }
    private static async Task LoadDefaultAvatarTask()
    {
        Task B = Task.Run(async () =>
        {
            try
            {
                await UserLogin.instance.GetAvatarFromServer();
                if (UserLogin.instance.UserAvatar != null)
                {

                    //foreach (var este in UserLogin.instance.UserAvatar.All)
                    //{

                    //    UnityMainThread.wkr.AddJob(() =>
                    //     {
                    //         SetUserParts.SetUserPartsSingleton.RemoveUserPartsOnServer(UserLogin.instance.LogInInfo.user.id, este.part);

                    //     });
                    //}
                }
                //Debug.Log("Parts size " + Parts.Count);
                foreach (AvatarPart este in Parts)
                {
                    if (este.Acc != null)
                    {
                        UnityMainThread.wkr.AddJob(() =>
                        {
                            este.SRenderer.sprite = null;
                            este.Acc = null;
                        });
                    }
                }

                Load_SVG_From_File.PrimaryCode = defaultAvatar.Primary;
                Load_SVG_From_File.SecondaryCode = defaultAvatar.secondary;
                UnityMainThread.wkr.AddJob(async () =>
                {
                    //LoadSVGs.SetUserColor();
                    //ButtonOutline.ClearAllSelections();




                    foreach (string este in defaultAvatar.AllAccFile)
                    {
                        await ChangePart(new AvatarAcc(este, AssetCreatingFromFolder.GetType(este), "0", 1, "Null", "Desc"), false);

                    }
                    foreach (var este in FindObjectsOfType<RemovePiece>())
                    {
                        if (Parts.Where(x => x.Part == este.container.Type && x.Acc != null).Count() == 0)
                        {
                            este.toggle.isOn = true;
                            este.Selected = true;
                        }
                    }
                    while (AssetCreatingFromFolder.LoadingBtns)
                    {
                        // Debug.Log("waiting for buttons " + AssetCreatingFromFolder.LoadingBtns);
                        await Task.Yield();
                    }
                    Debug.Log("Buttons count " + AssetCreatingFromFolder.AllAccBtns.Count());
                    AvatarChangeBtn.triggerchanges = false;
                    foreach (var este in defaultAvatar.AllAccFile)
                    {
                        Debug.Log("Piece Code " + este);
                        if (AssetCreatingFromFolder.AllAccBtns.Where(x => x.Acc.id == este).Count() == 0)
                        {
                            AvatarChangeBtn res = AssetCreatingFromFolder.AllAccBtns.Where(x => x.Acc.id == este).First();
                            res.toggle.isOn = true;
                            res.Selected = true;
                        }

                    }
                    AvatarChangeBtn.triggerchanges = true;
                });
                //foreach (string este in current.AllAccFile)
                //{
                //    if (AssetCreatingFromFolder.All.Find(x => x.Code == este) == null)
                //    {
                //        await Task.Delay(1000);
                //    }
                //    Accessory res = AssetCreatingFromFolder.All.Find(x => x.Code == este);
                //    //if (res != null)
                //    //    await ButtonOutline.SelectThisAccessoryFromFresh(res);
                //}
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }


        }
        );
        await Task.WhenAll(B);
    }

    public static async Task LoadUserAvatarSavedOnServer()
    {
        try
        {

            Task A = UserLogin.instance.GetAvatarFromServer();
            await Task.WhenAll(A);

            if (UserLogin.instance.UserAvatar == null || UserLogin.instance.UserAvatar.All.Count() == 0)
                LoadDefaultAvatar();
            else
            {
                //foreach (AvatarPart este in Parts)
                //{
                //    if (este.Acc != null)
                //    {
                //        SetUserParts.SetUserPartsSingleton.RemoveUserPartsOnServer(UserLogin.user.id, este.Acc.Code);
                //        este.SRenderer.sprite = null;
                //        este.Acc = null;

                //    }
                //}
                //ButtonOutline.ClearAllSelections();
                try
                {
                    UnityMainThread.wkr.AddJob(async () =>
                    {
                        foreach (var este in UserLogin.instance.UserAvatar.All)
                        {
                            //Debug.Log(este.part + " parte " + Load_SVG_From_File.PrimaryCode);
                            // Debug.Log("OLA 3");
                            // Debug.Log("Codigo " + este.part);
                            await ChangePart(new AvatarAcc(este.part, AssetCreatingFromFolder.GetType(este.part), "0", 1, "Null", "Desc"), false);

                        }
                    });
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
                foreach (var este in UserLogin.instance.UserAvatar.All)
                {
                    AvatarAcc res = new AvatarAcc(este.part, AssetCreatingFromFolder.GetType(este.part), "0", 1, "Null", "Desc");
                    //await ButtonOutline.SelectThisAccessoryFromFresh(res);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }
}

//private void OnApplicationQuit()
//{
//    List<string> novos = new List<string>();
//    foreach (AvatarPart este in Parts)
//    {
//        if (este.Acc != null)
//            novos.Add(este.Acc.Type.GetFilenameRequired() + este.Acc.Code);
//    }

//    current.AllAccFile = novos.ToArray();
//}




//------------------------------ Old Version
//private void Start()
//{
//    if (current.All == null)
//    {
//        current.All = new List<Accessory>();
//    }

//    display = GetComponent<Image>();
//    UpdateImg();
//    avatarReader = this;

//    FindObjectOfType<ResetAvatar>().Initial = current.All;
//}



//public void UpdateImg()
//{
//    foreach (Accessory este in current.All)
//    {
//        display.material.SetTexture(este.Type.GetDescription().Replace(" ", "_"), este.img.texture);
//    }

//    Debug.Log(current.All.Count + " Accessorios");
//}

//public void ChangePart(Accessory novo)
//{
//    if (current.All == null)
//    {
//        current.All = new List<Accessory>();
//    }
//    if (current.All.Where(x => x.Type == novo.Type).Count() != 0)
//    {
//        Debug.Log(current.All.Where(x => x.Type == novo.Type).Count());
//        current.All.RemoveAll(x => x.Type == novo.Type);
//    }
//    current.All.Add(novo);
//    UpdateImg();
//}

//public void RemoveAcc(AccType type)
//{
//    if (current.All == null)
//    {
//        current.All = new List<Accessory>();
//    }
//    if (current.All.Where(x => x.Type == type).Count() != 0)
//    {
//        current.All.RemoveAll(x => x.Type == type);
//    }
//    display.material.SetTexture(type.GetDescription().Replace(" ", "_"), null);
//    UpdateImg();
//}

public enum ColorSet
{
    PRIMARY,
    SECONDARY
}
