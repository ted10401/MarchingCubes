using UnityEngine;
using System.Collections.Generic;

public class ProceduralTerrain : MonoBehaviour
{
    public Vector3Int terrainSize = new Vector3Int(10, 10, 10);
    public float cubeSize = 1;
    public float surfaceHeight = 1;
    public float perlinNoise = 1f;
    public float perlinNoiseMultiplier = 1f;

    public MeshFilter meshFilter;

    private float[,,] m_maps;

    private void OnValidate()
    {
        GenerateMap();
        GenerateMesh();
    }

    private void GenerateMap()
    {
        if (m_maps == null ||
            m_maps.GetLength(0) != terrainSize.x ||
            m_maps.GetLength(1) != terrainSize.y ||
            m_maps.GetLength(2) != terrainSize.z)
        {
            m_maps = new float[terrainSize.x, terrainSize.y, terrainSize.z];
        }

        for (int x = 0; x < terrainSize.x; x++)
        {
            for (int y = 0; y < terrainSize.y; y++)
            {
                for (int z = 0; z < terrainSize.z; z++)
                {
                    m_maps[x, y, z] = y >= (surfaceHeight + Mathf.PerlinNoise(x * perlinNoise, z * perlinNoise) * perlinNoiseMultiplier) ? 1 : 0;
                }
            }
        }
    }

    private MarchingCubes m_marchingCubes = new MarchingCubes();
    private List<int> m_triangles = new List<int>();
    private List<Vector3> m_vertices = new List<Vector3>();
    private void GenerateMesh()
    {
        if(meshFilter == null)
        {
            return;
        }

        m_marchingCubes.SetCubeSize(cubeSize);
        m_triangles.Clear();
        m_vertices.Clear();

        for (int x = 0; x < terrainSize.x - 1; x++)
        {
            for (int y = 0; y < terrainSize.y - 1; y++)
            {
                for (int z = 0; z < terrainSize.z - 1; z++)
                {
                    m_marchingCubes.SetCenter(new Vector3((float)-terrainSize.x / 2 + x + cubeSize, y + cubeSize, (float)-terrainSize.z / 2 + z + cubeSize));
                    m_marchingCubes.SetNodes(new bool[] { m_maps[x, y, z] > 0, m_maps[x, y, z + 1] > 0, m_maps[x + 1, y, z + 1] > 0, m_maps[x + 1, y, z] > 0,
                    m_maps[x, y + 1, z] > 0, m_maps[x, y + 1, z + 1] > 0, m_maps[x + 1, y + 1, z + 1] > 0, m_maps[x + 1, y + 1, z] > 0 });

                    foreach (int triangle in m_marchingCubes.validTriangles)
                    {
                        m_triangles.Add(triangle + m_vertices.Count);
                    }
                    m_vertices.AddRange(m_marchingCubes.validVertices);
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = m_vertices.ToArray();
        mesh.triangles = m_triangles.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if(m_maps == null)
        {
            return;
        }

        for (int x = 0; x < terrainSize.x; x++)
        {
            for (int y = 0; y < terrainSize.y; y++)
            {
                for (int z = 0; z < terrainSize.z; z++)
                {
                    Color color = Color.white * (1 - m_maps[x, y, z]);
                    color.a = 1;
                    Gizmos.color = color;

                    Gizmos.DrawSphere(new Vector3((float)-terrainSize.x / 2 + x + cubeSize / 2, y + cubeSize / 2, (float)-terrainSize.z / 2 + z + cubeSize / 2), cubeSize * 0.1f);
                }
            }
        }
    }
}
