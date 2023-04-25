using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("parameter")]
    public int noiseSampleSize;
    public float scale;
    public int textureResolution = 1;

    private MeshRenderer tileMeshRender;
    private MeshFilter tileMeshFilter;
    private MeshCollider tileMeshCollider;

    private void Start()
    {
        // get the tile components
        tileMeshRender = GetComponent<MeshRenderer>();
        tileMeshFilter = GetComponent<MeshFilter>();
        tileMeshCollider = GetComponent<MeshCollider>();
        GenerateTile();
    }

    void GenerateTile()
    {
        //float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale);

        float[,] hdHeightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, textureResolution);

        Texture2D heightMapTexture = TextureBuilder.BuildTexture(hdHeightMap);

        tileMeshRender.material.mainTexture = heightMapTexture;
    }
}