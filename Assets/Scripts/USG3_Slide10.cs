using UnityEngine;
using UnityEngine.UI;

public class USG3_Slide10 : MonoBehaviour {
    public Image[] drops;
    public Image[] multiResults;
    public Image[] addResults;
    public Gradient[] gradients;
    public float[] durations;
    protected float[] progress;
    //------------------------------------------------------------------------------------------------------------------
    protected void Start () {
        progress = new float[ durations.Length ];
        for (int i = 0; i < progress.Length; ++i) {
            progress[i] = 0f;
        }
    }
    //------------------------------------------------------------------------------------------------------------------
    protected void Update () {
        float dt = Time.deltaTime;

        for (int i = 0; i < progress.Length; ++i) {
            progress[i] = (progress[i] + dt) % durations[i];
            float t = progress[i] / durations[i];
            Color c = gradients[i].Evaluate( t );
            drops[i].color = multiResults[i].color = addResults[i].color = c;
        }
    }
}
