using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;  // Assign your pause menu in the inspector
    public SpriteRenderer planeSpriteRenderer;  // Assign the SpriteRenderer of the plane
    public float opacityValue = 0.5f;  // Desired opacity when paused

    private bool isPaused = false;

    void Start()
    {
        // Set initial opacity to 0
        planeSpriteRenderer.color = new Color(planeSpriteRenderer.color.r, planeSpriteRenderer.color.g, planeSpriteRenderer.color.b, 0);
        pauseMenu.SetActive(false);  // Ensure the menu is hidden initially
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;  // Toggle pause state

            if (isPaused)
            {
                ActivatePauseMenu();
            }
            else
            {
                DeactivatePauseMenu();
            }
        }
    }

    void ActivatePauseMenu()
    {
        pauseMenu.SetActive(true);
        // Set the opacity of the sprite to the desired value
        planeSpriteRenderer.color = new Color(planeSpriteRenderer.color.r, planeSpriteRenderer.color.g, planeSpriteRenderer.color.b, opacityValue);
        Time.timeScale = 0f;  // Pause the game
    }

    void DeactivatePauseMenu()
    {
        pauseMenu.SetActive(false);
        // Reset the opacity of the sprite to 0
        planeSpriteRenderer.color = new Color(planeSpriteRenderer.color.r, planeSpriteRenderer.color.g, planeSpriteRenderer.color.b, 0);
        Time.timeScale = 1f;  // Resume the game
    }
}
