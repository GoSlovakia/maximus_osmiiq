using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
  public void ChangeToAvatarCreation()
  {
    SceneManager.LoadScene("Avatar Creation",LoadSceneMode.Single);
  }

  public void ChangeToMainMenu()
  {
    SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
  }
    public void ChangeToCardInventory()
    {
        SceneManager.LoadScene("Inventario", LoadSceneMode.Single);
    }

    public void ChangeToStore()
    {
        SceneManager.LoadScene("StoreMain", LoadSceneMode.Single);
    }

    public void ChangeToChangeTitles()
    {
        SceneManager.LoadScene("ChangeTitles", LoadSceneMode.Single);
    }
    public void ChangeToAchievements()
    {
        SceneManager.LoadScene("Achievements", LoadSceneMode.Single);
    }
    public void ChangeToInbox()
    {
        SceneManager.LoadScene("Inbox", LoadSceneMode.Single);
    }
}
