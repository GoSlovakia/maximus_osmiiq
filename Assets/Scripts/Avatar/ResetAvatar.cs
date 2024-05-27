using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ResetAvatar : MonoBehaviour
{
    [SerializeField]
    private GenerateColorPalettes colorPalettes;

    public void Reset()
    {
        AvatarReader.LoadAvatar();
    }
    private void Awake()
    {
        colorPalettes = FindObjectOfType<GenerateColorPalettes>();
    }

    public void ResettoDefault()
    {
        AvatarReader.LoadDefaultAvatar();
        ExportAvatar.instance.Saved = false;

        string code = "";

        Coulourset primary = ColourManager.colourSets.couloursets.Where(x => x.code == Load_SVG_From_File.PrimaryCodeDefault).First();

        ColorCategory.Instance.MainBG.color = new Color(primary.red / 255f, primary.green / 255f, primary.blue / 255f, 1);

        Coulourset sec = ColourManager.colourSets.couloursets.Where(x => x.code == Load_SVG_From_File.SecondaryCodeDefault).First();
        ColorCategory.Instance.SecBG.color = new Color(sec.red / 255f, sec.green / 255f, sec.blue / 255f, 1);

        switch (AvatarReader.avatarReader.currentColorSet)
        {
            case ColorSet.PRIMARY:
                code = Load_SVG_From_File.PrimaryCodeDefault;
                break;
            case ColorSet.SECONDARY:
                code = Load_SVG_From_File.SecondaryCodeDefault;
                break;
        }


        Debug.Log("Color Reset ");
        foreach (var este in colorPalettes.Colors)
        {
            if (este.Colorset.code == code)
            {
                este.Selected = true;
                este.toggle.interactable = false;
                este.toggle.isOn = true;

            }
            else
            {
                este.Selected = false;
                este.toggle.interactable = true;
                este.toggle.isOn = false;
            }


        }
    }

}
