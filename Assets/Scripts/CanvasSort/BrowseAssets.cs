using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BrowseAssets : MonoBehaviour
{
    public static BrowseAssets instance;


    [SerializeField]
    public GameObject Filter_btns;

    public TextMeshProUGUI Counter;

    [SerializeField]
    private GameObject AllAcc;

    public void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void SelectButtons(string tag)
    {
        foreach (Transform este in Filter_btns.transform)
        {
            este.gameObject.SetActive(este.tag == tag || este.tag == "Ignore");
        }


        
    }

    //public void SortAcc()
    //{


    //    //List<AvatarChangeBtn> all = new List<AvatarChangeBtn>(AllAcc.transform.GetComponentsInChildren<AvatarChangeBtn>());
    //    //Debug.Log("Sorting " + all.Count);
    //    //foreach (Transform este in Filter_btns.transform)
    //    //{
    //    //    if (este.GetComponentInChildren<AccessoryContainer>() != null)
    //    //    {
    //    //        AccessoryContainer res = este.GetComponentInChildren<AccessoryContainer>();
    //    //        foreach (AvatarChangeBtn asd in all.Where(x => x.Type == res.Type))
    //    //        {

    //    //            asd.transform.SetParent(res.gameObject.transform);
    //    //            asd.gameObject.tag = res.Type.GetCategory().GetCategoryDescription();
    //    //            asd.GetComponent<Button>().interactable = asd.Unlocked;
    //    //            if (asd.GetComponent<ButtonOutline>().accGroup == null)
    //    //            {
    //    //                //Debug.LogError("Still null");
    //    //                asd.GetComponent<ButtonOutline>().accGroup = res.gameObject.GetComponent<SelectedButton>();
    //    //                if (asd.GetComponent<ButtonOutline>().accGroup == null)
    //    //                {

    //    //                }
    //    //                if (asd.GetComponent<ButtonOutline>().outline == null)
    //    //                {
    //    //                    Debug.LogError("Oi?");
    //    //                }
    //    //                asd.GetComponent<ButtonOutline>().accGroup.outlines.Add(asd.GetComponent<ButtonOutline>().outline);
    //    //            }
    //    //        }
    //    //        res.UpdateCounter();
    //    //    }
    //    //    else
    //    //    {
    //    //        Debug.LogError("AccessoryContainer Not Found");
    //    //    }
    //    //}


    //}


}
