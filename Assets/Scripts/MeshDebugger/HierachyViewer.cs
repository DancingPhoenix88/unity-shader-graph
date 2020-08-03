using UnityEngine;

public class HierachyViewer : MonoBehaviour {
    [SerializeField] protected Color color = Color.green;
    #if UNITY_EDITOR
    Transform[] nodes;
    protected void Start () {
        nodes = GetComponentsInChildren<Transform>();
    }

    protected void LateUpdate() {
        foreach(Transform b in nodes) {
            if (b.parent == null) continue;
            Debug.DrawLine( b.position, b.parent.position, color, 1/30f, false );
        }
     }
    #endif
}
