using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenStore : MonoBehaviour
{
    public void OpenStor()
    {
        SceneManager.LoadScene("StoreMain");
    }
}
