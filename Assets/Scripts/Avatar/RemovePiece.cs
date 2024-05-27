using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemovePiece : MonoBehaviour
{
    [SerializeField]
    private bool allowchanges = false;
    public AccessoryContainer container;
    public Toggle toggle;
    private bool _selected;

    [SerializeField]
    private Image BG;
    [SerializeField]
    private Color BgSelected;
    [SerializeField]
    private Color BgDefault;

    private void Awake()
    {

        if (container.Type.GetRemovable())
        {
            Selected = true;
        }
    }
    private void Start()
    {
        if (!container.Type.GetRemovable())
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        gameObject.SetActive(container.Type.GetRemovable());

    }
    public IEnumerator Cooldown()
    {
        allowchanges = false;
        yield return new WaitForSeconds(0.2f);
        allowchanges = true;
    }
    private void OnDisable()
    {
        allowchanges = false;
    }
    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            toggle.interactable = !value;
            BG.color = value ? BgSelected : BgDefault;
            if (value && container.PartName != null)
            {
                container.PartName.text = "";
            }
        }
    }

    public void RemovePieceFromAvatar()
    {
        if (!allowchanges || !Selected)
        {
            allowchanges = true;
            Debug.Log("Not yet");
            return;
        }

        UnityMainThread.wkr.AddJob(async () =>
        {
            await AvatarReader.RemovePiece(container.Type, false);
            ExportAvatar.instance.Saved = false;
        });

    }
}
