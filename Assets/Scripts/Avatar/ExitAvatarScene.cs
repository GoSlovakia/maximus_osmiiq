using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitAvatarScene : MonoBehaviour
{
    [SerializeField]
    private ChangeScene ChangeScene;
    public GameObject NotSavedWarning;

    public void ExitSceneCheck()
    {
        if (ExportAvatar.instance.Saved)
        {
            ChangeScene.ChangeToMainMenu();
        }
        else
        {
            NotSavedWarning.SetActive(true);
        }
    }

    public void LeaveAndSave()
    {
        ExportAvatar.instance.Save();
        ChangeScene.ChangeToMainMenu();
    }

}
