using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetJPEG
{
    public static CancellationTokenSource cancel = new CancellationTokenSource();

    public static Dictionary<string, Texture> DownloadedAvatarThumbs= new Dictionary<string, Texture>();
    //public RawImage image;

    /*private async void Start()
    {
      if (GetComponent<CardFromSetContainer>())
      {
        CardFromSetContainer cardInfo = null;
        cardInfo = GetComponent<CardFromSetContainer>();
        image.texture = await GetThumbTexture(cardInfo.Card.id);
        cardInfo.cardType.texture = await GetJPEG.GetCardType(cardInfo.Card.type);
      }
      else if (GetComponent<CardButton>())
      {
        CardButton cardButton = null;
        cardButton = GetComponent<CardButton>();
        image.texture = await GetThumbTexture(cardButton.Card.id);
      }
    }*/

    public static async Task<Texture> GetTexture(string cardID)
    {
        string Job = "Fetching Card Image";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "CardsJPEGs/" + cardID + ".jpg");

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                LoadingManager.instance.DequeueLoad(Job);
                return null;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            LoadingManager.instance.DequeueLoad(Job);
            return null;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Card not found " + www.error);
            LoadingManager.instance.DequeueLoad(Job);
            return null;
        }
        else
        {
            LoadingManager.instance.DequeueLoad(Job);
            return DownloadHandlerTexture.GetContent(www);
        }

    }

    public static async Task<Texture> GetThumbTexture(string cardID)
    {
        string Job = "Fetching Card Thumbnail";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "CardsJPEGs / " + cardID + "_thumb.jpg");

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                LoadingManager.instance.DequeueLoad(Job);
                return null;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            LoadingManager.instance.DequeueLoad(Job);
            return null;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            //Debug.LogError("Card not found " + www.error);
            LoadingManager.instance.DequeueLoad(Job);
            return null;
        }
        else
        {
            LoadingManager.instance.DequeueLoad(Job);
            return DownloadHandlerTexture.GetContent(www);
        }
    }

    public static async Task<Texture> GetJorneyThumbTexture(string lvlID)
    {
        string Job = "Fetching Journey thumbnail texture";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "JourneyThumbs/" + lvlID + ".png");

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                LoadingManager.instance.DequeueLoad(Job);
                return null;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            LoadingManager.instance.DequeueLoad(Job);
            return null;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            //Debug.LogError("Card not found " + www.error);
            LoadingManager.instance.DequeueLoad(Job);
            return null;
        }
        else
        {
            LoadingManager.instance.DequeueLoad(Job);
            return DownloadHandlerTexture.GetContent(www);
        }
    }
    public static async Task<Texture> GetAvatarThumb(string PieceCode)
    {
        try
        {
            string Job = "Fetching Avatar Thumbnail";
            LoadingManager.instance.EnqueueLoad(Job);
            Texture res;
            if (DownloadedAvatarThumbs.TryGetValue(PieceCode, out res))
            {
                LoadingManager.instance.DequeueLoad(Job);
                return res;
            }
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "AvatarThumbs/" + PieceCode + "_thumb.png");

            var req = www.SendWebRequest();
            while (!req.isDone)
            {
                await Task.Yield();
                if (cancel.Token.IsCancellationRequested)
                {
                    Debug.Log("Canceled");
                    LoadingManager.instance.DequeueLoad(Job);
                    return null;
                }
            }

            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                LoadingManager.instance.DequeueLoad(Job);
                return null;
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                // Debug.LogWarning("Thumbnail not found " + www.error);
                LoadingManager.instance.DequeueLoad(Job);
                return null;
            }
            else
            {
                DownloadedAvatarThumbs.Add(PieceCode, DownloadHandlerTexture.GetContent(www));
                LoadingManager.instance.DequeueLoad(Job);
                return DownloadHandlerTexture.GetContent(www);

            }
        }catch(System.Exception e)
        {
            Debug.LogError(e);
            return null;
        }
    }

    public static async Task<Texture> GetCardType(string cardType)
    {
        string Job = "Fetching Card Type image";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "CardTypes/" + cardType + ".png");

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                LoadingManager.instance.DequeueLoad(Job);
                return null;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            LoadingManager.instance.DequeueLoad(Job);
            return null;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Card not found " + www.error);
            LoadingManager.instance.DequeueLoad(Job);
            return null;
        }
        else
        {
            LoadingManager.instance.DequeueLoad(Job);
            return DownloadHandlerTexture.GetContent(www);
        }
    }

    public static async Task<Texture> GetDomainTexture(string domain)
    {
        string Job = "Fetching domain texture";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "CardDomains/" + domain + ".png");

        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                LoadingManager.instance.DequeueLoad(Job);
                return null;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            LoadingManager.instance.DequeueLoad(Job);
            return null;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            //Debug.LogError("Card not found " + www.error);
            LoadingManager.instance.DequeueLoad(Job);
            return null;
        }
        else
        {
            LoadingManager.instance.DequeueLoad(Job);
            return DownloadHandlerTexture.GetContent(www);
        }
    }
}
