using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using UnityEngine.UI;

public class GetCardsRotation : MonoBehaviour
{
    private CardsRotationData cardsRotation;

    [SerializeField]
    private RectTransform directBuy, store;

    [SerializeField]
    private GameObject singleCard, objGroup;

    [SerializeField]
    private CardPopUpComponent cardPopUp;

    [SerializeField]
    private CardSellInfo[] cards;

    async void Awake()
    {
        await getCardsRotation();
    }


    public CancellationTokenSource cancel = new CancellationTokenSource();
    private async Task getCardsRotation()
    {
        UnityWebRequest www = UnityWebRequest.Get(GetDirectories.Instance.directories[DirectoryKey.SERVICE_URL.ToString()] + "getCardRotation.php");

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
            Debug.LogError("Purchase failed " + www.error);
        }
        else
        {
            cardsRotation = JsonUtility.FromJson<CardsRotationData>("{\"cardsRotations\":" + www.downloadHandler.text + "}");

            InstanciateCards();
        }
    }

    private void InstanciateCards()
    {
        cards = new CardSellInfo[cardsRotation.cardsRotations.Length];


        for (int i = 0; i < cardsRotation.cardsRotations.Length; i++)
        {
            cards[i] = Instantiate(singleCard, objGroup.transform).GetComponent<CardSellInfo>();
            cards[i].transform.localScale = Vector3.one;
            AssignCardValues(cards[i], cardsRotation.cardsRotations[i]);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(objGroup.GetComponent<RectTransform>());
        transform.parent.GetComponent<LayoutElementSetMin>().AdjustSize();
        // directBuy.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cardsSpacing);
        // store.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, store.sizeDelta.x + (450*3));
    }

    private void AssignCardValues(CardSellInfo card, CardRotation cardInfo)
    {
        card.card = cardInfo;
        card.UpdateCards();
        Button temp = card.gameObject.GetComponent<Button>();
        temp.onClick.AddListener(delegate { cardPopUp.UpdatePopUp(card); });
        temp.onClick.AddListener(delegate { cardPopUp.gameObject.SetActive(true); });
    }
}
