using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(SpriteRenderer))]
public class AvatarPart : MonoBehaviour
{
    public AvatarAcc Acc;

    private Vector3 size;

    [SerializeField]
    private AccType _part;


    public AccType Part { get => _part; }
    public SpriteRenderer SRenderer;

    private void Start()
    {

        {
            SRenderer = GetComponent<SpriteRenderer>();
            //size = SRenderer.sprite.bounds.size;
        }
    }

    public void Clear()
    {
        Acc = null;
        if (SRenderer != null)
            SRenderer.sprite = null;
    }

    public async void SetPart(AvatarAcc novo)
    {
        Sprite res;
        AssetBundleCacher.Instance.avatarsvgs.TryGetValue(novo.id, out res);
        //Debug.Log(res);
        //Debug.Log("Part id " + novo.id);
        Sprite spr = await LoadSVGs.GetPartSprite(novo.id, false, false);

        //Texture2D tex = await LoadSVGs.GetPart(novo.Code);
        if (spr != null)
        {
            // Debug.Log("spr size " + spr.bounds.max);
            Acc = novo;
            //Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one / 2f);
            if (SRenderer != null)
            {
                SRenderer.sprite = spr;
                SRenderer.sprite.name = novo.id;
                SRenderer.GetComponent<RectTransform>().sizeDelta = new Vector2(SRenderer.sprite.rect.size.x / 7f, SRenderer.sprite.rect.size.y / 7f);
                //SRenderer.GetComponent<RectTransform>().pivot = SRenderer.sprite.pivot;
            }


            //transform.localScale = new Vector3(0.13f, 0.13f, 1);
        }
        else
        {
            //Debug.LogError("Invalid piece! " + novo.Code);
        }
    }

    //public void SetPart(Texture2D novo, string Code)
    //{
    //  Acc = new Accessory(Part, Code);
    //  Sprite sprite = Sprite.Create(novo, new Rect(0, 0, novo.width, novo.height), Vector2.one / 2f);
    //  if (SRenderer != null)
    //  {
    //    SRenderer.sprite = sprite;
    //    //SRenderer.GetComponent<RectTransform>().sizeDelta = new Vector2(SRenderer.GetComponent<RectTransform>().sizeDelta.x / 2f, SRenderer.GetComponent<RectTransform>().sizeDelta.y / 2f);
    //    //transform.localScale = new Vector3(0.13f, 0.13f, 1);
    //  }
    //}

}
