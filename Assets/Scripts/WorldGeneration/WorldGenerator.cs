using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{


	public TerrainData terrainData;
	public NoiseData noiseData;

	[Range(0, 6)]
	public int levelOfDetail;
	float[,] falloffMap;

	public bool autoUpdate;

	public TerrainType[] regions;

    private void Awake()
    {
		falloffMap = FalloffGenerator.GenerateFalloffMap(terrainData.mapChunkSize);
		noiseData.seed = Random.Range(0, 1000000);
		GenerateWorld();
		
    }

	void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
			
        }

    }

    private void OnValidate()
    {
        if(terrainData != null)
        {
			terrainData.OnValuesUpdated -= OnValuesUpdated;
			terrainData.OnValuesUpdated += OnValuesUpdated;
        }

		if (noiseData != null)
		{
			noiseData.OnValuesUpdated -= OnValuesUpdated;
			noiseData.OnValuesUpdated += OnValuesUpdated;
		}
	}

    public void GenerateWorld()
	{
		float[,] noiseMap = Noise.GenerateNoise(terrainData.mapChunkSize, terrainData.mapChunkSize, noiseData.seed, noiseData.noiseScale, noiseData.octaves, noiseData.persistance, noiseData.lacunarity, noiseData.offset);

		Color[] colourMap = new Color[terrainData.mapChunkSize * terrainData.mapChunkSize];		
		
		for (int y = 0; y < terrainData.mapChunkSize; y++)
		{
			for (int x = 0; x < terrainData.mapChunkSize; x++)
			{
                if (terrainData.useFalloff)
                {
					noiseMap[x,y] = Mathf.Clamp01(noiseMap[x, y] - falloffMap[x, y]);
                }

				float currentHeight = noiseMap[x, y];
				for (int i = 0; i < regions.Length; i++)
				{
					if (currentHeight <= regions[i].height)
					{
						colourMap[y * terrainData.mapChunkSize + x] = regions[i].colour;
						break;
					}
				}
			}
		}

		WorldDisplay display = FindObjectOfType<WorldDisplay>();

		display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, terrainData.meshHeightMultiplier, terrainData.meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, terrainData.mapChunkSize, terrainData.mapChunkSize));
		falloffMap = FalloffGenerator.GenerateFalloffMap(terrainData.mapChunkSize);
	}
}

[System.Serializable]
public struct TerrainType
{
	public string name;
	public float height;
	public Color colour;
}