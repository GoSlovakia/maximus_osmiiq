using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LoadSVGs : MonoBehaviour
{
    //public static string IP = "https://game.maximus2020.sk/maximus/";
    public static string IP
    {
        get
        {
            if (GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] != null)
                return GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()];
            else
                return "";

        }
    }
    public static bool alreadyloaded = false;

    private static CancellationTokenSource cancel = new CancellationTokenSource();

    public static CancellationTokenSource Cancel
    {
        get => cancel; set
        {

            cancel = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetXTermPallet());
        GetUserInfoFromServer();
    }

    public static async Task GeneralInfo(bool forceload)
    {
        if (!alreadyloaded || forceload)
        {
            string Job = "Loading Avatar Scene";
            LoadingManager.instance.EnqueueLoad(Job);
            Task a = GetXTermPallett();
            Task b = GetHTMLColors();
            Task c = GetVariations();
            Task d = GetColourPacks();
            Task e = GetColorNames();
            Task f = GetColorCorrespondences();
            Task g = GetColorSets();
            Task l = GetAllAvatarUnlocks();

            Task h = GetAvatarParts();



            await Task.WhenAll(a, b, c, d, e, f, g, l);
            await Task.WhenAll(h);
            Task i = GetAllAvatarAccessories();
            await Task.WhenAll(i);
            alreadyloaded = true;
            LoadingManager.instance.DequeueLoad(Job);
        }
        else
        {
            Debug.LogWarning("Not loading files, since they have been already loaded!");
        }

    }


    private async void GetUserInfoFromServer()
    {
        string Job = "Downloading the User Avatar";
        LoadingManager.instance.EnqueueLoad(Job);
        Task a = GeneralInfo(false);

        Task j = GetUserColor();

        await Task.WhenAll(a, j);
        FindObjectOfType<GenerateCategories>().GenerateAll();



        Task x = AvatarReader.LoadUserAvatarSavedOnServer();

        await Task.WhenAll(x);

        Task asd = FindObjectOfType<AssetCreatingFromFolder>().GenerateFromListServer(AvatarAccessoriesManager.AvatarAccs);

        await Task.WhenAll(asd);





        LoadingManager.instance.DequeueLoad(Job);

        //Debug.Log("Loading default avatar");
        //AvatarReader.LoadDefaultAvatar();

        //Debug.Log("Loading Avatar from server")


        //StartCoroutine(FindObjectOfType<AssetCreatingFromFolder>().GenerateThumbnailsServer(0));
    }

    //public IEnumerator GetXTermPallet()
    //{
    //    UnityWebRequest www = UnityWebRequest.Get("s0.noip.me/maximus/getPalette.php");
    //    yield return www.SendWebRequest();

    //    if (www.result != UnityWebRequest.Result.Success)
    //    {
    //        Debug.Log(www.error);
    //    }
    //    else
    //    {
    //        // Show results as text
    //        //Debug.Log(www.downloadHandler.text);

    //        ColourManager.XTermPaletteJSON = "{ 	\"colours\": 	" + www.downloadHandler.text + "}";
    //        OnLoadEnded(GetHTMLColors());
    //    }
    //}

    public static async Task GetXTermPallett()
    {
        string Job = "Downloading the XTerm Palletts";
        LoadingManager.instance.EnqueueLoad(Job);
        using UnityWebRequest www = UnityWebRequest.Get(IP + "getPalette.php");
        var req = www.SendWebRequest();

        while (!req.isDone)
        {
            await Task.Yield();
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }
        //Debug.Log("Getting XtermPallet Async Finished");
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);

        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            ColourManager.XTermPaletteJSON = "{ 	\"colours\": 	" + www.downloadHandler.text + "}";
            //OnLoadEnded(GetHTMLColors());


        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task GetHTMLColors()
    {
        string Job = "Downloading HTML Colors";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(IP + "getHTMLColours.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            ColourManager.HTMLColoursJSON = "{ 	\"colours\": 	" + www.downloadHandler.text + "}";

        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task GetVariations()
    {
        string Job = "Downloading color variations";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(IP + "getVariations.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            ColourManager.VariationsJSON = "{ 	\"variations\": 	" + www.downloadHandler.text + "}";

        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task GetColourPacks()
    {
        string Job = "Downloading colorpacks";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(IP + "getColourPacks.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            ColourManager.PacksJSON = "{ 	\"packs\": 	" + www.downloadHandler.text + "}";


        }
        LoadingManager.instance.DequeueLoad(Job);
    }


    public static async Task GetColorNames()
    {
        string Job = "Downloading Color names";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(IP + "getSetColourNames.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            ColourManager.ColorNamesJSON = "{ 	\"ColorName\": 	" + www.downloadHandler.text + "}";
            //Avancar para o proximo


        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task GetColorCorrespondences()
    {
        string Job = "Downloading color correspondences";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(IP + "getColourCorrespondences.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            ColourManager.CorrespondencesJSON = "{ 	\"color\": 	" + www.downloadHandler.text + "}";
            //Avancar para o proximo


        }
        LoadingManager.instance.DequeueLoad(Job);
    }
    public static async Task GetColorSets()
    {
        string Job = "Downloading color sets";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(IP + "getColourSets.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);

            ColourManager.ColourSetsJSON = "{ 	\"couloursets\": 	" + www.downloadHandler.text + "}";
            //Gerar as Paletes todas, fazer so no fim sff


            //Avancar para o proximo


        }

        LoadingManager.instance.DequeueLoad(Job);
    }
    public static async Task GetAvatarParts()
    {
        string Job = "Downloading Avatar parts";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(IP + "getAvatarSets.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Get all avatar parts failed " + www.error);
        }
        else
        {
            // Show results as text
            //Debug.LogError(www.downloadHandler.text);

            AvatarGenerator.AvatarComponentsJSON = "{ 	\"Parts\": 	" + www.downloadHandler.text + "}";

            //Avancar para o proximo


        }
        LoadingManager.instance.DequeueLoad(Job);
    }
    public static async Task GetAllAvatarAccessories()
    {
        string Job = "Downloading Accessories";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(IP + "getAvatarParts.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Get all avatar accessories failed " + www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);
            AvatarAccessoriesManager.AvatarAccsJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";

        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task GetAllAvatarUnlocks()
    {
        string Job = "Downloading Unlocks";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(IP + "getAvatarPartsUnlocks.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Get all avatar accessories failed " + www.error);
        }
        else
        {
            // Show results as text
            //Debug.Log(www.downloadHandler.text);
            AvatarAccessoriesManager.AvatarAccsUnlocksJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";

        }
        LoadingManager.instance.DequeueLoad(Job);
    }
    //public IEnumerator LoadAvatarPartsFromDisk()
    //{
    //    yield return null;
    //    Load_SVG_From_File.LoadFromDisk(FindObjectOfType<GenerateCategories>(), FindObjectOfType<AssetCreatingFromFolder>());

    //    FindObjectOfType<AvatarReader>().GenerateAvatar();
    //}

    public async static Task<Texture2D> GetPart(string Code)
    {

        //Debug.LogError(LoadSVGs.IP + "GetSVG.php" + "?file=" + Code + ".svg");
        UnityWebRequest www = UnityWebRequest.Get(IP + "GetSVG.php" + "?file=" + Code + ".svg");
        var req = www.SendWebRequest();

        while (!req.isDone)
        {
            await Task.Yield();
            //Debug.LogError("Waiting...");
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.LogError("Canceled");
                return null;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.LogError("Canceled");
            return null;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Accessory does not exist on server " + Code + "  " + www.error);
            return null;
        }
        else
        {
            //Debug.LogError("Result for " + Code + ": " + www.downloadHandler.text);
            return CombineTextures.Load_SVG_as_Texture(ColourManager.ReplaceColors(www.downloadHandler.text, Load_SVG_From_File.PrimaryCode, Load_SVG_From_File.SecondaryCode));
        }
    }



    public async static Task<Sprite> GetPartSprite(string Code, bool GetFromServer, bool defaultcolors)
    {
        //Debug.Log("Code " + Code);
        string sprRes;
        try
        {
            if (AvatarAccessoriesManager.Downloaded.TryGetValue(Code, out sprRes))
            {
                //Debug.Log("Already present");
                return CombineTextures.Load_SVG_as_Sprite(ColourManager.ReplaceColors(sprRes, defaultcolors ? Load_SVG_From_File.PrimaryCodeDefault : Load_SVG_From_File.PrimaryCode, defaultcolors ? Load_SVG_From_File.SecondaryCodeDefault : Load_SVG_From_File.SecondaryCode));
            }
            else if (File.Exists(AssetCreatingFromFolder.filepath + Code + ".svg") && !GetFromServer)
            {
                //Debug.Log("File found " + Code);
                StreamReader inp_stm = new StreamReader(AssetCreatingFromFolder.filepath + Code + ".svg");
                string svg = inp_stm.ReadLine();
                inp_stm.Close();
                return CombineTextures.Load_SVG_as_Sprite(ColourManager.ReplaceColors(svg, defaultcolors ? Load_SVG_From_File.PrimaryCodeDefault : Load_SVG_From_File.PrimaryCode, defaultcolors ? Load_SVG_From_File.SecondaryCodeDefault : Load_SVG_From_File.SecondaryCode));
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }


        //Debug.LogError(LoadSVGs.IP + "GetSVG.php" + "?file=" + Code + ".svg");

        UnityWebRequest www = UnityWebRequest.Get(IP + "GetSVG.php" + "?file=" + Code + ".svg");
        var req = www.SendWebRequest();

        while (!req.isDone)
        {
            await Task.Yield();
            //Debug.LogError("Waiting...");
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.LogWarning("Canceled");
                return null;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.LogError("Canceled");
            return null;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            //Debug.LogError("Accessory does not exist on server, the file does not exist " + www.error + " \n Acessory Code" + Code);
            return null;
        }
        else
        {
            //AvatarAccessoriesManager.Downloaded.Add(Code, www.downloadHandler.text);
            //Debug.LogError("Result for " + Code + ": " + www.downloadHandler.text);
            await AssetCreatingFromFolder.SavePartToSVG(www.downloadHandler.text, Code);

            return CombineTextures.Load_SVG_as_Sprite(ColourManager.ReplaceColors(www.downloadHandler.text, Load_SVG_From_File.PrimaryCode, Load_SVG_From_File.SecondaryCode));

        }
    }

    public async Task GetUserColor()
    {
        string Job = "Downloading the Users Color";
        LoadingManager.instance.EnqueueLoad(Job);
        bool done = false;
        UnityMainThread.wkr.AddJob(async () =>
        {
            UnityWebRequest www = UnityWebRequest.Get(IP + "getUserAvatarColours.php?user=" + UserLogin.instance.LogInInfo.user.id);
            var req = www.SendWebRequest();
            while (!req.isDone)
            {
                await Task.Yield();
                if (Cancel.Token.IsCancellationRequested)
                {
                    Debug.Log("Canceled");
                    return;
                }
            }

            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Get all avatar accessories failed " + www.error);
            }
            else
            {
                // Show results as text
                //Debug.Log(www.downloadHandler.text);
                ColourManager.UserColorsJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";
            }
            done = true;
        });
        while (!done)
        {
            await Task.Yield();
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async void SetUserColor()
    {
        UnityWebRequest www = UnityWebRequest.Get(IP + "setUserAvatarColours.php?user=" + UserLogin.instance.LogInInfo.user.id + "&primary=" + Load_SVG_From_File.PrimaryCode + "&secondary=" + Load_SVG_From_File.SecondaryCode);
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (Cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (Cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Colour set failed " + www.error);
        }
        else
        {
            //Debug.Log("Color change success");
        }
    }

    private void OnDisable()
    {
        //if (Cancel != null)
        //    Cancel.Cancel();
    }

    private void OnEnable()
    {
        if (Cancel != null)
            Cancel = new CancellationTokenSource();
    }

}
