using UnityEngine;

public class BackgroundColorLerp : MonoBehaviour
{
    public Color color1 = Color.red;   // Starting color
    public Color color2 = Color.blue;  // Ending color
    public float duration = 5.0f;      // Time to interpolate

    private Camera cam;
    private float lerpTime;

    void Start()
    {
        cam = Camera.main;  // Reference to the main camera
        lerpTime = 0f;      // Initialize lerp time
    }

    void Update()
    {
        // Increment time based on how much time has passed since last frame
        lerpTime += Time.deltaTime;

        // Cycle lerpTime so it repeats back and forth between 0 and duration
        float t = Mathf.PingPong(lerpTime / duration, 1f);

        // Interpolate the background color
        cam.backgroundColor = Color.Lerp(color1, color2, t);
    }
}
