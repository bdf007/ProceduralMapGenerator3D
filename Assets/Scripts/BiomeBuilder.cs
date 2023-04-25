using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeBuilder : MonoBehaviour
{
    public BiomeRow[] biomeRows;

    public static BiomeBuilder Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Texture2D BuildTexture(TerrainType[,] heatMatTypes, TerrainType[,] moistureMapTypes)
    {
        int size = heatMatTypes.GetLength(0);
        Color[] pixels = new Color[size * size];

        for(int x = 0; x < size; x++)
        {
            for(int z = 0; z < size; z++)
            {
                int index = (x * size) + z;
                int heatIndex = heatMatTypes[x, z].index;
                int moistureIndex = moistureMapTypes[x, z].index;

                Biome biome = biomeRows[moistureIndex].biomes[heatIndex];
                pixels[index] = biome.color;
            }
        }

        Texture2D texture = new Texture2D(size, size);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}

[System.Serializable]
public class BiomeRow
{
    public Biome[] biomes;
}


[System.Serializable]
public class Biome
{
    public string name;
    public Color color;
}
