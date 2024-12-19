using System;
using UnityEngine;

public class HexMapGenerator : MonoBehaviour
{
    public GameObject hexPrefab;        // Altýgen prefab'ý
    public int mapWidth = 20;           // Harita geniþliði
    public int mapHeight = 20;          // Harita yüksekliði
    public float hexSize = 1f;          // Altýgen boyutu
    public float noiseScale = 10f;      // Perlin Noise ölçek deðeri

    private float hexWidth;             // Altýgen geniþliði
    private float hexHeight;            // Altýgen yüksekliði
    private Vector3 startPosition;      // Baþlangýç pozisyonu

    public bool GenerateSmooth = false;
    void Start()
    {
        hexWidth = Mathf.Sqrt(3) * hexSize;
        hexHeight = 2f * hexSize;

        startPosition = new Vector3(-mapWidth * hexWidth * 0.5f, 0, -mapHeight * hexHeight * 0.75f * 0.5f);

        GenerateMapWithPerlinNoise(GenerateSmooth);
    }

	private void GenerateMapWithPerlinNoise(bool generateSmooth)
	{
        if (GenerateSmooth) GenerateMapWithPerlinNoiseSmooth();
        else GenerateMapWithPerlinNoiseFlat();
	}

	[SerializeField] private float waterHeight = 0f;
    [SerializeField] private float grassHeight = 0.5f;
    [SerializeField] private float sandHeight = 1.0f;
    [SerializeField] private float mountainHeight = 1.5f;
    [SerializeField] private float hexPrefabHeight = 11.5f;

    void GenerateMapWithPerlinNoiseFlat()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 hexPos = CalculateHexPosition(x, y);

                float noiseValue = Mathf.PerlinNoise((x + 0.1f) / noiseScale, (y + 0.1f) / noiseScale);

                GameObject hex = Instantiate(hexPrefab, hexPos, Quaternion.identity);
                hex.transform.SetParent(this.transform);
                hex.name = $"Hex_{x}_{y}";

                float finalHeight = DetermineHexHeightAndColor(hex, noiseValue);

                finalHeight = Mathf.Round(finalHeight * hexPrefabHeight);
                hex.transform.position += new Vector3(0, finalHeight, 0);
            }
        }
    }

    float DetermineHexHeightAndColor(GameObject hex, float noiseValue)
    {
        MeshRenderer hexRenderer = hex.GetComponent<MeshRenderer>();
        float height = 0f;

        if (noiseValue < 0.3f)
        {
            // Su
            hexRenderer.material.color = Color.blue;
            height = waterHeight;
        }
        else if (noiseValue < 0.5f)
        {
            // Çimen
            hexRenderer.material.color = Color.green;
            height = grassHeight;
        }
        else if (noiseValue < 0.7f)
        {
            // Kum
            hexRenderer.material.color = Color.yellow;
            height = sandHeight;
        }
        else
        {
            // Dað
            hexRenderer.material.color = Color.gray;
            height = mountainHeight;
        }

        return height;
    }

    void GenerateMapWithPerlinNoiseSmooth()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 hexPos = CalculateHexPosition(x, y);

                float noiseValue = Mathf.PerlinNoise((x + 0.1f) / noiseScale, (y + 0.1f) / noiseScale);

                GameObject hex = Instantiate(hexPrefab, hexPos, Quaternion.identity);
                hex.transform.SetParent(this.transform);
                hex.name = $"Hex_{x}_{y}";

                SetHexColorBasedOnNoise(hex, noiseValue);

                float heightValue = noiseValue * 100f;
                hex.transform.position += new Vector3(0, heightValue, 0);
            }
        }
    }

    Vector3 CalculateHexPosition(int x, int y)
    {
        float offset = (y % 2 == 0) ? 0 : hexWidth / 2;
        float posX = startPosition.x + x * hexWidth + offset;
        float posZ = startPosition.z + y * (hexHeight * 0.75f);
        return new Vector3(posX, 0, posZ);
    }

    void SetHexColorBasedOnNoise(GameObject hex, float noiseValue)
    {
        Renderer renderer = hex.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (noiseValue < 0.3f)
            {
                renderer.material.color = Color.blue; // Su
                //SetHexHight(hex, noiseValue, 1f);
            }
            else if (noiseValue < 0.5f)
            {
                renderer.material.color = Color.green; // Çimen
                //SetHexHight(hex, noiseValue, 2f);
            }
            else if (noiseValue < 0.7f)
            {
                renderer.material.color = Color.yellow; // Kum
                //SetHexHight(hex, noiseValue, 11.5f);
            }
            else
            {
                renderer.material.color = Color.gray; // Dað
                //SetHexHight(hex, noiseValue, 20f);
            }
        }
    }

    private void SetHexHight(GameObject hex, float noiseValue, float multiplierValue)
    {
        float heightValue = noiseValue * multiplierValue;
        hex.transform.position += new Vector3(0, heightValue, 0);
    }
}

