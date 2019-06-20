using UnityEngine;
using System.Collections.Generic;

public class ProceduralCubes : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Vector3Int mapSize = new Vector3Int(10, 10, 10);
    [Range(0f, 1f)] public float cullingThreshold = 1f;
    public float cubeSize = 1f;

    private float[,,] m_floatMaps;
    private float[,,] m_cullingMaps;
    private int[,,] m_finalMaps;

    private void Awake()
    {
        Generate();
    }

    private void OnValidate()
    {
        if(!Application.isPlaying)
        {
            return;
        }

        if (m_floatMaps != null)
        {
            if (m_floatMaps.GetLength(0) != mapSize.x ||
                m_floatMaps.GetLength(1) != mapSize.y ||
                m_floatMaps.GetLength(2) != mapSize.z)
            {
                Generate();
            }
            else
            {
                GenerateCullingMap();
                GenerateFinalMap();
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Generate();
        }
    }

    private void Generate()
    {
        GenerateFloatMap();
        GenerateCullingMap();
        GenerateFinalMap();
        GenerateMesh();
    }

    private void GenerateFloatMap()
    {
        m_floatMaps = new float[mapSize.x, mapSize.y, mapSize.z];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    if (x == 0 || x == mapSize.x - 1 ||
                        y == 0 || y == mapSize.y - 1 ||
                        z == 0 || z == mapSize.z - 1)
                    {
                        m_floatMaps[x, y, z] = 1;
                    }
                    else
                    {
                        m_floatMaps[x, y, z] = Random.Range(0f, 1f);
                    }
                }
            }
        }
    }

    private void GenerateCullingMap()
    {
        if (m_floatMaps == null)
        {
            return;
        }

        m_cullingMaps = new float[mapSize.x, mapSize.y, mapSize.z];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    m_cullingMaps[x, y, z] = m_floatMaps[x, y, z] >= cullingThreshold ? 1 : m_floatMaps[x, y, z];
                }
            }
        }
    }

    private void GenerateFinalMap()
    {
        if (m_cullingMaps == null)
        {
            return;
        }

        m_finalMaps = new int[mapSize.x, mapSize.y, mapSize.z];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    m_finalMaps[x, y, z] = m_floatMaps[x, y, z] >= cullingThreshold ? 1 : 0;
                }
            }
        }
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
                    m_marchingCubes.SetCenter(new Vector3((float)-mapSize.x / 2 + x + cubeSize, (float)-mapSize.y / 2 + y + cubeSize, (float)-mapSize.z / 2 + z + cubeSize));
                    m_marchingCubes.SetNodes(new bool[] { m_finalMaps[x, y, z] == 1, m_finalMaps[x, y, z + 1] == 1, m_finalMaps[x + 1, y, z + 1] == 1, m_finalMaps[x + 1, y, z] == 1,
                    m_finalMaps[x, y + 1, z] == 1, m_finalMaps[x, y + 1, z + 1] == 1, m_finalMaps[x + 1, y + 1, z + 1] == 1, m_finalMaps[x + 1, y + 1, z] == 1 });

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
        if (m_finalMaps == null)
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

                    Gizmos.DrawSphere(new Vector3((float)-mapSize.x / 2 + x + cubeSize / 2, (float)-mapSize.y / 2 + y + cubeSize / 2, (float)-mapSize.z / 2 + z + cubeSize / 2), cubeSize * 0.1f);
                }
            }
        }
    }
}
