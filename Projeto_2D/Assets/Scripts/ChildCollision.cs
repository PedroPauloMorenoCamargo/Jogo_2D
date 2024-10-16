using UnityEngine;

public class ChildCollision : MonoBehaviour
{
    private Player_Movement parentScript;

    private void Start()
    {
        // Find and cache the parent script (Player_Movement)
        parentScript = GetComponentInParent<Player_Movement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Notify the parent that a child has collided with the ground
            parentScript.OnChildCollisionWithGround();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Notify the parent that a child has stopped colliding with the ground
            parentScript.OnChildCollisionExitGround();
        }
    }
}
