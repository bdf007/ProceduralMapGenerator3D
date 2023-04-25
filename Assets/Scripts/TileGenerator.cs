using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [Header("parameter")]
    public int noisSampleSize;
    public float scale;

    private void Start()
    {
        GenerateTile();
    }

    void GenerateTile()
    {
        float[,] heightMap = NoiseGenerator.GenerateNoiseMap(noisSampleSize, scale);
    }
}
