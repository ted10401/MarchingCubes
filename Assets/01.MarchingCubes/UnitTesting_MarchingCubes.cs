using UnityEngine;

public class UnitTesting_MarchingCubes : MonoBehaviour
{
    public float cubeSize = 1f;
    [Range(0f, 1f)] public float lerp = 0.5f;
    public bool[] nodes = new bool[8];
    public MeshFilter meshFilter;

    private MarchingCubes m_marchingCubes = new MarchingCubes();

    private void OnValidate()
    {
        m_marchingCubes.SetLerp(lerp);
        m_marchingCubes.SetCenter(transform.position);
        m_marchingCubes.SetCubeSize(cubeSize);
        m_marchingCubes.SetNodes(nodes);

        Mesh mesh = new Mesh();
        mesh.vertices = m_marchingCubes.validVertices.ToArray();
        mesh.triangles = m_marchingCubes.validTriangles.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        if(m_marchingCubes == null)
        {
            return;
        }

        Gizmos.color = Color.black;
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

        Vector3 sceneViewUp = UnityEditor.SceneView.currentDrawingSceneView.camera.transform.up;
        Vector3 sceneViewRight = UnityEditor.SceneView.currentDrawingSceneView.camera.transform.right;
        Vector3 sceneViewUpRight = (sceneViewUp + sceneViewRight).normalized;

        for (int i = 0; i < m_marchingCubes.nodePositions.Length; i++)
        {
            UnityEditor.Handles.Label(m_marchingCubes.nodePositions[i] + sceneViewUpRight * 0.15f, i.ToString());

            if(nodes[i])
            {
                Gizmos.color = Color.white;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawSphere(m_marchingCubes.nodePositions[i], 0.1f);
        }

        Gizmos.color = Color.green;
        for (int i = 0; i < m_marchingCubes.edgePositions.Length; i++)
        {
            UnityEditor.Handles.Label(m_marchingCubes.edgePositions[i] + sceneViewUpRight * 0.075f, i.ToString());
            Gizmos.DrawSphere(m_marchingCubes.edgePositions[i], 0.05f);
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < m_marchingCubes.validTriangles.Count; i += 3)
        {
            Gizmos.DrawLine(m_marchingCubes.validVertices[m_marchingCubes.validTriangles[i]], m_marchingCubes.validVertices[m_marchingCubes.validTriangles[i + 1]]);
            Gizmos.DrawLine(m_marchingCubes.validVertices[m_marchingCubes.validTriangles[i + 1]], m_marchingCubes.validVertices[m_marchingCubes.validTriangles[i + 2]]);
            Gizmos.DrawLine(m_marchingCubes.validVertices[m_marchingCubes.validTriangles[i]], m_marchingCubes.validVertices[m_marchingCubes.validTriangles[i + 2]]);
        }
    }
}
