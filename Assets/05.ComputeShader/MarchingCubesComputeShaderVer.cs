using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubesComputeShaderVer : MonoBehaviour
{
    public ComputeShader marchingCubeShader;
    public ComputeShader pointShader;
    public int numPointsPerAxis = 2;
    [Range(0f, 1f)] public float isoLevel = 1f;
    public MeshFilter meshFilter;

    struct Triangle
    {
#pragma warning disable 649 // disable unassigned variable warning
        public Vector3 a;
        public Vector3 b;
        public Vector3 c;

        public Vector3 this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    default:
                        return c;
                }
            }
        }
    }

    // Buffers
    ComputeBuffer triangleBuffer;
    ComputeBuffer pointsBuffer;
    ComputeBuffer triCountBuffer;

    private void Update()
    {
        if (marchingCubeShader == null || meshFilter == null)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Run();
        }
    }

    private void Run()
    {
        CreateBuffers();

        if (Application.isPlaying)
        {
            UpdateMesh();
        }

        // Release buffers immediately in editor
        if (!Application.isPlaying)
        {
            ReleaseBuffers();
        }
    }

    void CreateBuffers()
    {
        int numPoints = numPointsPerAxis * numPointsPerAxis * numPointsPerAxis;
        int numVoxelsPerAxis = numPointsPerAxis - 1;
        int numVoxels = numVoxelsPerAxis * numVoxelsPerAxis * numVoxelsPerAxis;
        int maxTriangleCount = numVoxels * 5;

        // Always create buffers in editor (since buffers are released immediately to prevent memory leak)
        // Otherwise, only create if null or if size has changed
        if (!Application.isPlaying || pointsBuffer == null || numPoints != pointsBuffer.count)
        {
            if (Application.isPlaying)
            {
                ReleaseBuffers();
            }
            triangleBuffer = new ComputeBuffer(maxTriangleCount, sizeof(float) * 3 * 3, ComputeBufferType.Append);
            pointsBuffer = new ComputeBuffer(numPoints, sizeof(float) * 4);
            triCountBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);

        }
    }

    void ReleaseBuffers()
    {
        if (triangleBuffer != null)
        {
            triangleBuffer.Release();
            pointsBuffer.Release();
            triCountBuffer.Release();
        }
    }

    const int threadGroupSize = 8;
    private void UpdateMesh()
    {
        UpdatePoints();

        int numVoxelsPerAxis = numPointsPerAxis - 1;
        int numThreadsPerAxis = Mathf.CeilToInt(numVoxelsPerAxis / (float)threadGroupSize);

        triangleBuffer.SetCounterValue(0);
        marchingCubeShader.SetBuffer(0, "points", pointsBuffer);
        marchingCubeShader.SetBuffer(0, "triangles", triangleBuffer);
        marchingCubeShader.SetInt("numPointsPerAxis", numPointsPerAxis);
        marchingCubeShader.SetFloat("isoLevel", isoLevel);

        marchingCubeShader.Dispatch(0, numThreadsPerAxis, numThreadsPerAxis, numThreadsPerAxis);

        // Get number of triangles in the triangle buffer
        ComputeBuffer.CopyCount(triangleBuffer, triCountBuffer, 0);
        int[] triCountArray = { 0 };
        triCountBuffer.GetData(triCountArray);
        int numTris = triCountArray[0];

        // Get triangle data from shader
        Triangle[] tris = new Triangle[numTris];
        triangleBuffer.GetData(tris, 0, 0, numTris);

        Mesh mesh = new Mesh();

        var vertices = new Vector3[numTris * 3];
        var meshTriangles = new int[numTris * 3];

        for (int i = 0; i < numTris; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                meshTriangles[i * 3 + j] = i * 3 + j;
                vertices[i * 3 + j] = tris[i][j];
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = meshTriangles;

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private Vector4[] m_points;
    private void UpdatePoints()
    {
        int numPoints = numPointsPerAxis * numPointsPerAxis * numPointsPerAxis;
        int numThreadsPerAxis = Mathf.CeilToInt(numPointsPerAxis / (float)threadGroupSize);

        m_points = new Vector4[numPoints];
        int i = 0;
        for(int x = 0; x < numPointsPerAxis; x++)
        {
            for (int y = 0; y < numPointsPerAxis; y++)
            {
                for (int z = 0; z < numPointsPerAxis; z++)
                {
                    m_points[i] = Vector4.one * (-numPointsPerAxis * 0.5f + 0.5f) + new Vector4(x, y, z);
                    m_points[i].w = Random.Range(0, 2);
                    //if (i == 0)
                    //{
                    //    m_points[i].w = 0;
                    //}
                    //else
                    //{
                    //    m_points[i].w = 1;
                    //}

                    i++;
                }
            }
        }

        pointsBuffer.SetData(m_points);
    }

    private void OnDrawGizmos()
    {
        if(m_points == null)
        {
            return;
        }

        for(int i = 0; i < m_points.Length; i++)
        {
            Gizmos.color = m_points[i].w == 0 ? Color.black : Color.white;
            Gizmos.DrawSphere(m_points[i], 0.1f);
        }
    }
}
