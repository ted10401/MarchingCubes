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

        Gizmos.DrawLine(m_marchingCubes.nodePositions[0], m_marchingCubes.nodePositions[1]);
        Gizmos.DrawLine(m_marchingCubes.nodePositions[1], m_marchingCubes.nodePositions[2]);
        Gizmos.DrawLine(m_marchingCubes.nodePositions[2], m_marchingCubes.nodePositions[3]);
        Gizmos.DrawLine(m_marchingCubes.nodePositions[3], m_marchingCubes.nodePositions[0]);
        Gizmos.DrawLine(m_marchingCubes.nodePositions[4], m_marchingCubes.nodePositions[5]);
        Gizmos.DrawLine(m_marchingCubes.nodePositions[5], m_marchingCubes.nodePositions[6]);
        Gizmos.DrawLine(m_marchingCubes.nodePositions[6], m_marchingCubes.nodePositions[7]);
        Gizmos.DrawLine(m_marchingCubes.nodePositions[7], m_marchingCubes.nodePositions[4]);
        Gizmos.DrawLine(m_marchingCubes.nodePositions[0], m_marchingCubes.nodePositions[4]);
        Gizmos.DrawLine(m_marchingCubes.nodePositions[1], m_marchingCubes.nodePositions[5]);
        Gizmos.DrawLine(m_marchingCubes.nodePositions[2], m_marchingCubes.nodePositions[6]);
        Gizmos.DrawLine(m_marchingCubes.nodePositions[3], m_marchingCubes.nodePositions[7]);

        for (int i = 0; i < m_marchingCubes.nodePositions.Length; i++)
        {
            if(nodes[i])
            {
                Gizmos.color = Color.black;
            }
            else
            {
                Gizmos.color = Color.white;
            }

            if(m_marchingCubes.nodePositions[i].y > 0)
            {
                UnityEditor.Handles.Label(m_marchingCubes.nodePositions[i] + Vector3.up * 0.2f, i.ToString());
            }
            else
            {
                UnityEditor.Handles.Label(m_marchingCubes.nodePositions[i] + Vector3.up * -0.1f, i.ToString());
            }

            Gizmos.DrawSphere(m_marchingCubes.nodePositions[i], 0.1f);
        }

        Gizmos.color = Color.grey;
        for (int i = 0; i < m_marchingCubes.edgePositions.Length; i++)
        {
            if (m_marchingCubes.edgePositions[i].y > 0)
            {
                UnityEditor.Handles.Label(m_marchingCubes.edgePositions[i] + Vector3.up * 0.2f, (i + 8).ToString());
            }
            else
            {
                UnityEditor.Handles.Label(m_marchingCubes.edgePositions[i] + Vector3.up * -0.1f, (i + 8).ToString());
            }

            Gizmos.DrawSphere(m_marchingCubes.edgePositions[i], 0.05f);
        }
    }
}
