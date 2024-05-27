using System;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAssetBundle
{
    [MenuItem("Assets/Create Asset Bundles", false, 150)]
    private static void BuildAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(assetBundleDirectory);
        AssetDatabase.Refresh();

        try
        {
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}
