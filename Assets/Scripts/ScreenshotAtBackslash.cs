using UnityEngine;
using System.IO;

public class ScreenshotAtBackslash : MonoBehaviour {
    protected void Update () {
        if (Input.GetKeyDown( KeyCode.Backslash )) {
            Save( Application.dataPath + "/../Screenshots/" + Time.realtimeSinceStartup + ".png" );
        }
    }
    //------------------------------------------------------------------------------------------------------------------
    public void Save (string filePath) {
        #if UNITY_EDITOR
            ScreenCapture.CaptureScreenshot( filePath, 1 );
            Debug.LogWarning( "[SCREEN SHOT]: " + filePath );
        #endif
    }
}

