using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTesting_MarchingCubes : MonoBehaviour
{
    public Vector3 center = Vector3.zero;
    public float cubeSize = 1f;
    public bool[] nodes = new bool[8];
    public MeshFilter meshFilter;

    private MarchingCubes m_marchingCubes = new MarchingCubes();

    private void OnValidate()
    {
        m_marchingCubes.SetCenter(center);
        m_marchingCubes.SetCubeSize(cubeSize);
        m_marchingCubes.SetNodes(nodes);

        Debug.LogError(m_marchingCubes.configuration);

        Mesh mesh = new Mesh();
        mesh.vertices = m_marchingCubes.vertices;
        mesh.triangles = m_marchingCubes.triangles.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if(m_marchingCubes == null)
        {
            return;
        }

        for(int i = 0; i < m_marchingCubes.nodePositions.Length; i++)
        {
            if(nodes[i])
            {
                Gizmos.color = Color.black;
            }
            else
            {
                Gizmos.color = Color.white;
            }

            Gizmos.DrawSphere(m_marchingCubes.nodePositions[i], 0.1f);
        }
    }
}
