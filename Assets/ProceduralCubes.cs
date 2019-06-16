using UnityEngine;

public class ProceduralCubes : MonoBehaviour
{
    public Vector3Int mapSize = new Vector3Int(10, 10, 10);
    [Range(0f, 1f)] public float cullingThreshold = 1f;
    public float cubeSize = 1f;

    private float[,,] m_floatMaps;
    private float[,,] m_cullingMaps;

    private void OnValidate()
    {
        if(m_floatMaps != null)
        {
            if (m_floatMaps.GetLength(0) != mapSize.x ||
                m_floatMaps.GetLength(1) != mapSize.y ||
                m_floatMaps.GetLength(2) != mapSize.z)
            {
                GenerateFloatMap();
            }
        }

        GenerateCullingMap();
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

    private void OnDrawGizmos()
    {
        if (m_cullingMaps == null)
        {
            return;
        }

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int z = 0; z < mapSize.z; z++)
                {
                    Color color = Color.white * (1 - m_cullingMaps[x, y, z]);
                    color.a = 1;
                    Gizmos.color = color;

                    Gizmos.DrawSphere(new Vector3((float)-mapSize.x / 2 + x + cubeSize / 2, (float)-mapSize.y / 2 + y + cubeSize / 2, (float)-mapSize.z / 2 + z + cubeSize / 2), cubeSize * 0.1f);
                }
            }
        }
    }
}
