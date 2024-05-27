using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.Core.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

public class GenerateCategories : MonoBehaviour
{
  public GameObject TitlePrefab;
  public GameObject AccContainerPrefab;
  public LocalizedString[] Descriptions;

  public static List<AccessoryContainer> AllCategories = new List<AccessoryContainer>();

  // Start is called before the first frame update

  private void Start()
  {
    AllCategories = new List<AccessoryContainer>();
  }
  public void GenerateAll()
  {
    GameObject novo;
    GameObject novo2;
    foreach (AccType este in (AccType[])System.Enum.GetValues(typeof(AccType)))
    {
      novo = Instantiate(TitlePrefab, transform);
      //novo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = este.GetDescription();
      novo.tag = este.GetCategory().GetCategoryDescription();
      LocalizeStringEvent temp = novo.transform.GetChild(0).GetChild(0).GetComponent<LocalizeStringEvent>();
      switch (este.GetDescription())
      {
        case "Crown":
          temp.StringReference = Descriptions[0];
          break;
        case "Head Accessory":
          temp.StringReference = Descriptions[1];
          break;
        case "Hair":
          temp.StringReference = Descriptions[2];
          break;
        case "Eyebrows":
          temp.StringReference = Descriptions[3];
          break;
        case "Glasses":
          temp.StringReference = Descriptions[4];
          break;
        case "Eyes":
          temp.StringReference = Descriptions[5];
          break;
        case "Head Shape":
          temp.StringReference = Descriptions[6];
          break;
        case "Nose":
          temp.StringReference = Descriptions[7];
          break;
        case "Features":
          temp.StringReference = Descriptions[8];
          break;
        case "Sides":
          temp.StringReference = Descriptions[9];
          break;
        case "Mouth Feature":
          temp.StringReference = Descriptions[10];
          break;
        case "Mouth":
          temp.StringReference = Descriptions[11];
          break;
        case "Chin":
          temp.StringReference = Descriptions[12];
          break;
        case "Jaw":
          temp.StringReference = Descriptions[13];
          break;
        case "Left Hand":
          temp.StringReference = Descriptions[14];
          break;
        case "Right Hand":
          temp.StringReference = Descriptions[15];
          break;
        case "Body Back":
          temp.StringReference = Descriptions[16];
          break;
        case "Tail":
          temp.StringReference = Descriptions[17];
          break;
        case "Body Feature":
          temp.StringReference = Descriptions[18];
          break;
        case "Body Cover":
          temp.StringReference = Descriptions[19];
          break;
        case "Body Shape":
          temp.StringReference = Descriptions[20];
          break;
      }

      novo2 = Instantiate(AccContainerPrefab, transform);
      novo2.GetComponent<AccessoryContainer>().Type = este;
      novo2.GetComponent<AccessoryContainer>().Counter = novo.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
      novo2.GetComponent<AccessoryContainer>().PartName = novo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
      novo2.tag = este.GetCategory().GetCategoryDescription();
      AllCategories.Add(novo2.GetComponent<AccessoryContainer>());
      if (novo2.GetComponent<AccessoryContainer>().Type == AccType.FHL || novo2.GetComponent<AccessoryContainer>().Type == AccType.FHR)
      {
        // novo2.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedRowCount;
        novo2.GetComponent<GridLayoutGroup>().constraintCount = 3;
        novo2.GetComponent<GridLayoutGroup>().cellSize = new Vector2(125, 250);
      }
    }

    GetComponent<GenerateColorPalettes>().GeneratePallet();
    BrowseAssets.instance.SelectButtons("Head");
  }

}
