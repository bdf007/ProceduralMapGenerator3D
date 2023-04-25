using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("parameter")]
    public int noiseSampleSize;
    public float scale;
    public float maxHeight = 1.0f;
    public int textureResolution = 1;

    [Header("terrain type")]
    public TerrainType[] heightTerrainTypes;

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
        float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale);

        float[,] hdHeightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, textureResolution);

        Vector3[] verts = tileMeshFilter.mesh.vertices;

        for (int x = 0; x < noiseSampleSize; x++)
        {
            for (int z = 0; z < noiseSampleSize; z++)
            {
                int index = (x * noiseSampleSize) + z;
                verts[index].y = heightMap[x, z] * maxHeight;
            }
        }

        tileMeshFilter.mesh.vertices = verts;
        tileMeshFilter.mesh.RecalculateBounds();
        tileMeshFilter.mesh.RecalculateNormals();

        tileMeshCollider.sharedMesh = tileMeshFilter.mesh;

        Texture2D heightMapTexture = TextureBuilder.BuildTexture(hdHeightMap, heightTerrainTypes);

        tileMeshRender.material.mainTexture = heightMapTexture;
    }
}

[System.Serializable]
public class TerrainType
{
    [Range(0.0f, 1.0f)]
    public float threshold;
    public Color color;
}
