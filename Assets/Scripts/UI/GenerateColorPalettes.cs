using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;

public class GenerateColorPalettes : MonoBehaviour
{
    public static GenerateColorPalettes instance;
    public GameObject ColorGroupPrefab;
    public GameObject ButtonPrefab;

    public List<ColorContainer> Colors = new List<ColorContainer>();
    //public GameObject ColorSetsPrefab;


    //public static List<ColorOption> All = new List<ColorOption>();


    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void GeneratePallet()
    {
        //nabo
        //All.Add(new ColorOption("ST01", new Color32(210, 245, 249, 255), new Color32(178, 229, 237, 255), new Color32(110, 179, 186, 255)));
        //All.Add(new ColorOption("ST02", new Color32(255, 250, 222, 255), new Color32(234, 228, 178, 255), new Color32(193, 182, 116, 255)));
        //All.Add(new ColorOption("ST03", new Color32(217, 255, 221, 255), new Color32(179, 232, 187, 255), new Color32(124, 191, 132, 255)));
        //All.Add(new ColorOption("EX0101", new Color32(212, 195, 182, 255), new Color32(190, 164, 146, 255), new Color32(169, 134, 109, 255)));
        //All.Add(new ColorOption("EX0102", new Color32(243, 221, 206, 255), new Color32(236, 205, 182, 255), new Color32(224, 171, 133, 255)));
        //All.Add(new ColorOption("EX0103", new Color32(255, 229, 223, 255), new Color32(255, 203, 199, 255), new Color32(229, 168, 165, 255)));
        //All.Add(new ColorOption("EX0104", new Color32(189, 163, 196, 255), new Color32(173, 140, 181, 255), new Color32(130, 105, 136, 255)));
        //All.Add(new ColorOption("EX0105", new Color32(188, 185, 214, 255), new Color32(154, 150, 193, 255), new Color32(120, 115, 173, 255)));
        //All.Add(new ColorOption("EX0106", new Color32(169, 186, 203, 255), new Color32(111, 140, 168, 255), new Color32(83, 105, 126, 255)));
        //All.Add(new ColorOption("EX0107", new Color32(177, 195, 195, 255), new Color32(138, 165, 164, 255), new Color32(99, 135, 134, 255)));
        //All.Add(new ColorOption("EX0108", new Color32(199, 206, 185, 255), new Color32(162, 173, 138, 255), new Color32(122, 130, 104, 255)));
        //All.Add(new ColorOption("EX0109", new Color32(240, 240, 238, 255), new Color32(225, 225, 222, 255), new Color32(211, 211, 205, 255)));
        //All.Add(new ColorOption("EX0110", new Color32(211, 211, 205, 255), new Color32(181, 181, 172, 255), new Color32(136, 136, 129, 255)));
        //All.Add(new ColorOption("EX0111", new Color32(104, 104, 99, 255), new Color32(91, 91, 86, 255), new Color32(79, 79, 75, 255)));





        //GameObject Cs = Instantiate(ColorSetsPrefab, transform);
        //Cs.tag = "Color";
        //Cs.transform.GetChild(0).GetComponent<Button>().interactable = false;

        GameObject Group = Instantiate(ColorGroupPrefab, transform);
        Group.tag = "Color";
        string code;
        if (ColourManager.userColors.All.Count() > 0)
        {
            code = ColourManager.userColors.All[0].PrimaryColor;
        }
        else
        {
            code = Load_SVG_From_File.PrimaryCodeDefault;
        }

        foreach (var este in ColourManager.colourSets.couloursets)
        {
            if (este.code.Length != 3 && este.variation == 1)
            {
                //Debug.Log(este.code);
                GameObject novo = Instantiate(ButtonPrefab, Group.transform.GetChild(2).GetChild(0));
                ColorContainer novoCont = novo.GetComponent<ColorContainer>();
                novoCont.Cor = new Color(este.red / 255f, este.green / 255f, este.blue / 255f);
                Color versaobright = new Color(ColourManager.colourSets.couloursets.Where(x => x.code == este.code && x.variation == 0).First().red, ColourManager.colourSets.couloursets.Where(x => x.code == este.code && x.variation == 0).First().green, ColourManager.colourSets.couloursets.Where(x => x.code == este.code && x.variation == 0).First().blue);
                // novo.GetComponent<ColorContainer>().Highlited = versaobright;
                novoCont.Colorset = este;
                Colors.Add(novoCont);

                if (este.code == code)
                {
                    novoCont.Selected = true;
                    novoCont.toggle.interactable = false;
                    novoCont.toggle.isOn = true;

                }
                else
                {
                    novoCont.Selected = false;
                    novoCont.toggle.interactable = true;
                    novoCont.toggle.isOn = false;
                }
            }

        }
        ExportAvatar.instance.Saved = true;


    }
}
