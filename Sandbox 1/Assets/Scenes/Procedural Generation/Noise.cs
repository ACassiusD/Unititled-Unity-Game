using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//http://www.campi3d.com/External/MariExtensionPack/userGuide5R4v1/Understandingsomebasicnoiseterms.html#amplitude
//Generate noise map that will be used to create terrain.
//Scale       - You need scale to make the x and y coordinate you pull from perlion noise function a decimal number.
//              For some reason it will give the same number every time if you use a int.
//Octaves     - Multiple noise maps (or "Octaves") are layered on top of each other to create more detailed maps
//Persistance - Controls the decrease in amplitude of octaves.
//Lacunarity  - Controls the frequency of octaves. Higher Lacunarity = more noise detail, better for small objects like rocks, lower Lacunarity is better for low detail, for creating bigger objects like montains
//Frequence   - (x axis) - How frequent terrain objects appear on the x axis
//Amplitude   - (y axis) - How strong terrain generated is on the y axis
public static class Noise
{
    //Generate a 2D noise map of passed in height and width using Perlin Noise.
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset){
        //Multidimentianal to store the perlin noise data for each x,y position in the noisemap.
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++){
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0){
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;


        //For each x,y position on the map.
        for (int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;
               
                for(int i = 0; i < octaves; i++){
                    //Get coordinate and scale it.
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    //Get the perlin noise for that coordinate and store in the  noiseMap
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;


                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if(noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;

                if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;
                
                noiseMap[x, y] = noiseHeight;
            }
        }

        //Normalize the noisemap after its created.
        for (int y = 0; y < mapHeight; y++){
            for (int x = 0; x < mapWidth; x++){
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
