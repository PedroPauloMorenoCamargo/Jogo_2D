using UnityEngine;
using UnityEngine.UI;  // Necessário para usar Button

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;           // Referência ao menu de pausa
    public SpriteRenderer planeSpriteRenderer;  // Sprite para efeito de opacidade
    public float opacityValue = 0.5f;      // Valor de opacidade
    public Button pauseButton;             // Botão de pausa

    public Button restartButton;             // Botão de pausa

    public bool isPaused = false;         // Controle do estado de pausa

    void Start()
    {
        // Inicializa a opacidade do plano
        planeSpriteRenderer.color = new Color(planeSpriteRenderer.color.r, planeSpriteRenderer.color.g, planeSpriteRenderer.color.b, 0);
        pauseMenu.SetActive(false);  // Desativa o menu de pausa no início

        // Adiciona o evento ao botão de pausa
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePauseMenu);
        }
    }

    // Alterna o estado do menu de pausa
    public void TogglePauseMenu()
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

    // Ativa o menu de pausa
    void ActivatePauseMenu()
    {
        pauseMenu.SetActive(true);
        planeSpriteRenderer.color = new Color(planeSpriteRenderer.color.r, planeSpriteRenderer.color.g, planeSpriteRenderer.color.b, opacityValue);
        Time.timeScale = 0f;  // Pausa o jogo
        restartButton.gameObject.SetActive(false);
    }

    // Desativa o menu de pausa
    void DeactivatePauseMenu()
    {
        pauseMenu.SetActive(false);
        restartButton.gameObject.SetActive(true);
        planeSpriteRenderer.color = new Color(planeSpriteRenderer.color.r, planeSpriteRenderer.color.g, planeSpriteRenderer.color.b, 0);
        Time.timeScale = 1f;  // Retoma o jogo
    }
}
