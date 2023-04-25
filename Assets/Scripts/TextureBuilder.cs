using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureBuilder
{
    // build a texture based on the noise map
    public static Texture2D BuildTexture(float[,] noiseMap)
    {
        // create colore array for the pixels
        Color[] pixels = new Color[noiseMap.Length];
        
        // calculate the length of the texture
        int pixelLength = noiseMap.GetLength(0);

        // loop trough each pixel and set the color based on the noise map
        for(int x= 0; x < pixelLength; x++)
        {
            for( int z = 0; z < pixelLength; z++)
            {
                // calculate the index of the pixel in the array 'pixels'
                int index = (x * pixelLength) + z;
                pixels[index] = Color.Lerp(Color.black, Color.white, noiseMap[x, z]);
            }
        }

        // create a new texture and set the pixels
        Texture2D texture = new Texture2D(pixelLength, pixelLength);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
}
