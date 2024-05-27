using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager instance;
    [SerializeField]
    private LoadingComponent LoadingPrefab;
    [SerializeField]
    private LoadingComponent LoadingDetailedPrefab;
    private List<string> loadingQueue = new List<string>();
    private bool loading = false;

    private LoadingComponent LoadingGO;



    public void EnqueueLoad(string Job)
    {
        loadingQueue.Add(Job);

        if (loadingQueue.Count > 0 && !Loading)
        {
            Loading = true;
            LoadingGO.slider.value = 1f / (loadingQueue.Count + 1f);
        }
        if (LoadingGO != null)
        {
            LoadingGO.Description.text = loadingQueue.Last();
        }
    }

    public void DequeueLoad(string Job)
    {
        loadingQueue.Remove(Job);

        if (Loading && loadingQueue.Count == 0)
        {
            Loading = false;
        }
        else if (LoadingGO != null && loadingQueue.Count != 0)
        {
            LoadingGO.Description.text = loadingQueue.Last();
            LoadingGO.slider.value = 1f / (loadingQueue.Count + 1f);
        }
    }

    public void ResetQueue()
    {
        loadingQueue.Clear();
        Loading = false;
    }

    public bool Loading
    {
        get => loading; set
        {
            loading = value;
            if (loading)
            {
                StartLoading();
            }
            else
            {
                EndLoad();
            }
        }
    }

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

        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        loadingQueue = new List<string>();
    }

    private void StartLoading()
    {
        //Debug.Log("Loading Started");
        if (LoadingGO == null)
        {
            LoadingGO = SceneManager.GetActiveScene().name != "Login Scene" ? Instantiate(LoadingPrefab, FindObjectOfType<Canvas>().transform) : Instantiate(LoadingDetailedPrefab, FindObjectOfType<Canvas>().transform);
        }

        LoadingGO.gameObject.SetActive(true);
        LoadingGO.loading = true;
    }

    private void EndLoad()
    {
        // Debug.Log("Loading Ended");
        if (LoadingGO == null)
        {
            return;
        }
        LoadingGO.loading = false;
        LoadingGO.gameObject.SetActive(false);
    }
}
