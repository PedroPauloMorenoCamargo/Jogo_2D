using UnityEngine;

public class CameraFollow: MonoBehaviour
{
    public Transform[] targets;  // Array of 4 objects to track
    public Vector2 offset;  // Offset to adjust the camera's position (X, Y only)
    public float smoothSpeed = 0.125f;  // Controls how smooth the camera movement is

    private Vector3 velocity = Vector3.zero;  // Vector3 needed for SmoothDamp

    void LateUpdate()
    {
        // Ensure there are targets to calculate the mean position
        if (targets.Length == 0)
            return;

        // Calculate the mean position (X, Y) of all targets using Vector2
        Vector2 centerPoint = GetMeanPosition();

        // Apply offset and calculate the desired position, keeping the camera's Z fixed
        Vector3 desiredPosition = new Vector3(centerPoint.x + offset.x, centerPoint.y + offset.y, transform.position.z);

        // Smoothly move the camera to the desired position (X, Y only, Z stays fixed)
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }

    // Function to calculate the mean (center) position of the targets based on X and Y
    Vector2 GetMeanPosition()
    {
        Vector2 sum = Vector2.zero;
        foreach (Transform target in targets)
        {
            sum.x += target.position.x;
            sum.y += target.position.y;
        }

        // Return the mean position as a Vector2
        return sum / targets.Length;
    }
}
