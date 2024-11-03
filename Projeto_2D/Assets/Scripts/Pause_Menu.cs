using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;  
    public SpriteRenderer planeSpriteRenderer;  
    public float opacityValue = 0.5f;  

    private bool isPaused = false;

    void Start(){
        
        planeSpriteRenderer.color = new Color(planeSpriteRenderer.color.r, planeSpriteRenderer.color.g, planeSpriteRenderer.color.b, 0);
        pauseMenu.SetActive(false);  
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused; 

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
        planeSpriteRenderer.color = new Color(planeSpriteRenderer.color.r, planeSpriteRenderer.color.g, planeSpriteRenderer.color.b, opacityValue);
        Time.timeScale = 0f;  
    }

    void DeactivatePauseMenu()
    {
        pauseMenu.SetActive(false);
        planeSpriteRenderer.color = new Color(planeSpriteRenderer.color.r, planeSpriteRenderer.color.g, planeSpriteRenderer.color.b, 0);
        Time.timeScale = 1f;  
    }
}
