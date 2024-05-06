using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

public class PurificationBlender : MonoBehaviour
{
    public Material Material;
    [Range(0f, 1f)] public float BlendFactor = 0f;
    public float BlendRadius = 0f;
    public Vector3 OriginPosition;

    public List<MeshRenderer> RenderList;

    public Texture2D _splat0;
    public Texture2D _splat1;
    public Texture2D _splat2;
    public Texture2D _splat3;

    [ButtonMethod]
    private void Swap()
    {
        Material.SetTexture("_Splat0Blend", _splat0);
        Material.SetTexture("_Splat1Blend", _splat1);
        Material.SetTexture("_Splat2Blend", _splat2);
        Material.SetTexture("_Splat3Blend", _splat3);
    }

    private void Update()
    {
        this.Material.SetFloat("_BlendFactor", BlendFactor);
        this.Material.SetFloat("_BlendRadius", BlendRadius);
        this.Material.SetVector("_OriginPosition", OriginPosition);

        foreach (MeshRenderer renderer in RenderList)
        {
            //this.Material.SetFloat("_BlendFactor", BlendFactor);
           renderer.material.SetFloat("_BlendRadius", BlendRadius);
           renderer.material.SetVector("_OriginPosition", OriginPosition);
        }
    }
}