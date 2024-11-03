using UnityEngine;

public class ChildCollision : MonoBehaviour
{
    private Player_Movement parentScript;
    public HeightBasedMusicController musicController;  // Referência ao controlador de música
    private bool hasLanded = false;

    private void Start()
    {
        parentScript = GetComponentInParent<Player_Movement>();

        if (musicController == null)
        {
            musicController = GameObject.FindObjectOfType<HeightBasedMusicController>();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {

            if (parentScript != null)
            {
                parentScript.OnChildCollisionWithGround();
            }

            if (!hasLanded && musicController != null)
            {
                hasLanded = true;
                musicController.gameStarted = true;
            }
        }
        else if (collision.collider.CompareTag("End")){
            parentScript.OnChildCollisionWithEnd();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (parentScript != null)
            {
                parentScript.OnChildCollisionExitGround();
            }
        }
    }
}
