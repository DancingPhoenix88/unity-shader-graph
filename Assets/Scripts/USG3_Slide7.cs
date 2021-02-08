using UnityEngine;
using UnityEngine.UI;

public class USG3_Slide7 : MonoBehaviour {
    public Image drop;
    public Gradient gradient;
    public float duration;
    protected float progress = 0f;
    //------------------------------------------------------------------------------------------------------------------
    protected void Update () {
        float dt = Time.deltaTime;
        progress = (progress + dt) % duration;
        float t = progress / duration;
        Color c = gradient.Evaluate( t );
        drop.color = c;
    }
}
