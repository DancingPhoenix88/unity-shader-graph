using UnityEngine;

public class MovingGrid : MonoBehaviour {
    [Range(0f, 20f)] public float space = 1f;
    public Vector3 magnitude3 = Vector3.one;
    public Vector3 speed3 = Vector3.one;
    public GameObject goVertex;
    public GameObject goEdge;
    public Transform tCam;
    public float minVertexSize = 0.1f;
    public float maxVertexSize = 100f;
    public float maxDistance = 100f;

    protected Vector3[,] positions;
    protected Transform[,] vertices;
    protected LineRenderer[] edges;
    protected Vector3[] segmentPoints;
    protected const int SIZE = 30;
    protected Vector3 camPos;
    protected float[,] randomSeeds;

    //------------------------------------------------------------------------------------------------------------------
    protected void Start() {
        positions = new Vector3[ SIZE, SIZE ];
        randomSeeds = new float[ SIZE, SIZE ];
        vertices  = new Transform[ SIZE, SIZE ];
        edges     = new LineRenderer[ SIZE * 2 ];
        segmentPoints = new Vector3[ SIZE * 2 ];
        Transform t = transform;

        camPos = tCam.position;

        for (int i = 0; i < SIZE; ++i) {
            for (int k = 0; k < SIZE; ++k) {
                randomSeeds[i,k] = Random.value;
            }
        }

        for (int i = 0; i < SIZE; ++i) {
            for (int k = 0; k < SIZE; ++k) {
                GameObject goV = GameObject.Instantiate( goVertex );
                vertices[i,k] = goV.transform;
                goV.name = "v_"+i+"_"+k;
                vertices[i,k].SetParent( t );
            }
        }

        for (int i = 0; i < SIZE; ++i) {
            GameObject goE = GameObject.Instantiate( goEdge );
            goE.name = "e_"+i;
            edges[i] = goE.GetComponent<LineRenderer>();
            goE.transform.SetParent( t );

            goE = GameObject.Instantiate( goEdge );
            goE.name = "e_"+(i + SIZE);
            edges[i + SIZE] = goE.GetComponent<LineRenderer>();
            goE.transform.SetParent( t );
        }
    }
    //------------------------------------------------------------------------------------------------------------------
    protected void Update() {
        SetPositions();
        for (int i = 0; i < SIZE; ++i) {
            for (int k = 0; k < SIZE; ++k) {
                vertices[i,k].localPosition = positions[i,k];
                vertices[i,k].localScale = Vector3.one * Mathf.Lerp( minVertexSize, maxVertexSize, Mathf.InverseLerp( 0f, maxDistance, Vector3.Distance( positions[i,k], camPos ) ) );
                // further vertices need to be bigger for better bokeh shape
            }
        }
        for (int i = 0; i < SIZE; ++i) {
            for (int k = 0; k < SIZE; ++k) {
                segmentPoints[k] = positions[i,k];
            }
            edges[i].positionCount = SIZE;
            edges[i].SetPositions( segmentPoints );
        }
        for (int i = 0; i < SIZE - 1; ++i) {
            int v = 0;
            for (int k = 0; k < SIZE; ++k) {
                segmentPoints[v++] = positions[i,k];
                segmentPoints[v++] = positions[i+1,k];
            }
            edges[i+SIZE].positionCount = SIZE * 2;
            edges[i+SIZE].SetPositions( segmentPoints );
        }
    }
    //------------------------------------------------------------------------------------------------------------------
    protected void SetPositions () {
        Vector3 offset = transform.position;
        float time = Time.timeSinceLevelLoad;
        for (int i = 0; i < SIZE; ++i) {
            for (int k = 0; k < SIZE; ++k) {
                Vector3 noiseInput = Vector3.Scale( new Vector3( i, 0f, k) + Vector3.one * randomSeeds[i,k], speed3 ) * time;
                positions[i, k] = new Vector3( i * space, 0f, k * space ) + offset + GetNoise( noiseInput );
            }
        }
    }
    //------------------------------------------------------------------------------------------------------------------
    protected Vector3 GetNoise (Vector3 v) {
        return Vector3.Scale( magnitude3, new Vector3( GetNoise(v.x), GetNoise(v.y), GetNoise(v.z) ) );
    }
    //------------------------------------------------------------------------------------------------------------------
    protected float GetNoise (float x) {
        return Mathf.Sin(x) + Mathf.Sin(x*2f);
    }
}
