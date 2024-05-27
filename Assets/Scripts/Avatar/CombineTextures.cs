using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.U2D;
using static Unity.VectorGraphics.SVGParser;

public class CombineTextures : MonoBehaviour
{
    [Tooltip("Name of the SVG file with its extension (example bee.svg)")]
    public string[] SVG_File_Name;

    [Tooltip("Path of the folder containing the SVG file (example C:/Users/YourUserName/AppData/LocalLow/CompanyName/ProductName)")]
    public string SVG_Path;

    public static Material material;


    //public static Material CombineAllTextures(string[] all)
    //{
    //    foreach (string thisSVG in all)
    //    {
    //        //string path = Path.Combine(SVG_Path, thisSVG);
    //        //material = new Material(prefab);

    //        //Para ficheiros svg que nao estao em xml
    //        //Texture tex = Load_SVG_as_Texture(GetSVGContent(path));

    //        Texture tex = Load_SVG_as_Texture(thisSVG);
    //        material.SetTexture("_MainTex", tex);
    //    }

    //    return material;
    //}

    //public static Material CombineAllTexturesLocal(string[] paths,string SVG_Path)
    //{
    //    foreach (string thisSVG in paths)
    //    {
    //        string path = Path.Combine(SVG_Path, thisSVG);
    //        material = new Material(prefab);

    //        //Para ficheiros svg que nao estao em xml
    //        Texture tex = Load_SVG_as_Texture(GetSVGContent(path));

    //        material.SetTexture("_MainTex", tex);
    //    }

    //    return material;
    //}

    private static string GetSVGContent(string path)
    {
        if (File.Exists(path))
        {
            //We read it
            StreamReader SVG_File_Reader;
            SVG_File_Reader = File.OpenText(path);
            string SVG_File_Content = SVG_File_Reader.ReadToEnd();
            SVG_File_Reader.Close();

            return SVG_File_Content;
        }
        else
            throw new FileNotFoundException("SVG not found");
    }
    public static Texture2D Load_SVG_as_Texture(string SVG_File_Content)
    {
        // Debug.Log(material.name);
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
            Sprite SVG_Sprite = VectorUtils.BuildSprite(TessGeo, SceneInfo.SceneViewport, 10.0f, VectorUtils.Alignment.SVGOrigin, new Vector2(1f, 0f), 8, true);
            Texture2D coloredTexture = VectorUtils.RenderSpriteToTexture2D(SVG_Sprite, (int)SVG_Sprite.rect.width, (int)SVG_Sprite.rect.height, material, 8, true);
            Debug.Log(coloredTexture.width + " " + coloredTexture.height);
            //Debug.Log((int)SVG_Sprite.rect.width+ " "+(int)SVG_Sprite.rect.height);
            if (SVG_Sprite == null)
            {
                Debug.LogError("Sprite is still null!!!");
            }
            if (coloredTexture == null)
            {
                Debug.LogError("coloredTexture is still null!!!");
            }

            return coloredTexture;
        }
        else //The SVG file is empty
        {
            Debug.Log("<color=orange>Load_SVG_as_Sprite(null): The SVG file is empty</color>");
            return null;
        }
    }

    public static Sprite Load_SVG_as_Sprite(string SVG_File_Content)
    {
        // Debug.Log(material.name);
        //If the SVG file is NOT empty
        if (SVG_File_Content.Trim().Length > 0)
        {
            int from = SVG_File_Content.IndexOf("viewBox=\"");
            int to = SVG_File_Content.IndexOf("\">");
            //Debug.Log(from + " " + to);
            if (SVG_File_Content == "Invalid file name!")
            {
                return null;
            }
            string sub = SVG_File_Content.Substring(from, to - from);

            string[] res = sub.Split(char.Parse(" "));
            //Debug.Log(res[2] + " " + res[3]);
            //We define the tessellation options
            var TessOptions = new VectorUtils.TessellationOptions()
            {
                StepDistance = 100.0f,
                MaxCordDeviation = 0.5f,
                MaxTanAngleDeviation = 0.1f,
                SamplingStepSize = 0.01f
            };
            //We import the vectorial data
            var SceneInfo = SVGParser.ImportSVG(new StringReader(SVG_File_Content), ViewportOptions.OnlyApplyRootViewBox, 13, 8, int.Parse(res[2]), int.Parse(res[3]));

            //We tesselate the geometry
            var TessGeo = VectorUtils.TessellateScene(SceneInfo.Scene, TessOptions);

            //We create the final Unity sprite
            Sprite SVG_Sprite = VectorUtils.BuildSprite(TessGeo, 10.0f, VectorUtils.Alignment.SVGOrigin, new Vector2(0, 1), 8, true);
            //Debug.Log((int)SVG_Sprite.rect.width+ " "+(int)SVG_Sprite.rect.height);
            if (SVG_Sprite == null)
            {
                Debug.LogError("Sprite is still null!!!");
            }
            SVG_Sprite.name = "SVG to sprite";
            return SVG_Sprite;
        }
        else //The SVG file is empty
        {
            Debug.Log("<color=orange>Load_SVG_as_Sprite(null): The SVG file is empty</color>");
            return null;
        }
    }

    //public static async Task<Sprite> Load_SVG_as_SpriteAsync(string SVG_File_Content)
    //{
    //    // Debug.Log(material.name);
    //    //If the SVG file is NOT empty

    //    if (SVG_File_Content.Trim().Length > 0)
    //    {
    //        var TessOptions = new VectorUtils.TessellationOptions()
    //        {
    //            StepDistance = 100.0f,
    //            MaxCordDeviation = 0.5f,
    //            MaxTanAngleDeviation = 0.1f,
    //            SamplingStepSize = 0.01f
    //        };

    //        Sprite SVG_Sprite;

    //        UnityMainThread.wkr.AddSpriteJob((Sprite SVG_Sprite) =>
    //        {
    //            int from = SVG_File_Content.IndexOf("viewBox=\"");
    //            int to = SVG_File_Content.IndexOf("\">");
    //            string sub = SVG_File_Content.Substring(from, to - from);
    //            string[] res = sub.Split(char.Parse(" "));
    //            //We import the vectorial data
    //            var SceneInfo = SVGParser.ImportSVG(new StringReader(SVG_File_Content), ViewportOptions.OnlyApplyRootViewBox, 13, 8, int.Parse(res[2]), int.Parse(res[3]));

    //            var TessGeo = VectorUtils.TessellateScene(SceneInfo.Scene, TessOptions);

    //            SVG_Sprite = VectorUtils.BuildSprite(TessGeo, 10.0f, VectorUtils.Alignment.SVGOrigin, new Vector2(0, 1), 8, true);

    //        });

    //        Debug.Log((int)SVG_Sprite.rect.width + " " + (int)SVG_Sprite.rect.height);
    //        if (SVG_Sprite == null)
    //        {
    //            Debug.LogError("Sprite is still null!!!");
    //        }

    //        return SVG_Sprite;
    //    }
    //    else //The SVG file is empty
    //    {
    //        Debug.Log("<color=orange>Load_SVG_as_Sprite(null): The SVG file is empty</color>");
    //        return null;
    //    }

    //}
}
