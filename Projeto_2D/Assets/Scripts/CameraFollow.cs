using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform[] targets;  // Array of 4 objects to track
    public Vector2 offset;  // Offset to adjust the camera's position (X, Y only)
    public float smoothSpeed = 0.125f;  // Controls how smooth the camera movement is

    private Vector3 velocity = Vector3.zero;  // Vector3 needed for SmoothDamp

    public Camera mainCamera;  // Reference to the main camera

    public int followPlayer = 0;  // Toggle to follow the player

    public float zoomSpeed = 2f;  // Speed of the zoom effect

    // Screen limits for the camera (define the boundaries of the level)
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    void LateUpdate()
    {
        if (followPlayer == 2)
        {
            FollowPlayer();
        }
        else if (followPlayer == 1)
        {
            StartCoroutine(ZoomIn());
            followPlayer = 2;
            smoothSpeed = 1f;
        }
    }

    // Function to follow the player
    void FollowPlayer()
    {
        // Calculate the mean position of the targets
        Vector2 meanPosition = GetMeanPosition();

        // Calculate the target position for the camera
        Vector3 targetPosition = new Vector3(meanPosition.x, meanPosition.y, transform.position.z);

        // Apply screen limits to the camera's target position
        targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);  // Clamp the x position
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);  // Clamp the y position

        // Smoothly move the camera towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition + (Vector3)offset, ref velocity, smoothSpeed);
        
        smoothSpeed = 0.125f;  // Reset smooth speed for normal follow behavior
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

    IEnumerator ZoomIn()
    {
        while (Mathf.Abs(mainCamera.orthographicSize - 10) > 0.01f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, 10, Time.deltaTime * zoomSpeed);
            yield return null;  // Wait for the next frame
        }

        // Ensure the final camera size is exactly the target size
        mainCamera.orthographicSize = 10;
    }
}
