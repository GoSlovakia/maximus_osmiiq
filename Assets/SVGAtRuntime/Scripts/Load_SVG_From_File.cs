using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VectorGraphics;
using UnityEngine.UI;
using System.Linq;

public static class Load_SVG_From_File
{
    //[Tooltip("Name of the SVG file with its extension (example bee.svg)")]
    //public string SVG_File_Name;

    // public GameObject test;

    //public Sprite Default;

    /*[SerializeField]
    [Tooltip("Path of the folder containing the SVG file (example C:/Users/YourUserName/AppData/LocalLow/CompanyName/ProductName)")]
    private string SVG_PathSerialize;

    [SerializeField]
    private string Png_PathSerialize;*/

    public static string SVG_Path;

    [Tooltip("Full path to the SVG file")]
    private static string SVG_Full_Path;

    public static string PNG_Path;

    /*[Tooltip("Full path to the SVG file")]
    private string PNG_Full_Path;

    [SerializeField]
    [Tooltip("Primary code")]
    private string SerializePrimaryCode;

    [SerializeField]
    [Tooltip("Secondary code")]
    private string SerializeSecondaryCode;*/

    [Tooltip("Primary code")]
    public volatile static string PrimaryCode;

    [Tooltip("Secondary code")]
    public volatile static string SecondaryCode;

    [Tooltip("Primary code")]
    public static string PrimaryCodeDefault="ST01";

    [Tooltip("Secondary code")]
    public static string SecondaryCodeDefault="ST02";



    public static List<Sprite> results = new List<Sprite>();

    public static void SetSVGS(string Png_PathSerialize, string SVG_PathSerialize, string SerializePrimaryCode, string SerializeSecondaryCode)
    {
        results = new List<Sprite>();
        // Debug.Log(Application.persistentDataPath);
        SVG_Path = SVG_PathSerialize;
        PNG_Path = Png_PathSerialize;
        PrimaryCode = SerializePrimaryCode;
        SecondaryCode = SerializeSecondaryCode;
        //If the path to the folder containing the SVG file is empty
        if (SVG_Path.Trim().Length == 0)
        {
            //Debug.Log("Filling");
            //We fill it
            SVG_Path = Application.persistentDataPath + "/SVG";
        }
        if (PNG_Path.Trim().Length == 0)
        {
            //Debug.Log("Filling");
            //We fill it
            PNG_Path = Application.persistentDataPath + "/Thumbnails";
        }

        ////If the SVG file name is empty
        //if (SVG_File_Name.Trim().Length == 0)
        //{
        //    //We fill it
        //    SVG_File_Name = "bee.svg";
        //}

        ////We fill the full path to the SVG file
        //SVG_Full_Path = SVG_Path + "/" + SVG_File_Name;
    }
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!DEPRECATED!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //public static void LoadFromDisk(GenerateCategories generateCategories, AssetCreatingFromFolder assetCreatingFromFolder)
    //{
    //    var info = new DirectoryInfo(SVG_Path);
    //    var fileInfo = info.GetFiles();

    //    foreach (FileInfo este in fileInfo)
    //    {
    //        //Debug.Log(este.Name);
    //        SVG_Full_Path = SVG_Path + "/" + este.Name;

    //        //If the SVG file exists
    //        if (File.Exists(SVG_Full_Path))
    //        {
    //            //We read it
    //            StreamReader SVG_File_Reader;
    //            SVG_File_Reader = File.OpenText(SVG_Full_Path);
    //            string SVG_File_Content = SVG_File_Reader.ReadToEnd();
    //            SVG_File_Reader.Close();


    //            ////Ok isto é pa um
    //            //GetComponent<Image>().sprite = Load_SVG_as_Sprite(ColourManager.ReplaceColors(SVG_File_Content, PrimaryCode, SecondaryCode, white, black));

    //            //test.GetComponent<SpriteRenderer>().sprite = Load_SVG_as_Sprite(ColourManager.ReplaceColors(SVG_File_Content, PrimaryCode, SecondaryCode, white, black));
    //            //Debug.Log(PrimaryCode);

    //            // Debug.Log(AssetCreatingFromFolder.GetType(este.Name).GetDescription());

    //            //Para a pasta inteira
    //            results.Add(Load_SVG_as_Sprite(ColourManager.ReplaceColors(SVG_File_Content, PrimaryCode, SecondaryCode)));
    //            results.Last().name = este.Name.Replace(".svg", "");

    //            //test.GetComponent<SpriteRenderer>().sprite = Load_SVG_as_Sprite(ColourManager.ReplaceColors(SVG_File_Content, PrimaryCode, SecondaryCode, white, black));
    //        }
    //        else //The SVG file doesn't exists
    //        {
    //            Debug.Log("<color=orange>" + "Missing SVG file at " + Application.persistentDataPath + "/" + este.Name + "</color>\nTo test it, please put bee.svg file into " + Application.persistentDataPath);
    //        }
    //    }
    //    generateCategories.GenerateAll();
    //    assetCreatingFromFolder.GenerateFromList(results);
    //    AvatarReader.LoadDefaultAvatar();
    //}

    public static Sprite GetAvatarPart(AvatarPart part, string code)
    {
        //Debug.Log(SVG_Path + "/PC-" + part.Part.GetFilenameRequired() + code + ".svg");
        string SVG_Full_Path = SVG_Path + "/PC-" + part.Part.GetFilenameRequired() + code + ".svg";

        //If the SVG file exists
        if (File.Exists(SVG_Full_Path))
        {
            //We read it
            StreamReader SVG_File_Reader;
            SVG_File_Reader = File.OpenText(SVG_Full_Path);
            string SVG_File_Content = SVG_File_Reader.ReadToEnd();
            SVG_File_Reader.Close();

            ////Ok isto é pa um
            return Load_SVG_as_Sprite(ColourManager.ReplaceColors(SVG_File_Content, PrimaryCode, SecondaryCode));

        }
        else //The SVG file doesn't exists
        {
            Debug.Log("<color=orange>" + "Missing SVG file at " + Application.persistentDataPath + "/" + SVG_Full_Path + "</color>\nTo test it, please put bee.svg file into " + Application.persistentDataPath);

            return null;
        }
    }


    public static Sprite GetThumbnail(AvatarAcc part, string code)
    {
        string PNG_Full_Path = PNG_Path + "/PC-" + part.avatarset.GetFilenameRequired() + code + "_thumb.png";

        if (File.Exists(PNG_Full_Path))
        {
            Texture2D SpriteTexture = LoadTexture(PNG_Full_Path);
            Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0, 0), 20);

            return NewSprite;              // If data = readable -> return texture
        }
        else
        {
            Debug.LogError("PNG at " + PNG_Full_Path);
            return null;
        }


    }




    public static Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }

    public static Sprite GetAvatarPart(string code)
    {

        string SVG_Full_Path = SVG_Path + "/" + code + ".svg";

        //If the SVG file exists
        if (File.Exists(SVG_Full_Path))
        {
            //We read it
            StreamReader SVG_File_Reader;
            SVG_File_Reader = File.OpenText(SVG_Full_Path);
            string SVG_File_Content = SVG_File_Reader.ReadToEnd();
            SVG_File_Reader.Close();


            ////Ok isto é pa um
            return Load_SVG_as_Sprite(ColourManager.ReplaceColors(SVG_File_Content, PrimaryCode, SecondaryCode));

        }
        else //The SVG file doesn't exists
        {
            Debug.Log("<color=orange>" + "Missing SVG file at " + Application.persistentDataPath + "/" + code + ".svg" + "</color>\nTo test it, please put bee.svg file into " + Application.persistentDataPath);

            return null;
        }


    }

    private static Sprite Load_SVG_as_Sprite(string SVG_File_Content)
    {
        //If the SVG file is NOT empty
        if (SVG_File_Content.Trim().Length > 0)
        {
            //We define the tessellation options
            var TessOptions = new VectorUtils.TessellationOptions()
            {
                StepDistance = 100.0f,
                MaxCordDeviation = 0.5f,
                MaxTanAngleDeviation = 0.1f,
                SamplingStepSize = 0.01f

            };


            //We import the vectorial data
            var SceneInfo = SVGParser.ImportSVG(new StringReader(SVG_File_Content), ViewportOptions.OnlyApplyRootViewBox, 13);
            //We tesselate the geometry
            var TessGeo = VectorUtils.TessellateScene(SceneInfo.Scene, TessOptions);
            //We create the final Unity sprite
            var SVG_Sprite = VectorUtils.BuildSprite(TessGeo, 10.0f, VectorUtils.Alignment.SVGOrigin, new Vector2(1f, 0f), 8, true);


            return SVG_Sprite;
        }
        else //The SVG file is empty
        {
            Debug.Log("<color=orange>Load_SVG_as_Sprite(null): The SVG file is empty</color>");
            return null;
        }
    }





    /// <summary>
    /// Returns a texture2D out of a sprite
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="material">needs to be unlit/vector or unlit/vectorGradient</param>
    /// <param name="antiAliasing">needs to be between 1 and 8</param>
    /// <returns></returns>
    public static Texture2D LoadSVGAsTexture2D(Sprite sprite, int width, int height, Material material, int antiAliasing)
    {
        if (antiAliasing > 8)
            antiAliasing = 8;
        else if (antiAliasing <= 0)
            antiAliasing = 1;

        return VectorUtils.RenderSpriteToTexture2D(sprite, width, height, material, antiAliasing);
    }


    /// <summary>
    /// Returns a texture2D out of a sprite path
    /// </summary>
    /// <param name="SVG_File_Content"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="material">needs to be unlit/vector or unlit/vectorGradient</param>
    /// <param name="antiAliasing">needs to be between 1 and 8</param>
    /// <returns></returns>
    public static Texture2D LoadSVGAsTexture2D(string SVG_File_Content, int width, int height, Material material, int antiAliasing)
    {
        Sprite sprite = Load_SVG_as_Sprite(SVG_File_Content);
        if (antiAliasing > 8)
            antiAliasing = 8;
        else if (antiAliasing <= 0)
            antiAliasing = 1;

        return VectorUtils.RenderSpriteToTexture2D(sprite, width, height, material, antiAliasing);
    }

    /// <summary>
    /// Returns a material out of a sprite
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="material">needs to be unlit/vector or unlit/vectorGradient</param>
    /// <param name="antiAliasing">needs to be between 1 and 8</param>
    /// <returns></returns>
    public static Material LoadSVGAsMaterial(Sprite sprite, int width, int height, Material material, int antiAliasing)
    {
        if (antiAliasing > 8)
            antiAliasing = 8;
        else if (antiAliasing <= 0)
            antiAliasing = 1;

        Texture2D texture2D = VectorUtils.RenderSpriteToTexture2D(sprite, width, height, material, antiAliasing);
        material.SetTexture("_MainTex", texture2D);
        return material;
    }

    /// <summary>
    /// Returns a material out of a sprite path
    /// </summary>
    /// <param name="SVG_File_Content"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="material">needs to be unlit/vector or unlit/vectorGradient</param>
    /// <param name="antiAliasing">needs to be between 1 and 8</param>
    /// <returns></returns>
    public static Material LoadSVGAsMaterial(string SVG_File_Content, int width, int height, Material material, int antiAliasing)
    {
        Sprite sprite = Load_SVG_as_Sprite(SVG_File_Content);

        if (antiAliasing > 8)
            antiAliasing = 8;
        else if (antiAliasing <= 0)
            antiAliasing = 1;

        Texture2D texture2D = VectorUtils.RenderSpriteToTexture2D(sprite, width, height, material, antiAliasing);
        material.SetTexture("_MainTex", texture2D);
        return material;
    }
}