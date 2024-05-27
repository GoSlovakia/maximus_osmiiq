using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class LoadCards
{
    public static CancellationTokenSource cancel = new CancellationTokenSource();
    private static bool EverLoaded = false;

    public static async Task LoadAll(bool ForceLoad)
    {
        if (!EverLoaded || ForceLoad)
        {
            await GetCardTypes();
            await GetCardDomains();
            await GetCardSets();
            await GetCardSubsets();
            await GetAllCards();
            await GetUserCards();
            EverLoaded = true;
        }

        // await GetUserSets();
    }

    public static async Task GetUserCards()
    {
        string Job = "Getting the Users Cards";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getUserCardInventory.php?user=" + UserLogin.instance.LogInInfo.user.id);
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Get user cards failed " + www.error);
        }
        //Debug.LogError(www.downloadHandler.text);
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            CardManager.UserCardsJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";


            //Avancar para o proximo

        }
        LoadingManager.instance.DequeueLoad(Job);
    }



    public static async Task GetCardTypes()
    {
        string Job = "Getting Card Types";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getCardTypes.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Get all card types failed " + www.error);
        }
        //Debug.LogError(www.downloadHandler.text);
        else
        {
            // Show results as text

            CardManager.CardTypesJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";


            //Avancar para o proximo

        }
        LoadingManager.instance.DequeueLoad(Job);
    }
    public static async Task GetCardSubsets()
    {
        string Job = "Getting Card Subsets";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getCardSubsets.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Get all subsets failed " + www.error);
        }
        //Debug.LogError(www.downloadHandler.text);
        else
        {
            // Show results as text

            CardManager.CardSubsetsJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";


            //Avancar para o proximo

        }
        LoadingManager.instance.DequeueLoad(Job);
    }
    public static async Task GetCardDomains()
    {
        string Job = "Getting Card Domains ";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getCardSetDomains.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Get all domains failed " + www.error);
        }
        //Debug.LogError(www.downloadHandler.text);
        else
        {
            // Show results as text

            CardManager.CardDomainJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";


            //Avancar para o proximo

        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task GetCardSets()
    {
        string Job = "Getting Card Sets ";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getCardSets.php");
        var req = www.SendWebRequest();
        while (!req.isDone)
        {
            await Task.Yield();
            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }
        }

        if (cancel.Token.IsCancellationRequested)
        {
            Debug.Log("Canceled");
            return;
        }

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Get all sets failed " + www.error);
        }
        //Debug.LogError(www.downloadHandler.text);
        else
        {
            // Show results as text

            CardManager.CardSetsJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";


            //Avancar para o proximo

        }
        LoadingManager.instance.DequeueLoad(Job);
    }

    public static async Task GetAllCards()
    {
        string Job = "Getting All cards ";
        LoadingManager.instance.EnqueueLoad(Job);
        UnityWebRequest www = UnityWebRequest.Get(LoadSVGs.IP + "getCards.php");
        if (www.url == "")
        {
            //this is for debuging
            Debug.Log("Using Local");
            CardManager.GenerateCardsFromServer();
        }
        else
        {
            var req = www.SendWebRequest();
            while (!req.isDone)
            {
                await Task.Yield();
                if (cancel.Token.IsCancellationRequested)
                {
                    Debug.Log("Canceled");
                    return;
                }
            }

            if (cancel.Token.IsCancellationRequested)
            {
                Debug.Log("Canceled");
                return;
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Get all Cards failed " + www.error);
            }
            //Debug.LogError(www.downloadHandler.text);
            else
            {
                // Show results as text
                CardManager.AllCardsJSON = "{ 	\"All\": 	" + www.downloadHandler.text + "}";


                //Avancar para o proximo

            }
            
        }
        LoadingManager.instance.DequeueLoad(Job);
    }
}
