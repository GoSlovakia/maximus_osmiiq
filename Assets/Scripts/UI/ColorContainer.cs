using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorContainer : MonoBehaviour
{
    private Color _co;
    public bool Unlocked = true;
    public bool HiddenFromView = false;
    //private Color _h;
    public Sprite lockedIcon;
    public Toggle toggle;
    [SerializeField]
    private Image border;
    public GameObject Checkmark;

    private Coulourset color;
    private bool _selected;

    public Coulourset Colorset
    {
        get => color; set
        {
            color = value;
            CheckUnlocked();
            if (Unlocked)
                NotificationTabManager.instance.ColorCheckNotif(this);
        }
    }
    private bool _highlighted;
    [SerializeField]
    private Color HiglightColor;

    public bool highlighted
    {
        get => _highlighted;
        set
        {
            _highlighted = value;
            if (_highlighted)
            {
                GetComponent<NotificationComponent>().showNotification = true;
                ColorBlock cb = toggle.colors;
                cb.normalColor = HiglightColor;
                toggle.colors = cb;
            }
            else
            {
                GetComponent<NotificationComponent>().showNotification = false;
                ColorBlock cb = toggle.colors;
                cb.normalColor = new Color(1, 1, 1, 0);
                toggle.colors = cb;
            }
        }
    }

    //public ColorOption Color
    //{
    //    get => _co;
    //    set
    //    {
    //        _co = value;
    //        ColorBlock colors = GetComponent<Button>().colors;
    //        colors.normalColor = _co.Main;
    //        colors.highlightedColor = _co.Bright;
    //        colors.pressedColor = _co.Dark;
    //        colors.selectedColor = _co.Bright;
    //        GetComponent<Button>().colors = colors;
    //        //transform.GetComponentInChildren<TextMeshProUGUI>().text = _co.Code;
    //        Code = _co.Code;
    //    }
    //}
    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            toggle.interactable = !value;
            Checkmark.SetActive(value);
            if (value && ColorCategory.Instance != null)
            {

                ColorCategory.Instance.CurrentColor.text = Colorset.ColorName;
                Debug.Log("Cor " + Cor);
                if (AvatarReader.avatarReader.currentColorSet == ColorSet.PRIMARY)
                {
                    ColorCategory.Instance.MainBG.color = Cor;
                }
                else
                {
                    ColorCategory.Instance.SecBG.color = Cor;
                }
                Debug.Log("changing name");
            }
            else
            {
                if (ColorCategory.Instance == null)
                {
                    Debug.Log("No color category found");
                }
            }
            // toggle.image.color = toggle.isOn ? BgSelected : BgDefault;
        }
    }

    public void Start()
    {
        toggle.group = transform.parent.GetComponent<ToggleGroup>();
    }

    public void CheckUnlocked()
    {
        if (!ColourManager.CheckUnlocked(Colorset))
        {
            GetComponent<Image>().sprite = lockedIcon;
            toggle.interactable = false;
            border.color = new Color(0, 0, 0, 0);
            Unlocked = false;
        }
    }

    public Color Cor
    {
        get { return _co; }
        set
        {
            GetComponent<Image>().color = value;
            _co = value;
            ////Debug.Log(value);
            //ColorBlock colors = GetComponent<Button>().colors;
            //colors.normalColor = value;
            //colors.pressedColor = value;
            //colors.selectedColor = value;
            //btn.colors = colors;
            //_co = value;
        }
    }

    //public Color Highlited
    //{
    //    get => _h;
    //    set
    //    {
    //        _h = value;
    //        ColorBlock colors = btn.colors;
    //        colors.highlightedColor = value;
    //        btn.colors = colors;
    //    }
    //}



    public void ChangeColor()
    {
        switch (AvatarReader.avatarReader.currentColorSet)
        {
            case ColorSet.PRIMARY:
                Load_SVG_From_File.PrimaryCode = Colorset.code;
                // ColourManager.userColors.All[0].PrimaryColor = Color.code;
                break;
            case ColorSet.SECONDARY:
                Load_SVG_From_File.SecondaryCode = Colorset.code;
                // ColourManager.userColors.All[0].SecondaryColor = Color.code;
                //AvatarReader.display.material.SetColor("Color2", Color.Main);
                break;
        }
        AvatarReader.ReloadAvatar();
        ExportAvatar.instance.Saved = false;
        if (highlighted)
            NotificationTabManager.instance.RemoveColorNotif(this);
    }
}
