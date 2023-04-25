using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator
{
    //generate a noise map based on the number of parameters
    // return a 2D array of floats
    public static float[,] GenerateNoiseMap(int noiseSampleSize, float scale, Wave[] waves, Vector2 offset, int resolution = 1)
    {
        float[,] noiseMap = new float[noiseSampleSize * resolution, noiseSampleSize * resolution];

        for(int x = 0; x < noiseSampleSize * resolution; x++)
        {
            for(int y = 0; y < noiseSampleSize * resolution; y++)
            {
                float samplePosX = ((float)x / scale / (float)resolution) + offset.y;
                float samplePosY = ((float)y / scale / (float)resolution) + offset.x;

                float noise = 0.0f;
                float normalization = 0.0f;

                foreach(Wave wave in waves)
                {
                    noise += wave.amplitude * Mathf.PerlinNoise(samplePosX * wave.frequency + wave.seed, samplePosY * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }

                noise /= normalization;
                noiseMap[x, y] = noise;
            }
        }

        return noiseMap;
    }
}

[System.Serializable]
public class Wave
{
    public float seed;
    public float frequency;
    public float amplitude;
}
