using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class MeshGenerator : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] verticies = new Vector3[14];
    private int[] triangles = new int[72];
    private List<Vector2> uvs = new();
    public float radius;
    public float height = 2f;

    private int TextureAtlasSize = 4;
    private float NormalizedBlockTextureSize { get { return 1f / (float)TextureAtlasSize; } }

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    private void Start()
    {
        CreateMeshData();

        CreateMesh();
    }

    void CreateMeshData()
    {
        verticies[0] = new Vector3(0, height * 0.5f, 0);
        verticies[7] = new Vector3(0, -height * 0.5f, 0);
        //uvs.Add(new Vector2(verticies[0].x, verticies[0].z));
        //uvs.Add(new Vector2(verticies[7].x, verticies[7].z));
        uvs = new List<Vector2>(new Vector2[verticies.Length]);
        uvs[0] = CalculateSquareRegion(1);  // Orta üst vertex için
        uvs[7] = CalculateSquareRegion(1);  // Orta alt vertex için
        CreateTopHexagonMeshData();
        CreateSideHexagonMeshData();
        
    }

    private void CreateTopHexagonMeshData()
    {
        for (int i = 0; i < 6; i++)
        {
            verticies[i + 1] = GetPoint(i, height);
            verticies[i + 8] = GetPoint(i, -height);

            //uvs.Add(new Vector2(verticies[i + 1].x, verticies[i + 1].z));
            //uvs.Add(new Vector2(verticies[i + 8].x, verticies[i + 8].z));

            uvs[i + 1] = CalculateSquareRegion(1);  // Üst yüzey için
            uvs[i + 8] = CalculateSquareRegion(1);  // Alt yüzey için
        }

        for (int i = 0; i < 6; i++)
        {
            // Top
            triangles[i * 3] = 0;              // center vertex
            triangles[i * 3 + 1] = i + 2 > 6 ? 1 : i + 2; // current outer vertex
            triangles[i * 3 + 2] = i + 1;      // next outer vertex (wrap around)

            // Bottom
            triangles[i * 3 + 18] = 7;
            triangles[i * 3 + 18 + 1] = i + 8;
            triangles[i * 3 + 18 + 2] = i + 8 > 12 ? 8 : i + 9;
        }
    }

    private void CreateSideHexagonMeshData()
    {
        // 12 triangles, 6 quads split into 2 triangles each
        for (int i = 0; i < 6; i++)
        {
            int next = (i + 1) % 6;

            // First triangle 
            triangles[i * 6 + 36] = i + 1;          
            triangles[i * 6 + 36 + 1] = next + 1;   
            triangles[i * 6 + 36 + 2] = i + 8;      

            // Second triangle 
            triangles[i * 6 + 36 + 3] = i + 8;      
            triangles[i * 6 + 36 + 4] = next + 1;   
            triangles[i * 6 + 36 + 5] = next + 8;
        }
    }

    private Vector2 CalculateSquareRegion(int textureId)
    {
        float y = textureId / TextureAtlasSize;
        float x = textureId - (y * TextureAtlasSize);

        x *= NormalizedBlockTextureSize;
        y *= NormalizedBlockTextureSize;

        y = 1f - y - NormalizedBlockTextureSize;

        //return new Vector2(x, y);
        //uvs.Add(new Vector2(x, y + NormalizedBlockTextureSize));
        //uvs.Add(new Vector2(x + NormalizedBlockTextureSize, y));
        //uvs.Add(new Vector2(x + NormalizedBlockTextureSize, y + NormalizedBlockTextureSize));

        return new Vector2(x, y + NormalizedBlockTextureSize);
    }

    private Vector3 GetPoint(int index, float height)
    {
        float angle_deg = 60 * index;
        float angle_rad = Mathf.Deg2Rad * angle_deg;
        return new Vector3(radius * Mathf.Cos(angle_rad), height * 0.5f, radius * Mathf.Sin(angle_rad));
    }

    void CreateMesh()
    {
        mesh.Clear();

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();
    }


    //void CreateHexagonalPrism()
    //{
    //    // Two sets of 7 vertices (for top and bottom)
    //    Vector3[] vertices = new Vector3[14];
    //    int[] triangles = new int[72]; // 6 triangles for top, 6 for bottom, and 12 for sides
    //    Vector2[] uvs = new Vector2[14];

    //    // Offset to move the top hexagon up and bottom hexagon down
    //    float halfHeight = height / 2f;

    //    // Calculate top and bottom hexagon vertices
    //    for (int i = 0; i < 7; i++)
    //    {
    //        // Top vertices
    //        if (i == 0)
    //            vertices[i] = new Vector3(0, halfHeight, 0); // Center top vertex
    //        else
    //        {
    //            float angle_deg = 60 * (i - 1); // Same hexagon math as before
    //            float angle_rad = Mathf.Deg2Rad * angle_deg;
    //            vertices[i] = new Vector3(radius * Mathf.Cos(angle_rad), halfHeight, radius * Mathf.Sin(angle_rad)); // Top vertex
    //        }

    //        // Bottom vertices (same positions as top, but lower by height)
    //        vertices[i + 7] = new Vector3(vertices[i].x, -halfHeight, vertices[i].z); // Bottom vertex

    //        // UVs for top and bottom
    //        uvs[i] = new Vector2(vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f); // Top UV
    //        uvs[i + 7] = new Vector2(vertices[i + 7].x / radius / 2 + 0.5f, vertices[i + 7].z / radius / 2 + 0.5f); // Bottom UV
    //    }

    //    // Define the top and bottom faces (6 triangles each)
    //    for (int i = 0; i < 6; i++)
    //    {
    //        // Top face (clockwise winding)
    //        triangles[i * 3] = 0; // Top center
    //        triangles[i * 3 + 1] = (i + 1) % 6 + 1; // Current outer vertex
    //        triangles[i * 3 + 2] = i + 1; // Next outer vertex


    //        // Bottom face (clockwise winding when viewed from below, counterclockwise when viewed from above)
    //        triangles[18 + i * 3] = 7; // Bottom center
    //        triangles[18 + i * 3 + 1] = i + 8; // Current outer vertex (bottom)
    //        triangles[18 + i * 3 + 2] = (i + 1) % 6 + 8; // Next outer vertex (bottom)
    //    }

    //    // Define the side faces (12 triangles, 6 quads split into 2 triangles each)
    //    for (int i = 0; i < 6; i++)
    //    {
    //        int next = (i + 1) % 6;

    //        // First triangle of the quad
    //        triangles[36 + i * 6] = i + 1;          // Top current
    //        triangles[36 + i * 6 + 1] = next + 1;   // Top next
    //        triangles[36 + i * 6 + 2] = i + 8;      // Bottom current

    //        // Second triangle of the quad
    //        triangles[36 + i * 6 + 3] = i + 8;      // Bottom current
    //        triangles[36 + i * 6 + 4] = next + 1;   // Top next
    //        triangles[36 + i * 6 + 5] = next + 8;   // Bottom next
    //    }

    //    mesh.Clear();
    //    mesh.vertices = vertices;
    //    mesh.triangles = triangles;
    //    mesh.uv = uvs;
    //    mesh.RecalculateNormals();
    //    mesh.RecalculateBounds();
    //}
}
