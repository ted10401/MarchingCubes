using UnityEngine;
using System.Collections.Generic;

public class Metaball : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Vector3Int mapSize = new Vector3Int(10, 10, 10);
    public float cubeSize = 1f;
    public Transform[] balls;

    private int[,,] m_intMaps;
    private int[,,] m_finalMaps;

    private void Awake()
    {
        Generate();
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        if (m_intMaps != null)
        {
            if (m_intMaps.GetLength(0) != mapSize.x ||
                m_intMaps.GetLength(1) != mapSize.y ||
                m_intMaps.GetLength(2) != mapSize.z)
            {
                Generate();
            }
            else
            {
                GenerateMesh();
            }
        }
        else
        {
            Generate();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Generate();
        }
    }

    private void Generate()
    {
        GenerateIntMap();
        GenerateFinalMap();
        GenerateMesh();
    }

    private void GenerateIntMap()
    {
        m_intMaps = new int[mapSize.x, mapSize.y, mapSize.z];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    m_intMaps[x, y, z] = 1;
                }
            }
        }
    }

    private void GenerateFinalMap()
    {
        m_finalMaps = m_intMaps;

        if(balls == null || balls.Length == 0)
        {
            return;
        }

        int[,,] cacheMap = new int[mapSize.x, mapSize.y, mapSize.z];
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    cacheMap[x, y, z] = 1;
                }
            }
        }

        for (int i = 0; i < balls.Length; i++)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    for (int z = 0; z < mapSize.z; z++)
                    {
                        if(Vector3.Distance(new Vector3((float)-mapSize.x / 2 * cubeSize + x * cubeSize + cubeSize / 2, (float)-mapSize.y / 2 * cubeSize + y * cubeSize + cubeSize / 2, (float)-mapSize.z / 2 * cubeSize + z * cubeSize + cubeSize / 2), balls[i].position) <= balls[i].localScale.x * 0.5f)
                        {
                            cacheMap[x, y, z] = 0;
                        }
                    }
                }
            }
        }

        m_finalMaps = cacheMap;
    }

    private List<Vector3> m_vertices = new List<Vector3>();
    private List<int> m_triangles = new List<int>();
    private MarchingCubes m_marchingCubes;
    private void GenerateMesh()
    {
        m_vertices.Clear();
        m_triangles.Clear();
        m_marchingCubes = new MarchingCubes(transform.position, cubeSize, null);
        for (int x = 0; x < mapSize.x - 1; x++)
        {
            for (int y = 0; y < mapSize.y - 1; y++)
            {
                for (int z = 0; z < mapSize.z - 1; z++)
                {
                    m_marchingCubes.SetCenter(new Vector3((float)-mapSize.x / 2 * cubeSize + x * cubeSize + cubeSize, (float)-mapSize.y / 2 * cubeSize + y * cubeSize + cubeSize, (float)-mapSize.z / 2 * cubeSize + z * cubeSize + cubeSize));
                    m_marchingCubes.SetNodes(new bool[] { m_finalMaps[x, y, z] == 1, m_finalMaps[x, y, z + 1] == 1, m_finalMaps[x + 1, y, z + 1] == 1, m_finalMaps[x + 1, y, z] == 1,
                    m_finalMaps[x, y + 1, z] == 1, m_finalMaps[x, y + 1, z + 1] == 1, m_finalMaps[x + 1, y + 1, z + 1] == 1, m_finalMaps[x + 1, y + 1, z] == 1 });

                    foreach (int triangle in m_marchingCubes.triangles)
                    {
                        m_triangles.Add(triangle + m_vertices.Count);
                    }
                    m_vertices.AddRange(m_marchingCubes.vertices);
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
        if (m_intMaps == null)
        {
            return;
        }

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    Color color = Color.white * (1 - m_finalMaps[x, y, z]);
                    color.a = 1;
                    Gizmos.color = color;

                    Gizmos.DrawSphere(new Vector3((float)-mapSize.x / 2 * cubeSize + x * cubeSize + cubeSize / 2, (float)-mapSize.y / 2 * cubeSize + y * cubeSize + cubeSize / 2, (float)-mapSize.z / 2 * cubeSize + z * cubeSize + cubeSize / 2), cubeSize * 0.1f);
                }
            }
        }
    }
}
