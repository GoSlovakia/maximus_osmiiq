using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingComponent : MonoBehaviour
{
    public static LoadingComponent instance;

    public bool loading
    {
        get => _loading;
        set
        {
            _loading = value;
            // LoadIcon.transform.rotation = Quaternion.Euler(0, 0, 0);
            //if (loading)
            //{
            //    StartCoroutine(RotateIcon());
            //}
        }
    }

    //[SerializeField]
    //private GameObject LoadIcon;
    private bool _loading;
    public TextMeshProUGUI Description;
    public Slider slider;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            Debug.Log("Loading set");
            instance = this;
        }

        gameObject.SetActive(false);
    }

    //public IEnumerator RotateIcon()
    //{
    //    LoadIcon.transform.Rotate(0, 0, -50f * Time.deltaTime);
    //    yield return null;
    //    if (loading)
    //        StartCoroutine(RotateIcon());
    //}



    public void StartLoadAnim()
    {
        gameObject.SetActive(true);
        loading = true;
    }

    public void EndLoadAnim()
    {
        gameObject.SetActive(false);
        loading = false;
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
