using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DynamicGrid : MonoBehaviour {
    protected int[][] vertexIndices;
    protected Transform[][] vertices;
    protected LineRenderer[] lines;
    protected Vector3[][] positions;
    public Color color = Color.white;
    public Material material = null;

    protected void Awake() {
        if (vertices == null) Init();
    }

    protected void Init () {
        vertexIndices = new int[][]{
            new int[]{ 2, 15, 32, 6, 19, 36, 2 },
            new int[]{ 3, 16, 33, 5, 18, 35, 3 },
            new int[]{ 4, 17, 34, 4 },
            new int[]{ 8, 21, 31, 12, 25, 27, 8 },
            new int[]{ 9, 22, 30, 11, 24, 28, 9 },
            new int[]{ 10, 23, 29, 10 },
            new int[]{ 7, 20 },
            new int[]{ 1, 26 },
            new int[]{ 14, 13 }
        };

        vertices = new Transform[ vertexIndices.Length ][];
        for (int i = 0; i < vertexIndices.Length; ++i) {
            vertices[i] = new Transform[ vertexIndices[i].Length ];
            for (int k = 0; k < vertexIndices[i].Length; ++k) {
                vertices[i][k] = GameObject.Find( "dot (" + vertexIndices[i][k] + ")" ).transform;
            }
        }

        while (transform.childCount > 0) {
            GameObject.DestroyImmediate( transform.GetChild(0).gameObject );
        }

        lines = new LineRenderer[ vertexIndices.Length ];
        for (int i = 0; i < vertexIndices.Length; ++i) {
            GameObject go = new GameObject();
            go.transform.SetParent( transform );
            lines[i] = go.AddComponent<LineRenderer>();
            lines[i].useWorldSpace = true;
            lines[i].startWidth = 1f;
            lines[i].endWidth = 1f;
            lines[i].startColor = lines[i].endColor = color;
            lines[i].material = material;
        }

        positions = new Vector3[ vertexIndices.Length ][];
        for (int i = 0; i < vertexIndices.Length; ++i) {
            positions[i] = new Vector3[ vertexIndices[i].Length ];
        }
        Debug.Log( "DONE init DynamicGrid" );
    }

    protected void Update() {
        if (vertices == null) Init();
        if (vertices == null) return;

        for (int i = 0; i < vertices.Length; ++i) {
            for (int k = 0; k < vertices[i].Length; ++k) {
                positions[i][k] = vertices[i][k].position + Vector3.forward * 10f;
            }
            lines[i].positionCount = positions[i].Length;
            lines[i].SetPositions( positions[i] );
            lines[i].startColor = lines[i].endColor = color;
        }
    }
}
