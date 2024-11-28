using UnityEngine;
public class MenuItem : MonoBehaviour
{
    public GameObject targetObject;  
    private SpriteRenderer spriteRenderer;
    public Player_Movement playerMovement; 
    public CameraFollow cameraController; 
    public float launchForce = 10f;
    public float zoomSpeed = 2f;  
    public float targetCameraSize = 10f;  
    private Color originalColor;

    void Start()
    {
        spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnMouseEnter(){
        // Mudar a cor do sprite para um tom mais escuro on hover
        if (!playerMovement.can_move){
            Color hoverColor = originalColor;
            hoverColor.a = 0.5f;
            spriteRenderer.color = hoverColor;
        }
    }

    void OnMouseDown(){
        cameraController.followPlayer = 1;
        
        spriteRenderer.color = originalColor;

        if (!playerMovement.can_move){
            playerMovement.joints[0].AddForce(Vector2.right * launchForce, ForceMode2D.Impulse);
        }
    }

}
