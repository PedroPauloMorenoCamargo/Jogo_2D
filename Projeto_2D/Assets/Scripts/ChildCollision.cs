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
            Debug.Log("Assigned HeightBasedMusicController via script.");
        }

        if (parentScript == null)
        {
            Debug.LogError("Player_Movement script not found in parent.");
        }

        if (musicController == null)
        {
            Debug.LogError("HeightBasedMusicController is not assigned in ChildCollision.");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Collision with Ground detected.");

            if (parentScript != null)
            {
                parentScript.OnChildCollisionWithGround();
            }

            if (!hasLanded && musicController != null)
            {
                hasLanded = true;
                musicController.gameStarted = true;
                Debug.Log("Game started set to true.");
            }
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
