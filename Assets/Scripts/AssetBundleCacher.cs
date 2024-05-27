using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Pipes;
using System;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SocialPlatforms;
using UnityEngine.Localization;
using JetBrains.Annotations;

public class AssetBundleCacher : MonoBehaviour
{
    public static AssetBundleCacher Instance;
    public string manifestBundleName = "AssetBundles";
    public readonly string CacheFolder = "Osmiiq_Cache";
    public string url, manifestBundleURL;
    public bool NoCache = false;
    [SerializeField]
    private bool clearCache;
    public DownloadWindow downloadWindow;
    public Sprite[] cardSprites, avatarpartsthumbsSprites, cardsdomainsSprites, cardstypesSprites, journeythumbsSprites, avatarsvgsSprites;
    public Dictionary<string, Sprite> cards, avatarpartsthumbs, cardsdomains, cardstypes, journeythumbs, avatarsvgs;
    public AssetBundle manifestBundle;
    public AssetBundleManifest manifest;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        downloadWindow.gameObject.SetActive(false);
    }
    void Start()
    {

        DontDestroyOnLoad(gameObject);
        cards = new Dictionary<string, Sprite>();
        avatarpartsthumbs = new Dictionary<string, Sprite>();
        cardsdomains = new Dictionary<string, Sprite>();
        cardstypes = new Dictionary<string, Sprite>();
        journeythumbs = new Dictionary<string, Sprite>();
        avatarsvgs = new Dictionary<string, Sprite>();

        //var filestream = new FileStream(Path.Combine(Application.persistentDataPath, "AssetBundles"), FileMode.Open, FileAccess.Read);
        //var myLoadedAssetBundle = AssetBundle.LoadFromStream(filestream);
        //Debug.Log(Caching.GetCacheByPath(Application.persistentDataPath+ "/AssetBundleManifest").spaceOccupied + " Caches");



        if (clearCache)
            Caching.ClearCache();
    }

    public IEnumerator CheckForCache(string url)
    {
        //Debug.Log(Path.Combine(url, manifestBundleName));
        //UnityWebRequest request = UnityWebRequest.Head(Path.Combine(url, manifestBundleName));

        //yield return request.SendWebRequest();
        //string size = request.GetResponseHeader("Content-Length");

        float totalSizeMB = 0;


        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(Path.Combine(url, manifestBundleName), 0);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            manifestBundle = DownloadHandlerAssetBundle.GetContent(request);
            manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

            foreach (string bundleName in manifest.GetAllAssetBundles())
            {

                string uri = Path.Combine(url, bundleName);
                Debug.Log(uri);
                request = UnityWebRequest.Head(Path.Combine(uri));

                yield return request.SendWebRequest();
                totalSizeMB += Convert.ToInt64(request.GetResponseHeader("Content-Length")) / (1024f * 1024f);
            }

            downloadWindow.gameObject.SetActive(true);
            downloadWindow.InfoBox.text = totalSizeMB.ToString("0.00") + " MB";

        }

    }
    public void StartDownloadingNewCache()
    {
        StartCoroutine(DownloadNewCache());
    }
    public IEnumerator DownloadNewCache()
    {
        string Job = "Downloading asset bundles";
        LoadingManager.instance.EnqueueLoad(Job);



        //Download of the manifest
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(Path.Combine(url, manifestBundleName), 0);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            //Load the manifest
            string today = DateTime.Today.ToLongDateString();

            Debug.Log("test");

            //System.IO.Directory.CreateDirectory(today);
            //Create new cache
            string path = manifestBundleURL + "/" + CacheFolder;
            if (!Directory.Exists(path))
            {

                Debug.LogWarning("Creating directory!");
                Directory.CreateDirectory(path);
            }

            Cache newCache = Caching.AddCache(path);

            if (newCache.valid)
                Caching.currentCacheForWriting = newCache;


            //Set current cache for writing to the new cache if the cache is valid

            foreach (string bundleName in manifest.GetAllAssetBundles())
            {

                string uri = Path.Combine(url, bundleName);
                //Download the bundle
                Hash128 hash = manifest.GetAssetBundleHash(bundleName);
                request = UnityWebRequestAssetBundle.GetAssetBundle(uri, hash, 0);
                yield return request.SendWebRequest();
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

                //Get all the cached versions
                List<Hash128> listOfCachedVersions = new();
                Caching.GetCachedVersions(bundle.name, listOfCachedVersions);

                //check if the cached version is equal to the one on server
                if (!AssetBundleContainsAssetIWantToLoad(uri, listOfCachedVersions[0]))     //Or any conditions you want to check on your new asset bundle
                {
                    Debug.LogWarning("Removing Cache");
                    //If our criteria wasn't met, we can remove the new cache and revert back to the most recent one
                    Caching.currentCacheForWriting = Caching.GetCacheAt(Caching.cacheCount - 1);
                    Caching.RemoveCache(newCache);
                    for (int i = listOfCachedVersions.Count - 1; i > 0; i--)
                    {
                        //Load a different bundle from a different cache
                        request = UnityWebRequestAssetBundle.GetAssetBundle(uri, listOfCachedVersions[i], 0);
                        yield return request.SendWebRequest();
                        bundle = DownloadHandlerAssetBundle.GetContent(request);

                        //Check and see if the newly loaded bundle from the cache meets your criteria
                        if (AssetBundleContainsAssetIWantToLoad(uri, listOfCachedVersions[0]))
                            break;
                    }
                }
                else
                {
                    ////This is if we only want to keep X local caches at any time
                    //if (Caching.cacheCount > 3)
                    //{
                    //    Caching.RemoveCache(Caching.GetCacheAt(0));
                    //    Debug.Log("Too many caches, deleting last");
                    //    //Removes the oldest user created cache
                    //}
                }
                switch (bundleName)
                {
                    case "cardsbundle":
                        cardSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < cardSprites.Length; i++)
                        {
                            cards.Add(cardSprites[i].name, cardSprites[i]);
                        }
                        break;
                    case "avatarpartsthumbsbundle":
                        avatarpartsthumbsSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < avatarpartsthumbsSprites.Length; i++)
                        {
                            avatarpartsthumbs.Add(avatarpartsthumbsSprites[i].name, avatarpartsthumbsSprites[i]);
                        }
                        break;
                    case "cardsdomainsbundle":
                        cardsdomainsSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < cardsdomainsSprites.Length; i++)
                        {
                            cardsdomains.Add(cardsdomainsSprites[i].name, cardsdomainsSprites[i]);
                        }
                        break;
                    case "cardstypesbundle":
                        cardstypesSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < cardstypesSprites.Length; i++)
                        {
                            cardstypes.Add(cardstypesSprites[i].name, cardstypesSprites[i]);
                        }
                        break;
                    case "journeythumbsbundle":
                        journeythumbsSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < journeythumbsSprites.Length; i++)
                        {
                            journeythumbs.Add(journeythumbsSprites[i].name, journeythumbsSprites[i]);
                        }
                        break;
                    case "avatarsvgsbundle":
                        avatarsvgsSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < avatarsvgsSprites.Length; i++)
                        {
                            avatarsvgs.Add(avatarsvgsSprites[i].name, avatarsvgsSprites[i]);
                        }
                        break;
                }
            }
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public IEnumerator DownloadAndCacheAssetBundle(string url, string manifestBundleURL)
    {
        string Job = "Downloading asset bundles";
        LoadingManager.instance.EnqueueLoad(Job);
        this.url = url;
        this.manifestBundleURL = manifestBundleURL;

        Cache local;
        if (Directory.Exists(Path.Combine(Application.persistentDataPath, CacheFolder)))
        {
            local = Caching.AddCache(Application.persistentDataPath + "/" + CacheFolder);

            if (!local.valid)
            {
                Debug.LogWarning("No cache found! " + (Application.persistentDataPath + "/" + CacheFolder));
                NoCache = true;
                StartCoroutine(CheckForCache(url));
                LoadingManager.instance.DequeueLoad(Job);
                yield break;

            }
        }
        else
        {
            Debug.LogWarning("No cache found! " + (Application.persistentDataPath + "/" + CacheFolder));
            NoCache = true;
            StartCoroutine(CheckForCache(url));
            LoadingManager.instance.DequeueLoad(Job);
            yield break;
        }

        Debug.Log(Caching.cacheCount);
        Hash128 test = new Hash128();
        test.Append(local.GetHashCode());
        Debug.Log(test.ToString());
        //Download of the manifest


        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(Path.Combine(url, manifestBundleName), test);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            Debug.LogWarning("No cache found! " + (Application.persistentDataPath + "/" + CacheFolder));
            NoCache = true;
            StartCoroutine(CheckForCache(url));
            LoadingManager.instance.DequeueLoad(Job);
            yield break;
        }
        else
        {
            //Load the manifest
            manifestBundle = DownloadHandlerAssetBundle.GetContent(request);
            manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            //string today = DateTime.Today.ToLongDateString();



            //System.IO.Directory.CreateDirectory(today);
            //Create new cache
            string path = manifestBundleURL + "/" + CacheFolder;
            if (!Directory.Exists(path))
            {

                Debug.LogWarning("Creating directory!");
                Directory.CreateDirectory(path);
            }

            if (local.valid)
                Caching.currentCacheForWriting = local;


            //Set current cache for writing to the new cache if the cache is valid

            foreach (string bundleName in manifest.GetAllAssetBundles())
            {

                string uri = Path.Combine(url, bundleName);
                //Download the bundle
                Hash128 hash = manifest.GetAssetBundleHash(bundleName);
                request = UnityWebRequestAssetBundle.GetAssetBundle(uri, hash, 0);
                yield return request.SendWebRequest();
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

                //Get all the cached versions
                List<Hash128> listOfCachedVersions = new();
                Caching.GetCachedVersions(bundle.name, listOfCachedVersions);

                //check if the cached version is equal to the one on server
                if (!AssetBundleContainsAssetIWantToLoad(uri, listOfCachedVersions[0]))     //Or any conditions you want to check on your new asset bundle
                {
                    Debug.LogWarning("Removing Cache");
                    //If our criteria wasn't met, we can remove the new cache and revert back to the most recent one
                    Caching.currentCacheForWriting = Caching.GetCacheAt(Caching.cacheCount - 1);
                    Caching.RemoveCache(local);
                    for (int i = listOfCachedVersions.Count - 1; i > 0; i--)
                    {
                        //Load a different bundle from a different cache
                        request = UnityWebRequestAssetBundle.GetAssetBundle(uri, listOfCachedVersions[i], 0);
                        yield return request.SendWebRequest();
                        bundle = DownloadHandlerAssetBundle.GetContent(request);

                        //Check and see if the newly loaded bundle from the cache meets your criteria
                        if (AssetBundleContainsAssetIWantToLoad(uri, listOfCachedVersions[0]))
                            break;
                    }
                }
                else
                {
                    ////This is if we only want to keep X local caches at any time
                    //if (Caching.cacheCount > 3)
                    //{
                    //    Caching.RemoveCache(Caching.GetCacheAt(0));
                    //    Debug.Log("Too many caches, deleting last");
                    //    //Removes the oldest user created cache
                    //}
                }
                switch (bundleName)
                {
                    case "cardsbundle":
                        cardSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < cardSprites.Length; i++)
                        {
                            cards.Add(cardSprites[i].name, cardSprites[i]);
                        }
                        break;
                    case "avatarpartsthumbsbundle":
                        avatarpartsthumbsSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < avatarpartsthumbsSprites.Length; i++)
                        {
                            avatarpartsthumbs.Add(avatarpartsthumbsSprites[i].name, avatarpartsthumbsSprites[i]);
                        }
                        break;
                    case "cardsdomainsbundle":
                        cardsdomainsSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < cardsdomainsSprites.Length; i++)
                        {
                            cardsdomains.Add(cardsdomainsSprites[i].name, cardsdomainsSprites[i]);
                        }
                        break;
                    case "cardstypesbundle":
                        cardstypesSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < cardstypesSprites.Length; i++)
                        {
                            cardstypes.Add(cardstypesSprites[i].name, cardstypesSprites[i]);
                        }
                        break;
                    case "journeythumbsbundle":
                        journeythumbsSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < journeythumbsSprites.Length; i++)
                        {
                            journeythumbs.Add(journeythumbsSprites[i].name, journeythumbsSprites[i]);
                        }
                        break;
                    case "avatarsvgsbundle":
                        avatarsvgsSprites = bundle.LoadAllAssets<Sprite>();
                        for (int i = 0; i < avatarsvgsSprites.Length; i++)
                        {
                            avatarsvgs.Add(avatarsvgsSprites[i].name, avatarsvgsSprites[i]);
                        }
                        break;
                }
            }
        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    /*bool AssetBundleContainsAssetIWantToLoad(AssetBundle bundle)
    {
      return (bundle.LoadAsset<GameObject>("MyAsset") != null);     //this could be any conditional
    }*/

    bool AssetBundleContainsAssetIWantToLoad(string uri, Hash128 hash)
    {
        return Caching.IsVersionCached(uri, hash);
    }


}
