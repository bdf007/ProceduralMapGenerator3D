using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainVisualization
{
    Height,
    Heat,
    Moisture
}

public class TileGenerator : MonoBehaviour
{
    [Header("parameter")]
    public int noiseSampleSize;
    public float scale;
    public float maxHeight = 1.0f;
    public int textureResolution = 1;
    public TerrainVisualization visualizationType;

    [HideInInspector]
    public Vector2 offset;

    [Header("terrain type")]
    public TerrainType[] heightTerrainTypes;
    public TerrainType[] heatTerrainTypes;
    public TerrainType[] moistureTerrainTypes;

    [Header("Waves")]
    public Wave[] waves;
    public Wave[] heatWaves;
    public Wave[] moistureWaves;

    [Header("Curves")]
    public AnimationCurve heightCurve;

    private MeshRenderer tileMeshRender;
    private MeshFilter tileMeshFilter;
    private MeshCollider tileMeshCollider;

    private MeshGenerator meshGenerator;
    private MapGenerator mapGenerator;

    private void Start()
    {
        // get the tile components
        tileMeshRender = GetComponent<MeshRenderer>();
        tileMeshFilter = GetComponent<MeshFilter>();
        tileMeshCollider = GetComponent<MeshCollider>();

        meshGenerator = GetComponent<MeshGenerator>();
        mapGenerator = FindObjectOfType<MapGenerator>();

        GenerateTile();
    }

    void GenerateTile()
    {
        float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, waves, offset);

        float[,] hdHeightMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize -1, scale, waves, offset, textureResolution);

        Vector3[] verts = tileMeshFilter.mesh.vertices;

        for (int x = 0; x < noiseSampleSize; x++)
        {
            for (int z = 0; z < noiseSampleSize; z++)
            {
                int index = (x * noiseSampleSize) + z;
                verts[index].y = heightCurve.Evaluate(heightMap[x, z]) * maxHeight;
            }
        }

        tileMeshFilter.mesh.vertices = verts;
        tileMeshFilter.mesh.RecalculateBounds();
        tileMeshFilter.mesh.RecalculateNormals();

        tileMeshCollider.sharedMesh = tileMeshFilter.mesh;

        Texture2D heightMapTexture = TextureBuilder.BuildTexture(hdHeightMap, heightTerrainTypes);

        float[,] heatMap = GenerateHeatMap(heightMap);
        float[,] moistureMap = GenerateMoistureMap(heightMap);

        switch(visualizationType)
        {
            case TerrainVisualization.Height:
                tileMeshRender.material.mainTexture = heightMapTexture;
                break;
            case TerrainVisualization.Heat:
                tileMeshRender.material.mainTexture = TextureBuilder.BuildTexture(heatMap, heatTerrainTypes);
                break;
            case TerrainVisualization.Moisture:
                tileMeshRender.material.mainTexture = TextureBuilder.BuildTexture(moistureMap, moistureTerrainTypes);
                break;
        }

        //float[,] moistureMap = GenerateMoistureMap(heightMap);
        //tileMeshRender.material.mainTexture = TextureBuilder.BuildTexture(moistureMap, moistureTerrainTypes);
    }

    // generates a heat map based on the height map
    float[,] GenerateHeatMap(float[,] heightMap)
    {
        float[,] uniformHeatMap = NoiseGenerator.GenerateUniformNoiseMap(noiseSampleSize, transform.position.z * (noiseSampleSize / meshGenerator.xSize), (noiseSampleSize / 2 * mapGenerator.numX) + 1);

        float[,] randomHeatMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, heatWaves, offset);

        float[,] heatMap = new float[noiseSampleSize, noiseSampleSize];

        for(int x = 0; x < noiseSampleSize; x++)
        {
            for(int z = 0; z < noiseSampleSize; z++)
            {
                heatMap[x, z] = uniformHeatMap[x, z] * randomHeatMap[x, z];
                heatMap[x, z] += 0.5f * heightMap[x, z];

                heatMap[x, z] = Mathf.Clamp(heatMap[x, z], 0.0f, 0.99f);
            }
        }

        return heatMap;
    }

    float[,] GenerateMoistureMap(float[,] heightMap)
    {
        float[,] moistureMap = NoiseGenerator.GenerateNoiseMap(noiseSampleSize, scale, moistureWaves, offset);

        for(int x = 0; x < noiseSampleSize; x++)
        {
            for(int z = 0; z < noiseSampleSize; z++)
            {
                moistureMap[x, z] -= 0.1f * heightMap[x, z];
                moistureMap[x, z] = Mathf.Clamp(moistureMap[x, z], 0.0f, 0.99f);
            }
        }

        return moistureMap;
    }
}

[System.Serializable]
public class TerrainType
{
    [Range(0.0f, 1.0f)]
    public float threshold;
    public Gradient colorGradient;
}
