using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PackSumary : MonoBehaviour
{
  [SerializeField]
  private TextMeshProUGUI duplicates, newsText;
  [SerializeField]
  private RectTransform content;
  [SerializeField]
  private SummaryCard summaryCard;
  [SerializeField]
  private ViewCards viewCards;
  public int dups, news;


  private void OnEnable()
  {
    foreach (Card card in viewCards.boosterCardList)
    {
      SummaryCard temp = Instantiate(summaryCard,content);
      GetJpeg(card,temp);


      if (card.copies > 1)
      {
        ++dups;
      }
      else
      {
        ++news;
      }
    }
    if(viewCards.boosterCardList.Count >= 10)
      content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, content.rect.xMax + (30 * viewCards.boosterCardList.Count));

    if(news == 0)
    {
      newsText.color = new Color(255.0f/255.0f,204.0f/255.0f,49.0f/255.0f);
      duplicates.enabled = false;
      newsText.color = Color.white;
      newsText.text = dups + " duplicates!";
    }
    else if(dups == 0)
    {
      newsText.color = new Color(255.0f / 255.0f, 204.0f / 255.0f, 49.0f / 255.0f);
      duplicates.enabled = false;
      newsText.text = news + " new cards!";
    }
    else
    {
      duplicates.enabled = true;
      newsText.color = new Color(255.0f / 255.0f, 204.0f / 255.0f, 49.0f / 255.0f);
      newsText.text = news + " new cards";
      duplicates.text = "and " + dups + " duplicates!";
    }
    dups = 0;
    news = 0;

    viewCards.boosterCardList.Clear();
  }

  private void OnDisable()
  {
    for (int i = 0; i < content.childCount; i++)
    {
      Destroy(content.GetChild(i).gameObject);
    }
    content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 720);
  }

  void GetJpeg(Card card,SummaryCard summaryCard)
  {
    //summaryCard.rawImage.texture = await GetJPEG.GetThumbTexture(card.id);
    if (card.id != "PC0012")
       summaryCard.rawImage.texture = AssetBundleCacher.Instance.cards[card.id + "_thumb"].texture;
    else
      summaryCard.rawImage.texture = AssetBundleCacher.Instance.cards[card.id + "_thum"].texture;
  }
}
