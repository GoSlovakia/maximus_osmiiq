using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.Rendering.DebugUI;

public class RadialSlider : MonoBehaviour
{
    [SerializeField]
    private RectTransform Edge;

    [SerializeField]
    [Range(0f, 1f)]
    private float value;

    public AnimationCurve Offset;


    //Max 2.08, Min -2.08
    //Offset: Min-0.16 ,

    public float DistancefromCenter;

    public Image ShadergraphImg;

    public float Value
    {
        get => value; set
        {
            this.value = value;
            ShadergraphImg.material.SetFloat("progress", Value);
            MoveEdge();
        }
    }

    private void OnValidate()
    {
        Value = value;
        

    }

    public void MoveEdge()
    {
        Edge.anchoredPosition = new Vector2(Mathf.Sin(Mathf.LerpUnclamped(-2f, 2f, Value) + Offset.Evaluate(Value)) * DistancefromCenter, Mathf.Cos(Mathf.LerpUnclamped(-2f, 2f, Value) + Offset.Evaluate(Value)) * DistancefromCenter);
    }
}
