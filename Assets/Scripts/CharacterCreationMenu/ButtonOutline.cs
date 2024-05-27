using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SelectedButton accGroup;
    public Color normal, selected;
    public Button button;
    public Image outline;
    public bool isPressed;


    public void Start()
    {
        button = GetComponent<Button>();
        normal = button.colors.highlightedColor;
        selected = button.colors.selectedColor;

        if (GetComponentInParent<SelectedButton>() != null)
        {
            accGroup = GetComponentInParent<SelectedButton>();
            accGroup.outlines.Add(outline);
        }
        else
        {
            // Debug.LogError("Null");
        }

        if (outline != null)
            outline.enabled = false;
        else
            Debug.LogError("Null");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outline != null)
        {
            outline.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outline != null)
        {
            if (isPressed)
                outline.enabled = true;
            else
                outline.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (accGroup == null)
        {
            accGroup = GetComponentInParent<SelectedButton>();

            accGroup.outlines.Add(outline);
            accGroup.ChangeOutline(outline, isPressed, normal, button.colors);

        }
        else
            accGroup.ChangeOutline(outline, isPressed, normal, button.colors);

        outline.enabled = true;

        if (isPressed)
        {
            //isPressed = false;
            //ColorBlock colorBlock = button.colors;
            //colorBlock.normalColor = normal;
            //colorBlock.selectedColor = normal;
            //button.colors = colorBlock;
            Deselect();
        }
        else
        {
            //isPressed = true;
            //ColorBlock colorBlock = button.colors;
            //colorBlock.normalColor = selected;
            //colorBlock.selectedColor = selected;
            //button.colors = colorBlock;
            SetToSelected();
        }

    }

    private void SetToSelected()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        isPressed = true;
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = selected;
        colorBlock.selectedColor = selected;
        button.colors = colorBlock;
    }

    private void Deselect()
    {

        isPressed = false;
        ColorBlock colorBlock = new ColorBlock();

        if (button != null)
        {
            colorBlock = button.colors;
        }
        else
        {
            button = GetComponent<Button>();
            colorBlock = button.colors;
        }
        colorBlock.normalColor = normal;
        colorBlock.selectedColor = normal;

        button.colors = colorBlock;
    }

    public void MachineSelect()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        UnityMainThread.wkr.AddJob(() =>
        {
            // Debug.Log("Match! " + GetComponent<AvatarChangeBtn>().Acc.Type.GetDescription());
            if (accGroup == null)
            {

                accGroup = GetComponentInParent<SelectedButton>();
                if (accGroup == null)
                {
                    //Debug.Log("accgroup null " + (GetComponentInParent<SelectedButton>() == null) + " parent " + transform.parent.name);
                }
                else
                {
                    accGroup.outlines.Add(outline);
                    accGroup.ChangeOutline(outline, isPressed, normal, button.colors);
                }
            }
            else
                accGroup.ChangeOutline(outline, isPressed, normal, button.colors);

            outline.enabled = true;
            SetToSelected();
        });

    }

    public void MachineDeselect()
    {

        //Debug.Log("Deselect! " + GetComponent<AvatarChangeBtn>().Acc.Type.GetDescription());
        if (accGroup == null)
        {
            UnityMainThread.wkr.AddJob(() =>
            {
                accGroup = GetComponentInParent<SelectedButton>();
                if (accGroup != null)
                {

                    accGroup.outlines.Add(outline);
                }
            });
        }
        UnityMainThread.wkr.AddJob(() =>
        {
            outline.enabled = false;
            Deselect();
        });

    }

    public static void ClearAllSelections()
    {
        //foreach (AvatarChangeBtn este in AssetCreatingFromFolder.AllAccBtns.Where(x => x.ButtonOutline.isPressed))
        //{
        //    este.ButtonOutline.MachineDeselect();
        //}
    }

    //public static async Task SelectThisAccessoryFromFresh(Accessory novo)
    //{
    //    //while (AssetCreatingFromFolder.AllAccBtns == null)
    //    //{
    //    //    await Task.Yield();
    //    //}
    //    //foreach (AvatarChangeBtn este in AssetCreatingFromFolder.AllAccBtns.Where(x => x.Acc.Type == novo.Type))
    //    //{
    //    //    if (este.Acc == novo)
    //    //    {
    //    //        // Debug.Log("Selecting");
    //    //        este.ButtonOutline.MachineSelect();

    //    //    }
    //    //    else
    //    //    {
    //    //        // Debug.Log("Deselecting");
    //    //        este.ButtonOutline.MachineDeselect();
    //    //    }
    //    //}
    //}

    public static void SelectThisAccessory(AvatarAcc novo)
    {
        //foreach (AvatarChangeBtn este in AssetCreatingFromFolder.AllAccBtns.Where(x => x.Acc.Type == novo.Type))
        //{
        //    este.ButtonOutline.MachineSelect();
        //}
    }
}
