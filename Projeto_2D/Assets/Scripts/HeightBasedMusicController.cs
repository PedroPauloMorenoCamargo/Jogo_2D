using UnityEngine;
using System.Collections;

public class HeightBasedMusicController : MonoBehaviour
{
    public Transform cobra; // Arraste o Transform da cobra diretamente aqui
    public AudioSource groundMusic;
    public AudioSource spaceMusic;
    public float transitionHeight = 190f;
    public float fadeDuration = 1.0f; // Duração do fade em segundos

    private bool isInSpace = false;
    public bool gameStarted = false;
    private Coroutine fadeCoroutine;

    void Start()
    {
        if (groundMusic != null && spaceMusic != null)
        {
            Debug.Log("Starting with ground music.");
            groundMusic.volume = 1.0f; // Inicia a música do chão no volume máximo
            groundMusic.Play();
            spaceMusic.volume = 0.0f; // Inicia a música do espaço no volume mínimo
            spaceMusic.Play();
        }
        else
        {
            Debug.LogError("Audio sources are not assigned.");
        }
    }

    void Update()
    {
        if (!gameStarted)
        {
            Debug.Log("Waiting for the game to start...");
            return;
        }

        if (groundMusic == null || spaceMusic == null)
        {
            Debug.LogError("Audio sources are not assigned.");
            return;
        }

        float cobraHeight = cobra.position.y;
        Debug.Log("Cobra Global Height: " + cobraHeight);  // Log para verificar a altura da cobra em tempo real

        if (cobraHeight >= transitionHeight && !isInSpace)
        {
            Debug.Log("Switching to space music.");
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine); // Para qualquer fade em andamento
            fadeCoroutine = StartCoroutine(FadeMusic(groundMusic, spaceMusic));
            isInSpace = true;
        }
        else if (cobraHeight < transitionHeight && isInSpace)
        {
            Debug.Log("Switching to ground music.");
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine); // Para qualquer fade em andamento
            fadeCoroutine = StartCoroutine(FadeMusic(spaceMusic, groundMusic));
            isInSpace = false;
        }
    }

    private IEnumerator FadeMusic(AudioSource fromMusic, AudioSource toMusic)
    {
        float startVolume = fromMusic.volume;
        float endVolume = toMusic.volume;

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            fromMusic.volume = Mathf.Lerp(startVolume, 0, elapsedTime / fadeDuration);
            toMusic.volume = Mathf.Lerp(endVolume, 1, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fromMusic.volume = 0;
        toMusic.volume = 1;
    }
}
