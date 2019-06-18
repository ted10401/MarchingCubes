using System.Collections.Generic;
using UnityEngine;

public class MarchingCubes
{
    public int configuration;
    public Vector3 center = Vector3.zero;
    public float cubeSize = 1f;

    public Vector3[] nodePositions = new Vector3[8];
    public Vector3[] edgePositions = new Vector3[12];
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
        nodePositions[0] = center + new Vector3(-1, -1, -1) * (cubeSize * 0.5f);
        nodePositions[1] = center + new Vector3(1, -1, -1) * (cubeSize * 0.5f);
        nodePositions[2] = center + new Vector3(1, -1, 1) * (cubeSize * 0.5f);
        nodePositions[3] = center + new Vector3(-1, -1, 1) * (cubeSize * 0.5f);
        nodePositions[4] = center + new Vector3(-1, 1, -1) * (cubeSize * 0.5f);
        nodePositions[5] = center + new Vector3(1, 1, -1) * (cubeSize * 0.5f);
        nodePositions[6] = center + new Vector3(1, 1, 1) * (cubeSize * 0.5f);
        nodePositions[7] = center + new Vector3(-1, 1, 1) * (cubeSize * 0.5f);

        edgePositions[0] = center + new Vector3(0, -1, -1) * (cubeSize * 0.5f);
        edgePositions[1] = center + new Vector3(1, -1, 0) * (cubeSize * 0.5f);
        edgePositions[2] = center + new Vector3(0, -1, 1) * (cubeSize * 0.5f);
        edgePositions[3] = center + new Vector3(-1, -1, 0) * (cubeSize * 0.5f);
        edgePositions[4] = center + new Vector3(-1, 0, -1) * (cubeSize * 0.5f);
        edgePositions[5] = center + new Vector3(1, 0, -1) * (cubeSize * 0.5f);
        edgePositions[6] = center + new Vector3(1, 0, 1) * (cubeSize * 0.5f);
        edgePositions[7] = center + new Vector3(-1, 0, 1) * (cubeSize * 0.5f);
        edgePositions[8] = center + new Vector3(0, 1, -1) * (cubeSize * 0.5f);
        edgePositions[9] = center + new Vector3(1, 1, 0) * (cubeSize * 0.5f);
        edgePositions[10] = center + new Vector3(0, 1, 1) * (cubeSize * 0.5f);
        edgePositions[11] = center + new Vector3(-1, 1, 0) * (cubeSize * 0.5f);

        for(int i = 0; i < nodePositions.Length; i++)
        {
            vertices[i] = nodePositions[i];
        }

        for (int i = 0; i < edgePositions.Length; i++)
        {
            vertices[i + nodePositions.Length] = edgePositions[i];
        }
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
            case 0:   //00000000
                break;
            case 1:   //00000001
                AddTriagnles(8, 11, 12);
                break;
            case 2:   //00000010
                AddTriagnles(8, 13, 9);
                break;
            case 3:   //00000011
                AddTriagnles(12, 13, 9, 12, 9, 11);
                break;
            case 4:   //00000100
                AddTriagnles(9, 14, 10);
                break;
            case 5:   //00000101
                AddTriagnles(8, 11, 12, 9, 14, 10);
                break;
            case 6:   //00000110
                AddTriagnles(13, 14, 10, 13, 10, 8);
                break;

            case 7:   //00000111
                break;

            case 8:   //00001000
                AddTriagnles(15, 11, 10);
                break;
            case 9:   //00001001
                AddTriagnles(15, 12, 8, 15, 8, 10);
                break;
            case 10:  //00001010
                AddTriagnles(8, 13, 9, 15, 11, 10);
                break;

            case 11:  //00001011
                break;

            case 12:  //00001100
                AddTriagnles(14, 15, 11, 14, 11, 9);
                break;

            case 13:   //00001101
                break;
            case 14:   //00001110
                break;
            case 15:   //00001111
                break;

            case 16:  //00010000
                AddTriagnles(12, 19, 16);
                break;
            case 17:  //00010001
                AddTriagnles(16, 8, 11, 16, 11, 19);
                break;
            case 18:  //00010010
                AddTriagnles(8, 13, 9, 12, 19, 16);
                break;

            case 19:  //00010011
                break;

            case 20:  //00010100
                AddTriagnles(9, 14, 10, 12, 19, 16);
                break;

            case 21:  //00010101
                break;
            case 22:  //00010110
                break;
            case 23:  //00010111
                break;

            case 24:  //00011000
                AddTriagnles(15, 11, 10, 12, 19, 16);
                break;

            case 25:  //00011001
                break;
            case 26:  //00011010
                break;
            case 27:  //00011011
                break;
            case 28:  //00011100
                break;
            case 29:  //00011101
                break;
            case 30:  //00011110
                break;
            case 31:  //00011111
                break;

            case 32:  //00100000
                AddTriagnles(13, 16, 17);
                break;
            case 33:  //00100001
                AddTriagnles(8, 11, 12, 13, 16, 17);
                break;
            case 34:  //00100010
                AddTriagnles(8, 16, 17, 8, 17, 9);
                break;

            case 35:  //00100011
                break;

            case 36:  //00100100
                AddTriagnles(9, 14, 10, 13, 16, 17);
                break;

            case 37:  //00100101
                break;
            case 38:  //00100110
                break;
            case 39:  //00100111
                break;

            case 40:  //00101000
                AddTriagnles(15, 11, 10, 13, 16, 17);
                break;

            case 41:  //00101001
                break;
            case 42:  //00101010
                break;
            case 43:  //00101011
                break;
            case 44:  //00101100
                break;
            case 45:  //00101101
                break;
            case 46:  //00101110
                break;
            case 47:  //00101111
                break;

            case 48:  //00110000
                AddTriagnles(19, 17, 13, 19, 13, 12);
                break;

            case 49:  //00110001
                break;
            case 50:  //00110010
                break;
            case 51:  //00110011
                break;
            case 52:  //00110100
                break;
            case 53:  //00110101
                break;
            case 54:  //00110110
                break;
            case 55:  //00110111
                break;
            case 56:  //00111000
                break;
            case 57:  //00111001
                break;
            case 58:  //00111010
                break;
            case 59:  //00111011
                break;
            case 60:  //00111100
                break;
            case 61:  //00111101
                break;
            case 62:  //00111110
                break;
            case 63:  //00111111
                break;

            case 64:  //01000000
                AddTriagnles(14, 17, 18);
                break;
            case 65:  //01000001
                AddTriagnles(8, 11, 12, 14, 17, 18);
                break;
            case 66:  //01000010
                AddTriagnles(8, 13, 9, 14, 17, 18);
                break;

            case 67:  //01000011
                break;

            case 68:  //01000100
                AddTriagnles(9, 17, 18, 9, 18, 10);
                break;

            case 69:  //01000101
                break;
            case 70:  //01000110
                break;
            case 71:  //01000111
                break;

            case 72:  //01001000
                AddTriagnles(15, 11, 10, 14, 17, 18);
                break;

            case 73:  //01001001
                break;
            case 74:  //01001010
                break;
            case 75:  //01001011
                break;
            case 76:  //01001100
                break;
            case 77:  //01001101
                break;
            case 78:  //01001110
                break;
            case 79:  //01001111
                break;

            case 80:  //01010000
                AddTriagnles(12, 19, 16, 14, 17, 18);
                break;

            case 81:  //01010001
                break;
            case 82:  //01010010
                break;
            case 83:  //01010011
                break;
            case 84:  //01010100
                break;
            case 85:  //01010101
                break;
            case 86:  //01010110
                break;
            case 87:  //01010111
                break;
            case 88:  //01011000
                break;
            case 89:  //01011001
                break;
            case 90:  //01011010
                break;
            case 91:  //01011011
                break;
            case 92:  //01011100
                break;
            case 93:  //01011101
                break;
            case 94:  //01011110
                break;
            case 95:  //01011111
                break;

            case 96:  //01100000
                AddTriagnles(16, 18, 14, 16, 14, 13);
                break;

            case 97:  //01100001
                break;
            case 98:  //01100010
                break;
            case 99:  //01100011
                break;
            case 100: //01100100
                break;
            case 101: //01100101
                break;
            case 102: //01100110
                break;
            case 103: //01100111
                break;
            case 104: //01101000
                break;
            case 105: //01101001
                break;
            case 106: //01101010
                break;
            case 107: //01101011
                break;
            case 108: //01101100
                break;
            case 109: //01101101
                break;
            case 110: //01101110
                break;
            case 111: //01101111
                break;
            case 112: //01110000
                break;
            case 113: //01110001
                break;
            case 114: //01110010
                break;
            case 115: //01110011
                break;
            case 116: //01110100
                break;
            case 117: //01110101
                break;
            case 118: //01110110
                break;
            case 119: //01110111
                break;
            case 120: //01111000
                break;
            case 121: //01111001
                break;
            case 122: //01111010
                break;
            case 123: //01111011
                break;
            case 124: //01111100
                break;
            case 125: //01111101
                break;
            case 126: //01111110
                break;
            case 127: //01111111
                break;

            case 128: //10000000
                AddTriagnles(15, 18, 19);
                break;
            case 129: //10000001
                AddTriagnles(8, 11, 12, 15, 18, 19);
                break;
            case 130: //10000010
                AddTriagnles(8, 13, 9, 15, 18, 19);
                break;
            case 132: //10000100
                AddTriagnles(9, 14, 10, 15, 18, 19);
                break;
            case 136: //10001000
                AddTriagnles(19, 11, 10, 19, 10, 18);
                break;
            case 144: //10010000
                AddTriagnles(18, 16, 12, 18, 12, 15);
                break;
            case 160: //10100000
                AddTriagnles(13, 16, 17, 15, 18, 19);
                break;
            case 192: //11000000
                AddTriagnles(17, 19, 15, 17, 15, 14);
                break;
            case 255: //11111111
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
