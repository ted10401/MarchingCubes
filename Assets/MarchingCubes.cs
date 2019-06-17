using System.Collections.Generic;
using UnityEngine;

public class MarchingCubes
{
    public int configuration;
    public Vector3 center = Vector3.zero;
    public float cubeSize = 1f;

    public Vector3[] nodePositions = new Vector3[8];
    public Vector3[] vertices = new Vector3[20];
    public List<int> triangles = new List<int>();

    public MarchingCubes()
    {

    }

    public MarchingCubes(Vector3 center, float cubeSize, bool[] nodes)
    {
        this.center = center;
        this.cubeSize = cubeSize;
        UpdateVertices();
        SetNodes(nodes);
    }

    public void SetCenter(Vector3 center)
    {
        this.center = center;
        UpdateVertices();
    }

    public void SetCubeSize(float cubeSize)
    {
        this.cubeSize = cubeSize;
        UpdateVertices();
    }

    private void UpdateVertices()
    {
        vertices[0] = center + new Vector3(-1, -1, -1) * (cubeSize * 0.5f);
        vertices[1] = center + new Vector3(0, -1, -1) * (cubeSize * 0.5f);
        vertices[2] = center + new Vector3(1, -1, -1) * (cubeSize * 0.5f);
        vertices[3] = center + new Vector3(1, -1, 0) * (cubeSize * 0.5f);
        vertices[4] = center + new Vector3(1, -1, 1) * (cubeSize * 0.5f);
        vertices[5] = center + new Vector3(0, -1, 1) * (cubeSize * 0.5f);
        vertices[6] = center + new Vector3(-1, -1, 1) * (cubeSize * 0.5f);
        vertices[7] = center + new Vector3(-1, -1, 0) * (cubeSize * 0.5f);

        vertices[8] = center + new Vector3(-1, 0, -1) * (cubeSize * 0.5f);
        vertices[9] = center + new Vector3(1, 0, -1) * (cubeSize * 0.5f);
        vertices[10] = center + new Vector3(1, 0, 1) * (cubeSize * 0.5f);
        vertices[11] = center + new Vector3(-1, 0, 1) * (cubeSize * 0.5f);

        vertices[12] = center + new Vector3(-1, 1, -1) * (cubeSize * 0.5f);
        vertices[13] = center + new Vector3(0, 1, -1) * (cubeSize * 0.5f);
        vertices[14] = center + new Vector3(1, 1, -1) * (cubeSize * 0.5f);
        vertices[15] = center + new Vector3(1, 1, 0) * (cubeSize * 0.5f);
        vertices[16] = center + new Vector3(1, 1, 1) * (cubeSize * 0.5f);
        vertices[17] = center + new Vector3(0, 1, 1) * (cubeSize * 0.5f);
        vertices[18] = center + new Vector3(-1, 1, 1) * (cubeSize * 0.5f);
        vertices[19] = center + new Vector3(-1, 1, 0) * (cubeSize * 0.5f);

        nodePositions[0] = vertices[0];
        nodePositions[1] = vertices[2];
        nodePositions[2] = vertices[4];
        nodePositions[3] = vertices[6];
        nodePositions[4] = vertices[12];
        nodePositions[5] = vertices[14];
        nodePositions[6] = vertices[16];
        nodePositions[7] = vertices[18];
    }

    public void SetNodes(bool[] nodes)
    {
        if(nodes == null || nodes.Length != 8)
        {
            Debug.LogError("Nodes are not valid");
            return;
        }

        configuration = 0;
        int bit = 1;
        for(int i = 0; i < nodes.Length; i++)
        {
            bit = 1 << i;
            configuration += nodes[i] ? bit : 0;
        }

        GenerateTriangles();
    }

    private void GenerateTriangles()
    {
        triangles.Clear();

        switch (configuration)
        {
            case 0:
            case 255:
                break;

                //One Node
            case 1:
                AddTriagnles(1, 7, 8);
                break;
            case 2:
                AddTriagnles(1, 9, 3);
                break;
            case 4:
                AddTriagnles(3, 10, 5);
                break;
            case 8:
                AddTriagnles(11, 7, 5);
                break;
            case 16:
                AddTriagnles(8, 19, 13);
                break;
            case 32:
                AddTriagnles(9, 13, 15);
                break;
            case 64:
                AddTriagnles(10, 15, 17);
                break;
            case 128:
                AddTriagnles(11, 17, 19);
                break;

                //Two Nodes
                //1 + 2^N
            case 3:
                AddTriagnles(8, 9, 3, 8, 3, 7);
                break;
            case 5:
                AddTriagnles(1, 7, 8, 3, 10, 5);
                break;
            case 9:
                AddTriagnles(11, 8, 1, 11, 1, 5);
                break;
            case 17:
                AddTriagnles(13, 1, 7, 13, 7, 19);
                break;
            case 33:
                AddTriagnles(1, 7, 8, 9, 13, 15);
                break;
            case 65:
                AddTriagnles(1, 7, 8, 10, 15, 17);
                break;
            case 129:
                AddTriagnles(1, 7, 8, 11, 17, 19);
                break;

                //2 + 2^N
            case 6:
                AddTriagnles(9, 10, 5, 9, 5, 1);
                break;
            case 10:
                AddTriagnles(1, 9, 3, 11, 7, 5);
                break;
            case 18:
                AddTriagnles(1, 9, 3, 8, 19, 13);
                break;
            case 34:
                AddTriagnles(1, 13, 15, 1, 15, 3);
                break;
            case 66:
                AddTriagnles(1, 9, 3, 10, 15, 17);
                break;
            case 130:
                AddTriagnles(1, 9, 3, 11, 17, 19);
                break;

            //4 + 2^N
            case 12:
                AddTriagnles(10, 11, 7, 10, 7, 3);
                break;
            case 20:
                AddTriagnles(3, 10, 5, 8, 19, 13);
                break;
            case 36:
                AddTriagnles(3, 10, 5, 9, 13, 15);
                break;
            case 68:
                AddTriagnles(3, 15, 17, 3, 17, 5);
                break;
            case 132:
                AddTriagnles(3, 10, 5, 11, 17, 19);
                break;

                //8 + 2N
            case 24:
                AddTriagnles(11, 7, 5, 8, 19, 13);
                break;
            case 40:
                AddTriagnles(11, 7, 5, 9, 13, 15);
                break;
            case 72:
                AddTriagnles(11, 7, 5, 10, 15, 17);
                break;
            case 136:
                AddTriagnles(19, 7, 5, 19, 5, 17);
                break;

                //16 + 2^N
            case 48:
                AddTriagnles(19, 15, 9, 19, 9, 8);
                break;
            case 80:
                AddTriagnles(8, 19, 13, 10, 15, 17);
                break;
            case 144:
                AddTriagnles(17, 13, 8, 17, 8, 11);
                break;

                //32 + 2^N
            case 96:
                AddTriagnles(13, 17, 10, 13, 10, 9);
                break;
            case 160:
                AddTriagnles(9, 13, 15, 11, 17, 19);
                break;

                //64 + 2^N
            case 192:
                AddTriagnles(15, 19, 11, 15, 11, 10);
                break;
        }
    }

    private void AddTriagnles(params int[] indexes)
    {
        for(int i = 0; i < indexes.Length; i++)
        {
            triangles.Add(indexes[i]);
        }
    }
}
