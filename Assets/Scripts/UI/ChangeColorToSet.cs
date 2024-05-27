using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorToSet : MonoBehaviour
{
    [SerializeField]
    private ColorSet novo;
    [SerializeField]
    private Toggle toggle;
    private bool _selected;
    private GenerateColorPalettes Allbuttons;


    // Start is called before the first frame update
    public void Change()
    {
        AvatarReader.avatarReader.currentColorSet = novo;

        string code = novo == ColorSet.PRIMARY ? Load_SVG_From_File.PrimaryCode : Load_SVG_From_File.SecondaryCode;

        foreach (var este in Allbuttons.Colors)
        {
            if (este.Colorset.code == code)
            {
                este.Selected = true;
                este.toggle.interactable = false;
                este.toggle.isOn = true;
                ColorCategory.Instance.CurrentColor.text = este.Colorset.ColorName;

            }
            else
            {
                este.Selected = false;
                este.toggle.interactable = true;
                este.toggle.isOn = false;
            }
        }
    }
    private void Start()
    {
        Allbuttons = FindObjectOfType<GenerateColorPalettes>();

        ColorCategory.Instance.Counter.text = "(" + (ColourManager.colourSets.couloursets.Where(x => ColourManager.CheckUnlocked(x)).Count() / 3) + "/" + (ColourManager.colourSets.couloursets.Where(x => !x.code.StartsWith("C0") && !x.code.StartsWith("C1")).Count() / 3) + ")";

        if (ColourManager.userColors.All.Count() > 0)
        {
            Coulourset primary = ColourManager.colourSets.couloursets.Where(x => x.code == ColourManager.userColors.All[0].PrimaryColor).First();

            ColorCategory.Instance.MainBG.color = new Color(primary.red / 255f, primary.green / 255f, primary.blue / 255f, 1);

            Coulourset sec = ColourManager.colourSets.couloursets.Where(x => x.code == ColourManager.userColors.All[0].SecondaryColor).First();
            ColorCategory.Instance.SecBG.color = new Color(sec.red / 255f, sec.green / 255f, sec.blue / 255f, 1);
        }
        else
        {
            
            Coulourset primary = ColourManager.colourSets.couloursets.Where(x => x.code == Load_SVG_From_File.PrimaryCodeDefault).First();

            ColorCategory.Instance.MainBG.color = new Color(primary.red / 255f, primary.green / 255f, primary.blue / 255f, 1);

            Coulourset sec = ColourManager.colourSets.couloursets.Where(x => x.code == Load_SVG_From_File.SecondaryCodeDefault).First();
            ColorCategory.Instance.SecBG.color = new Color(sec.red / 255f, sec.green / 255f, sec.blue / 255f, 1);


        }
    }
    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            toggle.interactable = !value;
        }
    }
}
