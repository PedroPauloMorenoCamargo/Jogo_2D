using UnityEngine;
public class MenuItem : MonoBehaviour
{
    public GameObject targetObject;  // Assign the GameObject that has the SpriteRenderer you want to change
    private SpriteRenderer spriteRenderer;
    public Player_Movement playerMovement;  // Reference to the Player_Movement script
    public CameraFollow cameraController; // Reference to your camera controllera
    public float launchForce = 10f;
    public float zoomSpeed = 2f;  // Speed of the zoom effect
    public float targetCameraSize = 10f;  // Target camera size
    private Color originalColor;

    void Start()
    {
        spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnMouseEnter()
    {
        // Change only the alpha to give the appearance of selection
        Color hoverColor = originalColor;
        hoverColor.a = 0.5f;
        spriteRenderer.color = hoverColor;
    }

    void OnMouseDown()
    {
        // Set a variable in the camera controller to true
        cameraController.followPlayer = 1;
        
        // Reset the color back to original after the click
        spriteRenderer.color = originalColor;

        playerMovement.joints[0].AddForce(Vector2.right * launchForce*1.2f, ForceMode2D.Impulse);
    }

}
